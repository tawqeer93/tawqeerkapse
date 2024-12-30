using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    public float movHor = 0f;
    public float speed = 3f;

    public bool isGroundFloor = true;
    public bool isGroundFront = false;

    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public float frontGrndRayDist = 0.25f;
    public float floorCheckY = 0.52f;
    public float frontCheck = 0.51f;
    public float frontDist = 0.001f;

    public int scoreGive = 50;

    public Animator anim;
    private RaycastHit2D hit;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.obj.gamePaused)
        {
            return;
        }

        isGroundFloor = (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - floorCheckY, transform.position.z),
            new Vector3(movHor, 0, 0), frontGrndRayDist, groundLayer));

        
        if (Physics2D.Raycast(transform.position, transform.right * movHor, frontCheck, groundLayer))
        {
            if (movHor == 1)
            {
                movHor = -1;
            }
            else
            {
                movHor = 1;
            }
            transform.localScale = new Vector3(-(movHor), 1, 1);
        }

        //if (Physics2D.Raycast(transform.position, transform.right * movHor, frontCheck, playerLayer))
        //{
        //    Debug.Log("Player");
        //}

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y);    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destory Enemy
            AudioManager.obj.playEnemyHit();
            getKilled();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (movHor == -1)
            {
                movHor = 1;
            }
            else
            {
                movHor = -1;
            }
            transform.localScale = new Vector3(-(movHor), 1, 1);

        }


    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position,
            (transform.right * movHor) * frontCheck);
    }

    private void getKilled()
    {
        FXManager.obj.showPop(transform.position);
        anim.SetTrigger("Death");
        gameObject.SetActive(false);
    }
}
