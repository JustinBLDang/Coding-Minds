using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject fThrust;
    [SerializeField] GameObject bThrust;
    [SerializeField] GameObject rThrust;
    [SerializeField] GameObject lThrust;
    [SerializeField] float mainThrust = 1;
    [SerializeField] float controlThrust = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey("space")){
            Debug.Log("body");
            rb.AddForce(transform.up.normalized * mainThrust, ForceMode.Force);
        }
        if(Input.GetKey("up")){
            Debug.Log("f");
            rb.AddForceAtPosition(transform.up.normalized  * controlThrust, fThrust.transform.position, ForceMode.Force);
        }
        if(Input.GetKey("down")){
            Debug.Log("b");
            rb.AddForceAtPosition(transform.up.normalized  * controlThrust, bThrust.transform.position, ForceMode.Force);
        }
        if(Input.GetKey("right")){
            Debug.Log("r");
            rb.AddForceAtPosition(transform.up.normalized  * controlThrust, rThrust.transform.position, ForceMode.Force);
        }
        if(Input.GetKey("left")){
            Debug.Log("l");
            rb.AddForceAtPosition(transform.up.normalized  * controlThrust, lThrust.transform.position, ForceMode.Force);
        }
    }
}
