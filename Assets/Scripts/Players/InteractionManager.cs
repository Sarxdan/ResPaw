using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class InteractionManager : MonoBehaviour
{
    public bool occupied = false;
    public bool equipped = false;
    public Bow bow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && equipped)
        {
            bow.Fire();
        }
    }

    
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon" && Input.GetButtonDown("Interact") && !equipped)
        {
            equipped = true;
            other.transform.position = transform.position + new Vector3(.75f,0.5f,0);
            other.transform.rotation = transform.rotation;
            other.transform.parent = transform;
            bow = other.GetComponent<Bow>();
        }
    }
}
