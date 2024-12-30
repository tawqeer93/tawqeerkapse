using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{

    float speed = 40; 
    Animator animator;
    public Transform ball;
    public Transform aimTarget; 

    public Transform[] targets; 

    float force = 13; 
    Vector3 targetPosition; 

    ShotManager shotManager; 


    void Start()
    {
        targetPosition = transform.position; 
        animator = GetComponent<Animator>(); 
        shotManager = GetComponent<ShotManager>(); 
    }

    void Update()
    {
        Move(); 
    }

    void Move()
    {
        targetPosition.x = ball.position.x; 
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime); 
    }

    Vector3 PickTarget() // picks a random target from the targets array to be aimed at
    {
        int randomValue = Random.Range(0, targets.Length); 
        return targets[randomValue].position; 
    }

    Shot PickShot() // picks a random shot to be played
    {
        int randomValue = Random.Range(0, 2); 
        if (randomValue == 0) 
            return shotManager.upSpin;
        else                   
            return shotManager.ground;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // if it collides with the ball
        {
            Shot currentShot = PickShot(); // pick a random shot to be played

            Vector3 dir = PickTarget() - transform.position; 
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitPower + new Vector3(0, currentShot.upPower, 0); 

            Vector3 ballDir = ball.position - transform.position; 
            if (ballDir.x >= 0) // To check to see the correct animation
            {
                animator.Play("forehand"); 
            }
            else
            {
                animator.Play("backhand"); 
            }


        }
    }
}