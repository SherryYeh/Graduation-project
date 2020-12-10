using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Role.PlayerV2
{
    //User Interface
    public class Player : MonoBehaviour
    {
        System.Action<GameObject> gameAction = null;
        Control control;

        bool isGameStarted = false;

        public void GameStart(System.Action<GameObject> callback)
        {
            gameAction = callback;
            isGameStarted = true;
        }
        void Awake()
        {
            control = GetComponent<Control>();
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.tag)
            {
                case "dashItemLeft":
                    control.Dash(new Vector2(-1,0));
                    break;
                case "dashItemRight":
                    control.Dash(new Vector2(1, 0));
                    break;
                case "dashItemUp":
                    control.Dash(new Vector2(0, 1));
                    break;
                case "dashItemDown":
                    control.Dash(new Vector2(0, -1));
                    break;
                case "dashItem":
                    control.Dash();
                    break;
                default:
                    break;
            }
        }
    }
}
