//Created by Mehmet & Fares
using Assets.Scripts.Enums;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerBase : MonoBehaviour
{
    [SerializeField]
    private GameObject playerFace;

    [SerializeField]
    private GameObject playerRoof;

    [SerializeField]
    private GameObject PlayerBottom;

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
    private bool HeadTouchingPlayer;
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

    public PlayerBase()
    {
        horizontalAxies = GetHorizontalAxies();
        jumpButton = GetJumpButton();

    }

    private void Awake()
    {
        isJumping = true;
        touchingOtherPlayer = false;
        HeadTouchingPlayer = false;
        isTouchingGround = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        playerFace.GetComponent<PlayerFace>().PlayerIsFacingSomething += playerFacingObject;
        playerRoof.GetComponent<PlayerRoof>().PlayerIsCarryingAnotherPlayer += SetHeadTouchingPlayer;
        PlayerBottom.GetComponent<playerBottom>().PlayerIsAboveGround += SetIsTouchingGround;
        PlayerBottom.GetComponent<playerBottom>().PlayerIsAbovePlayer += SetLegTouchingPlayer;
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
        if (IsPlayerFalling())
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 2) * Time.deltaTime;
        }
        else if (IsJumpingSmall())
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool IsJumpingSmall()
    {
        return rb.velocity.y > 0 && !Input.GetButton(jumpButton);
    }

    private bool IsPlayerFalling()
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

            var jumpPower = HeadTouchingPlayer && isTouchingGround ? maxJumpSpeed : minJumpSpeed;

            var currrentVelocity = rb.velocity;
            currrentVelocity.y = jumpPower;
            
            rb.velocity = currrentVelocity;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == (int)LayerEnum.Spike)
        {
            OnDeath(this);
        }
    }

    private void OnDeath<T>(T Me) where T : PlayerBase
    {
        if (GetComponent<T>().enabled)
        {
            playerSpawner.SpawnPlayer(gameObject);
            RemoveAllEvents();
            StopWalkAnimation();
            anim.enabled = false;
            //anim.SetTrigger("Death");
            GetComponent<T>().enabled = false;
            rb.mass = 1;
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }

    private void RemoveAllEvents()
    {
        playerFace.GetComponent<PlayerFace>().PlayerIsFacingSomething -= playerFacingObject;
        playerRoof.GetComponent<PlayerRoof>().PlayerIsCarryingAnotherPlayer -= SetHeadTouchingPlayer;
        PlayerBottom.GetComponent<playerBottom>().PlayerIsAboveGround -= SetIsTouchingGround;
        PlayerBottom.GetComponent<playerBottom>().PlayerIsAbovePlayer -= SetLegTouchingPlayer;
    }



    void SetPlayerFriction(float friction)
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
            SetPlayerFriction(normalFriction);
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
            SetPlayerFriction(normalFriction);
        }
        else if (notMovingAbove(x))
        {
            SetPlayerFriction(maxFriction);
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

    private void playerFacingObject(object sender, bool isFacing)
    {
        isFacingObject = isFacing;
    }

    private void SetHeadTouchingPlayer(object sender, (Rigidbody rb, bool isCarying) values)
    {
        HeadTouchingPlayer = values.isCarying;

        if (values.isCarying)
            playerAbove = values.rb;

    }


    private void SetLegTouchingPlayer(object sender, (Rigidbody rb, bool isAbove) values)
    {
        touchingOtherPlayer = values.isAbove;

        if (values.isAbove)
        {
            playerBelow = values.rb;
            PreventSliding();
            SetPlayerFriction(maxFriction);
        }

        SetIsjumping();
    }
    private void SetIsTouchingGround(object sender, bool isGrounded)
    {
        isTouchingGround = isGrounded;
        if (isGrounded)
        {
            PreventSliding();
            SetPlayerFriction(normalFriction);
        }
        SetIsjumping();
    }

    private void SetIsjumping()
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
