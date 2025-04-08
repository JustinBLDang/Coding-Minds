using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] Transform goalPosition;
    [SerializeField] float moveSpeed = 1f;
    
    public override void OnEpisodeBegin()
    {
        transform.position = Vector3.zero;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(goalPosition);
        sensor.AddObservation(transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        transform.position += new Vector3(moveX, 0, moveY) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall")
        {
            SetReward(-1f);
        }

        if (other.gameObject.tag == "goal")
        {
            SetReward(1f);
        }

        EndEpisode();
    }
}
