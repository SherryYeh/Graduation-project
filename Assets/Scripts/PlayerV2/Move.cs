using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Role.PlayerV2
{
    ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    /// <summary>
    /// Moving APIs
    /// </summary>
    ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    public class Move
    {
        public Model Model { get; set; }
        Rigidbody2D rb;
        public Move(Model model, Rigidbody2D rb)
        {
            this.Model = model;
            this.rb = rb;
        }
        public void DOMove(Vector2 force)
        {
            rb.velocity = force;
        }
        public void ADOMove(Vector2 force) // DOAddvelocity
        {
            rb.velocity += force;
        }
        public void DOMoveX(float force)
        {
            rb.velocity = new Vector2(force, rb.velocity.y);
        }
        public void DOMoveY(float force)
        {
            rb.velocity = new Vector2(rb.velocity.x, force);
        }
        public void ADOMoveY(float force)
        {
            rb.AddForce(new Vector2(0, force));
        }
        public void DOAddforce(Vector2 force)
        {
            rb.AddForce(force);
        }
        public void DOAddforceImpulse(Vector2 force)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }
        public void DOAddforceX(float force)
        {
            rb.AddForce(new Vector2(force, rb.velocity.y));
        }
        public void DOAddforceY(float force)
        {
            rb.AddForce(new Vector2(rb.velocity.x, force));
        }
        public void StopMoveY()
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        public void DOWallSlide(float value)
        {
            //if (raycast)
            //    DOMoveX(raycast.point.x);

            DOMoveY(-Model.WallSlideForce);
        }
        public void DOJump(float force)
        {
            DOMoveY(force);
        }
        public void DOJumpStop()
        {
            DOMoveY((rb.velocity.y) / 5);
        }
        public void DOMoveXStop()
        {
            DOMoveX(0);
        }
        public void DOSlowdown()
        {
            ADOMove(rb.velocity * -0.05f);
        }
        public void DODash(Vector2 value)
        {
            DOAddforceImpulse(value);
        }
        public void DOStopOnSlope()
        {
            rb.AddForce(Vector2.right * Model.GetLocalScale.x, ForceMode2D.Impulse);
        }
        public void DOIceSlide(float force)
        {
            if (Mathf.Abs(rb.velocity.x) < 30) DOAddforceX(force);
            else DOMove(rb.velocity);
        }
        public Vector2 GetVelocity()
        {
            return rb.velocity;
        }
        public float GetVelocityMag()
        {
            return rb.velocity.magnitude;
        }
    }
    ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    /// <summary>
    /// Horizontal Movement
    /// </summary>
    ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    internal class MoveHandling
    {
        [Header("Components")]
        Move move = null;
        Anime anime = null;

        [Header("MoveState : enum")]
        MoveState moveState;

        public MoveHandling(Move move, Anime anime)
        {
            this.move = move;
            this.anime = anime;
        }
        public void Move(float value, ref MoveState moveState)
        {
            this.moveState = moveState;

            //if (value == 0 && isGrounded && this.moveState!= MoveState.dashing) move.DOStopOnSlope(); //Remove x force on slope when player is standing

            if (move.Model.IsIceGrounded) this.moveState = MoveState.iceGround;
            else this.moveState = MoveState.normalGround;
            switch (moveState)
            {
                case MoveState.isDashing:

                    break;
                case MoveState.onJumpControl:
                    break;
                case MoveState.iceGround:
                    if (value != 0 && move.GetVelocityMag() < 20) move.DOAddforceX(value * 10);
                    else move.DOSlowdown();
                    break;
                default:
                    if (value != 0) move.DOMoveX(value * move.Model.GetSpeedGain);
                    else move.DOMoveXStop();
                    break;
            }
            anime.DOAnimation("movement", Mathf.Abs(value));
            moveState = this.moveState;
        }
        public void Dash(Vector2 value)
        {
            float dashPower = move.Model.GetDashPower;
            move.DODash(new Vector2(dashPower, dashPower) * value);
        }
    }
    ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    /// <summary>
    /// Vertical Movement
    /// </summary>
    ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    internal class JumpHandling
    {
        [Header("Component")]
        Move move = null;
        Anime anime = null;

        [Header("JumpState : enum")]
        JumpState jumpState;

        [Header("Input Value")]
        float horizontalInput;

        [Header("Jump Counters : float")]
        private float jumpTimeCounter = 0;
        private float wallJumpTimeCounter = 0;

        public JumpHandling(Move move, Anime anime)
        {
            this.move = move;
            this.anime = anime;
        }
        public void Jump(float horizontalInput, ref JumpState jumpState)
        {
            this.jumpState = jumpState;
            switch (jumpState)
            {
                case JumpState.preWallSliding:
                    PreWallSliding();
                    break;
                case JumpState.isWallSliding:
                    wallJumpTimeCounter = 0;
                    IsWallSliding();
                    break;
                case JumpState.preWallJumping:
                    PreWallJumping();
                    break;
                case JumpState.isWallJumping:
                    IsWallJumping(horizontalInput);
                    break;
                case JumpState.preJumping:
                    PreJumping();
                    break;
                case JumpState.isJumping:
                    IsJumping();
                    break;
                case JumpState.preFalling:
                    move.DOJumpStop();
                    PreFalling();
                    break;
                case JumpState.isFalling:
                    IsFalling();
                    break;
                case JumpState.preGrounded:
                    PreGrounded();
                    break;
                case JumpState.isGrounded:
                    jumpTimeCounter = 0;
                    wallJumpTimeCounter = 0;
                    break;
                default:
                    break;
            }
            jumpState = this.jumpState;
        }
        void PreWallSliding()
        {
            JumpStating("fall", JumpState.isWallSliding);
            IsWallSliding();
        }
        void IsWallSliding()
        {

            RaycastHit2D raycast = Physics2D.Raycast(
                                    new Vector2(move.Model.GetPosition.x, move.Model.GetPosition.y - 1f),
                                    Vector2.right * horizontalInput,
                                    Mathf.Infinity,
                                    move.Model.GroundLayer);
            Debug.DrawRay(new Vector2(move.Model.GetPosition.x, move.Model.GetPosition.y - 1f),
                                                Vector2.right * horizontalInput,
                                                Color.red,
                                                10f);
            Debug.Log(move.Model.GetPosition);
            Debug.Log(raycast.point);
            move.DOWallSlide(horizontalInput);
            if (move.Model.IsGrounded) JumpStating(JumpState.preGrounded);
            else if (move.Model.IsTouchingFront == false) JumpStating(JumpState.preFalling);
        }
        void PreWallJumping()
        {
            JumpStating("wallJump", JumpState.isWallJumping);
            IsWallJumping(horizontalInput);
        }
        void IsWallJumping(float inputValue)
        {
            if (wallJumpTimeCounter < move.Model.WallJumpTime)
            {
                move.DOMove(new Vector2(move.Model.WallJumpForce.x * -inputValue, move.Model.WallJumpForce.y));
                wallJumpTimeCounter += Time.deltaTime;
            }
            else
            {
                wallJumpTimeCounter = 0;
                JumpStating(JumpState.preFalling);
            }
        }
        void PreJumping()
        {
            JumpStating("jump", JumpState.isJumping);
            IsJumping();

        }
        void IsJumping()
        {
            if (jumpTimeCounter < move.Model.JumpTime)
            {
                move.DOJump(Vector2.up.y * move.Model.JumpForce * move.Model.GetJumpGain);
                jumpTimeCounter += Time.deltaTime;
            }
            else
                JumpStating(JumpState.preFalling);
        }
        void PreFalling()
        {
            JumpStating("fall", JumpState.isFalling);
        }
        void IsFalling()
        {
            if (move.Model.IsTouchingFront)
                JumpStating(JumpState.preWallSliding);
            if (move.Model.IsGrounded)
                JumpStating(JumpState.preGrounded);
        }
        void PreGrounded()
        {
            JumpStating("exit", JumpState.isGrounded);
        }
        void JumpStating(JumpState state)
        {
            jumpState = state;
        }
        void JumpStating(string name, JumpState state)
        {
            anime.DOAnimation(name);
            jumpState = state;
        }
    }
}
