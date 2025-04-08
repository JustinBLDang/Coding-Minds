using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecordAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] ChatGPTScript chatGPTScript;

    AudioClip recordedClip;
    float recordingStartTime;
    float recordingEndTime;
    float recordingLength;
    string filePath;

    public void PlayRecording()
    {
        audioSource.clip = recordedClip;
        audioSource.Play();
    }

    public void StartRecording()
    {
        string device = Microphone.devices[0];
        int sampleRate = 44100; // Affects quality of audio
        int lengthSec = 3599;   // Max length of recordedClip
        
        //Looping will result in overwriting if the recordedAudio clip becomes larger than the lengthSec
        recordedClip = Microphone.Start(device, false, lengthSec, sampleRate);
        recordingStartTime = Time.realtimeSinceStartup;
    }
    
    public void StopRecording()
    { 
        Microphone.End(null);
        recordingEndTime = Time.realtimeSinceStartup;
        recordingLength = recordingEndTime - recordingStartTime;
        Debug.Log("Before Trim: " + recordedClip.length);
        recordedClip = AudioManager.TrimClip(recordedClip, recordingLength);
        Debug.Log("After Trim: " + recordedClip.length);
        filePath = AudioManager.SaveRecording(recordedClip);

        chatGPTScript.GPTGradeAudio(filePath, recordedClip.length);
    }
}