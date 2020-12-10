using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Role.PlayerV2
{
    public class Collide : MonoBehaviour
    {
        System.Action<string> collideAction = null;
        public void Init(System.Action<string> callback) { collideAction = callback; }

        string weapon = "weapon";
        string item1 = "item1";

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name == weapon)
            {
                collideAction(weapon);
            }
            else if (other.name == item1)
            {
                collideAction(item1);
            }
        }
    }
}
