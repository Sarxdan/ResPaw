//Created by Mehmet & Fares
using Assets.Scripts.Enums;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerBase : MonoBehaviour
{
    private GameObject playerFace;

    private GameObject playerRoof;

    private GameObject playerBottom;

    private Animator anim;
    [SerializeField]
    private float jumpSpeed = 7f;
    private Quaternion lookLeft;
    private Quaternion lookRight;
    private Rigidbody rb;
    private string horizontalAxies;
    private string jumpButton;
    private bool isIdle = false;
    [SerializeField]
    private bool isJumping = true;

    [SerializeField]
    private float velocityY;
    [SerializeField]
    private bool touchingOtherPlayer;
    [SerializeField]
    private bool headTouchingPlayer;
    private bool isTouchingGround;
    private float fallMultiplier = 4f;
    private float lowJumpMultiplier = 2f;
    private Collider playerCollider;

    private Rigidbody playerAbove;
    private Rigidbody playerBelow;

    private float maxFriction = 500f;
    private float normalFriction = 0.6f;

    private float maxJumpSpeed = 10f;
    private float minJumpSpeed = 8f;

    private float maxMoveSpeed = 10f;
    private float minMoveSpeed = 5f;

    private PlayerSpawner playerSpawner;

    private PlayerBase playerClass;

    private int belowMeCount = 0;
    [SerializeField]
    private bool isFacingObject;


    public bool killedByPlayer = false;
    public bool killedByRoof = false;
    
    
    

    public PlayerBase()
    {
        horizontalAxies = GetHorizontalAxies();
        jumpButton = GetJumpButton();

    }

    private void Awake()
    {
        isJumping = true;
        touchingOtherPlayer = false;
        headTouchingPlayer = false;
        isTouchingGround = false;
        playerFace = gameObject.transform.Find("Face").gameObject;
        playerBottom = gameObject.transform.Find("Bottom").gameObject;
        playerRoof = gameObject.transform.Find("Roof").gameObject;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        playerFace.GetComponent<PlayerFace>().PlayerIsFacingSomething += PlayerFacingObject;
        playerRoof.GetComponent<PlayerRoof>().PlayerIsCarryingAnotherPlayer += TouchingPlayerAbove;
        playerBottom.GetComponent<PlayerBottom>().PlayerIsAboveGround += touchingGround;
        playerBottom.GetComponent<PlayerBottom>().PlayerIsAbovePlayer += LegTouchingPlayer;

    }

    public abstract string GetHorizontalAxies();
    public abstract string GetJumpButton();

    
    void Start()
    {

        //locking the rotation that so we can just replace the current rotation with the new rotations
        lookRight = transform.rotation;
        lookLeft = lookRight * Quaternion.Euler(0, -180, 0);

    }

    void FixedUpdate()
    {
        MoveThePlayer();

        Jump();

        FallingGravity();


    }


    void FallingGravity()
    {
        if (playerFalling())
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 2) * Time.deltaTime;
        }
        else if (jumpingSmall())
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool jumpingSmall()
    {
        return rb.velocity.y > 0 && !Input.GetButton(jumpButton);
    }

    private bool playerFalling()
    {
        return rb.velocity.y < 0;
    }


    private void Jump()
    {

        if (Input.GetButton(jumpButton) && !isJumping)
        {

            isJumping = true;
            isIdle = false;

            StopWalkAnimation();

            anim.SetTrigger("Jump Inplace");

            var jumpPower = headTouchingPlayer && isTouchingGround ? maxJumpSpeed : minJumpSpeed;

            var currrentVelocity = rb.velocity;
            currrentVelocity.y = jumpPower;
            
            rb.velocity = currrentVelocity;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == (int)LayerEnum.Spike)
        {
            OnDeath();
        }
        else if(collision.gameObject.layer == (int)LayerEnum.RoofSpike)
        {
            killedByRoof = true;
            OnDeath();
        }
    }
    

    /*public void OnDeath<T>(T Me) where T : PlayerBase
    {
        if (GetComponent<T>().enabled)
        {
            playerSpawner.SpawnPlayer(gameObject);
            RemoveAllEvents();
            StopWalkAnimation();
            anim.enabled = false;
            //anim.SetTrigger("Death");
            GetComponent<T>().enabled = false;
            //rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            if (isJumping)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
    */
    
    public void OnDeath()
    {
        if (enabled)
        {
            playerSpawner.SpawnPlayer(gameObject);
            RemoveAllEvents();
            StopWalkAnimation();
            anim.enabled = false;
            //anim.SetTrigger("Death");
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            enabled = false;
            if (isJumping)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            if (killedByRoof)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    private void RemoveAllEvents()
    {
        playerFace.GetComponent<PlayerFace>().PlayerIsFacingSomething -= PlayerFacingObject;
        playerRoof.GetComponent<PlayerRoof>().PlayerIsCarryingAnotherPlayer -= TouchingPlayerAbove;
        playerBottom.GetComponent<PlayerBottom>().PlayerIsAboveGround -= touchingGround;
        playerBottom.GetComponent<PlayerBottom>().PlayerIsAbovePlayer -= LegTouchingPlayer;
    }



    void PlayerFriction(float friction)
    {
        playerCollider.material.dynamicFriction = friction;

    }

    private void PreventSliding()
    {

        rb.velocity = new Vector3(0, 0, 0);


    }


    private void IfThereIsNoAnimalsBelowMe()
    {
        if (belowMeCount < 1)
        {
            PlayerFriction(normalFriction);
            touchingOtherPlayer = false;

        }
    }

    private void MoveThePlayer()
    {
        var x = Input.GetAxis(horizontalAxies);


        

        HandleFacing(x);
        


        if (isJumping)
        {
            MoveWhileInAir(x);
        }
        else
        {
            SetFriciton(x);
        }

    }

    private void HandleFacing(float x)
    {
        if (x > 0)
        {
            isIdle = false;
            transform.rotation = lookRight;

            if (!isJumping)
            {
                PlayWalkAnimation();
            }
        }
        else if (x < 0)
        {
            isIdle = false;
            transform.rotation = lookLeft;
            if (!isJumping)
            {
                PlayWalkAnimation();

            }

        }
        else if (!isIdle)
        {
            isIdle = true;
            StopWalkAnimation();

            anim.SetTrigger("Idle");

            //StartCoroutine(Idling());
        }
    }
    
    private void SetFriciton(float x)
    {
        if (PlayerMovingAbove(x))
        {
            PlayerFriction(normalFriction);
        }
        else if (notMovingAbove(x))
        {
            PlayerFriction(maxFriction);
        }
        if (x == 0)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x = 0;
            rb.velocity = currentVelocity;
        }

        if (x < 0)
        {

            var currentVelocity = rb.velocity;
            currentVelocity.x = SpeedAbovePlayer(true);

            rb.velocity = currentVelocity;

        }
        else if (x > 0)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x = SpeedAbovePlayer(false);
            rb.velocity = currentVelocity;
        }
    }

    private bool notMovingAbove(float x)
    {
        return x == 0 && touchingOtherPlayer && playerCollider.material.dynamicFriction != maxFriction;
    }

    private bool PlayerMovingAbove(float x)
    {
        return x != 0 && touchingOtherPlayer && playerCollider.material.dynamicFriction != normalFriction;
    }

    private float SpeedAbovePlayer(bool movingLeft)
    {

        if (touchingOtherPlayer)
        {
            var speed = Math.Abs(playerBelow.velocity.x) > 1 ? maxMoveSpeed : minMoveSpeed;

            return movingLeft ? speed * -1 : speed;
        }

        return movingLeft ? -minMoveSpeed : minMoveSpeed;


    }

    private void MoveWhileInAir(float x)
    {
        if (x == 0)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x = 0;
            rb.velocity = currentVelocity;
        }

        if (x < 0 && !isFacingObject)
        {

            var currentVelocity = rb.velocity;
            currentVelocity.x = SpeedAbovePlayer(true);
            rb.velocity = currentVelocity;

        }
        else if (x > 0 && !isFacingObject)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x = SpeedAbovePlayer(false);
            rb.velocity = currentVelocity;
        }
        
        
    }

    private void PlayerFacingObject(object sender, bool isFacing)
    {
        isFacingObject = isFacing;
    }

    private void TouchingPlayerAbove(object sender, (Rigidbody rb, bool isCarying) values)
    {
        headTouchingPlayer = values.isCarying;

        if (values.isCarying)
            playerAbove = values.rb;

    }


    private void LegTouchingPlayer(object sender, (Rigidbody rb, bool isAbove) values)
    {
        touchingOtherPlayer = values.isAbove;

        if (values.isAbove)
        {
            playerBelow = values.rb;
            PreventSliding();
            PlayerFriction(maxFriction);
        }

        jumpCheck();
    }
    private void touchingGround(object sender, bool isGrounded)
    {
        isTouchingGround = isGrounded;
        if (isGrounded)
        {
            PreventSliding();
            PlayerFriction(normalFriction);
        }
        jumpCheck();
    }

    private void jumpCheck()
    {
        isJumping = isTouchingGround || touchingOtherPlayer ? false : true;
    }

    private void PlayWalkAnimation()
    {
        anim.SetBool("Walk", true);
    }
    private void StopWalkAnimation()
    {
        anim.SetBool("Walk", false);
    }

    public void func() { }

    IEnumerator Idling()
    {
        yield return new WaitForSeconds(3);
        anim.SetTrigger("Idle2");
    }

}
