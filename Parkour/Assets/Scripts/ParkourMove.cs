using UnityEngine;

[CreateAssetMenu(menuName = "Parkour/Create New Parkour Move")] 
public class ParkourMove : ScriptableObject
{
    [SerializeField] string moveName; 
    [SerializeField] float minAllowedHeight; 
    [SerializeField] float maxAllowedHeight; 

    public bool IsAvailable(ObstacleData obstacleData, Transform playerTransform)
    {
        float heightDifference = obstacleData.heightInfo.point.y - playerTransform.position.y;

        if (heightDifference < minAllowedHeight || heightDifference > maxAllowedHeight)
            return false; 

        return true; 
    }

    public string MoveName => moveName;
}