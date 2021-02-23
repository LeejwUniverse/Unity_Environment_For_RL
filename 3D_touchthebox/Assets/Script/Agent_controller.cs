using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent_controller : Agent
{
    GameObject[] targets;
    Rigidbody rBody;
    int touch_count;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        targets = GameObject.FindGameObjectsWithTag("Target");
        touch_count = 0;
    }

    public override void OnEpisodeBegin()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, 2f, 0);

        touch_count = 0;

        foreach (GameObject target in targets)
        {
            target.GetComponent<Collider>().isTrigger = true;
            target.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        }
    
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(this.transform.localPosition); // 3

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x); // 1
        sensor.AddObservation(rBody.velocity.z); // 1

        foreach (GameObject target in targets)
        {
            sensor.AddObservation(target.GetComponent<Transform>().localPosition); // 3 * 4
            if (Color.blue == target.GetComponent<Renderer>().material.GetColor("_Color")) // 1* 4
            {
                sensor.AddObservation(0);
            }
            else
            {
                sensor.AddObservation(1);
            }
        }
    }

    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        SetReward(-0.01f);
        if (touch_count != 0)
        { 
            SetReward(1.0f);
        }

        if (touch_count == 4)
        {
            EndEpisode();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            touch_count += 1;
            other.isTrigger = false;
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
