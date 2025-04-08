using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

/*  
 *  Action Mapping:
 *  ---------------------------------------------
 *  Main Thrust     = continuousAction[0]
 *  Forward Thrust  = continuousAction[1]
 *  Backward Thrust = continuousAction[2]
 *  Left Thrust     = continuousAction[3]
 *  Right Thrust    = continuousAction[4]
*/

public class RocketAgent : Agent
{
    [SerializeField] GoalManager goalManager;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject fThrust;
    [SerializeField] GameObject bThrust;
    [SerializeField] GameObject rThrust;
    [SerializeField] GameObject lThrust;
    [SerializeField] float mainThrust = 1;
    [SerializeField] float controlThrust = 1;
    [SerializeField] Transform startPosition;
    [SerializeField] Material failMaterial;
    [SerializeField] Material successMaterial;
    [SerializeField] MeshRenderer platformMesh;
    int successRange = 15;
    int itr = 0;
    float score;
    float lastReward;
    float currentReward;
    float timeSinceLastExecution;

    public override void OnEpisodeBegin()
    {
        score = 0;
        transform.localPosition = startPosition.localPosition;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        goalManager.resetGoals();
        timeSinceLastExecution = Time.time;
        if (currentReward - 1 >= lastReward && currentReward + 1 <= lastReward && lastReward < 0)
        {
            AddReward((-itr * itr)/4);
        }
    }
    private void FixedUpdate()
    {
        if (Time.time - timeSinceLastExecution >= 1f)
        {
            AddReward(-50f);
            timeSinceLastExecution = Time.time;
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {

        foreach (GameObject goal in goalManager.goals)
        {
            sensor.AddObservation(goal.transform.localPosition);
        }
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //string message = "action: [";
        //foreach (float action in actions.ContinuousActions)
        //{
        //    message += action + ", ";
        //}
        //message = message.Remove(message.Length - 2, 2);
        //message += "]";
        //Debug.Log(message);
        //Debug.Log("Velocity: " + rb.velocity);

        MainThrust(actions.ContinuousActions[0]);
        //ForwardThrust(actions.ContinuousActions[1]);
        //BackwardThrust(actions.ContinuousActions[2]);
        //LeftThrust(actions.ContinuousActions[1]);
        //RightThrust(actions.ContinuousActions[2]);
    }

    /*  
     *  Main Thrust     = continuousAction[0]
     *  Forward Thrust  = continuousAction[1]
     *  Backward Thrust = continuousAction[2]
     *  Left Thrust     = continuousAction[3]
     *  Right Thrust    = continuousAction[4]
     */
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetKey("space")    ? 1 : 0;
        //continuousActions[1] = Input.GetKey("up")       ? 1 : 0;
        //continuousActions[2] = Input.GetKey("down")     ? 1 : 0;
        //continuousActions[3] = Input.GetKey("right")    ? 1 : 0;
        //continuousActions[4] = Input.GetKey("left")     ? 1 : 0;
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.tag == "ground")
        {
            //Debug.Log("ground");
            SetReward(-999f);
            score -= 999f;
            EndEpisode();
        }

        if (other.gameObject.tag == "heightGoal")
        {
            //Debug.Log("heightGoal");
            lastReward = currentReward;
            currentReward = -1 * rb.velocity.y * rb.velocity.y;
            if(-1f * successRange <= currentReward && successRange >= currentReward)
            {
                platformMesh.material = successMaterial;
                score += 500;
                SetReward(500f + currentReward);
            }
            else
            {
                platformMesh.material = failMaterial;
                score += currentReward;
                AddReward(currentReward);
            }
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "landingGoal")
        {
            //Debug.Log("landingGoal");
            AddReward(10f);
            score += 10;
            EndEpisode();
        }
    }

    void MainThrust(float strength)
    {
        rb.AddForce(transform.up.normalized * mainThrust * Math.Abs(strength), ForceMode.Force);
    }

    void ForwardThrust(float strength)
    {
        rb.AddForceAtPosition(transform.up.normalized * controlThrust * Math.Abs(strength), fThrust.transform.localPosition, ForceMode.Force);
    }

    void BackwardThrust(float strength)
    {
        rb.AddForceAtPosition(transform.up.normalized * controlThrust * Math.Abs(strength), bThrust.transform.localPosition, ForceMode.Force);
    }

    void LeftThrust(float strength)
    {
        rb.AddForceAtPosition(transform.up.normalized * controlThrust * Math.Abs(strength), lThrust.transform.localPosition, ForceMode.Force);
    }

    void RightThrust(float strength)
    {
        rb.AddForceAtPosition(transform.up.normalized * controlThrust * Math.Abs(strength), rThrust.transform.localPosition, ForceMode.Force);
    }


}
