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
    private string dragButton;
    private bool isIdle = false;
    [SerializeField]
    private bool isJumping = true;

    [SerializeField]
    private float velocityY;
    [SerializeField]
    private bool touchingOtherPlayerFromBelow;
    [SerializeField]
    private bool headTouchingPlayer;
    [SerializeField]
    private bool isTouchingGround;
    private float fallMultiplier = 4f;
    private float lowJumpMultiplier = 2f;
    private Collider playerCollider;
    private FixedJoint fixedJoint;

    private Rigidbody playerAbove;
    private Rigidbody playerBelow;
    private Rigidbody playerFacing;

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
    public bool isDead = false;

    [SerializeField]
    private float playerFriction = 0f;

    Shader transParent;

    GameManager manager;
    [SerializeField]
    AudioClip[] animalClips;
    AudioSource animalSource;

    [SerializeField]
    private bool isDragging;

    public PlayerBase()
    {
        horizontalAxies = GetHorizontalAxies();
        jumpButton = GetJumpButton();
        dragButton = GetDragButton();
    }

    public abstract string GetHorizontalAxies();
    public abstract string GetJumpButton();
    public abstract string GetDragButton();
    public abstract Vector3 GetPosition();


    void Start()
    {

        touchingOtherPlayerFromBelow = false;
        headTouchingPlayer = false;
        isTouchingGround = false;
        isFacingAnotherPlayer = false;
        isFacingObject = false;
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
        transParent = Shader.Find("Unlit/GreyScale");
        SetPlayerFriction(normalFriction);
        lookRight = transform.rotation;
        lookLeft = lookRight * Quaternion.Euler(0, -180, 0);
        rb.velocity = new Vector3(0, 0, 0);
        animalSource = GetComponentInChildren<AudioSource>();
        animalClips = Resources.LoadAll<AudioClip>("Audio/Character");
    }

    void FixedUpdate()
    {
        MoveThePlayer();

        Jump();

        FallingGravity();
        if (!isDead)
            DragPlayer();
    }

    private void DragPlayer()
    {
        if (Input.GetButton(dragButton) && isFacingAnotherPlayer && !isJumping)
        {
            isDragging = true;
            JoinOtherPlayerToDrag();
        }
        if (!Input.GetButton(dragButton))
        {
            RemoveDragging();
        }
    }

    private void RemoveDragging()
    {
        isDragging = false;
        if (fixedJoint != null)
            Destroy(fixedJoint);
    }

    private void JoinOtherPlayerToDrag()
    {
        if (fixedJoint == null)
        {
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.breakForce = 2f;
            fixedJoint.enableCollision = true;
        }


        fixedJoint.connectedBody = playerFacing;
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


    //Peer-Reviewed By Daniel
    private void Jump()
    {

        if (Input.GetButton(jumpButton) && !isJumping && !isDead)
        {

            isIdle = false;

            StopWalkAnimation();
            RemoveDragging();

            var jumpPower = headTouchingPlayer && isTouchingGround ? maxJumpSpeed : minJumpSpeed;

            var currrentVelocity = rb.velocity;
            currrentVelocity.y = jumpPower;


            if ((isFacingAnotherPlayer || isFacingObject) && Math.Abs(rb.velocity.x) > 0)
            {
                currrentVelocity.x = 0;
            }


            rb.velocity = currrentVelocity;
            animalSource.clip = animalClips[1];
            animalSource.Play();
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

        if (!isDead)
        {

            if (manager.CanSpawn(this))
            {
                StartCoroutine(Delay());
                StartCoroutine(PlaySpawnSound());
                playerSpawner.SpawnPlayer(gameObject);
            }
            RemoveDragging();
            GetComponentInChildren<Renderer>().material.shader = transParent;
            isDead = true;
            animalSource.clip = animalClips[3];
            animalSource.Play();
            StopWalkAnimation();
            anim.enabled = false;

            if (freezeLocation)
                rb.constraints = RigidbodyConstraints.FreezeAll;

            manager.RemoveLife(this);

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

        x = isDead ? 0 : x;
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
        if (x > 0 && !isDragging)
        {
            isIdle = false;
            transform.rotation = lookRight;

            if (!isJumping)
            {
                PlayWalkAnimation();
            }
        }
        else if (x < 0 && !isDragging)
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

    private void PlayerFacingObject(object sender, (Rigidbody OtherPlayerRB, bool isFacingObject, bool isItAPlayer) values)
    {
        isFacingObject = values.isFacingObject;

        isFacingAnotherPlayer = values.isItAPlayer;

        if (values.OtherPlayerRB != null)
            playerFacing = values.OtherPlayerRB;


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
        if (isJumping && isDragging)
        {
            RemoveDragging();
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
    IEnumerator PlaySpawnSound()
    {
        yield return new WaitForSeconds(animalClips[3].length);
        animalSource.clip = animalClips[2];
        animalSource.Play();
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(animalClips[3].length);
    }

}
