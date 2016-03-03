using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 3;
    public float speed = 50f;
    public float jumpPower = 150f;

    public bool canDoubleJump;
    public bool grounded;

    private Animator anime;
    private Rigidbody2D rb2d;

	void Start () {

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anime = gameObject.GetComponent<Animator>();

	}

    void Update()
    {

        anime.SetBool("Grounded", grounded);
        anime.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));

        if (Input.GetAxis("Horizontal") > -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                rb2d.AddForce(Vector2.up * jumpPower);
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                    canDoubleJump = false;
                    rb2d.AddForce(Vector2.up * jumpPower * 0.75f);
                }
            }
        }


    }

    void FixedUpdate ()
    {

        Vector3 easeVelocity = rb2d.velocity;
        easeVelocity.y = rb2d.velocity.y;
        easeVelocity.z = 0.0f;
        easeVelocity.x *= 0.75f;

        float h = Input.GetAxis("Horizontal");

        //Fake Friction
        if (grounded)
        {

            rb2d.velocity = easeVelocity;

        }

        rb2d.AddForce((Vector2.right * speed) * h);

        if (rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }
    }
}
