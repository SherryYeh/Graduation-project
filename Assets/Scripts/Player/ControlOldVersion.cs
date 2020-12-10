using UnityEngine;

namespace Role.PlayerSpace
{

    public class ControlOldVersion
    {
        private bool fallJump = false;
        public void DoVelocity(Rigidbody2D rb, Vector2 force)
        {
            rb.velocity = force;
            if (rb.velocity.y < -40 && fallJump == false) rb.velocity = new Vector2(force.x, -40);
        }
        public void SpeedLimit(Rigidbody2D rb)
        {
            if (rb.velocity.y < -40 && fallJump == false) rb.velocity = new Vector2(0, -40);
        }
        public void DoDash(Rigidbody2D rb, Vector2 force)
        {
            rb.AddForce(force);
        }
        public void DoFallJump(bool fallJumping)
        {
            fallJump = fallJumping;
        }
        public void AddVelocity(Rigidbody2D rb, Vector2 force)
        {
            rb.velocity += force;
        }
    }

}