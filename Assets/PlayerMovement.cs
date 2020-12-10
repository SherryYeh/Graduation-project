using UnityEngine;

public class PlayerMovement : Bolt.EntityBehaviour<ICustomCubeState>
{
    //void Start()
    public override void Attached()
    {
        state.SetTransforms(state.CubeTransform,transform);
    }

    //void Update()
    public override void SimulateOwner()
    {
        var speed = 4f;
        var movement = Vector2.zero;

        
        if (movement != Vector2.zero)
        {
            transform.position = (Vector2)transform.position + (movement.normalized * speed * BoltNetwork.FrameDeltaTime);
        }
    }
}
