using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;
using UnityEditor.PackageManager.Requests;

//
// Created by: Sandra Andersson
//
// Peer Reviewed by: 
//

public class HorizontalEnemy : MonoBehaviour
{
    public float movementSpeed = 2.5f;

    public float rayLength = 5.0f;
    public RaycastHit[] hits;
    public Vector3 rayDir = new Vector3(1, 0, 0);
    
    private CharacterController charContr;
    private Vector3 direction = Vector3.forward;
    private Animator anim;
    
    private float lastTime = 1;
    private float lastPos;
    private float checkSec = .2f;
    private float distanceCheck = .02f;

    private bool rotated = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        lastPos = transform.position.x;
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * movementSpeed * direction);

        if ((Time.time - lastTime) >= checkSec)
        {
            if ((((transform.position.x - lastPos)* -direction.z) < (distanceCheck * -direction.z)) && !rotated)
            {
                Rotate();
            }
            else if (rotated)
            {
                rotated = false;
            }

            lastPos = transform.position.x;
            lastTime = Time.time;
        }
        
    }

    private void Rotate()
    {
        direction = -direction;
        anim.SetTrigger((direction.z >= 1 ? "TurnLeft" : "TurnRight"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI Border")
        {
            Rotate();
            rotated = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBase>().OnDeath();
        }
        
    }
}
