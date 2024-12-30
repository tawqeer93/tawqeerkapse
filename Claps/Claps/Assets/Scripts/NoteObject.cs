using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool isPressable;
    public KeyCode inputKey;
    public AudioSource source;
    public AudioLoudnessDetection detector;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public GameObject hitFX, goodFX, perfectFX, missFX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inputKey))
        {
            if (isPressable)
            {
                gameObject.SetActive(false);

                if (Mathf.Abs(transform.position.y) > 0.25f)
                {
                    Debug.Log("Regular Hit");
                    GameManager.instance.RegularHit();
                    Instantiate(hitFX, transform.position, hitFX.transform.rotation);
                }
                else if (Mathf.Abs(transform.position.y) > 0.05f)
                {
                    Debug.Log("Good Hit");
                    GameManager.instance.GoodHit();
                    Instantiate(goodFX, transform.position, goodFX.transform.rotation);
                }
                else
                {
                    Debug.Log("Perfect Hit");
                    GameManager.instance.PerfectHit();
                    Instantiate(perfectFX, transform.position, perfectFX.transform.rotation);
                }
            }
        }

        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness < threshold)
            loudness = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            isPressable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            isPressable = false;
            GameManager.instance.NoteMissed();
            Instantiate(missFX, transform.position, missFX.transform.rotation);
        }
    }

}
