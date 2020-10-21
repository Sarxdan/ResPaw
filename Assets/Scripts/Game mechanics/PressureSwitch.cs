﻿using Packages.Rider.Editor.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Daniel
//peer reviewed by Mehmet
public class PressureSwitch : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Transform doorStop;
    [SerializeField] GameObject doorKiller;
    
    float doorSpeed = 1f;
    public bool doorOpen = false;
    public bool doorClosed = true;
      
    Vector3 orignalpos;
    private void Start()
    {
        orignalpos = door.transform.position;
        doorKiller = GameObject.FindGameObjectWithTag("DoorKill"); //finds the gameobject with the tag DoorKill to get its trigger
    }
    private void OnTriggerEnter(Collider other)
    {
        if (doorClosed == true)
        {

            doorOpen = true;
            doorClosed = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(doorOpen == true)
        {
            doorOpen = false;
            doorClosed = true;
        }
    }

    private void Update()
    {           
        if (doorOpen == true)
        {
            DoorOpen(); 
        }
        
        if(doorClosed == true)
        {
            if (doorKiller.GetComponent<DoorKiller>().underDoor) //checks if the trigger on the door is true or false
            {              
                DoorStop();
            }

            if (!doorKiller.GetComponent<DoorKiller>().underDoor)
            {
                DoorClosed();                
            }            
        }

    }
   
    void DoorOpen() // makes the door move towards a location giving it a smooth look.
    {
        door.transform.position = Vector3.MoveTowards(door.transform.position, doorStop.position, doorSpeed * Time.deltaTime);
        Invoke("TriggerActive", 0.5f);        
    }
    void DoorClosed()
    {      
        door.transform.position = Vector3.MoveTowards(door.transform.position, orignalpos, 1 * Time.deltaTime);
        doorKiller.SetActive(true);
    }
    private void DoorStop()
    {
        door.transform.position = Vector3.MoveTowards(door.transform.position, door.transform.position, 1 * Time.deltaTime);
    }

    private void TriggerActive()
    {
        doorKiller.SetActive(false);
    }




}
