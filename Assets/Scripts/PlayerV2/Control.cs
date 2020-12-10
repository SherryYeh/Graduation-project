using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Role.PlayerV2
{
    enum PlayerState { nonState, ghost, execution };
    enum MoveState { onJumpControl, normalGround, iceGround, nonState, isDashing };
    enum JumpState { preWallSliding, isWallSliding, preWallJumping, isWallJumping, preGrounded, isGrounded, preJumping, isJumping, preFalling, isFalling, nonState };
    [RequireComponent(typeof(Rigidbody2D))]
    public class Control : MonoBehaviour
    {
        [Header("State : enum")]
        PlayerState playerState = PlayerState.nonState;
        JumpState jumpState = JumpState.isGrounded;
        MoveState moveState = MoveState.normalGround;
        [Header("Movement : float")]
        float horizontalInput = 0;

        [Header("Model")]
        Model model;
        
        [Header("Components")]
        Rigidbody2D rb = null;
        Animator animator = null;
        Move move = null;
        MoveHandling moveHandling = null;
        JumpHandling jumpHandling = null;
        Anime anime = null;
        Status status = null;

        public void SpeedUp(float value, float sec) { model.SpeedUp(value, sec); }
        public void JumpUp(float value, float sec) { model.JumpUp(value, sec); }
        public void Dash()
        {
            moveState = MoveState.isDashing;
            moveHandling.Dash(new Vector2(transform.localScale.x, 0));
        }
        public void Dash(Vector2 value)
        {
            moveState = MoveState.isDashing;
            moveHandling.Dash(value);
        }
        public void OnMovement(InputAction.CallbackContext context)
        {
            horizontalInput = context.ReadValue<float>();
            transform.localScale = new Vector2(
                transform.localScale.x >= 0 ?
                    horizontalInput >= 0 ? 1 : -1 :
                    horizontalInput <= 0 ? -1 : 1
                , 1);
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    if (JumpStateCompare(JumpState.isWallSliding))
                        JumpStating(JumpState.preWallJumping);
                    else if (model.IsGrounded)
                        JumpStating(JumpState.preJumping);
                    else if (model.LeaveWallTime <= model.WallJumpColdDuration
                        && (JumpStateCompare(JumpState.preFalling)
                        || JumpStateCompare(JumpState.isGrounded)
                        || JumpStateCompare(JumpState.isFalling))
                        )
                    {
                        model.LeaveWallTime += model.WallJumpColdDuration;
                        JumpStating(JumpState.preJumping);
                    }
                    break;
                case InputActionPhase.Canceled:
                    if (JumpStateCompare(JumpState.isWallJumping) == false)
                        JumpStating(JumpState.preFalling);
                    break;
            }
        }
        void Update()
        {
            model.IsGrounded = Physics2D.OverlapCircle(model.GroundCheck.position, model.CheckGroundRadius, model.GroundLayer);
            model.IsIceGrounded = Physics2D.OverlapCircle(model.GroundCheck.position, model.CheckGroundRadius, model.IceGroundLayer);
            model.IsTouchingFront = Physics2D.OverlapCircle(model.FrontCheck.position, model.CheckFrontRadius * Mathf.Abs(horizontalInput), model.WallLayer);
        }
        void FixedUpdate()
        {
            StateControl();
            switch (playerState)
            {
                case PlayerState.execution:
                    break;
                case PlayerState.ghost:
                    break;
                default:
                    Move();
                    Jump();
                    break;
            }
        }
        void StateControl()
        {
            //Jump
            if (jumpState == JumpState.isWallSliding)
                model.leaveWalltTime = 0;
            else
                model.leaveWalltTime += Time.deltaTime;

            //Move
            if (rb.velocity.magnitude > 30)
                moveState = MoveState.isDashing;
            else if (JumpControlMove)
                moveState = MoveState.onJumpControl;
            else if (model.IsIceGrounded)
                moveState = MoveState.iceGround;
            else
                moveState = MoveState.normalGround;
        }
        bool JumpControlMove
        {
            get
            {
                return JumpStateCompare(JumpState.preWallSliding)
                    || JumpStateCompare(JumpState.isWallSliding)
                    || JumpStateCompare(JumpState.isWallJumping)
                    || JumpStateCompare(JumpState.preWallJumping);
            }
        }
        void JumpStating(JumpState state)
        {
            jumpState = state;
        }
        bool JumpStateCompare(JumpState state)
        {
            return jumpState == state;
        }
        void Move()
        {
            moveHandling.Move(horizontalInput * model.WalkSpeed, ref moveState);
        }
        void Jump()
        {
            jumpHandling.Jump(horizontalInput, ref jumpState);
        }
        private void Awake()
        {
            SetComponent();
            Init();
        }
        void SetComponent()
        {
            model = GetComponent<Model>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
        void Init()
        {
            move = new Move(model, rb);
            anime = new Anime(animator);
            moveHandling = new MoveHandling(move, anime);
            jumpHandling = new JumpHandling(move, anime);
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (model.IsGrounded)
                JumpStating(JumpState.preGrounded);
            else if (model.IsTouchingFront)
                JumpStating(JumpState.preWallSliding);
        }
    }

}