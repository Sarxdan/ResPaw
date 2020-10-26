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
    public bool pickupCheck = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (equipped)
            {
                bow.Fire();
            }

            else if (!equipped)
            {
                Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

                int i = 0;
                while (i < hits.Length)
                {
                    if (hits[i].tag == "Weapon")
                    {
                        
                        equipped = true;
                        hits[i].transform.position = transform.position + new Vector3(.75f,0.5f,0);
                        hits[i].transform.rotation = transform.rotation;
                        hits[i].transform.parent = transform;
                        bow = hits[i].GetComponent<Bow>();
                        return;
                    }

                    i++;
                }
                
            }
        }
    }
}
