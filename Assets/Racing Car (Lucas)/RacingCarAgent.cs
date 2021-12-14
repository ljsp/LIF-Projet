using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;


public class RacingCarAgent : Agent
{
    public CheckpointManager _checkpointManager;
    private CarController _carController;

    public override void Initialize()
    {
        _carController = GetComponent<CarController>();
    }

    public override void OnEpisodeBegin()
    {
        _checkpointManager.ResetCheckpoints();
        _carController.Respawn();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 diff = _checkpointManager.nextCheckPointToReach.transform.position - transform.position;
        sensor.AddObservation(diff / 30f);
        AddReward(-0.001f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var Actions = actions.ContinuousActions;

        _carController.HandleMotor(Actions[1]);
        _carController.HandleSteering(Actions[0]);
        _carController.UpdateWheels();

        //_carController.ApplyAcceleration(Actions[1]);
        //_carController.Steer(Actions[0]);
        //_carController.AnimateCar(Actions[0]);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continousActions = actionsOut.ContinuousActions;
        continousActions.Clear();

        continousActions[0] = Input.GetAxis("Horizontal");
        continousActions[1] = Input.GetKey(KeyCode.W) ? 1f : 0f;
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }
}

