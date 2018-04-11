﻿using System.Collections;
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

	enum Behavior {
		none,
		patrol,
		alert,
		chase
	} // none behavior used in prevState so first time thru will enter *Enter state instead of *Update

	Behavior guardState;
	Behavior prevState;
	bool facingRight = true;
	GameObject player;

	// Use this for initialization
	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
		flashlight = GetComponentInChildren<Flashlight> ();
		emote = transform.Find ("GuardState").GetComponent<SpriteRenderer> ();
		player = GameObject.FindGameObjectWithTag ("Player");

		guardState = Behavior.patrol; // start guard in patrol
		prevState = Behavior.none;
	}

	// Update is called once per frame
	void Update ()
	{
		// TODO is this the best way to implement this State Machine?
		switch (guardState)
		{
			case Behavior.alert:
				if (guardState != prevState)
				{
					AlertEnter ();
				}
				else
				{
					AlertUpdate ();
				}
				break;
			case Behavior.patrol:
				if (guardState != prevState)
				{
					PatrolEnter ();
				}
				else
				{
					PatrolUpdate ();
				}
				break;
			case Behavior.chase:
				if (guardState != prevState)
				{
					ChaseEnter ();
				}
				else
				{
					ChaseUpdate ();
				}
				break;
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, hearDist); // draw circle (sphere) for hear distance

		if (flashlight != null) 
		{
			Vector3 lookDir = flashlight.transform.localRotation * Vector3.forward; // use flashlight direction as direction guard is looking

			Gizmos.color = Color.green;
			Vector3 ray = Quaternion.Euler (0f, -seeAngleHalf, -seeAngleHalf) * lookDir * seeDist;
			Gizmos.DrawRay (transform.position, ray);

			ray = Quaternion.Euler (0f, seeAngleHalf, seeAngleHalf) * lookDir * seeDist;
			Gizmos.DrawRay (transform.position, ray);
		}
	}

	int patrolIndex = 0;
	Vector2 moveTo;
	void PatrolEnter ()
	{
		Debug.Log ("PatrolEnter");
		prevState = Behavior.patrol;

		emote.enabled = false; // hide emote above head

		moveTo = points [patrolIndex].position.Vector2; // set destination vector

		guardState = Behavior.patrol; // go to patrol update
	}

	void PatrolUpdate ()
	{
		//Debug.Log ("PatrolUpdate");
		Move (walkSpeed);
		float dist = Vector2.Distance (moveTo, transform.position.Vector2);

		if (SeesPlayer ())
		{
			guardState = Behavior.chase;
		}
		else if (HearsPlayer ())
		{
			guardState = Behavior.alert;
		}
		else if (dist < 0.01f) // if guard has reached patrol point
		{
			patrolIndex++; // get next point index
			if (patrolIndex >= points.Length) // reset index to first point if at the end
			{
				patrolIndex = 0;
			}
			moveTo = points [patrolIndex].position.Vector2; // update destination vector
		}

		// TODO play whistle sound at random times

		prevState = Behavior.patrol;
	}

	Vector2 lastPosition;
	Sequence lookSeq;

	void AlertEnter ()
	{
		Debug.Log ("AlertEnter");
		prevState = Behavior.alert;

		// stop
		rb2d.velocity = Vector2.zero;
		anim.SetFloat ("speed", 0);

		// set question mark above head
		emote.sprite = alertEmote;
		emote.enabled = true;

		// TODO play "Hmmm?" or "What's that?" sound

		lastPosition = player.transform.position.Vector2; // save location sound was heard

		// guard will look in direction he heard sound, then scan around looking for player. Using random values to make it
		// look like guard is searching erratically.

		Vector3 playerVector = lastPosition.Vector3 - transform.position; // create transform to player position
		
		// set up look sequence for panning back and forth
		lookSeq = DOTween.Sequence ();
		lookSeq.Append (flashlight.transform.DOLookAt (lastPosition.Vector3, Random.Range (0.2f, 1f))); // point flashlight in direction last heard
		lookSeq.AppendInterval (Random.Range (0.2f, 1f)); // add random length pause
		// create new vector3 rotated by random amount
		Vector3 newRotate = Quaternion.AngleAxis (Random.Range (-seeAngleHalf, seeAngleHalf), -Vector3.forward) * playerVector;
		lookSeq.Append (flashlight.transform.DOLookAt (newRotate, Random.Range (0.2f, 1f))); // add to sequence
		lookSeq.AppendInterval (Random.Range (0.2f, 1f)); // add random length pause
		// make another rotated vector by random amount
		newRotate = Quaternion.AngleAxis (Random.Range (-seeAngleHalf, seeAngleHalf), -Vector3.forward) * playerVector;
		lookSeq.Append (flashlight.transform.DOLookAt (newRotate, Random.Range (0.2f, 1f))); // add to sequence
		lookSeq.AppendInterval (Random.Range (0.2f, 1f)); // add random length pause
		// make another rotated vector by random amount
		newRotate = Quaternion.AngleAxis (Random.Range (-seeAngleHalf, seeAngleHalf), -Vector3.forward) * playerVector;
		lookSeq.Append (flashlight.transform.DOLookAt (newRotate, Random.Range (0.2f, 1f))); // add to sequence
		lookSeq.AppendInterval (Random.Range (0.2f, 1f)); // add random length pause
		// go back to patrol if we reach the end of this sequene
		lookSeq.AppendCallback (() => {
			guardState = Behavior.patrol;
		});
		lookSeq.Play (); // start the tween sequence

		guardState = Behavior.alert; // go to alert update
	}
		
	void AlertUpdate ()
	{
		prevState = Behavior.alert;
		//Debug.Log ("AlertUpdate");
		// Enter Chase if sees player, otherwise return to patrol
		if (SeesPlayer ())
		{
			lookSeq.Complete (); // stop the current look tween
			guardState = Behavior.chase;
		}
		else if (HearsPlayer ()) // TODO Guard goes crazy if player runs while he is searching - need to add delay
		{
			lookSeq.Complete (); // stop the current look tween
			prevState = Behavior.none; // set to none so guard re-enters alert state to start over
			guardState = Behavior.alert;
		}
	}
	
	float waited;
	void ChaseEnter ()
	{
		prevState = Behavior.chase;
		Debug.Log ("ChaseEnter");
		// set exclamation mark above head
		emote.sprite = ChaseEmote;
		emote.enabled = true;
		
		waited = 0f;
		
		// play "yell" sound

		moveTo = player.transform.position.Vector2; // set new target position to player position

		guardState = Behavior.chase; // go to chase update
	}

	void ChaseUpdate ()
	{
		if (waited > 0.5f) // wait a little bit before rushing
		{
			if (SeesPlayer())
			{
				moveTo = player.transform.position.Vector2; // set new target position to player position
				lastPosition = moveTo; // remember last position player was seen
			}
			else
			{
				moveTo = lastPosition;
			}

			Move(runSpeed);
			
			float dist = Vector2.Distance (moveTo, transform.position.Vector2);
			if (dist < 0.1f) // reached destination last seen position (reached player handled in OnColliderEnter2D)
			{
				guardState = Behavior.alert; // start searching again, before resuming patrol
			}
		}
		else
		{
			waited += Time.deltaTime;
		}
		
		prevState = Behavior.chase;
		//Debug.Log ("ChaseUpdate");
	}

	bool SeesPlayer ()
	{
		// find the angle between player and direction guard is looking to see if within cone of vision
		Vector2 lookDir = (Vector2)(flashlight.transform.localRotation * Vector3.forward); // use flashlight direction as direction guard is looking
		Vector2 playerDir = (Vector2)player.transform.position - (Vector2)transform.position; // direction to player
		float playerAngle = Vector2.Angle (lookDir, playerDir);

		// find distance to player
		float playerDist = Vector3.Distance (player.transform.position, transform.position);
		float seeDistance = player.GetComponent<PlayerController> ().flashlight.isOn ? float.PositiveInfinity : seeDist;

		// if player could be seen, check if there are obstacles (walls) between
		if (playerAngle <= seeAngleHalf && playerDist <= seeDistance)
		{
			RaycastHit2D hit = Physics2D.Raycast (transform.position, playerDir, playerDist, cantSeeThru);
			if (!hit) // no walls in the way
			{
				//Debug.Log ("I see you!");
				return true;
			}
		}

		return false; // can't see player
	}

	bool HearsPlayer ()
	{
		Collider2D col = Physics2D.OverlapCircle (transform.position, hearDist, hearLayers); // check if player is in range to hear
		PlayerController pc = col.GetComponent<PlayerController> ();
		if (pc != null) // if we have a valid playercontroller
		{
			if (pc.running) // if player is running, hence making noise
			{
				return true;
			}
		}
		return false;
	}

	void Move (float speed)
	{
		Vector2 moveVector = moveTo - transform.position.Vector2;
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

		if (!rb2d.velocity.Equals (Vector2.zero)) // if guard is moving
			flashlight.SetDirection (moveVector.normalized); // set flashlight direction
	}

	void FlipSprite ()
	{
		bool flipState = sr.flipX;
		sr.flipX = !flipState;
		facingRight = !facingRight;
	}
	
	void OnColliderEnter2D (Collider2D col)
	{
		if (col.CompareTag ("Player"))
		{
			Debug.Log ("Caught you!");
			// pause gameplay
			// show "caught" jailbars and text
			// button to restart
		}
	}
}
