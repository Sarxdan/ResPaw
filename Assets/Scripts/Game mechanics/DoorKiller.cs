﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Daniel
//peer reviewed by Mehmet
public class DoorKiller : MonoBehaviour
{   
    [HideInInspector]public bool underDoor;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            underDoor = true;
        }
       
       
    }

    public void OnTriggerExit(Collider other)
    {
        underDoor = false;
    }

   

}
