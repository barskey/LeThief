using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomFloor : MonoBehaviour {

	SpriteRenderer ceilingSprite;
	Collider2D roomCollider;

	// Use this for initialization
	void Start () {
		Transform room = transform.parent.parent; // find the room that this script is attached to
		ceilingSprite = room.Find ("Ceiling").GetComponent<SpriteRenderer> (); // TODO fix in case Ceiling game object is not named ceiling
		roomCollider = GetComponent<CompositeCollider2D> ();

		if (Application.isEditor)
		{
			ceilingSprite.color = new Color (1, 1, 1, 0);
		}
	}

	void Update ()
	{
		//Debug.Log (roomCollider.bounds.Contains (transform.position));
	}

	// when entering this room
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.isTrigger && !roomCollider.bounds.Contains (col.transform.position))
		{
			ceilingSprite.DOFade (0f, 0.5f);
			//Debug.Log ("Entered room");
		}
	}

	// when exiting this room
	void OnTriggerExit2D (Collider2D col)
	{
		if (col.isTrigger && !roomCollider.bounds.Contains (col.transform.position))
		{
			ceilingSprite.DOFade (1f, 0.5f);
			//Debug.Log ("Exited room");
		}
	}
}
