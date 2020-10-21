using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;
using UnityEditor;
using UnityEditor.PackageManager.Requests;

//
// Created by: Sandra Andersson
//
// Peer Reviewed by: 
//

public class HorizontalEnemy : MonoBehaviour
{
    public float movementSpeed = 2.5f;

    private CharacterController charContr;
    private Vector3 direction = Vector3.forward;
    private Animator anim;
    
    private float lastTime = 1;
    private float lastPos;
    private float checkSec = .2f;
    private float distanceCheck = .02f;

    private bool rotated = false;    // Used for making the distance check and border rotation not screw with eachother

    void Start()
    {
        anim = GetComponent<Animator>();
        lastPos = transform.position.x;
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * movementSpeed * direction);

        // Check distance moved compare to last check
        // Prevents the agent from getting stuck
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
        // Kill the agent and gives it a death effect
        if (other.tag == "Spike")
        {
            gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/GreyScale");
            Destroy(GameObject.Find("Horizontal Enemy/Root/Body1/Body2/Arm_R/Sword1"));
            GetComponent<Animator>().enabled = false;
            enabled = false;
        }

        else if (other.tag == "AI Border")
        {
            Rotate();
            rotated = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && enabled)
        {
            other.gameObject.GetComponent<PlayerBase>().OnDeath();
        }
        
    }
}
