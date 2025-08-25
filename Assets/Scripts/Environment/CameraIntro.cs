using UnityEngine;

public class CameraIntro : MonoBehaviour
{
    public Transform targetPosition; 
    public float moveSpeed = 5f;
    public float stopDistance = 0.1f; 
    public OceanManager oceanManager; 

    private bool reachedTarget = false;

    void Update()
    {
        if (!reachedTarget)
        {
            
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition.position,
                moveSpeed * Time.deltaTime
            );

            
            if (Vector3.Distance(transform.position, targetPosition.position) <= stopDistance)
            {
                reachedTarget = true;
                oceanManager.startMoving = true; 
            }
        }
    }

    public void SetTargetY(float newY)
    {
        if (targetPosition != null)
        {
            Vector3 pos = targetPosition.position;
            pos.y = newY;
            targetPosition.position = pos;
            reachedTarget = false; // allow camera to move to new target
        }
    }

}
