using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
/*  Adapted from: https://github.com/AgrMayank/AudioRecorder
 *  
 *  FUNCTION: 
 *  - Take unity AudioClip and convert to .wav
 *  - Save converted .wav 
 */

public static class AudioManager
{
    static float[] samplesData;
    const int HEADER_SIZE = 44;
    const string fileName = "Recording";
    const string folderName = "Recordings"; // Will appear inside Project folder that contains the asset folder.

    // Creates a folder at head of project and saves AudioClip as .wav
    public static void SaveRecording(AudioClip audioClip)
    {
        if (!audioClip)
        {
            Debug.Log("Error with given AudioClip in AudioManager.SaveRecording().");
            return;
        }

        DirectoryInfo directoryInfo;
        // Checks if folderName exists at head of this project folder, the one that contains the asset folder
        if (!Directory.Exists(folderName))
        {
            // CreateDirectory will create folders at the head of this project folder, the one that contains the asset folder
            directoryInfo = Directory.CreateDirectory(folderName);
        }
        else
        {
            directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + "TestFolder");
        }

        string filePath = Path.Combine(directoryInfo.ToString(), fileName + " " + DateTime.UtcNow.ToString("yyyy_MM_dd HH_mm_ss_ffff") + ".wav");

        // Delete the file if it exists.
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        try
        {
            WriteWAVFile(audioClip, filePath);
            Debug.Log("File Saved Successfully at " + filePath);
        }
        catch (DirectoryNotFoundException)
        {
            Debug.LogError("Persistent Data Path not found!");
        }
    }

    // WAV file format from http://soundfile.sapp.org/doc/WaveFormat/
    static void WriteWAVFile(AudioClip clip, string filePath)
    {
        float[] clipData = new float[clip.samples];

        //Create the file.
        using (Stream fs = File.Create(filePath))
        {
            int frequency = clip.frequency;
            int numOfChannels = clip.channels;
            int samples = clip.samples;
            fs.Seek(0, SeekOrigin.Begin);

            //Header

            // Chunk ID
            byte[] riff = Encoding.ASCII.GetBytes("RIFF");
            fs.Write(riff, 0, 4);

            // ChunkSize
            byte[] chunkSize = BitConverter.GetBytes((HEADER_SIZE + clipData.Length) - 8);
            fs.Write(chunkSize, 0, 4);

            // Format
            byte[] wave = Encoding.ASCII.GetBytes("WAVE");
            fs.Write(wave, 0, 4);

            // Subchunk1ID
            byte[] fmt = Encoding.ASCII.GetBytes("fmt ");
            fs.Write(fmt, 0, 4);

            // Subchunk1Size
            byte[] subChunk1 = BitConverter.GetBytes(16);
            fs.Write(subChunk1, 0, 4);

            // AudioFormat
            byte[] audioFormat = BitConverter.GetBytes(1);
            fs.Write(audioFormat, 0, 2);

            // NumChannels
            byte[] numChannels = BitConverter.GetBytes(numOfChannels);
            fs.Write(numChannels, 0, 2);

            // SampleRate
            byte[] sampleRate = BitConverter.GetBytes(frequency);
            fs.Write(sampleRate, 0, 4);

            // ByteRate
            byte[] byteRate = BitConverter.GetBytes(frequency * numOfChannels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
            fs.Write(byteRate, 0, 4);

            // BlockAlign
            ushort blockAlign = (ushort)(numOfChannels * 2);
            fs.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            // BitsPerSample
            ushort bps = 16;
            byte[] bitsPerSample = BitConverter.GetBytes(bps);
            fs.Write(bitsPerSample, 0, 2);

            // Subchunk2ID
            byte[] datastring = Encoding.ASCII.GetBytes("data");
            fs.Write(datastring, 0, 4);

            // Subchunk2Size
            byte[] subChunk2 = BitConverter.GetBytes(samples * numOfChannels * 2);
            fs.Write(subChunk2, 0, 4);

            // Data

            clip.GetData(clipData, 0);
            short[] intData = new short[clipData.Length];
            byte[] bytesData = new byte[clipData.Length * 2];

            int convertionFactor = 32767;

            for (int i = 0; i < clipData.Length; i++)
            {
                intData[i] = (short)(clipData[i] * convertionFactor);
                byte[] byteArr = new byte[2];
                byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            fs.Write(bytesData, 0, bytesData.Length);
        }
    }

    public static AudioClip TrimClip(AudioClip audioClip, float length)
    {
        AudioClip trimmedClip;
        int samples = (int)(audioClip.frequency * length);
        float[] data = new float[samples];

        audioClip.GetData(data, 0);
        trimmedClip = AudioClip.Create(audioClip.name, samples, audioClip.channels, audioClip.frequency, false);
        trimmedClip.SetData(data, 0);

        return trimmedClip;
    }
}
