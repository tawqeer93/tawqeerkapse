using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    public static int totalClicks = 0;
    public KeyCode click;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(click))
        {
            totalClicks += 1;
        }

        if (totalClicks >= 5)
        {
            Debug.Log("Fail!!");
            totalClicks = 0;
        }
    }
}
