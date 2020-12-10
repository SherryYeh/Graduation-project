using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Role.PlayerSpace
{
    public class PlayerOldVersion : MonoBehaviour
    {
        public int team;

        [SerializeField]
        LayerMask detect;
        [SerializeField]
        GameObject bump;
        [SerializeField]
        float speed;
        [SerializeField]
        float runSpeed;
        [SerializeField]
        float dash;
        [SerializeField]
        float jumpForce;
        [SerializeField]
        Transform frontCheck;
        [SerializeField]
        Transform groundCheck;
        [SerializeField]
        float checkRadius;
        [SerializeField]
        LayerMask whatIsGround;
        [SerializeField]
        LayerMask whatIsWall;
        [SerializeField]
        float wallSlidingSpeed;
        bool isFalling = false;
        bool isJumping = false;
        bool isGrounded = false;
        bool wallSliding = false;
        bool isTouchingFront = false;
        bool fallJumping = false;
        bool wallJumping = false;
        bool skyJumping = false;
        bool isDashing = false;
        [SerializeField]
        float xWallForce;
        [SerializeField]
        float yWallForce;
        [SerializeField]
        float wallJumpTime;
        [SerializeField]
        float slideJumpTime;
        [SerializeField]
        float doubleClickTime;

        ControlOldVersion control = new ControlOldVersion();
        public Rigidbody2D rb;
        Animator animator;
        Vector2 movement;
        float leaveSlidingTime;
        float dashTime;
        float PressATime;
        float PressDTime;

        public void Move(Vector2 force)
        {
            control.DoVelocity(rb, force);
        }
        public void Dash(Vector2 force)
        {
            control.DoDash(rb, force * dash);
        }
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
        void Update()
        {
            HandleMoving();
            if (rb.velocity.y < 0) animator.SetTrigger("fall");
            control.DoFallJump(fallJumping);
        }
        void FixedUpdate()
        {
            if (wallJumping == false) animator.SetFloat("movement", Mathf.Abs(movement.x));
            transform.localScale = new Vector2(transform.localScale.x >= 0 ? movement.x >= 0 ? 1 : -1 : movement.x <= 0 ? -1 : 1, 1);
        }

        void HandleMoving()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

            if (wallJumping == false)
            {
                movement = new Vector2(Input.GetAxisRaw("Horizontal"), rb.velocity.y);
            }
            else Invoke("DelayMoving", 1f);
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {

                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (Time.time - PressATime >= doubleClickTime)
                    {
                        PressATime = Time.time;
                    }
                    else
                    {
                        isDashing = true;
                        Dash(new Vector2(-1, 0) * dash);
                    }
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (Time.time - PressDTime >= doubleClickTime)
                    {
                        PressDTime = Time.time;
                    }
                    else if (Time.time - PressDTime < doubleClickTime)
                    {
                        isDashing = true;
                        Dash(new Vector2(1, 0) * dash);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.LeftControl) && isDashing == false)
                Move(new Vector2(movement.x * runSpeed, movement.y));
            else if (movement.x != 0 && isDashing == false)
                Move(new Vector2(movement.x * speed, movement.y));
            else
                control.SpeedLimit(rb);
            if (isJumping && isGrounded) isJumping = false;
            if (isDashing)
            {
                dashTime += Time.deltaTime;
                if (dashTime >= 0.5)
                {
                    dashTime = 0;
                    isDashing = false;
                }

            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                isJumping = true;
                animator.SetTrigger("jump");
                Move(Vector2.up * jumpForce);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (rb.velocity.y > 0)
                {
                    animator.SetTrigger("jump");
                    Move(Vector2.up * rb.velocity.y * 0.5f);
                }

            }

            else if (fallJumping == false && isGrounded == false && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
            {
                if (Physics2D.OverlapCircle(transform.position, checkRadius, whatIsGround) || Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround))
                {
                    isFalling = false;
                    fallJumping = false;
                    Debug.Log("Touch");
                    return;
                }
                fallJumping = true;
                Move(Vector2.down * wallSlidingSpeed * wallSlidingSpeed * wallSlidingSpeed);
                isFalling = true;
                animator.SetTrigger("fall");
            }

            isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsWall);

            if (isTouchingFront && isGrounded == false && movement.x != 0)
            {
                leaveSlidingTime = 0;
                wallSliding = true;
            }
            else
            {
                leaveSlidingTime += Time.deltaTime;
                wallSliding = false;
            }

            if (wallSliding)
                Move(new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, -wallSlidingSpeed + 1)));

            if (Input.GetKeyDown(KeyCode.Space) && wallSliding)
            {
                wallJumping = true;
                Invoke("SetWallJumpingToFalse", wallJumpTime);
            }
            if (wallJumping)
            {
                Move(new Vector2(xWallForce * -movement.x, yWallForce));
            }
            if (Input.GetKeyDown(KeyCode.Space) && leaveSlidingTime <= slideJumpTime && leaveSlidingTime > 0 && !wallJumping)
            {
                skyJumping = true;
                Invoke("SetSkyJumpingToFalse", wallJumpTime);
            }
            if (skyJumping)
            {
                Move(new Vector2(xWallForce * movement.x, yWallForce));
            }
            if (isGrounded)
            {
                if (isFalling) DoBump();
                fallJumping = false;
                isFalling = false;
                isJumping = false;
                animator.SetTrigger("exit");
            }
        }

        void DelayMoving()
        {
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), rb.velocity.y);
            animator.SetFloat("movement", Mathf.Abs(movement.x));
        }

        void DoBump()
        {
            Instantiate(bump, groundCheck.position, transform.rotation);
        }

        void SetWallJumpingToFalse()
        {
            wallJumping = false;
        }

        void SetSkyJumpingToFalse()
        {
            skyJumping = false;
        }
    }

}