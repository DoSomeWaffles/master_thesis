using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float start_offset=0f;
    public float end_offset=13f;
    public float speed_multiplier=6.01f;
    private float min,max;

    // Start is called before the first frame update
    void Start()
    {
        min=transform.position.z;
        max=transform.position.z+end_offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time*speed_multiplier,max-min)+min);
    }
}
