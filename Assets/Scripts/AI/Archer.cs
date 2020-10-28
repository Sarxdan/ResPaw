using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer-reviewed by: Mehmet
//

public class Archer : EnemyBase
{
    
    // Seconds between each shot
    public float fireRate = 1f;
    
    [SerializeField]
    private Bow bow;
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
    
    public override void Death()
    {
        base.Death();
        enabled = false;

    }
    
}
