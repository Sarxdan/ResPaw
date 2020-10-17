using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
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

            var jumpPower = isHeadTouchingOtherPlayer && isTouchingGround ? 15f : 8f;

            var currrentVelocity = rb.velocity;
            currrentVelocity.y = jumpPower;

            rb.velocity = currrentVelocity;
            Debug.Log(rb.velocity.y);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {



        if (isTheColliderGroundBelowMe(collision))
        {
            isTouchingGround = true;
            PreventSlidingAfterJumping();

        }
        if (isTheColliderPlayerBelowMe(collision))
        {

            isLegsTouchingOtherPlayer = true;
            PreventSlidingAfterJumping();
            SetPlayerDriction(500);
            theOtherPlayerBelowMe = collision.gameObject.GetComponent<Rigidbody>();

        }
        if (IsTheColliderPlayerAboveMe(collision))
        {
            theOtherPlayerOboveMe = collision.gameObject.GetComponent<Rigidbody>();
            isHeadTouchingOtherPlayer = true;
        }
        isJummping = (isTouchingGround || isLegsTouchingOtherPlayer) ? false : true;
    }



    private bool IsTheColliderPlayerAboveMe(Collision collision)
    {
        var diffrentBetweenPlayerAndOther = transform.position.y - collision.transform.position.y;
        return collision.gameObject.layer == (int)LayerEnum.Player && diffrentBetweenPlayerAndOther < 0;
    }

    void SetPlayerDriction(float friction)
    {
        playerCollider.material.dynamicFriction = friction;

    }

    private void PreventSlidingAfterJumping()
    {

        rb.velocity = new Vector3(0, 0, 0);
        if (theOtherPlayerOboveMe != null)
        {
            theOtherPlayerOboveMe.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

    }

    private bool isTheColliderGroundBelowMe(Collision collision)
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
            SetPlayerDriction(0.6f);
            isLegsTouchingOtherPlayer = false;
            isHeadTouchingOtherPlayer = false;
        }

        isJummping = isTouchingGround || isLegsTouchingOtherPlayer ? false : true;
    }

    private void MoveThePlayer()
    {
        Vector3 movement = new Vector3(Input.GetAxis(horizontalAxies), 0f, 0f);
        if (movement.x > 0)
        {
            isIdel = false;
            transform.rotation = lookRight;

            if (!isJummping)
            {
                PlayWalkAnimation();
            }
        }
        else if (movement.x < 0)
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

        var x = Input.GetAxis(horizontalAxies);
        var movmentSpeed = isJummping ? 15f : 30;

        if (isJummping)
        {
            MoveWhileInAir(x, movmentSpeed);

        }
        else
        {
            MoveWhileInGround(x);
        }

    }

    private void MoveWhileInGround(float x)
    {
        if (x != 0 && isLegsTouchingOtherPlayer && playerCollider.material.dynamicFriction != 0.6f)
        {
            SetPlayerDriction(0.6f);
        }
        else if (x == 0 && isLegsTouchingOtherPlayer && playerCollider.material.dynamicFriction != 50f)
        {

            SetPlayerDriction(500f);
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

    private float PlayerSpeedIfAboveOtherPlayerOrNot(bool movingLeft)
    {

        if (isLegsTouchingOtherPlayer)
        {
            var speed = System.Math.Abs(theOtherPlayerBelowMe.velocity.x) < 1 ? 5 : 10;
            Debug.Log(speed);
            Debug.Log(theOtherPlayerBelowMe.velocity.x);
            return movingLeft ? speed * -1 : speed;
        }

        return movingLeft ? -5 : 5;


    }

    private void MoveWhileInAir(float x, float movmentSpeed)
    {
        movmentSpeed = System.Math.Abs(rb.velocity.x) > 5 ? 0.1f : movmentSpeed;
        if (x < 0)
        {

            rb.AddForce(new Vector3(-1f, 0, 0) * movmentSpeed * Time.deltaTime, ForceMode.Impulse);

        }
        else if (x > 0)
        {
            rb.AddForce(new Vector3(1f, 0, 0) * movmentSpeed * Time.deltaTime, ForceMode.Impulse);
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
