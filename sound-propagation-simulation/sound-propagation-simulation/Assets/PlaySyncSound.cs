using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySyncSound : MonoBehaviour
{
    private bool has_played = false;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= 0.5f && !has_played)
        {
            GetComponent<AudioSource>().Play();
            has_played = true;
        }
    }
}
