using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionCounter : MonoBehaviour
{
    int count = 0;
    void OnTriggerEnter(Collider other) {
        count++;
        Debug.Log("Objects Counted: " + count);
    }
}
