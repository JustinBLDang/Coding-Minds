using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replicator : MonoBehaviour
{
    [SerializeField] int amount = 0;
    [SerializeField] int delay = 0;
    [SerializeField] GameObject obj;
    IEnumerator ReplicateObject(GameObject obj, int numberOfObjects, float delay = 0f){
        float scheduledTime = Time.time;
        int amountInstantiated = 0;
        while(amountInstantiated < numberOfObjects){
            if(Time.time >= scheduledTime){
                Instantiate(obj, transform.position + Random.insideUnitSphere, transform.rotation);
                scheduledTime += delay;
                amountInstantiated++;
            }
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReplicateObject(obj, amount, delay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
