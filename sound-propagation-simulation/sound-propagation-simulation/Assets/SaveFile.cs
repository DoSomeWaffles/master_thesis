using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // write "test" to test.txt file
        System.IO.File.WriteAllText("../dataset/test.txt", "test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
