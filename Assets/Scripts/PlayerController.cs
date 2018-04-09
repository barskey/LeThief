using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 2f;
	public bool armsFull = false;
	public LayerMask pictureLayer;
	[HideInInspector]
	public bool running = false;

	Rigidbody2D rb2d;
	Animator anim;
	SpriteRenderer sr;
	[HideInInspector]
	public Flashlight flashlight;

	bool facingRight = true;
	Vector2 lookDir; // direction player was last looking

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
		flashlight = GetComponentInChildren<Flashlight> ();

		lookDir = Vector2.right;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 movement = Vector2.zero;

		// get movement input from keyboard or controller
		movement.x = Input.GetAxis ("Horizontal");
		movement.y = Input.GetAxis ("Vertical");
		movement.Normalize ();

		Move (movement);

		// set lookdir, flashlight direction and running state only if player is moving
		if (!rb2d.velocity.Equals (Vector2.zero))
		{
			lookDir = movement;
			flashlight.SetDirection (lookDir);
			running = true;
		}
		else
		{
			running = false;
		}

		// Toggle flashlight if Q key is pressed
		if (Input.GetKeyDown (KeyCode.Q))
			flashlight.ToggleLight ();
	}

	void Move (Vector2 moveVector)
	{
		rb2d.velocity = moveVector * speed;

		if (facingRight) // if player was facing right
		{
			if (rb2d.velocity.x < 0) // if moving to the left
			{
				FlipSprite ();
			}
		}
		else // player was facing left
		{
			if (rb2d.velocity.x > 0) // if moving to the right
			{
				FlipSprite ();
			}
		}

		anim.SetFloat ("speed", rb2d.velocity.magnitude);

		// TODO play running sound
	}

	void FlipSprite()
	{
		bool flipState = sr.flipX;
		sr.flipX = !flipState;
		facingRight = !facingRight;
	}

	public bool LookingAtPicture()
	{
		RaycastHit2D hit = Physics2D.Raycast (transform.position, lookDir.normalized, 1f, pictureLayer);
		if (hit.collider != null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
