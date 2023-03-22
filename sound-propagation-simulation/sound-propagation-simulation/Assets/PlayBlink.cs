using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBlink : MonoBehaviour
{

    private float t;
    public GameObject sourre_changing_color;
    public AudioSource audioSource; 
    public AudioListener audioListener;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // each 1 second, change the color of the object
        t += Time.deltaTime;
        if (t > 1)
        {
            t = 0;
            sourre_changing_color.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
            // play the sound delayed based on the distance with the listener
            audioSource.PlayDelayed(Vector3.Distance(audioListener.transform.position, transform.position) / 343);
        }
        
    }
}
