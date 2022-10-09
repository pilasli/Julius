using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Give clouds different speeds to create parallax effect
public class BackgroundLoop : MonoBehaviour
{
    public float speed = 4f;
    private Vector3 startPosition;
    private float repeatWidth;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        repeatWidth = GetComponent<BoxCollider2D>().size.x / 2;        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(translation:Vector3.right * -speed * Time.deltaTime);
        if(transform.position.x < startPosition.x - repeatWidth)
        {
            transform.position = startPosition;
        }       
    }
}
