using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : TimeControlled
{

    public int direction = 1;

    private void Start() {
        speedMultiplier = 1;
    } 
    public override void TimeUpdate()
    {
    
        velocity.x = speedMultiplier * direction;

        Vector2 pos = transform.position;
        pos.x += velocity.x * Time.deltaTime;
        transform.position = pos;

        if (pos.x >= 13){direction = -1;}
        if (pos.x <= 0.5){direction = 1;}
   }
}
