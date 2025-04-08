using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Clock : MonoBehaviour
{
    [SerializeField] TMP_Text clockText;
    // Update is called once per frame
    void Update()
    {
        clockText.text = DateTime.Now.ToString();
    }
}
