using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgentsExamples;

public class SnakeAgent2 : Agent
{

    Rigidbody rBody;
    Vector3 startPosition;

    public Transform Target;

    JointDriveController m_JdController;

    [Header("Body Parts")] 
    public Transform bodySegment0;
    public Transform bodySegment1;
    public Transform bodySegment2;
    public Transform bodySegment3;

    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        startPosition = this.transform.localPosition;
        m_JdController = GetComponent<JointDriveController>();
        //Setup each body part
        m_JdController.SetupBodyPart(bodySegment0);
        m_JdController.SetupBodyPart(bodySegment1);
        m_JdController.SetupBodyPart(bodySegment2);
        m_JdController.SetupBodyPart(bodySegment3);
    }

    public override void OnEpisodeBegin()
    {
        foreach (var bodyPart in m_JdController.bodyPartsList)
        {
            bodyPart.Reset(bodyPart);
        }

        //Random start rotation to help generalize
        bodySegment0.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveCar(actions.ContinuousActions);
    }

    private void MoveCar(ActionSegment<float> actionBuffers)
    {

        // The dictionary with all the body parts in it are in the jdController
        var bpDict = m_JdController.bodyPartsDict;

        var i = -1;
        var continuousActions = actionBuffers;
        // Pick a new target joint rotation
        bpDict[bodySegment0].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[bodySegment1].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[bodySegment2].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[bodySegment3].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);


        // Update joint strength
        bpDict[bodySegment0].SetJointStrength(continuousActions[++i]);
        bpDict[bodySegment1].SetJointStrength(continuousActions[++i]);
        bpDict[bodySegment2].SetJointStrength(continuousActions[++i]);
        bpDict[bodySegment3].SetJointStrength(continuousActions[++i]);

        //Reset if Worm fell through floor;
        if (bodySegment0.position.y < startPosition.y - 2)
        {
            EndEpisode();
        }
    }


    public override void Heuristic(in ActionBuffers actionBuffers)
    {
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");
        float force = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;

        var continuousActions = actionBuffers.ContinuousActions;
        var i = -1;

        //SetJointTargetRotation
        continuousActions[++i] = x; continuousActions[++i] = y;
        continuousActions[++i] = x; continuousActions[++i] = y;

        //SetJointStrength 
        continuousActions[++i] = force;
        continuousActions[++i] = force;


        if (Input.GetKey(KeyCode.UpArrow)) continuousActions[0] = x;
        if (Input.GetKey(KeyCode.DownArrow)) continuousActions[0] = y;
        if (Input.GetKey(KeyCode.LeftArrow)) continuousActions[1] = 1;
        if (Input.GetKey(KeyCode.RightArrow)) continuousActions[1] = 2;
        //discreteActionsOut[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
