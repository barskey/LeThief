using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuardController : MonoBehaviour {

	Rigidbody2D rb2d;
	Animator anim;
	SpriteRenderer sr;
	[HideInInspector]
	Flashlight flashlight;
	SpriteRenderer emote;

	public float walkSpeed = 1f;
	public float runSpeed = 2.5f;
	public LayerMask hearLayers;
	public Transform[] points;
	public Sprite alertEmote;
	public Sprite ChaseEmote;
	public float seeAngleHalf = 45f;
	public float seeDist = 3f;
	public float hearDist = 4f;
	public LayerMask cantSeeThru;
	public float totSearchTime = 4f;

	enum Behavior {
		none,
		patrol,
		alert,
		chase
	}

	Behavior guardState;
	Behavior prevState;
	bool facingRight = true;
	Vector2 lookDir; // direction guard is looking
	GameObject player;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
		flashlight = GetComponentInChildren<Flashlight> ();
		emote = transform.Find ("GuardState").GetComponent<SpriteRenderer> ();
		player = GameObject.FindGameObjectWithTag ("Player");

		lookDir = Vector2.right;
		guardState = Behavior.patrol;
	}

	// Update is called once per frame
	void Update () {
		// TODO is this the best way to implement this State Machine?
		switch (guardState)
		{
		case Behavior.alert:
			if (guardState != prevState) {
				prevState = guardState;
				AlertEnter ();
			} else {
				AlertUpdate ();
			}
			break;
		case Behavior.patrol:
			if (guardState != prevState) {
				prevState = guardState;
				PatrolEnter ();
			} else {
				PatrolUpdate ();
			}
			break;
		case Behavior.chase:
			if (guardState != prevState) {
				prevState = guardState;
				ChaseEnter ();
			} else {
				ChaseUpdate ();
			}
			break;
		default:
			PatrolEnter ();
			break;
		}
	}

	int patrolIndex = 0;
	Vector2 moveTo;
	void PatrolEnter()
	{
		Debug.Log ("PatrolEnter");
		emote.enabled = false; // hide emote above head

		moveTo = (Vector2)points [patrolIndex].position; // set destination vector

		guardState = Behavior.patrol; // go to patrol update
	}

	void PatrolUpdate()
	{
		//Debug.Log ("PatrolUpdate");
		Move (walkSpeed);
		float dist = Vector2.Distance (moveTo, (Vector2)transform.position);

		if (SeesPlayer ()) {
			prevState = guardState;
			guardState = Behavior.chase;
		} else if (HearsPlayer ()) {
			prevState = guardState;
			guardState = Behavior.alert;
		} else if (dist < 0.01f) { // if guard has reached patrol point
			patrolIndex++; // get next point index
			if (patrolIndex >= points.Length) // reset index to first point if at the end
				patrolIndex = 0;
			moveTo = (Vector2)points [patrolIndex].position; // update destination vector
		}
	}

	float waitedSecs;
	void AlertEnter()
	{
		Debug.Log ("AlertEnter");
		// stop
		rb2d.velocity = Vector2.zero;
		anim.SetFloat ("speed", 0);

		// set question mark above head
		emote.sprite = alertEmote;
		emote.enabled = true;

		waitedSecs = 0f; // reset time have waited so far

		// TODO play "Hmmm?" or "What's that?" sound

		guardState = Behavior.alert; // go to alert update
	}
		
	void AlertUpdate()
	{
		//Debug.Log ("AlertUpdate");
		// Look around
		// Enter Chase if sees player, otherwise return to patrol
		// maybe turn on light first?

		// use 1/4 of totSearch time to search in each of 4 directions
		// TODO Pick the direction closest to the current lookDir to start
		if (waitedSecs < (totSearchTime / 4) * 1) {
			DOTween.To (() => lookDir, x => lookDir = x, Vector2.left, waitedSecs / 4);
		}

		if (SeesPlayer ()) {
			prevState = guardState;
			guardState = Behavior.chase;
		} else if (HearsPlayer ()) {
			prevState = Behavior.none; // set to none so guard re-enters alert state to start over
			guardState = Behavior.alert;
		}
	}

	void ChaseEnter()
	{
		Debug.Log ("ChaseEnter");
		// set question mark above head
		emote.sprite = ChaseEmote;
		emote.enabled = true;

		moveTo = player.transform.position; // set new target position to player position

		guardState = Behavior.chase; // go to chase update
	}

	void ChaseUpdate()
	{
		moveTo = player.transform.position; // set new target position to player position
		Move(runSpeed);
		Debug.Log ("ChaseUpdate");
	}

	bool SeesPlayer()
	{
		// find the angle between player and lookDir to see if within cone of vision
		Vector2 playerDir = (Vector2)player.transform.position - (Vector2)transform.position;
		float playerAngle = Vector2.Angle (lookDir, playerDir);

		// find distance to player
		float playerDist = Vector3.Distance (player.transform.position, transform.position);

		// if player could be seen, check if there are obstacles (walls) between
		if (playerAngle <= seeAngleHalf && playerDist <= seeDist) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, playerDir, playerDist, cantSeeThru);
			if (!hit) { // no walls in the way
				//Debug.Log ("I see you!");
				return true;
			}
		}

		return false; // can't see player
	}

	bool HearsPlayer()
	{
		Collider2D col = Physics2D.OverlapCircle (transform.position, hearDist, hearLayers); // check if player is in range to hear
		PlayerController pc = col.GetComponent<PlayerController>();
		if (pc != null) { // if we have a valid playercontroller
			if (pc.running) { // if player is running, hence making noise
				return true;
			}
		}
		return false;
	}

	void Move(float speed)
	{
		Vector2 moveVector = moveTo - (Vector2)transform.position;
		rb2d.velocity = moveVector.normalized * speed;

		if (facingRight) // if guard was facing right
		{
			if (rb2d.velocity.x < 0) // if moving to the left
			{
				FlipSprite ();
			}
		}
		else // guard was facing left
		{
			if (rb2d.velocity.x > 0) // if moving to the right
			{
				FlipSprite ();
			}
		}

		anim.SetFloat ("speed", rb2d.velocity.magnitude);

		// TODO play walking sound

		lookDir = moveVector.normalized; // set looking direction (used for seeing player)
		flashlight.SetDirection (lookDir); // set flashlight direction

	}

	void FlipSprite()
	{
		bool flipState = sr.flipX;
		sr.flipX = !flipState;
		facingRight = !facingRight;
	}
}
