using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgentsExamples;
using Unity.MLAgents.Sensors;

public class SausageAgent : Agent
{
    Rigidbody rBody;
    public Transform LeglegLeft;
    public Transform legRight;
    public Transform feetLeft;
    public Transform feetRight;

    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] == 1)
        {
            Vector3 bodyRotation = new Vector3(this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y, this.transform.localRotation.eulerAngles.z + 5);
            Quaternion deltaRotation = Quaternion.Euler(bodyRotation * Time.fixedDeltaTime);
            rBody.MoveRotation(rBody.rotation * deltaRotation);

            
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
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.Z))
        {
            discreteActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 3;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 4;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ground")
        {
            GameObject.Find("Legendary Sausage").GetComponent<SausageAgent>().AddReward(0.1f);

        }
    }
}
