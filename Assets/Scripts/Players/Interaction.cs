﻿using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer-reviewed by: Mehmet
//

public class Interaction : MonoBehaviour
{
    public float fireRate = 1.0f;

    [SerializeField]
    private PlayerBase playerBase;
    
    private Bow bow;
    private bool equipped = false;
    private float lastFire = 0;

    private void Start()
    {
        playerBase = GetComponent<PlayerBase>();
    }

    void Update()
    {
        if (Input.GetButtonDown(playerBase.actionButton))
        {
            if (equipped && (Time.time - lastFire) >= fireRate)
            {
                bow.Fire();
                lastFire = Time.time;
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
                        hits[i].transform.position = new Vector3(transform.position.x + (.75f*((transform.rotation.y > 0) ? 1.0f : -1.0f)), hits[i].transform.position.y + .5f, 0);
                        hits[i].transform.rotation = transform.rotation;
                        hits[i].transform.parent = transform;
                        
                        bow = hits[i].GetComponent<Bow>();
                        return;
                    }
                }
                
            }
        }
        
        else if(Input.GetButtonDown(playerBase.dropButton) && equipped)
        {
            Drop();
        }
    }

    public void Drop()
    {
        if (equipped)
        {
            equipped = false;
            // Reset transforms for the bow
            bow.transform.position = bow.transform.position - new Vector3(0,0.5f,0);
            bow.transform.rotation = Quaternion.Euler(90, 90, 0);
            bow.transform.parent = null;
        }
        
    }


}
