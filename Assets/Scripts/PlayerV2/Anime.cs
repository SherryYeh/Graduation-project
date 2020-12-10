using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Role.PlayerV2
{
    public class Anime
    {
        Animator animator;

        public Anime(Animator animator)
        {
            this.animator = animator;
        }
        public void DOAnimation(string name)
        {
            animator.SetTrigger(name);
        }
        public void DOAnimation(string name, float value)
        {
            animator.SetFloat(name, value);
        }
    }
}