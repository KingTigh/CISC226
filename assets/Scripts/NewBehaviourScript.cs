using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GearFactory;


public class NewBehaviourScript : TimeControlled
{
    
    private float jumpVelocity = 30;
    private float moveSpeed = 2;
    private float dashForce = 5;
    private float direction = 1;
    public float horMomentum = 0; 
    private float vertMomentum = 0;

    public bool floating;
    private bool hasKey;
    private bool facingR; 
    private bool dashReady;
    private bool jumpReady;

    public AnimationClip idleAnimation;
    public AnimationClip walkingAnimation;
    public AnimationClip flyingAnimation;

    public Animator animator;
    private SpriteRenderer spi; 
    private EdgeCollider2D groundCollider;
    private Rigidbody2D rb2d;
    
    // Start is called before the first frame update
    void Start()
    {
        groundCollider = GetComponent<EdgeCollider2D>();
        spi = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        hasKey = false;
        floating = false;
        facingR = true;  
        dashReady = true;
        jumpReady = true;
        speedMultiplier = 1;

    }

    // Update is called once per frame
    public override void TimeUpdate()
    {   
        base.TimeUpdate();
        
        Vector2 pos = transform.position;

        pos.y += velocity.y * Time.deltaTime;
        velocity.y += TimeController.gravity * Time.deltaTime;
        pos.x += (velocity.x + horMomentum) * speedMultiplier * Time.deltaTime * direction;

        if (velocity.x > 0){velocity.x -= 3 * Time.deltaTime;}        
        if (velocity.x < 0){velocity.x = 0;}

        if (horMomentum > 0){horMomentum -= 3 * Time.deltaTime;}
        if (horMomentum < 0){horMomentum = 0;}
        
        if (pos.y < 1)
        {
            pos.y = 1;
            velocity.y = 0;
        }
        if (Input.GetKeyDown(KeyCode.W) && jumpReady == true)
        {   
            // rb2d.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            velocity.y = jumpVelocity;
            // floating = true;
            jumpReady = false;   
        }
        if (Input.GetKey(KeyCode.A))
        {   
            direction = -1;
            if (facingR)
            {
                facingR = false;
                spi.flipX = true;
            }
            velocity.x = moveSpeed;
            pos.x -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = 1; 
            if (!facingR)
            {
                facingR = true;
                spi.flipX = false;  
            }
            velocity.x = moveSpeed;
            pos.x += moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && dashReady)
        {
            pos.x += dashForce * direction;
            dashReady = false;
        }

        if (!Input.anyKey){
            currentAnimation = idleAnimation;
        }
        transform.position = pos;

        
    }    
    // Triggers
    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "Floor")
        {
                floating = false;
                dashReady = true;
                jumpReady = true; 
                TimeController.gravity = 0;
                currentAnimation = walkingAnimation;
                velocity.y = 0;
                transform.parent = null;
                if (velocity.x > 0){ currentAnimation = walkingAnimation;}
                else{ currentAnimation = idleAnimation;}
        }
        if (other.gameObject.tag == "Key")
        {
            hasKey = true; 
            Destroy(other.gameObject);
            
        }
        else if (other.gameObject.tag == "Gear")
        {
            floating = false;
            dashReady = true;
            jumpReady = true; 
            velocity.y = 0;
            velocity.x = other.gameObject.GetComponent<Gear>().speed / 32;
            TimeController.gravity = 0;
            currentAnimation = walkingAnimation;
        }
            
        else
            {
                floating = false;
                dashReady = true;
                jumpReady = true; 
                TimeController.gravity = 0;
                currentAnimation = walkingAnimation;
                velocity.y = 0;
                if (velocity.x > 0){ currentAnimation = walkingAnimation;}
                else{ currentAnimation = idleAnimation;}


                if (other.gameObject.tag == "Door" && hasKey)
                {
                    other.isTrigger = true;
                }
                else if (other.gameObject.tag == "MovingPlatform")
                {
                    transform.parent = other.transform;
                }
            }
        
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Floor")
        {
            floating = false;
            dashReady = true;
            jumpReady = true;
            transform.parent = null;
            if (velocity.x > 0){ currentAnimation = walkingAnimation;}
            else{ currentAnimation = idleAnimation;}
        }

        else if (other.gameObject.tag == "MovingPlatform")
        {
            floating = false;
            dashReady = true;
            jumpReady = true;
            transform.parent = other.transform;
            if (velocity.x > 0){ currentAnimation = walkingAnimation;}
            else{ currentAnimation = idleAnimation;}
        }
        else if (other.gameObject.tag == "Gear")
        {
            floating = false;
            dashReady = true;
            jumpReady = true; 
            velocity.x = other.gameObject.GetComponent<Gear>().speed / 32;
            TimeController.gravity = 0;
            currentAnimation = walkingAnimation;
        }

    }
    private void OnTriggerExit2D(Collider2D other) {

        transform.parent = null;
        if (other.gameObject.tag == "Key")
        {

        }
        if (other.gameObject.tag == "Gear")
        {
            jumpReady = false; 
            TimeController.gravity = -100;
            floating = true;
            currentAnimation = flyingAnimation;
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
            rb2d.constraints = RigidbodyConstraints2D.None;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            velocity.x += other.GetComponent<Gear>().speed /32;
            velocity.y += other.GetComponent<Gear>().speed /32;
            
        }
        else
        {
            jumpReady = false; 
            TimeController.gravity = -100;
            floating = true;
            currentAnimation = flyingAnimation;
            
            if (other.gameObject.tag == "MovingPlatform")
            {
                if (other.transform.TryGetComponent(out MovingPlatformScript MP))
                {   
                    direction = MP.direction;
                    horMomentum = MP.velocity.x * MP.direction * 2;
                }
                else if (other.transform.TryGetComponent(out VMovingPlatformScript MPV))
                {
                    velocity.y += MPV.velocity.y *MPV.direction*2;
                }
            }
       
        }
    }
}
