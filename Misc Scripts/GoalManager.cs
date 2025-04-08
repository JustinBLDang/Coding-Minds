using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    int count = 0;
    public void incrementGoalCount(){ 
        count++; 
        Debug.Log("Goal Count: " + count);
    }
}
