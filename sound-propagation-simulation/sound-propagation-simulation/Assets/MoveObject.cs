using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public int speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object based on the horizontal and vertical input
        transform.Translate(speed * Input.GetAxis("Horizontal") * Time.deltaTime, 0, speed * Input.GetAxis("Vertical") * Time.deltaTime);
    }
}
