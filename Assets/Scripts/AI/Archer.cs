using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer-reviewed by: 
//

public class Archer : EnemyBase
{
    
    // Seconds between each shot
    public float fireRate = 1f;
    public Bow bow;

    private float lastTime;
    private Animator anim;

    void Start()
    {
        bow = GetComponent<Bow>();
        anim = GetComponent<Animator>();
        lastTime = Time.time;
    }

    void Update()
    {
        if ((Time.time - lastTime) >= fireRate)
        {
            anim.SetTrigger("Shoot");
            lastTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Spike" || other.tag == "DoorKill")
        {
            Death();
        }
    }
}
