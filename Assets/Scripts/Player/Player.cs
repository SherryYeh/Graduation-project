using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Role.PlayerSpace
{
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(Animator))]
    public class Player : MonoBehaviour
    {
        GameManager gm;
        [Header("ServerVariables")]
        public int team;
        public bool isCaught;

        [Header("NormalMoving")]
        [SerializeField] Transform frontCheck;
        public Transform FrontCheck { get { return frontCheck; } }
        [SerializeField] Transform groundCheck;
        public Transform GroundCheck { get { return groundCheck; } }
        [SerializeField] float checkRadius;
        public float CheckRadius { get { return checkRadius; } }
        [SerializeField] LayerMask whatIsGround;
        public LayerMask GroundLayer { get { return whatIsGround; } }
        [SerializeField] float speed;
        public float Speed { get { return speed; } set { speed = value; } }
        [SerializeField] float jumpForce;
        public float JumpForce { get { return jumpForce; } }
        [SerializeField] float jumpTime;
        public float JumpTime { get { return jumpTime; } }
        [Header("ObstacleMoving")]
        [SerializeField] LayerMask whatIsWall;
        public LayerMask WallMask { get { return whatIsWall; } }
        [SerializeField] float wallSlidingSpeed;
        public float WallSlidingSpeed { get { return wallSlidingSpeed; } }
        [SerializeField] float xWallForce;
        public float XWallForce { get { return xWallForce; } }
        [SerializeField] float yWallForce;
        public float YWallForce { get { return yWallForce; } }
        [SerializeField] float wallJumpTime;
        public float WallJumpingTime { get { return wallJumpTime; } }
        [SerializeField] float wallJumpDelay;
        public float WallJumpDelay { get { return wallJumpDelay; } }
        [Header("NoneSerializedVariables")]
        float horizontalInput;
        public float HorizontalInput { get { return horizontalInput; } }
        bool isWallJumpDelay;
        public bool IsWallJumpDelay { get { return isWallJumpDelay; } set { isWallJumpDelay = value; } }
        bool isGrounded;
        public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
        bool isJumping;
        public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
        bool isFalling;
        public bool IsFalling { get { return isFalling; } set { isFalling = value; } }
        float jumpTimeCounter;
        public float JumpTimeCounter { get { return jumpTimeCounter; } set { jumpTimeCounter = value; } }
        bool isTouchingFront = false;
        public bool IsTouchingFront { get { return isTouchingFront; } }
        bool isWallSliding = false;
        public bool IsWallSliding { get { return isWallSliding; } set { isWallSliding = value; } }
        bool isWallJumping = false;
        public bool IsWallJumping { get { return isWallJumping; } set { isWallJumping = value; } }
        bool isWallDelayJumping = false;
        public bool IsWallDelayJumping { get { return isWallDelayJumping; } set { isWallDelayJumping = value; } }
        [Header("Physics")]
        Rigidbody2D rb;
        public Rigidbody2D Rb { get { return rb; } }

        [Header("Animation")]
        Animator animator;
        public Animator Animator { get { return animator; } }

        [Header("Scripts")]
        Control control = new Control();
        MoveHandler move;

        public void OnMovement(InputAction.CallbackContext context)
        {
            horizontalInput = context.ReadValue<float>();
            transform.localScale = new Vector2(transform.localScale.x >= 0 ? horizontalInput >= 0 ? 1 : -1 : horizontalInput <= 0 ? -1 : 1, 1);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    if (isWallSliding)
                    {
                        IsWallJumping = true;
                    }
                    if (isWallJumpDelay&& !isWallSliding)
                    {
                        isWallDelayJumping = true;
                        
                    }
                    if (isGrounded)
                    {
                        isJumping = true;
                        jumpTimeCounter = jumpTime;
                    }
                    break;
                case InputActionPhase.Canceled:
                    isJumping = false;
                    isFalling = true;
                    break;
            }
        }

        private void Awake()
        {
            move = GetComponent<MoveHandler>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
            isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius * 2, whatIsWall);
            if (IsWallSliding) isWallJumpDelay = true;
            else if(!IsWallSliding && isWallJumpDelay) Invoke("SetWallJumpDelayToFalse", wallJumpDelay);
            move.MoveHandling();
            move.JumpHandling();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                move.Dash();
            }

            Dector();
        }
        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
        }
        void Dector()
        {
            if (isGrounded && isFalling) isFalling = false;
            if (isGrounded) animator.SetTrigger("exit");
            if (isJumping || isWallJumping) animator.SetTrigger("jump");
            if (isFalling || (isGrounded == false && isJumping == false)) animator.SetTrigger("fall");
        }
        void OnCollisionEnter2D(Collision2D col)
        {
            if (gm.isGameStarted)
            {
                if (col.gameObject.tag == "Player")
                {
                    Player trigger = col.gameObject.GetComponent<Player>();
                    if (trigger.team == 1)
                    {
                        gameObject.SetActive(false);
                        gm.Catch(this);
                    }
                }

            }
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Flag")
            {
                col.gameObject.SetActive(false);
                gm.HumanWin();
            }
            if (col.gameObject.tag == "dashItem")
            {
                Debug.Log("test");
                move.Dash();
            }
        }
        public void SetWallJumpDelayToFalse()
        {
            isWallJumpDelay = false;
            Debug.Log("000");
        }
    }

}