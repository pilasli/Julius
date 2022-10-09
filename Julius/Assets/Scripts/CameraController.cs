using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform camTarget;
    [SerializeField] private float followSpeed = 3f;

    // Update is called once per frame
    // To make the camera follow player
    void Update()
    {
        Vector3 newPos = new Vector3(camTarget.position.x, camTarget.position.y, camTarget.position.z - 10);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
