using UnityEngine;
using System.Collections;

public class RyansAnimationPractise : MonoBehaviour {

    private Rigidbody2D player;

    public float movementSpeed;
    public float jumpHeight;

    private bool moveRight;

    private Animator myAnimator;

    public float groundRad;
    public LayerMask isGround;
    public Transform grounded;
    private bool isgrounded;
    private bool hasJumped;

    // Use this for initialization
    void Start()
    {
        moveRight = true;
        player = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        hasJumped = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        // float vertical = Input.GetKey()
        Movement(horizontal);
        changeDirection(horizontal);

        isgrounded = Physics2D.OverlapCircle(grounded.position, groundRad, isGround);

        if (hasJumped && !isgrounded)
            myAnimator.SetBool("jump", true);
        if (isgrounded)
            myAnimator.SetBool("jump", false);



        if (player.position.x < -8.24 || player.position.x > 8.86 || player.position.y < -4.6 || player.position.y > 9)
        {
            player.position = new Vector2(0, 0);
        }

    }

    private void Movement(float horizontal)
    {

        player.velocity = new Vector2(horizontal * movementSpeed, player.velocity.y);
        //player.AddForce(new Vector2(horizontal * movementSpeed, player.velocity.y));

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

        if (Input.GetKeyDown(KeyCode.UpArrow) && isgrounded)
        {
            player.velocity = new Vector2(player.velocity.x, jumpHeight);
            //player.AddForce(new Vector2(player.velocity.x, jumpHeight));
            hasJumped = true;
        }

    }

    private void changeDirection(float horizontal)
    {
        if (horizontal > 0 && !moveRight || horizontal < 0 && moveRight)
        {
            moveRight = !moveRight;

            Vector3 scale = transform.localScale;

            scale.x = scale.x * -1;

            transform.localScale = scale;
        }
    }



    void Update()
    {

    }
}

