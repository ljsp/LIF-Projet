using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgentsExamples;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    Rigidbody rBody;
    float initialPositionZ;
    public float speed = 0.06f;
    int steps = 0;
    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        initialPositionZ = this.transform.localPosition.z;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] == 1)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z - speed);
            steps++;
        }
        if (actions.DiscreteActions[0] == 2)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z + speed);
            steps++;
        }
    }

    public override void OnEpisodeBegin()
    {
        this.transform.rotation = Quaternion.Euler(0, 90, 0);

        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
        }
        this.transform.localPosition = new Vector3(3, 1, initialPositionZ);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall")
        {
            AddReward(-0.1f);
            EndEpisode();
            Debug.Log("xxxxxxxx");
        }

        if (other.gameObject.tag == "door")
        {
            float doorDeward = 0.5f - (0.01f * steps);
            if (doorDeward <= 0)
            {
                AddReward(0.1f);
            }
            else
            {
                AddReward(doorDeward);
            }

            Debug.Log(doorDeward);
            //other.gameObject.SetActive(false);

            
            steps = 0;
            Debug.Log("vvvvvvvv");
        }
    }

}
