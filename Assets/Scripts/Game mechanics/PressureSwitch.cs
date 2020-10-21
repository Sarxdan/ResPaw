using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Transform doorStop;
    [SerializeField] Transform doorStop_2;
    float doorSpeed = 1f;
    bool doorOpen = false;
    bool doorClosed = true;
    
    Vector3 orignalpos;
    private void Start()
    {
        orignalpos = door.transform.position;
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
            DoorClosed();
        }

    }

    void DoorOpen()
    {
        door.transform.position = Vector3.MoveTowards(door.transform.position, doorStop.position, doorSpeed * Time.deltaTime);
    }
    void DoorClosed()
    {
        door.transform.position = Vector3.MoveTowards(door.transform.position, orignalpos, doorSpeed * Time.deltaTime);
    }
}
