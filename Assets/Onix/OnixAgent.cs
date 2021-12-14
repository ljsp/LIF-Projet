using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgentsExamples;
using Unity.MLAgents.Sensors;

[RequireComponent(typeof(JointDriveController))]
public class OnixAgent : Agent
{
    public Transform rock2;
    public Transform rock3;
    public Transform rock4;
    public Transform rock5;
    public Transform rock6;
    public Transform rock7;
    public Transform rock8;
    public Transform rock9;
    public Transform rock10;
    public Transform rock11;
    public Transform rock12;
    public Transform rock13;
    public Transform rock14;
    public Transform rock15;

    JointDriveController m_JdController;

    Rigidbody rBody;
    Vector3 initialPosition;
    public float speed = 0.06f;
    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        initialPosition = this.transform.localPosition;
        m_JdController.SetupBodyPart(rock2);

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var i = -1;
        var continuousActions = actionBuffers.ContinuousActions;

        var bpDict = m_JdController.bodyPartsDict;
        bpDict[rock2].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[rock2].SetJointStrength(continuousActions[++i]);
    }

    public override void OnEpisodeBegin()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
        }
        this.transform.localPosition = new Vector3(0, 4, 0);

    }


}
