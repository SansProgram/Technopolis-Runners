using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //For Funcionality Logic 
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    //For Animation
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask  jumpableGround;
    
    //Variables for values
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 14f;
    [SerializeField] private float jumpForce = 7f; // [SerializeField] - allows for the changing of values inside of unity, also making it private but we should minimize the use of public variables. SerField is the best way
    
    private enum MovementState { idle, running, jumping, falling }
    
    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //allows access to rb everywhere
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Things you do every frame ie Movements
        if (Input.GetButtonDown("Jump") && IsGrounded()) //checks if space key is pressed (Checks for every frame)
            {
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, moveSpeed); //Allows access to rigid body component and store the 3(X, Y Z) values, or in this case 2 - 2D only x and y00
            }
        
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * jumpForce, rb.velocity.y);
       
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        //For Animation
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    
}
