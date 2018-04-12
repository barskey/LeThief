using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float runSpeed = 2f;
	public float sneakSpeed = 0.5f;
	public bool armsFull = false;
	public LayerMask pictureLayer;
	[HideInInspector]
	public bool running = false;
	[HideInInspector]
	public Flashlight flashlight;
	[HideInInspector]
	public Painting carriedPic;

	Rigidbody2D rb2d;
	Animator anim;
	SpriteRenderer sr;
	SpriteRenderer picSr;

	bool facingRight = true;
	float speed = 0f;
	Picture picController;

	// Use this for initialization
	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
		picSr = transform.Find ("Pic").GetComponent<SpriteRenderer> ();
		Debug.Log (picSr);
		flashlight = GetComponentInChildren<Flashlight> ();

		speed = runSpeed; // start out in run mode
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector2 movement = Vector2.zero;

		// get movement input from keyboard or controller
		movement.x = Input.GetAxis ("Horizontal");
		movement.y = Input.GetAxis ("Vertical");
		movement.Normalize (); // so moving in diagonals is at same speed as moving straight

		Move (movement);

		// set flashlight direction and running state only if player is moving
		if (!rb2d.velocity.Equals (Vector2.zero))
		{
			flashlight.SetDirection (movement);
			running = speed > sneakSpeed ? true : false;
		}
		else
		{
			running = false;
		}

		// Toggle flashlight if Q key is pressed
		if (Input.GetKeyDown (KeyCode.Q))
		{
			flashlight.ToggleLight ();
		}

		// Toggle sneak if C is pressed
		if (Input.GetKeyDown (KeyCode.C))
		{
			speed = speed > sneakSpeed ? sneakSpeed : runSpeed;
			// play sneak idle animation
		}

		// Grab the pic if E is pressed
		if (Input.GetKeyDown (KeyCode.E))
		{
			if (picController != null)
			{
				GotPicture ();
			}
		}
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

		// TODO play running sound if running
	}

	void FlipSprite ()
	{
		bool flipState = sr.flipX;
		sr.flipX = !flipState;
		facingRight = !facingRight;
	}

	public bool LookingAtPicture ()
	{
		Vector2 lookDir = (Vector2)(flashlight.transform.localRotation * Vector3.forward); // Get lookDir from flashlight, since it is always pointed in direction player is looking
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

	void GotPicture ()
	{
		carriedPic = picController.GrabPicture ();
		picSr.sprite = 	carriedPic.bigSprite;
		picSr.enabled = true;
		// TODO tell game object to set alarm

	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.CompareTag ("Picture")) // if this is a Picture
		{
			picController = col.GetComponent<Picture> ();

		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.gameObject.CompareTag ("Picture")) // if this is a Picture
		{
			picController = null;

		}
	}
}
