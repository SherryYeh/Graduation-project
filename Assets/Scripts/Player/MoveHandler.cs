using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Role.PlayerSpace
{
    public class MoveHandler : MonoBehaviour
    {
        [SerializeField]
        public float slowMovePower;
        Player player;
        Control control = new Control();
        public float dashPower;
        bool moving = false;

        void Start()
        {
            player = GetComponent<Player>();
        }
        public void Dash()
        {
            
            if (transform.localScale.x == 1) 
                GetComponent<Rigidbody2D>().AddForce(Vector2.right*dashPower);
            if (transform.localScale.x == -1)
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * dashPower);
        }
        public void MoveHandling()
        {
            bool isTouchingFront = player.IsTouchingFront;
            float force = player.HorizontalInput * player.Speed;
            player.Animator.SetFloat("movement", Mathf.Abs(force));

            if (isTouchingFront && Mathf.Abs(force) > 0)
            {
                if (player.IsGrounded) return;
                player.IsWallSliding = true;
                DoSlide(-Vector2.up * player.WallSlidingSpeed);
            }
            else player.IsWallSliding = false;

            if(force!=0)
            DoMoveX(new Vector2(force, player.Rb.velocity.y));
        }

        public void JumpHandling()
        {
            if (player.IsWallJumping)
            {
                DoWallJumping(new Vector2(player.XWallForce * -player.HorizontalInput, player.YWallForce));
                Invoke("SetWallJumpingToFalse", player.WallJumpingTime);
            }
            else if (player.IsWallDelayJumping)
            {
                DoWallJumping(new Vector2(player.XWallForce * player.HorizontalInput, player.YWallForce));
                Invoke("SetWallDelayJumpingToFalse", player.WallJumpingTime);
                player.SetWallJumpDelayToFalse();
            }
            else if (player.IsJumping)
            {
                if (player.JumpTimeCounter > 0)
                {
                    DoMoveY(Vector2.up * player.JumpForce);
                    player.JumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    player.IsJumping = false;
                    player.IsFalling = true;
                }
            }
        }

        void SetWallJumpingToFalse()
        {
            player.IsWallJumping = false;
        }
        void SetWallDelayJumpingToFalse()
        {
            player.IsWallDelayJumping = false;
        }
        void DoWallJumping(Vector2 force)
        {
            control.DoMove(player.Rb, force);
        }
        void DoMove(Vector2 force) {
            if(player.Rb.velocity.magnitude >30)
                control.SlowDoMove(player.Rb, force, slowMovePower);
            else
                control.DoMove(player.Rb, force);
        }
        void DoSlide(Vector2 force)
        {
            control.DoMove(player.Rb, force);
        }
        void DoMoveX(Vector2 force) {
            if (player.Rb.velocity.magnitude > 30 )
                control.SlowDoMoveX(player.Rb, force,slowMovePower);
            else
                control.DoMoveX(player.Rb, force);
        }
        void DoMoveY(Vector2 force) { control.DoMoveY(player.Rb, force); }
    }
}
