using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float arrowSpeed = 15f;
    private Rigidbody2D rb;
    [SerializeField] private float destroyTimer = 10f;
    private float destroyTimerCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -(transform.right * arrowSpeed);
        destroyTimerCounter = destroyTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy the arrow when timer reaches 0
        if(destroyTimerCounter >= 0)
        {
            destroyTimerCounter -= Time.deltaTime;
        }
        else if(destroyTimerCounter <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("World"))
        {
            Destroy(this.gameObject);
        }
    }
}
