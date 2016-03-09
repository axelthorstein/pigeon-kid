using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 3;
    public float speed = 400f;
    public float jumpPower = 650f;

    public Transform groundedEnd;
	public Transform firePoint;
	public GameObject projectile;

    public bool canDoubleJump;
    public bool grounded;
	public bool sideGrounded;

    private Animator anime;
    private Rigidbody2D rb2d;

	void Start () {

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anime = gameObject.GetComponent<Animator>();

	}

    void Update()
    {
		Vector2 wallCheckLeft = new Vector2 (this.transform.position.x - 0.5f, this.transform.position.y);
		Vector2 wallCheckRight = new Vector2 (this.transform.position.x + 0.5f, this.transform.position.y);
		Debug.DrawLine(wallCheckLeft, wallCheckRight, Color.green);

        grounded = Physics2D.Linecast(this.transform.position, groundedEnd.position, 1 << LayerMask.NameToLayer("Ground"));
		sideGrounded = Physics2D.Linecast(wallCheckLeft, wallCheckRight, 1 << LayerMask.NameToLayer("Ground"));

        anime.SetBool("Grounded", grounded);
        anime.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));

        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }


        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (Input.GetButtonDown("Jump"))
        {
			if (grounded || sideGrounded)
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
		if (Input.GetButton("Jump")) {
			rb2d.gravityScale = 1.8f;
		} else {
			rb2d.gravityScale = 3f;
		}

		if (Input.GetKeyDown (KeyCode.X)) {
		
			Instantiate (projectile, firePoint.position, firePoint.rotation);
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
