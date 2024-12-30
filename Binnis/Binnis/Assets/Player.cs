using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimTarget; 
    float speed = 3f; 
    float force = 13; 

    bool hitting; 

    public Transform ball; 
    Animator animator;

    Vector3 aimTargetInitialPosition; 

    ShotManager shotManager; 
    Shot currentShot; 

    private void Start()
    {
        animator = GetComponent<Animator>(); 
        aimTargetInitialPosition = aimTarget.position; 
        shotManager = GetComponent<ShotManager>(); 
        currentShot = shotManager.upSpin; 
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical"); 

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.upSpin; 
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false; // we let go of the key so we are not hitting anymore and this 
        }                    

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.ground; 
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }



        if (hitting)  // if we are trying to hit the ball
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * 2 * Time.deltaTime); 
        }


        if ((h != 0 || v != 0) && !hitting) // if we want to move and we are not hitting the ball
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime); 
        }



    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // if we collide with the ball 
        {
            Vector3 dir = aimTarget.position - transform.position; 
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitPower + new Vector3(0, currentShot.upPower, 0);

            Vector3 ballDir = ball.position - transform.position; // get the direction of the ball compared to us to know if it is
            if (ballDir.x >= 0)                                   
            {
                animator.Play("forehand");                        
            }
            else                                                  
            {
                animator.Play("backhand");
            }

            aimTarget.position = aimTargetInitialPosition; 

        }
    }


}