using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    // The number of samples per frame
    public int samples = 512;

    // The output sample rate
    public int sampleRate = 44100;

    // The output file name
    public string fileName = "output.wav";

    // The list of output data
    private List<float> outputData;

    // The float array of output data
    private float[] outputValues;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the list
        outputData = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the output data for this frame
        float[] data = new float[samples];
        AudioListener.GetOutputData(data, 0);

        // Add the data to the list
        outputData.AddRange(data);
    }

    // Save the wav file when a key is pressed
    void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R)
        {
            // Convert the list to an array
            outputValues = outputData.ToArray();

            // Write the wav file in the dataset folder
            WriteWavFile(Path.Combine("dataset", fileName), outputValues);

            // log the path of the file
            Debug.Log("Wav file saved to: " + Path.GetFullPath(fileName));

            // Clear the list for next recording
            outputData.Clear();
        }
    }

    // Write a wav file from a float array
    void WriteWavFile(string filename, float[] data)
    {
        // Create a file stream
        FileStream fs = new FileStream(filename, FileMode.Create);

        // Write the wav header

        byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fs.Write(riff, 0, 4);

        byte[] chunkSize = BitConverter.GetBytes(fs.Length - 8);
        fs.Write(chunkSize, 0, 4);

        byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fs.Write(wave, 0, 4);

        byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fs.Write(fmt, 0, 4);

        byte[] subChunk1 = BitConverter.GetBytes(16);
        fs.Write(subChunk1, 0, 4);

        ushort two = 2;
        ushort one = 1;
        ushort eight = 8;

        byte[] audioFormat = BitConverter.GetBytes(one);
        fs.Write(audioFormat, 0, 2);

        byte[] numChannels = BitConverter.GetBytes(two);
        fs.Write(numChannels, 0, 2);

        byte[] sampleRateBytes = BitConverter.GetBytes(sampleRate);
        fs.Write(sampleRateBytes, 0, 4);

        byte[] byteRate = BitConverter.GetBytes(sampleRate * two * two / one);
        fs.Write(byteRate, 0, 4);

        ushort blockAlign = (ushort)(two * two / one);
        fs.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        ushort bps = (ushort)(two * two * eight / one);
        byte[] bitsPerSample = BitConverter.GetBytes(bps);
        fs.Write(bitsPerSample, 0, 2);
        // Write the data chunk header

        byte[] dataString = System.Text.Encoding.UTF8.GetBytes("data");
        fs.Write(dataString, 0, 4);

        byte[] subChunk2 = BitConverter.GetBytes(data.Length * two);
        fs.Write(subChunk2, 0, 4);

        // Write the data as short values

        for (int i = 0; i < data.Length; i++)
        {
            float value = data[i];
            short sample = (short)(value * short.MaxValue);
            byte[] bytes = BitConverter.GetBytes(sample);
            fs.Write(bytes, 0, 2);
        }

        // Close the file stream
        fs.Close();
    }
}