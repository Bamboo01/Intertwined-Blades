using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunController : MonoBehaviour
{
    public float radius = 2.0f;
    public Transform center;
    public float circleSpeed = 10.0f;
    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;

    void Update()
    {
        //transform.RotateAround(center.position, axis, circleSpeed * Time.deltaTime);
        //desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        //Vector3 temp = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * circleSpeed);
        //temp.y = center.position.y;
        //transform.position = temp;
    }
}
