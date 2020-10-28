using Assets.Scripts.Enums;
using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer Reviewed by: Daniel
//

public class HorizontalEnemy : EnemyBase
{
    public float movementSpeed = 2.5f;

    private Vector3 direction = Vector3.forward;
    private Animator anim;

    private float lastTime = 1;
    private float lastPos;
    private float checkSec = .2f;
    private float distanceCheck = .02f;

    private bool rotated = false;    // Used for making the distance check and border rotation not screw with eachother

    void Start()
    {
        anim = GetComponent<Animator>();
        lastPos = transform.position.x;
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * movementSpeed * direction);

        // Check distance moved compare to last check
        // Prevents the agent from getting stuck
        if ((Time.time - lastTime) >= checkSec)
        {
            if ((((transform.position.x - lastPos) * -direction.z) < (distanceCheck * -direction.z)) && !rotated)
            {
                Rotate();
            }
            else if (rotated)
            {
                rotated = false;
            }

            lastPos = transform.position.x;
            lastTime = Time.time;
        }

    }

    private void Rotate()
    {
        direction = -direction;
        anim.SetTrigger((direction.z >= 1 ? "TurnLeft" : "TurnRight"));
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kills the agent and gives it a death effect
        if (other.tag == "Spike" || other.tag == "DoorKill")
        {
            gameObject.layer = (int)LayerEnum.Ground;    // For playerbase movement
            Death();
        }

        else if (other.tag == "AI Border")
        {
            Rotate();
            rotated = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && enabled)
        {
            other.gameObject.GetComponent<PlayerBase>().OnDeath();
        }

    }
    
    public override void Death()
    {
        base.Death();
        // Shrink and center collider to dead enemy
        BoxCollider boxCol = GetComponent<BoxCollider>();
        boxCol.center = new Vector3(boxCol.center.x, boxCol.center.y, 0);
        boxCol.size = new Vector3(boxCol.size.x, boxCol.size.y, 1.055f);
        enabled = false;
    }
}
