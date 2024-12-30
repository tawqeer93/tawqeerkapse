using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health = 3;
    public Image[] healthUI;
    public GameObject blastPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);

            Instantiate(blastPrefab, transform.position, Quaternion.identity);

            health -= 1;

            for (int i = 0; i < healthUI.Length; i++)
            {
                if (i < health)
                {
                    healthUI[i].enabled = true;
                }
                else
                {
                    healthUI[i].enabled = false;
                }
            }

            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy Blast")
        {
            Destroy(collision.gameObject);

            Instantiate(blastPrefab, transform.position, Quaternion.identity);

            health -= 1;

            for (int i = 0; i < healthUI.Length; i++)
            {
                if (i < health)
                {
                    healthUI[i].enabled = true;
                }
                else
                {
                    healthUI[i].enabled = false;
                }
            }

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
