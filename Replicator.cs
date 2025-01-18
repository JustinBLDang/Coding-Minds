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
    [SerializeField] GameObject obj;
    
    /*
        IEnumerator is a return type, dont worry about it. We need it to have access to the yield return.
        - This is important otherwise this function will hog Unity to itself
    */
    IEnumerator ReplicateObject(GameObject obj, int numberOfObjects, float delay = 0f){
        float scheduledTime = Time.time;    // To see what Time.time does, hover over .time in your IDE.
        int amountInstantiated = 0;

        while(amountInstantiated < numberOfObjects){
            if(Time.time >= scheduledTime){
                Instantiate(obj, transform.position + Random.insideUnitSphere, transform.rotation); // 
                scheduledTime += delay;
                amountInstantiated++;
            }
            yield return null;
        }
    }

    private void Start() {
        StartCoroutine(ReplicateObject(obj, amount, delay));
    }
}
