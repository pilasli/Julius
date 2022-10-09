using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawControl : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform pos1;
    [SerializeField] private Transform pos2;
    private bool turnBack = false;

    // Update is called once per frame
    void Update()
    {
        //Control saw rotation
        transform.Rotate(0, 0, rotateSpeed);

        //Control saw move
        if(transform.position.x <= pos1.position.x)
        {
            turnBack = true;
        }
        else if(transform.position.x >= pos2.position.x)
        {
            turnBack = false;
        }
        if(turnBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos2.position, moveSpeed * Time.deltaTime);
        }
        else{
            transform.position = Vector2.MoveTowards(transform.position, pos1.position, moveSpeed * Time.deltaTime);
        }
    }
}
