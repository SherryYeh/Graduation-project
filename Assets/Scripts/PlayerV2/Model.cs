using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Role.PlayerV2
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Model : MonoBehaviour
    {
        [Header("ServerVariables")]
        public int team;
        public bool isCaught;
        public float health;

        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// State Check
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        [Header("Detection : bool")]
        bool isGrounded = false;
        public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
        bool isTouchingFront = false;
        public bool IsTouchingFront { get { return isTouchingFront; } set { isTouchingFront = value; } }
        bool isIceGrounded = false;
        public bool IsIceGrounded { get { return isIceGrounded; } set { isIceGrounded = value; } }

        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// Transform Variable
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        public Vector2 GetPosition { get { return transform.position; } }
        public Quaternion GetRotation { get { return transform.rotation; } }
        public Vector2 GetLocalScale { get { return transform.localScale; } }
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// NormalMoving
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        [Header("Detection")]
        [SerializeField] float checkGroundRadius;
        public float CheckGroundRadius { get { return checkGroundRadius; } }
        [SerializeField] float checkFrontRadius;
        public float CheckFrontRadius { get { return checkFrontRadius; } }
        [SerializeField] Transform frontCheck;
        public Transform FrontCheck { get { return frontCheck; } }
        [SerializeField] Transform groundCheck;
        public Transform GroundCheck { get { return groundCheck; } }

        [Header("Detection Layer")]
        [SerializeField] LayerMask whatIsGround;
        public LayerMask GroundLayer { get { return whatIsGround; } }
        [SerializeField] LayerMask whatIsWall;
        public LayerMask WallLayer { get { return whatIsWall; } }
        [SerializeField] LayerMask whatIsIceGround;
        public LayerMask IceGroundLayer { get { return whatIsIceGround; } }
        [SerializeField] LayerMask whatIsSlimeGround;
        public LayerMask SlimeGroundLayer { get { return whatIsSlimeGround; } }

        [Header("Movement")]
        [SerializeField] float walkSpeed;
        public float WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
        [SerializeField] float jumpForce;
        public float JumpForce { get { return jumpForce; } }
        [SerializeField] float jumpTime;
        public float JumpTime { get { return jumpTime; } }

        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// SpecialMoving
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        [Header("SpecialMovement")]
        [SerializeField] float dashPower;
        public float GetDashPower { get { return dashPower; } }
        [SerializeField] float wallSlideForce;
        public float WallSlideForce { get { return wallSlideForce; } }
        [SerializeField] Vector2 wallJumpForce;
        public Vector2 WallJumpForce { get { return wallJumpForce; } }
        [SerializeField] float wallJumpTime;//按下跳躍時，持續施力的時間
        public float WallJumpTime { get { return wallJumpTime; } }
        [SerializeField] float wallJumpColdDuration;//要有跳躍離開牆壁後的跳躍時間，離開牆壁後可以跳躍的時間
        public float WallJumpColdDuration { get { return wallJumpColdDuration; } }
        [SerializeField] public float leaveWalltTime;
        public float LeaveWallTime { get { return leaveWalltTime; } set { leaveWalltTime = value; } }

        [Header("ObstacleMoving")]//重構英文單字
        [SerializeField] float gravity;
        public float Gravity { get { return gravity; } }

        [Header("Damage")]
        [SerializeField] float spike = 1;
        public float GetSpike { get { return spike; } }

        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// Attritube Gain
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        [Header("Gain with items")]
        [SerializeField] float speedGain = 1;
        public float GetSpeedGain { get { return speedGain; } }
        [SerializeField] float jumpGain = 1;
        public float GetJumpGain { get { return jumpGain; } }

        public IEnumerator SpeedUp(float value, float sec)
        {
            speedGain = value;
            yield return new WaitForSeconds(sec);
            speedGain = 1;
        }
        public IEnumerator JumpUp(float value, float sec)
        {
            jumpGain = value;
            yield return new WaitForSeconds(sec);
            jumpGain = 1;
        }
    }
}

