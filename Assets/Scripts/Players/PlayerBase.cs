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
    private GameObject playerButtom;

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
    [SerializeField]
    private bool isLegsTouchingOtherPlayer;
    [SerializeField]
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
    [SerializeField]
    private bool isPlayerFacingObject;

    public PlayerBase()
    {
        horizontalAxies = GetHorizontalAxies();
        jumpButton = GetJumpButton();

    }

    private void Awake()
    {
        isJummping = true;
        isLegsTouchingOtherPlayer = false;
        isHeadTouchingOtherPlayer = false;
        isTouchingGround = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        playerFace.GetComponent<PlayerFace>().PlayerIsFacingSomthing += SetIsPlayerFacingObject;
        playerRoof.GetComponent<PlayerRoof>().PlayerIsCarryingAnotherPlayer += SetIsHeadTouchingOtherPlayer;
        playerButtom.GetComponent<PlayerButtom>().PlayerIsAboveGround += SetIsTouchingGround;
        playerButtom.GetComponent<PlayerButtom>().PlayerIsAbovePlayer += SetIsLegsTouchingOtherPlayer;
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
    }

    private void OnDeath<T>(T Me) where T : PlayerBase
    {
        if (GetComponent<T>().enabled)
        {
            playerSpawner.SpawnPlayer(Me.gameObject);
            RemoveAllEvents();
            StopWalkAnimation();
            anim.enabled = false;
            //anim.SetTrigger("Death");
            GetComponent<T>().enabled = false;
        }
    }

    private void RemoveAllEvents()
    {
        playerFace.GetComponent<PlayerFace>().PlayerIsFacingSomthing -= SetIsPlayerFacingObject;
        playerRoof.GetComponent<PlayerRoof>().PlayerIsCarryingAnotherPlayer -= SetIsHeadTouchingOtherPlayer;
        playerButtom.GetComponent<PlayerButtom>().PlayerIsAboveGround -= SetIsTouchingGround;
        playerButtom.GetComponent<PlayerButtom>().PlayerIsAbovePlayer -= SetIsLegsTouchingOtherPlayer;
    }



    void SetPlayerFriction(float friction)
    {
        playerCollider.material.dynamicFriction = friction;

    }

    private void PreventSlidingAfterJumping()
    {

        rb.velocity = new Vector3(0, 0, 0);


    }


    private void IfThereIsNoAnimalsBelowMe()
    {
        if (playerBelowMeCount < 1)
        {
            SetPlayerFriction(normalFriction);
            isLegsTouchingOtherPlayer = false;

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

        if (x < 0 && !isPlayerFacingObject)
        {

            var currentVelocity = rb.velocity;
            currentVelocity.x = PlayerSpeedIfAboveOtherPlayerOrNot(true);
            rb.velocity = currentVelocity;

        }
        else if (x > 0 && !isPlayerFacingObject)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x = PlayerSpeedIfAboveOtherPlayerOrNot(false);
            rb.velocity = currentVelocity;
        }
    }

    private void SetIsPlayerFacingObject(object sender, bool isFacing)
    {
        isPlayerFacingObject = isFacing;
    }

    private void SetIsHeadTouchingOtherPlayer(object sender, (Rigidbody rb, bool isCarying) values)
    {
        isHeadTouchingOtherPlayer = values.isCarying;

        if (values.isCarying)
            theOtherPlayerOboveMe = values.rb;

    }


    private void SetIsLegsTouchingOtherPlayer(object sender, (Rigidbody rb, bool isAbove) values)
    {
        isLegsTouchingOtherPlayer = values.isAbove;

        if (values.isAbove)
        {
            theOtherPlayerBelowMe = values.rb;
            PreventSlidingAfterJumping();
            SetPlayerFriction(maxFriction);
        }

        SetIsjumping();
    }
    private void SetIsTouchingGround(object sender, bool isGrounded)
    {
        isTouchingGround = isGrounded;
        if (isGrounded)
        {
            PreventSlidingAfterJumping();
            SetPlayerFriction(normalFriction);
        }
        SetIsjumping();
    }

    private void SetIsjumping()
    {
        isJummping = isTouchingGround || isLegsTouchingOtherPlayer ? false : true;
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
