using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switches : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Transform doorStop;
    float doorSpeed = 1f;
    bool isTrigger = false;
    bool isOpen;
    private void Start()
    {        
    }
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
            test();
        }
            
    }

    void test()
    {
        door.transform.position = Vector3.MoveTowards(door.transform.position, doorStop.position, doorSpeed * Time.deltaTime);
    }

}
