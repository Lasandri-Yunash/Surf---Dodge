using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        
    public Vector3 offset;       
    public float followSpeed = 5f; 

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = new Vector3(player.position.x + offset.x,
                                transform.position.y,  
                                transform.position.z);  


        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}

