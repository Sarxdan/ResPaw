using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer-reviewed by: 
//

public class Archer : MonoBehaviour
{
    
    // Seconds between each shot
    private float fireRate = 1f;
    public Bow bow;

    private float lastTime;

    void Start()
    {
        bow = GetComponent<Bow>();
        lastTime = Time.time;
    }

    void Update()
    {
        if ((Time.time - lastTime) >= fireRate)
        {
            lastTime = Time.time;
            bow.Fire();
            
        }
    }
}
