using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourSystem : MonoBehaviour
{
    public ObstacleChecker obstacleChecker; 
    bool isPlayerPerformingAction; 
    public Animator playerAnimator; 

    [Header("Parkour Actions")]
    public List<ParkourActionData> parkourActions; 

    private void Update()
    {
        if (Input.GetButton("Jump") && !isPlayerPerformingAction) 
        {
            var obstacleData = obstacleChecker.CheckObstacle(); 

            if (obstacleData.isObstacleAhead) 
            {
                foreach (var action in parkourActions) 
                {
                    if (action.IsAvailable(obstacleData, transform)) 
                    {
                        StartCoroutine(PerformParkourAction(action)); 
                        break;
                    }
                }
            }
        }
    }

    IEnumerator PerformParkourAction(ParkourActionData action)
    {
        isPlayerPerformingAction = true; 

        playerAnimator.CrossFade(action.animationName, 0.2f); 
        yield return null;

        var nextState = playerAnimator.GetNextAnimatorStateInfo(0); 

        if (!nextState.IsName(action.animationName)) 
            Debug.Log("Incorrect Animation Name");

        yield return new WaitForSeconds(nextState.length); 

        isPlayerPerformingAction = false; 
    }
}

[System.Serializable]
public struct ParkourActionData
{
    public string animationName; 

    public bool IsAvailable(ObstacleData obstacleData, Transform playerTransform)
    {
        return true;
    }
}
