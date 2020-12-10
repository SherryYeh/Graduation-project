using UnityEngine;

namespace Role.PlayerSpace
{
    public class Control
    {
        private Rigidbody2D rigidbody;
        public void DoMove(Rigidbody2D rb, Vector2 force)
        {
            rb.velocity = force;
        }
        public void SlowDoMove(Rigidbody2D rb, Vector2 force, float power)
        {
            rb.velocity += (force / power);
        }
        public void DoMove(Rigidbody2D rb, Transform transform, Vector2 force)
        {
            Vector3 movement = new Vector3(force.x, force.y, 0);
            rb.MovePosition(transform.position + movement);
        }
        public void DoMoveX(Rigidbody2D rb, Vector2 force)
        {
            rb.velocity = new Vector2(force.x, rb.velocity.y);
        }
        public void SlowDoMoveX(Rigidbody2D rb, Vector2 force,float power)
        {
            rb.velocity += new Vector2(force.x/power, 0);
        }
        public void DoMoveY(Rigidbody2D rb, Vector2 force)
        {
            rb.velocity = new Vector2(rb.velocity.x, force.y);
        }
        public void DoMoveForce(Rigidbody2D rb, Vector2 force)
        {
            rb.AddForce(force);
        }
    }
}