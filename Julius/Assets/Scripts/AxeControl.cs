using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeControl : MonoBehaviour
{
    [SerializeField] private AudioSource swingSound;
    [SerializeField] private float rotateSpeed = 70f;
    [SerializeField] private float leftPushRange = -0.35f;
    [SerializeField] private float rightPushRange = 0.35f;
    [SerializeField] private float startTime = 0f;
    [SerializeField] private bool movingClockwise = false;
    private bool canRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartTime");
    }

    // Update is called once per frame
    void Update()
    {
        //Check and rotate axe
        if(!movingClockwise && canRotate)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
        if(movingClockwise && canRotate)
        {
            transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
        }

        //Axe waits a little bit when reaches edges
        if(transform.rotation.z >= rightPushRange && transform.rotation.z > 0)
        {
            movingClockwise = true;
            transform.Rotate(0, 0, 0);
            StartCoroutine("HangTime");
            StartCoroutine("SwingSoundDelayRoutine");
        }
        if(transform.rotation.z <= leftPushRange && transform.rotation.z < 0)
        {
            movingClockwise = false;
            transform.Rotate(0, 0, 0);
            StartCoroutine("HangTime");
            StartCoroutine("SwingSoundDelayRoutine");
        }
    }

    IEnumerator HangTime()
    {
        canRotate = false;
        yield return new WaitForSeconds(0.15f);
        canRotate = true;
    }

    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(startTime);
        canRotate = true;
    }

    IEnumerator SwingSoundDelayRoutine()
    {
        yield return new WaitForSeconds(0.9f);
        swingSound.Play();
    }
}
