//Created by Mehmet & Fares
using Assets.Scripts.Enums;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerBase : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private float jumpSpeed = 7f;
    private Quaternion lookLeft;
    private Quaternion lookRight;
    private Rigidbody rb;
    private string horizontalAxies;
    private string jumpButton;
    private bool isIdel = false;
    [SerializeField]
    private bool isJummping = true;

    [SerializeField]
    private float velocityY;

    private bool isLegsTouchingOtherPlayer;
    private bool isHeadTouchingOtherPlayer;
    private bool isTouchingGround;
    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;
    private Collider playerCollider;

    private Rigidbody theOtherPlayerOboveMe;
    private Rigidbody theOtherPlayerBelowMe;

    private float maxFriction = 500f;
    private float normalFriction = 0.6f;

    private float maxJumpSpped = 15f;
    private float minJumpSpped = 8f;

    private float maxMoveSpeed = 10f;
    private float minMoveSpeed = 5f;

    private PlayerSpawner playerSpawner;

    private PlayerBase playerClass;

    private int playerBelowMeCount = 0;
    public PlayerBase()
    {
        horizontalAxies = GetHorizontalAxies();
        jumpButton = GetJumpButton();

    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        playerSpawner = FindObjectOfType<PlayerSpawner>();
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
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
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

        if (Input.GetButton(jumpButton) && !isJummping)
        {

            isJummping = true;
            isIdel = false;

            StopWalkAnimation();

            anim.SetTrigger("Jump Inplace");

            var jumpPower = isHeadTouchingOtherPlayer && isTouchingGround ? maxJumpSpped : minJumpSpped;

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


        if (IsTheColliderGroundBelowMe(collision))
        {
            isTouchingGround = true;
            PreventSlidingAfterJumping();
            SetPlayerFriction(normalFriction);

        }
        if (isTheColliderPlayerBelowMe(collision))
        {
            theOtherPlayerBelowMe = collision.gameObject.GetComponent<Rigidbody>();
            playerBelowMeCount++;
            isLegsTouchingOtherPlayer = true;
            PreventSlidingAfterJumping();
            SetPlayerFriction(maxFriction);

        }
        if (IsTheColliderPlayerAboveMe(collision))
        {
            theOtherPlayerOboveMe = collision.gameObject.GetComponent<Rigidbody>();
            isHeadTouchingOtherPlayer = true;

        }

        isJummping = (isTouchingGround || isLegsTouchingOtherPlayer) ? false : true;
    }

    private void OnDeath<T>(T Me) where T : PlayerBase
    {
        if (GetComponent<T>().enabled)
        {
            playerSpawner.SpawnPlayer(Me.gameObject);
            StopWalkAnimation();
            anim.enabled = false;
            //anim.SetTrigger("Death");
            GetComponent<T>().enabled = false;
        }
    }


    private bool IsTheColliderPlayerAboveMe(Collision collision)
    {
        var diffrentBetweenPlayerAndOther = Math.Abs(collision.transform.position.y) - Math.Abs(transform.position.y);
        var isAbove = collision.gameObject.layer == (int)LayerEnum.Player && diffrentBetweenPlayerAndOther > 1 && diffrentBetweenPlayerAndOther < 2.0f;

        return isAbove;
    }

    void SetPlayerFriction(float friction)
    {
        playerCollider.material.dynamicFriction = friction;

    }

    private void PreventSlidingAfterJumping()
    {

        rb.velocity = new Vector3(0, 0, 0);

        //if (theOtherPlayerOboveMe != null)
        //{
        //    theOtherPlayerOboveMe.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //}

    }

    private bool IsTheColliderGroundBelowMe(Collision collision)
    {
        var diffrentBetweenPlayerAndOther = transform.position.y - collision.transform.position.y;

        return (collision.gameObject.layer == (int)LayerEnum.Ground && diffrentBetweenPlayerAndOther >= 0) || transform.position.y < 0;
    }

    private bool isTheColliderPlayerBelowMe(Collision collision)
    {
        var diffrentBetweenPlayerAndOther = transform.position.y - collision.transform.position.y;
        return collision.gameObject.layer == (int)LayerEnum.Player && diffrentBetweenPlayerAndOther >= 1;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == (int)LayerEnum.Ground)
        {
            isTouchingGround = false;

        }
        if (collision.gameObject.layer == (int)LayerEnum.Player)
        {
            playerBelowMeCount--;
            IfThereIsNoAnimalsBelowMe();

        }

        isJummping = isTouchingGround || isLegsTouchingOtherPlayer ? false : true;
    }

    private void IfThereIsNoAnimalsBelowMe()
    {
        if (playerBelowMeCount < 1)
        {
            SetPlayerFriction(normalFriction);
            isLegsTouchingOtherPlayer = false;
            isHeadTouchingOtherPlayer = false;
        }
    }

    private void MoveThePlayer()
    {
        var x = Input.GetAxis(horizontalAxies);

        HandleFacing(x);



        if (isJummping)
        {
            MoveWhileInAir(x);
        }
        else
        {
            MoveWhileInGroundOrPlayer(x);
        }

    }

    private void HandleFacing(float x)
    {
        if (x > 0)
        {
            isIdel = false;
            transform.rotation = lookRight;

            if (!isJummping)
            {
                PlayWalkAnimation();
            }
        }
        else if (x < 0)
        {
            isIdel = false;
            transform.rotation = lookLeft;
            if (!isJummping)
            {
                PlayWalkAnimation();

            }

        }
        else if (!isIdel)
        {
            isIdel = true;
            StopWalkAnimation();

            anim.SetTrigger("Idle");

            //StartCoroutine(Idling());
        }
    }

    private void MoveWhileInGroundOrPlayer(float x)
    {
        if (CheckIfPlayerIsMovingAboveAnotherPlayer(x))
        {
            SetPlayerFriction(normalFriction);
        }
        else if (CheckIfPlayerIsNoteMovingAboveOtherPlayer(x))
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
            currentVelocity.x = PlayerSpeedIfAboveOtherPlayerOrNot(true);

            rb.velocity = currentVelocity;

        }
        else if (x > 0)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x = PlayerSpeedIfAboveOtherPlayerOrNot(false);
            rb.velocity = currentVelocity;
        }
    }

    private bool CheckIfPlayerIsNoteMovingAboveOtherPlayer(float x)
    {
        return x == 0 && isLegsTouchingOtherPlayer && playerCollider.material.dynamicFriction != maxFriction;
    }

    private bool CheckIfPlayerIsMovingAboveAnotherPlayer(float x)
    {
        return x != 0 && isLegsTouchingOtherPlayer && playerCollider.material.dynamicFriction != normalFriction;
    }

    private float PlayerSpeedIfAboveOtherPlayerOrNot(bool movingLeft)
    {

        if (isLegsTouchingOtherPlayer)
        {
            var speed = Math.Abs(theOtherPlayerBelowMe.velocity.x) > 1 ? maxMoveSpeed : minMoveSpeed;

            return movingLeft ? speed * -1 : speed;
        }

        return movingLeft ? -minMoveSpeed : minMoveSpeed;


    }

    private void MoveWhileInAir(float x)
    {

        if (x < 0)
        {

            var currentVelocity = rb.velocity;
            currentVelocity.x = PlayerSpeedIfAboveOtherPlayerOrNot(true);
            rb.velocity = currentVelocity;

        }
        else if (x > 0)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x = PlayerSpeedIfAboveOtherPlayerOrNot(false);
            rb.velocity = currentVelocity;
        }
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
