using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleChecker : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.2f, 0); 
    public float forwardRayLength = 0.9f; 
    public float heightRayLength = 6f; 
    public LayerMask obstacleLayer; 

    public ObstacleData CheckObstacle()
    {
        var data = new ObstacleData(); 

        var rayOrigin = transform.position + offset; 

        data.isObstacleAhead = Physics.Raycast(rayOrigin, transform.forward, out data.hitInfo, forwardRayLength, obstacleLayer); 

        Debug.DrawRay(rayOrigin, transform.forward * forwardRayLength, (data.isObstacleAhead) ? Color.red : Color.green); 

        if (data.isObstacleAhead) 
        {
            var heightOrigin = data.hitInfo.point + Vector3.up * heightRayLength; 
            data.isObstacleAbove = Physics.Raycast(heightOrigin, Vector3.down, out data.heightInfo, heightRayLength, obstacleLayer); 

            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, (data.isObstacleAbove) ? Color.blue : Color.green); 
        }

        return data; 
    }
}

public struct ObstacleData
{
    public bool isObstacleAhead; 
    public bool isObstacleAbove; 
    public RaycastHit hitInfo; 
    public RaycastHit heightInfo; 
}
