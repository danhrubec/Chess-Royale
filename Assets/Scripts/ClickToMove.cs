using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{

    private float speed = 10;
    private Vector3 targetPos;
    private bool isMoving = false;
    private Ray ray;
    private RaycastHit hit;

    // Update is called once per frame
    // void Update()
    // {
    //     if(Input.GetMouseButton(0))
    //     {
    //         SetTargetPosition();
    //     }

    //     if(isMoving == true)
    //     {
    //         Move();
    //     }
    // }

    // void SetTargetPosition()
    // {
    //     targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     targetPos.z = 0;

    //     isMoving = true;

    // }


    // void Move()
    // {
    //     //transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPos);
    //     transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    //     if(transform.position == targetPos)
    //     {
    //         isMoving = false;
    //     }

    // }
    
}
