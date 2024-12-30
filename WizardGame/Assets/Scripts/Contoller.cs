using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contoller : MonoBehaviour
{
    public static string nameObject;
    public GameObject objectText;
    public Transform objectTextPosition;
    public Transform perfectClick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        nameObject = gameObject.name;
        //Debug.Log (nameObject);
        Destroy (gameObject);
        Destroy (objectText);
        Tracking.totalClicks = 0;
        Instantiate (perfectClick, objectTextPosition.position, perfectClick.rotation);
    }
}
