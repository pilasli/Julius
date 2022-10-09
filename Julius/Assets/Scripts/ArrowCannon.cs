using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCannon : MonoBehaviour
{
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject arrowPrefab;
    private float timeBetween;
    [SerializeField] private float startTimeBetween = 3f;
    [SerializeField] private AudioSource cannonSound;

    // Update is called once per frame
    void Update()
    {
        //Create arrow with cooldown
        if(timeBetween <= 0)
        {
            Instantiate(arrowPrefab, firepoint.position, firepoint.rotation);
            cannonSound.Play();
            timeBetween = startTimeBetween;
        }
        else
        {
            timeBetween -= Time.deltaTime;
        }        
    }
}
