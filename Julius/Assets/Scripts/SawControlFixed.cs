using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawControlFixed : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1.7f;

    // Update is called once per frame
    void Update()
    {
        // Control saw rotation
        transform.Rotate(0, 0, rotateSpeed);        
    }
}
