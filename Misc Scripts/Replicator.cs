using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replicator : MonoBehaviour
{
    /*
        Spawns a object in a random position within the unit sphere of the GameObject this script is attached to.
    */
    [SerializeField] int amount = 0;    // [SerializedField] lets us see private variables in the inspector. 
    [SerializeField] int delay = 0;
    [SerializeField] GameObject[] objects;
    
    /*
        IEnumerator is a return type, dont worry about it. We need it to have access to the yield return.
        - This is important otherwise this function will hog Unity to itself


    */
    IEnumerator ReplicateObject(GameObject[] objects, int numberOfObjects, float delay = 0f){
        float scheduledTime = Time.time;    // To see what Time.time does, hover over .time in your IDE.
        int amountInstantiated = 0;
        int index = 0;

        while(amountInstantiated < numberOfObjects){
            if(Time.time >= scheduledTime){
                Instantiate(objects[index++], transform.position + UnityEngine.Random.insideUnitSphere, transform.rotation);
                scheduledTime += delay;
                amountInstantiated++;
                if(index >= objects.Length){ index = 0; }
            }
            yield return null;
        }
    }

    private void Start() {
        StartCoroutine(ReplicateObject(objects, amount, delay));
    }
}
