﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class InteractionManager : MonoBehaviour
{
    public bool occupied = false;
    public bool equipped = false;
    public Bow bow;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (equipped)
            {
                bow.Fire();
            }

            // Pick up weapon if near it
            else if (!equipped)
            {
                // Check if the player is near the bow
                Collider[] hits = Physics.OverlapBox(transform.position + new Vector3(0,0.5f,0), transform.localScale / 2, Quaternion.identity);
                
                for(int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].tag == "Weapon")
                    {
                        
                        equipped = true;
                        // Set transforms for bow
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
