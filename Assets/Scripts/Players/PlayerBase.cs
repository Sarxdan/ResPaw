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
    private GameObject playerDrag;

    private Animator anim;
    [SerializeField]
    private Quaternion lookLeft;
    private Quaternion lookRight;
    private Rigidbody rb;
    private string horizontalAxies;
    private string jumpButton;
    public string actionButton;
    public string dropButton;
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
    private FixedJoint fixedJointToDrag;
    private FixedJoint fixedJointToPlayerBelow;

    private Rigidbody playerAbove;
    private Rigidbody playerBelow;
    private Rigidbody playerFacing;




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


    [SerializeField]
    public bool isDead = false;




    Shader transParent;
    Shader normalShader;

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
        actionButton = GetActionButton();
        dropButton = GetDropButton();
    }

    public abstract string GetHorizontalAxies();
    public abstract string GetJumpButton();
    public abstract string GetActionButton();
    public abstract string GetDropButton();


    void Start()
    {
        isDead = false;
        touchingOtherPlayerFromBelow = false;
        headTouchingPlayer = false;
        isTouchingGround = false;
        isFacingAnotherPlayer = false;
        isFacingObject = false;
        isJumping = true;
        playerFace = gameObject.transform.Find("Face").gameObject;
        playerBottom = gameObject.transform.Find("Bottom").gameObject;
        playerRoof = gameObject.transform.Find("Roof").gameObject;
        playerDrag = gameObject.transform.Find("Drag").gameObject;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        playerSpawner = FindObjectOfType<PlayerSpawner>();
        playerFace.GetComponent<PlayerFace>().PlayerIsFacingPlayer += PlayerFacingPlayer;
        playerFace.GetComponent<PlayerFace>().PlayerIsFacingObject += PlayerFacingObject;
        playerDrag.GetComponent<PlayerDrag>().PlayerIsFacingPlayer += PlayerFacingPlayerToDrag;
        playerRoof.GetComponent<PlayerRoof>().PlayerIsCarryingAnotherPlayer += TouchingPlayerAbove;
        playerBottom.GetComponent<PlayerBottom>().PlayerIsAboveGround += touchingGround;
        playerBottom.GetComponent<PlayerBottom>().PlayerIsAbovePlayer += LegTouchingPlayer;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        transParent = Shader.Find("Unlit/GreyScale");
        normalShader = Shader.Find("Unlit/Double-Sided");

        lookRight = transform.rotation;
        lookLeft = lookRight * Quaternion.Euler(0, -180, 0);
        rb.velocity = new Vector3(0, 0, 0);
        animalSource = GetComponentInChildren<AudioSource>();
        animalClips = Resources.LoadAll<AudioClip>("Audio/Character");
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        GetComponentInChildren<Renderer>().material.shader = normalShader;
        anim.enabled = true;
    }

    void Update()
    {
        MoveThePlayer();

        Jump();

        FallingGravity();
        if (!isDead)
            DragPlayer();
    }

    private void DragPlayer()
    {
        if (Input.GetButton(actionButton) && isFacingAnotherPlayer && !isJumping)
        {
            isDragging = true;
            JoinOtherPlayerToDrag();
            PlayWalkAnimation();
        }
        if (!Input.GetButton(actionButton))
        {
            RemoveDragging();
        }
    }

    private void RemoveDragging()
    {
        isDragging = false;
        if (fixedJointToDrag != null)
            Destroy(fixedJointToDrag);
    }

    private void AttachToBelowPlayer()
    {
        if (fixedJointToPlayerBelow == null && touchingOtherPlayerFromBelow && !isDead)
        {
            fixedJointToPlayerBelow = gameObject.AddComponent<FixedJoint>();
            fixedJointToPlayerBelow.breakForce = 2f;
            fixedJointToPlayerBelow.enableCollision = true;


            fixedJointToPlayerBelow.connectedBody = playerBelow;
        }
    }

    private void RemoveAttachedToBelowPlayer()
    {
        if (fixedJointToPlayerBelow != null)
        {

            fixedJointToPlayerBelow.connectedBody = null;
            Destroy(fixedJointToPlayerBelow);
        }
    }

    private void JoinOtherPlayerToDrag()
    {
        if (fixedJointToDrag == null)
        {
            fixedJointToDrag = gameObject.AddComponent<FixedJoint>();
            fixedJointToDrag.breakForce = 2f;
            fixedJointToDrag.enableCollision = true;
        }


        fixedJointToDrag.connectedBody = playerFacing;
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
            RemoveAttachedToBelowPlayer();

            var jumpPower = headTouchingPlayer && isTouchingGround ? maxJumpSpeed : minJumpSpeed;

            var currrentVelocity = rb.velocity;

            currrentVelocity.y = jumpPower;


            if ((isFacingAnotherPlayer || isFacingObject) && Math.Abs(rb.velocity.x) > 0)
            {
                currrentVelocity.x = 0;
            }


            rb.velocity = currrentVelocity;


            if (!animalSource.isPlaying)
            {
                animalSource.clip = animalClips[1];
                animalSource.Play();
            }
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



    public void OnDeath(bool freezeLocation = false)
    {
        StartCoroutine(PlaySpawnSound(freezeLocation));
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

        if (x == 0)
        {
            if (fixedJointToPlayerBelow == null && touchingOtherPlayerFromBelow)
            {
                AttachToBelowPlayer();

            }

            var currentVelocity = rb.velocity;
            currentVelocity.x = 0;
            rb.velocity = currentVelocity;
        }

        if (x < 0)
        {
            RemoveAttachedToBelowPlayer();
            var currentVelocity = rb.velocity;
            currentVelocity.x = GetPlayerSpeed(true);

            rb.velocity = currentVelocity;
        }
        else if (x > 0)
        {
            RemoveAttachedToBelowPlayer();
            var currentVelocity = rb.velocity;
            currentVelocity.x = GetPlayerSpeed(false);
            rb.velocity = currentVelocity;
        }
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
    private void PlayerFacingPlayerToDrag(object sender, (Rigidbody OtherPlayerRB, bool isFacingPlayer) values)
    {
        isFacingAnotherPlayer = values.isFacingPlayer;
        playerFacing = values.OtherPlayerRB;
    }

    private void PlayerFacingPlayer(object sender, (Rigidbody OtherPlayerRB, bool isFacingPlayer) values)
    {
        if (IsFacingTheSamePlayerOboveMe(playerAbove, values.OtherPlayerRB))
        {

            return;
        }
        if (values.isFacingPlayer)
        {
            isFacingObject = true;
        }
        else
        {
            isFacingObject = isFacingObject || values.isFacingPlayer;
        }

    }
    private void PlayerFacingObject(object sender, bool isFacingOtherObject)
    {

        isFacingObject = isFacingOtherObject;
    }

    private bool IsFacingTheSamePlayerOboveMe(Rigidbody otherPlayerAbove, Rigidbody otherPlayerFacing)
    {
        if (otherPlayerAbove == null)
            return false;

        if (otherPlayerFacing == null)
            return false;

        return otherPlayerAbove.name == otherPlayerFacing.name;
    }

    private void TouchingPlayerAbove(object sender, (Rigidbody otherPlayerAbove, bool isCarying) values)
    {
        if (IsFacingTheSamePlayerOboveMe(values.otherPlayerAbove, playerFacing))
        {
            return;
        }

        headTouchingPlayer = values.isCarying;

        if (values.isCarying)
            playerAbove = values.otherPlayerAbove;
        else
            playerAbove = null;

    }


    private void LegTouchingPlayer(object sender, (Rigidbody rb, bool isAbove) values)
    {
        touchingOtherPlayerFromBelow = values.isAbove;

        if (values.isAbove)
        {
            playerBelow = values.rb;
            PreventSliding();

            AttachToBelowPlayer();
        }
        else
        {
            RemoveAttachedToBelowPlayer();


        }

        jumpCheck();
    }

    private void touchingGround(object sender, bool isGrounded)
    {
        isTouchingGround = isGrounded;
        if (isGrounded)
        {
            PreventSliding();

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
        if (isJumping && touchingOtherPlayerFromBelow)
        {
            RemoveAttachedToBelowPlayer();
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
    IEnumerator PlaySpawnSound(bool freezeLocation)
    {
        if (!isDead)
        {
            isDead = true;
            GetComponent<Interaction>().Drop();
            animalSource.clip = animalClips[3];
            animalSource.Play();
            GetComponentInChildren<Renderer>().material.shader = transParent;
            anim.enabled = false;
            if (freezeLocation)
                rb.constraints = RigidbodyConstraints.FreezeAll;

            yield return new WaitForSeconds(animalClips[3].length);
            animalSource.clip = animalClips[2];
            animalSource.Play();


            if (manager.CanSpawn(this))
            {
                playerSpawner.SpawnPlayer(gameObject);
            }



            RemoveDragging();



            StopWalkAnimation();



            manager.RemoveLife(this);


        }

    }


}
