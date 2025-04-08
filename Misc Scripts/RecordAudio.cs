using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordAudio : MonoBehaviour
{
    private AudioClip recordedClip;
    [SerializeField] AudioSource audioSource;
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
        /*
         * Looping will result in overwriting if the recordedAudio clip becomes larger than the lengthSec
         */
        recordedClip = Microphone.Start(device, false, lengthSec, sampleRate);
    }


    public void StopRecording()
    {
        Microphone.End(null);
    }
}
