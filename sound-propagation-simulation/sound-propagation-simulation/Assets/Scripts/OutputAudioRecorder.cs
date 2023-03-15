using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OutputAudioRecorder : MonoBehaviour
{
    public const string FILE_EXTENSION = ".wav";
    public const string DEFAULT_FILENAME = "record";

    public bool IsRecording
    {
        get { return recOutput; }
    }

    private int bufferSize;
    private int numBuffers;
    private int outputRate;
    private int headerSize = 44; //default for uncompressed wav
    private String fileName;
    private bool recOutput = false;
    private AudioClip newClip;
    private FileStream fileStream;
    private AudioClip[] audioClips;
    private AudioSource[] audioSources;
    private float timer;
    private bool finished_recording;
    public int currentSlot;
    public String folder;
    public String csv_filename = "dataset.csv";
    public float simulation_speed = 1f;

    // target to follow to add its position to the dataset
    public GameObject target_followed;
    public AudioSource source;
    public float target_z_position;
    float[] tempDataSource;

    void Awake()
    {
        outputRate = AudioSettings.outputSampleRate;
    }

    void Start()
    {
        AudioSettings.GetDSPBufferSize(out bufferSize, out numBuffers);
        finished_recording = true;
        target_z_position = target_followed.transform.position.z;
        string folderName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        string pathString = Path.Combine(Application.dataPath, folder, folderName);
        Directory.CreateDirectory(pathString);
        folder = pathString;
        Debug.Log("Folder created at: " + pathString);
    }

    void Update()
    {
        Time.timeScale = simulation_speed;
        source.pitch = simulation_speed;
        // start recording on r key pressed

        if (!recOutput && finished_recording)
        {
            finished_recording = false;
            // start timer
            timer = Time.time;
            StartRecording(DEFAULT_FILENAME);
        }

        // stop recording after 1 second
        if (recOutput && Time.time - timer > 1 / simulation_speed)
        {
            target_z_position = target_followed.transform.position.z;
            // AppendCSV();
            StopRecording();
            finished_recording = true;
        }
    }

    void AppendCSV()
    {
        // append line to csv file
        string text =
            fileName
            + ","
            + (target_followed.transform.position.z > this.transform.position.z)
            + ","
            + simulation_speed
            + "\n";
        File.AppendAllText(folder + "/" + csv_filename, text);
    }

    public void StartRecording(string recordFileName)
    {
        fileName = Path.GetFileNameWithoutExtension(recordFileName) + Time.time + FILE_EXTENSION;
        if (!recOutput)
        {
            StartWriting(fileName);
            recOutput = true;
            Debug.Log("Recording started");
        }
        else
        {
            Debug.LogError("Recording is in progress already");
        }
    }

    public void StopRecording()
    {
        recOutput = false;
        WriteHeader();
        Debug.Log("Recording stopped, file saved at: " + folder + "/" + fileName);
    }

    private void StartWriting(String name)
    {
        String side = "";
        if (target_followed.transform.position.z > this.transform.position.z)
        {
            side = "/fromright";
        }
        else
        {
            side = "/fromleft";
        }
        if(!Directory.Exists(folder + side))
            Directory.CreateDirectory(folder + side);
        // create the folders if they don't exist

        /* if (target_z_position < target_followed.transform.position.z)
        {
            side += "toleft";
        }
        else
        {
            side += "toright";
        } */
        fileStream = new FileStream(folder + side + "/" + name, FileMode.Create);

        var emptyByte = new byte();
        for (int i = 0; i < headerSize; i++) //preparing the header
        {
            fileStream.WriteByte(emptyByte);
        }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (recOutput)
        {
            ConvertAndWrite(data); //audio data is interlaced
        }
    }

    private void ConvertAndWrite(float[] dataSource)
    {
        var intData = new Int16[dataSource.Length];
        //converting in 2 steps : float[] to Int16[], //then Int16[] to Byte[]

        var bytesData = new Byte[dataSource.Length * 2];
        //bytesData array is twice the size of
        //dataSource array because a float converted in Int16 is 2 bytes.

        var rescaleFactor = 32767; //to convert float to Int16

        for (var i = 0; i < dataSource.Length; i++)
        {
            intData[i] = (Int16)(dataSource[i] * rescaleFactor);
            var byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        fileStream.Write(bytesData, 0, bytesData.Length);

        tempDataSource = new float[dataSource.Length];
        tempDataSource = dataSource;
    }

    private void WriteHeader()
    {
        fileStream.Seek(0, SeekOrigin.Begin);

        var riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        var chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        var wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        var fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        var subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        var audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        var numChannels = BitConverter.GetBytes(two);
        fileStream.Write(numChannels, 0, 2);

        var sampleRate = BitConverter.GetBytes(outputRate);
        fileStream.Write(sampleRate, 0, 4);

        var byteRate = BitConverter.GetBytes(outputRate * 4);

        fileStream.Write(byteRate, 0, 4);

        UInt16 four = 4;
        var blockAlign = BitConverter.GetBytes(four);
        fileStream.Write(blockAlign, 0, 2);

        UInt16 sixteen = 16;
        var bitsPerSample = BitConverter.GetBytes(sixteen);
        fileStream.Write(bitsPerSample, 0, 2);

        var dataString = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(dataString, 0, 4);

        var subChunk2 = BitConverter.GetBytes(fileStream.Length - headerSize);
        fileStream.Write(subChunk2, 0, 4);

        fileStream.Close();
    }
}
