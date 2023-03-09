using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlled : MonoBehaviour
{
    public float speedMultiplier; 
    public Vector2 velocity;
    public AnimationClip currentAnimation; 
    public float animationTime;

    public virtual void TimeUpdate()
    {
        if (currentAnimation != null)
        {
            animationTime += Time.deltaTime; 
            if (animationTime > currentAnimation.length)
            {
                animationTime = animationTime - currentAnimation.length;
            }
        }
    }

    public void updateAnimation()
    {
        if (currentAnimation != null)
        {
            currentAnimation.SampleAnimation(gameObject, animationTime);
        }
    }
}
