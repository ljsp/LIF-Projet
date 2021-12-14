using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarControl : Agent
{
    Rigidbody rBody;
    Vector3 startPosition;

    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    private bool isBreaking;

    public Transform Target;
    public Transform Target2;
    public Transform Target3;
    [SerializeField] public WheelCollider frontLeftWheelCollider;
    [SerializeField] public WheelCollider frontRightWheelCollider;
    [SerializeField] public WheelCollider rearLeftWheelCollider;
    [SerializeField] public WheelCollider rearRightWheelCollider;
    [SerializeField] public Transform frontLeftWheelTransform;
    [SerializeField] public Transform frontRightWheelTransform;
    [SerializeField] public Transform rearLeftWheelTransform;
    [SerializeField] public Transform rearRightWheelTransform;

    [SerializeField] public float maxSteeringAngle = 30f;
    [SerializeField] public float motorForce = 50f;
    [SerializeField] public float brakeForce = 0f;
    
    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        startPosition = this.transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = startPosition;
        this.transform.localRotation = Quaternion.identity;
        
        // Move the target to a new spot
        Target.localPosition = new Vector3(UnityEngine.Random.value * 8 - 4,-2.5f,UnityEngine.Random.value * 20 + 10);
        Target2.localPosition = new Vector3(UnityEngine.Random.value * 8 - 4, -2.5f, UnityEngine.Random.value * 20 + 30);
        Target3.localPosition = new Vector3(UnityEngine.Random.value * 8 - 4, -2.5f, UnityEngine.Random.value * 20 + 50);
        /*
        for (int i = 0; i < 3; i++)
        {
            Transform po = Instantiate(
                Target, 
                this.transform.position + new Vector3(UnityEngine.Random.value * 8 - 4,
                                           -2.5f,
                                           UnityEngine.Random.value * 40 + 20), 
                Quaternion.identity);
            Destroy(po.gameObject);
        }
        */

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
        MoveCar(actions.DiscreteActions);
        /*
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if(hit.collider.gameObject.name == "EndPoint")
            {
                AddReward(0.005f);
            }
            if (hit.collider.gameObject.name == "Tree")
            {
                AddReward(-0.005f);
            }

        }*/
    }
    
    private void MoveCar(ActionSegment<int> act)
    {
        AddReward(-0.0005f);
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        var dirToGoForwardAction = act[0];
        var rotateDirAction = act[1];
        //var brakeAction = act[2];
        if (dirToGoForwardAction == 1) 
        {
            AddReward(0.001f);
            dirToGo = 1f * transform.forward;
        }
        
        //else if (dirToGoForwardAction == 2) dirToGo = -1f * transform.forward;
        if (rotateDirAction == 1) rotateDir = transform.right * -1f;
        else if (rotateDirAction == 2) rotateDir = transform.right * 1f;

        //HandleBrake(brakeAction == 1);
        HandleMotor(dirToGo.z);
        HandleSteering(rotateDir.x);
        UpdateWheels();
        
    }

    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.UpArrow)) discreteActionsOut[0] = 1;
        //if (Input.GetKey(KeyCode.DownArrow)) discreteActionsOut[0] = 2;
        if (Input.GetKey(KeyCode.LeftArrow)) discreteActionsOut[1] = 1;
        if (Input.GetKey(KeyCode.RightArrow)) discreteActionsOut[1] = 2;
        //discreteActionsOut[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
    
    private void HandleSteering(float x)
    {
        steerAngle = maxSteeringAngle * x;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor(float z)
    {
        //print(z);
        frontLeftWheelCollider.motorTorque = z * motorForce;
        frontRightWheelCollider.motorTorque = z * motorForce;
        rearLeftWheelCollider.motorTorque = z * motorForce;
        rearRightWheelCollider.motorTorque = z * motorForce;
    }

    private void HandleBrake(bool v)
    {
        brakeForce = v ? 3000f : 0f;
        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-1.0f);
            EndEpisode();
        } 
        else if (other.gameObject.CompareTag("CheckPoint"))
        {
            AddReward(0.5f);
        }
        else if (other.gameObject.CompareTag("EndPoint"))
        {
            AddReward(1.0f);
            EndEpisode();
        }
    }
}
