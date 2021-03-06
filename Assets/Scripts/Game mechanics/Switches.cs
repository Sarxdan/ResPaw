﻿using UnityEngine;

public class Switches : MonoBehaviour
{
    //created by Daniel
    //peer reviewed by Mehmet
    public GameObject door;
    public Transform doorStop;
    public float doorSpeed;
    bool isTrigger = false;
    bool isOpen;
   
    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {
            isOpen = true;
            isTrigger = true;
        }

    }

    private void Update()
    {
        if(isTrigger == true)
        {
            OpenDoor();
        }
            
    }

    void OpenDoor()
    {
        door.transform.position = Vector3.MoveTowards(door.transform.position, doorStop.position, doorSpeed * Time.deltaTime);
    }

    

}
