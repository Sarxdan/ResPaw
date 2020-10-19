using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer Reviewed by: 
//

public class HorizontalEnemy : MonoBehaviour
{
    public float movementSpeed = 1.5f;
    
    private CharacterController charContr;
    private Vector3 direction = Vector3.forward;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * movementSpeed * direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI Border")
        {
            direction = -direction;
            anim.SetTrigger((direction.z >= 1 ? "TurnLeft" : "TurnRight"));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBase>().OnDeath2();
        }
        
    }
}
