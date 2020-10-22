using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer-reviewed by: 
//


public class Bow : MonoBehaviour
{
    
    public GameObject arrowPrefab;
    private float arrowSpeed = 4;
    private float arrowLifetime = 4.0f;
    private float direction;

    public void Fire()
    {
        direction = (transform.rotation.y > 0) ? 1.0f : -1.0f;
        
        // Create arrow and place it in correct position and rotation
        GameObject arrow = Instantiate(arrowPrefab, transform.position + new Vector3(0,0.5f,0), transform.rotation * Quaternion.Euler(90,0,0));
        arrow.GetComponent<Rigidbody>().velocity = new Vector3(arrowSpeed*direction, 0,0);
        arrow.GetComponent<Arrow>().StartCoroutine("DestroyWhenFinished", arrowLifetime);
    }
}
