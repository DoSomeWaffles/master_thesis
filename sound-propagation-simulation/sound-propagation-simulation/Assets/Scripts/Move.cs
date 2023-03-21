using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Move : MonoBehaviour
{
    public float speed_multiplier = 6.01f;
    public GameObject start_points_folder;
    public GameObject audio_listener;
    public GameObject audio_source_folder;
    public AudioSource audio_source;
    public OutputAudioRecorder outputAudioRecorder;
    public AudioSource sync_sound;

    private Transform[] start_points;
    private bool finished_recording = true;
    private float startTime;
    private float journeyLength;
    private float speed;
    private bool finished_pass = true;
    private int rec_position_number = 0;
    private Transform[] childrens;
    private string class_label;
    private int rec_counter = 0;
    private Transform start_point;
    private Transform end_point;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        speed = journeyLength * speed_multiplier;
        childrens = audio_source_folder.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        start_points = start_points_folder.GetComponentsInChildren<Transform>().Skip(1).ToArray();
    }

    void Update()
    {
        if (finished_recording)
        {
            startTime = Time.time;
            finished_recording = false;
            float random = Random.Range(0f, 1f);
            rec_counter++;
            // get min and max Z value of the childrens
            float min_z = start_points.Min(t => t.position.z);
            float max_z = start_points.Max(t => t.position.z);

            // choose 2 start points randomly
            start_point = start_points[Random.Range(0, start_points.Length)];
            end_point = start_points.Where(x => x != start_point).ToArray()[Random.Range(0, start_points.Length-1)];

            class_label = "/"+start_point.name + "_" + end_point.name;
            journeyLength = Vector3.Distance(start_point.position, end_point.position);
            speed = journeyLength * speed_multiplier;
        }
        if (finished_pass)
        {
            finished_pass = false;
            rec_position_number++;
            sync_sound.Play();
            audio_source.Play();
            outputAudioRecorder.StartRecording(
                class_label,
                rec_counter.ToString(),
                rec_position_number.ToString()
            );
        }
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(start_point.position, end_point.position, fracJourney);

        if (fracJourney >= 1f)
        {
            outputAudioRecorder.StopRecording();
            audio_source.Stop();
            finished_pass = true;
            if (rec_position_number >= childrens.Length)
            {
                finished_recording = true;
                rec_position_number = 0;
            }
            MoveListener(rec_position_number);
            startTime = Time.time;
        }
    }

    void MoveListener(int rec_number)
    {
        Debug.Log("Moving listener to " + childrens[rec_number].position);
        audio_listener.transform.position = childrens[rec_number].position;
    }
}
