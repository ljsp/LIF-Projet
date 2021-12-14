using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgentsExamples;
using Unity.MLAgents.Sensors;


[RequireComponent(typeof(JointDriveController))] // Required to set joint forces

public class SnakeAgent : Agent
{
    [Header("Snake Body Parts")]
    public Transform head;
    public Transform lower1;
    public Transform lower2;

    [Header("Apple Target")]
    public Transform targetApple;
    private Transform m_Target;

    Rigidbody rBody;
    Vector3 initialPos;
    float speed = 0.005f;
    float angle;

    //The indicator graphic gameobject that points towards the target
    JointDriveController m_JdController;

    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        initialPos = this.transform.localPosition;

        SpawnTarget(targetApple, transform.position); //spawn target

        m_JdController = GetComponent<JointDriveController>();

        //Setup each body part
        m_JdController.SetupBodyPart(head);
        m_JdController.SetupBodyPart(lower1);
        m_JdController.SetupBodyPart(lower2);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Apple and Snake positions
        sensor.AddObservation(targetApple.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Snake velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public void CollectObservationBodyPart(BodyPart bp, VectorSensor sensor)
    {
        // À définir
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // The dictionary with all the body parts in it are in the jdController
        var bpDict = m_JdController.bodyPartsDict;

        var i = -1;
        var continuousActions = actions.ContinuousActions;
        // Pick a new target joint rotation
        bpDict[head].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[lower1].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[lower2].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);

        // Update joint strength
        bpDict[head].SetJointStrength(continuousActions[++i]);
        bpDict[lower1].SetJointStrength(continuousActions[++i]);
        bpDict[lower2].SetJointStrength(continuousActions[++i]);

        //Reset if Worm fell through floor;
        if (head.position.y < initialPos.y - 3)
        {
            EndEpisode();
        }
    }
    /*angle = head.transform.localRotation.eulerAngles.y;

    if (actions.ContinuousActions[0] == 1)
    {
        angle -= 1f;
        head.transform.rotation = Quaternion.Euler(head.transform.localRotation.eulerAngles.x, angle, head.transform.localRotation.eulerAngles.z);
        move(angle);
    }
    if (actions.ContinuousActions[0] == 2)
    {
        angle += 1f;
        head.transform.rotation = Quaternion.Euler(head.transform.localRotation.eulerAngles.x, angle, head.transform.localRotation.eulerAngles.z);
        move(angle);
    }
    if (actions.ContinuousActions[1] == 1)
    {
        lower1.transform.rotation = Quaternion.Euler(lower1.transform.localRotation.eulerAngles.x, lower1.transform.localRotation.eulerAngles.y - 1f, lower1.transform.localRotation.eulerAngles.z);
        move(angle);
    }
    if (actions.ContinuousActions[1] == 2)
    {
        lower1.transform.rotation = Quaternion.Euler(lower1.transform.localRotation.eulerAngles.x, lower1.transform.localRotation.eulerAngles.y + 1f, lower1.transform.localRotation.eulerAngles.z);
        move(angle);
    }
    if (actions.ContinuousActions[2] == 1)
    {
        lower2.transform.rotation = Quaternion.Euler(lower2.transform.localRotation.eulerAngles.x, lower2.transform.localRotation.eulerAngles.y - 1f, lower2.transform.localRotation.eulerAngles.z);
        move(angle);
    }
    if (actions.ContinuousActions[2] == 2)
    {
        lower2.transform.rotation = Quaternion.Euler(lower2.transform.localRotation.eulerAngles.x, lower2.transform.localRotation.eulerAngles.y + 1f, lower2.transform.localRotation.eulerAngles.z);
        move(angle);
    }*/

void move(float angle)
    {
        if (angle > 180)
        {
            while (angle > 180)
            {
                angle -= 180;
            }
        }
        if (angle < -180)
        {
            while (angle < 180)
            {
                angle += 180;
            }
        }

        float xR;
        float zR;
        if (angle < 90 && angle >= 0)
        {
            xR = -(angle / 90);
            zR = 1 - (-xR);
            head.transform.localPosition = new Vector3(head.transform.localPosition.x + xR * speed, head.transform.localPosition.y, head.transform.localPosition.z + zR * speed);
        }
        if (angle >= 90 && angle <= 180)
        {
            zR = -((angle - 90) / 90);
            xR = 1 - (-zR);
            head.transform.localPosition = new Vector3(head.transform.localPosition.x + xR * speed, head.transform.localPosition.y, head.transform.localPosition.z + zR * speed);
        }
        if (angle < 0 && angle >= -90)
        {
            xR = -angle / 90;
            zR = 1 - xR;
            head.transform.localPosition = new Vector3(head.transform.localPosition.x + xR * speed, head.transform.localPosition.y, head.transform.localPosition.z + zR * speed);
        }
        if (angle < -90 && angle >= -180)
        {
            zR = -(-(angle + 90f) / 90);
            xR = 1 - (-zR);
            head.transform.localPosition = new Vector3(head.transform.localPosition.x + xR * speed, head.transform.localPosition.y, head.transform.localPosition.z + zR * speed);
        }
    }

    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
        }
        this.transform.localPosition = initialPos;

        /*foreach (var bodyPart in m_JdController.bodyPartsList)
        {
            bodyPart.Reset(bodyPart);
        }

        //Random start rotation to help generalize
        head.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);*/

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        if (Input.GetKey(KeyCode.A))
        {
            continuousActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            continuousActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            continuousActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            continuousActionsOut[1] = 2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            continuousActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.C))
        {
            continuousActionsOut[2] = 2;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "target")
        {
            AddReward(1f);
            Debug.Log("vvvvvvvv");
            EndEpisode();
        }
        /*if (other.gameObject.tag == "Respawn")
        {
            OnEpisodeBegin();
        }*/
    }

    void SpawnTarget(Transform apple, Vector3 pos)
    {
        pos.x += 3;
        m_Target = Instantiate(apple, pos, Quaternion.identity, transform.parent);
    }
}
