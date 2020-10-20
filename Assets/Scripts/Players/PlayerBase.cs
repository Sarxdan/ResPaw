﻿//Created by Mehmet & Fares
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
    private bool touchingOtherPlayerFromBelow;
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
    private float mediumFriction = 5f;

    private float maxJumpSpeed = 10f;
    private float minJumpSpeed = 8f;

    private float maxMoveSpeed = 10f;
    private float minMoveSpeed = 5f;

    private PlayerSpawner playerSpawner;

    private PlayerBase playerClass;

    private int belowMeCount = 0;
    [SerializeField]
    private bool isFacingObject;

    [SerializeField]
    private bool isFacingAnotherPlayer;


    public bool killedByPlayer = false;
    public bool killedByRoof = false;

    [SerializeField]
    private float playerFriction = 0f;

    GameManager manager;


    public PlayerBase()
    {
        horizontalAxies = GetHorizontalAxies();
        jumpButton = GetJumpButton();

    }

    private void Start()
    {
        isJumping = true;
        touchingOtherPlayerFromBelow = false;
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
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
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
        return headTouchingPlayer ? rb.velocity.y < 0 : rb.velocity.y < 4;
    }


    private void Jump()
    {

        if (Input.GetButton(jumpButton) && !isJumping)
        {

            isJumping = true;
            isIdle = false;

            StopWalkAnimation();

            var jumpPower = headTouchingPlayer && isTouchingGround ? maxJumpSpeed : minJumpSpeed;

            var currrentVelocity = rb.velocity;
            currrentVelocity.y = jumpPower;

            rb.velocity = currrentVelocity;

        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == (int)LayerEnum.Spike)
        {
            OnDeath();
        }
        if (collision.gameObject.layer == (int)LayerEnum.WallSpike)
        {
            OnDeath(true);
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

    public void OnDeath(bool freezeLocation = false)
    {
        if (enabled)
        {
            if (manager.CanSpawn(this))
            {
                playerSpawner.SpawnPlayer(gameObject);
            }

            RemoveAllEvents();
            StopWalkAnimation();
            anim.enabled = false;

            if (freezeLocation)
                rb.constraints = RigidbodyConstraints.FreezeAll;
            touchingOtherPlayerFromBelow = false;
            isFacingObject = false;
            isFacingAnotherPlayer = false;
            headTouchingPlayer = false;
            isTouchingGround = true;
            isJumping = false;
            manager.RemoveLife(this);
            rb.mass = 3;
            enabled = false;
            //if (isJumping)
            //{
            //    rb.constraints = RigidbodyConstraints.FreezeAll;
            //}
        }
    }

    private void RemoveAllEvents()
    {
        playerFace.GetComponent<PlayerFace>().PlayerIsFacingSomething -= PlayerFacingObject;
        playerRoof.GetComponent<PlayerRoof>().PlayerIsCarryingAnotherPlayer -= TouchingPlayerAbove;
        playerBottom.GetComponent<PlayerBottom>().PlayerIsAboveGround -= touchingGround;
        playerBottom.GetComponent<PlayerBottom>().PlayerIsAbovePlayer -= LegTouchingPlayer;
    }



    void SetPlayerFriction(float friction)
    {
        playerCollider.material.dynamicFriction = friction;
        playerFriction = friction;
    }

    private void PreventSliding()
    {

        rb.velocity = new Vector3(0, 0, 0);


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
            MoveOnGroundOrPlayer(x);
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


            //StartCoroutine(Idling());
        }
    }

    private void MoveOnGroundOrPlayer(float x)
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
            currentVelocity.x = GetPlayerSpeed(true);

            rb.velocity = currentVelocity;
        }
        else if (x > 0)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x = GetPlayerSpeed(false);
            rb.velocity = currentVelocity;
        }
    }

    private bool notMovingAbove(float x)
    {
        return x == 0 && touchingOtherPlayerFromBelow && playerCollider.material.dynamicFriction != maxFriction;
    }

    private bool PlayerMovingAbove(float x)
    {
        return x != 0 && touchingOtherPlayerFromBelow && playerCollider.material.dynamicFriction != normalFriction;
    }

    private float GetPlayerSpeed(bool movingLeft)
    {

        var finalSpeed = 0f;


        if (touchingOtherPlayerFromBelow)
        {
            var speed = Math.Abs(playerBelow.velocity.x) > 1 ? maxMoveSpeed : minMoveSpeed;

            finalSpeed = movingLeft ? speed * -1 : speed;

            finalSpeed *= isFacingAnotherPlayer ? 2 : 1;

            return finalSpeed;


        }

        finalSpeed = movingLeft ? -minMoveSpeed : minMoveSpeed;
        finalSpeed *= isFacingAnotherPlayer ? 2 : 1;
        return finalSpeed;

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
            currentVelocity.x = GetPlayerSpeed(true);
            rb.velocity = currentVelocity;

        }
        else if (x > 0 && !isFacingObject)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x = GetPlayerSpeed(false);
            rb.velocity = currentVelocity;
        }


    }

    private void PlayerFacingObject(object sender, (bool isFacingObject, bool isItAPlayer) values)
    {
        isFacingObject = values.isFacingObject;

        isFacingAnotherPlayer = values.isItAPlayer;


    }

    private void TouchingPlayerAbove(object sender, (Rigidbody rb, bool isCarying) values)
    {
        headTouchingPlayer = values.isCarying;

        if (values.isCarying)
            playerAbove = values.rb;

    }


    private void LegTouchingPlayer(object sender, (Rigidbody rb, bool isAbove) values)
    {
        touchingOtherPlayerFromBelow = values.isAbove;

        if (values.isAbove)
        {
            playerBelow = values.rb;
            PreventSliding();
            SetPlayerFriction(maxFriction);
        }
        else
        {
            SetPlayerFriction(normalFriction);

        }

        jumpCheck();
    }

    private void touchingGround(object sender, bool isGrounded)
    {
        isTouchingGround = isGrounded;
        if (isGrounded)
        {
            PreventSliding();
            SetPlayerFriction(normalFriction);
        }
        jumpCheck();
    }

    private void jumpCheck()
    {
        isJumping = isTouchingGround || touchingOtherPlayerFromBelow ? false : true;

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
