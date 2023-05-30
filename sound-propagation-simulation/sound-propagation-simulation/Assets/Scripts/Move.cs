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
    public float min_random_speed = 0.7f;
    public float max_random_speed = 1.3f;
    public float random_delay = 0.1f;
    public AudioClip[] audio_clips;

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
    private float play_random_delay;
    private AudioClip current_clip;

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
            NewRecording();
        }
        if (finished_pass)
        {
            NewPass();
        }
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(start_point.position, end_point.position, fracJourney);

        if (fracJourney >= 1f)
        {
            StopRecording();
        }
    }

    private void MoveListener(int rec_number)
    {
        //Debug.Log("Moving listener to " + childrens[rec_number].position);
        audio_listener.transform.position = childrens[rec_number].position;
    }

    private void NewPass()
    {
        audio_source.Stop();
        // get every wav file in the folder Assets/Sounds/train_sounds
        // print the number of files
        // set the audio source file to a random one in the list
        audio_source.clip = current_clip;
        // print the name of the file
        Debug.Log(audio_source.clip.name);
        finished_pass = false;
        rec_position_number++;
        // play audio based on the distance between the listener and the source
        float delay = Vector3.Distance(audio_listener.transform.position, transform.position) / 343;
        // make the delay a bit random
        sync_sound.PlayDelayed(delay);
        delay += play_random_delay;
        audio_source.PlayDelayed(delay);
        outputAudioRecorder.StartRecording(
            class_label,
            rec_counter.ToString(),
            rec_position_number.ToString()
        );
    }

    private void NewRecording()
    {
        play_random_delay = Random.Range(-1*random_delay, random_delay);
        startTime = Time.time;
        finished_recording = false;
        Random.Range(0f, 1f);
        rec_counter++;
        // get min and max Z value of the childrens
        float min_z = start_points.Min(t => t.position.z);
        float max_z = start_points.Max(t => t.position.z);

        // choose 2 start points close to each other
        // shuffle point list
        start_points = start_points.OrderBy(x => Random.value).ToArray();
        // get the first point and the closest to the first point
        start_point = start_points[0];

        // array with only the other points
        Transform[] other_points = start_points.Where(x => x != start_point).ToArray();
        // get an array with the points that have a z value close bigger than the first point
        Transform[] bigger_points = other_points
            .Where(x => x.position.z > start_point.position.z)
            .ToArray();
        // get an array with the points that have a z value close smaller than the first point
        Transform[] smaller_points = other_points
            .Where(x => x.position.z < start_point.position.z)
            .ToArray();
        // sort both arrays by z value
        bigger_points = bigger_points.OrderBy(x => x.position.z).ToArray();
        smaller_points = smaller_points.OrderBy(x => x.position.z).ToArray().Reverse().ToArray();
        if (bigger_points.Length == 0)
        {
            end_point = smaller_points[0];
        }
        else if (smaller_points.Length == 0)
        {
            end_point = bigger_points[0];
        }
        else
        {
            // choose the closest point randomly
            if (Random.Range(0f, 1f) < 0.5f)
            {
                end_point = bigger_points[0];
            }
            else
            {
                end_point = smaller_points[0];
            }
        }

        // change rotation of the audio source if the end point is on the left or on the right
        if (end_point.position.z < start_point.position.z)
        {
            audio_source.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            audio_source.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // change randomly the volume of the audio source
        audio_source.volume = Random.Range(0.7f, 1f);
        // change randomly the pitch of the audio source
        audio_source.pitch = Random.Range(0.9f, 1.1f);
        // get a random clip from the folder
        current_clip = audio_clips[Random.Range(0, audio_clips.Length)];
        class_label = "/" + start_point.name + "_" + end_point.name;
        journeyLength = Vector3.Distance(start_point.position, end_point.position);
        // random multiplier for the speed
        float random_multiplier = Random.Range(min_random_speed, max_random_speed);
        speed = journeyLength * speed_multiplier * random_multiplier;
    }

    private void StopRecording()
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
