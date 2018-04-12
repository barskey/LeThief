using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour
{
	public GameObject picGameObject;
	public Painting painting;

	private SpriteRenderer sr;
	private PlayerController pc;

	void Start ()
	{
		picGameObject.GetComponent<SpriteRenderer> ().sprite = painting.bigSprite;
		picGameObject.transform.rotation = Quaternion.Euler (0, 0, 0); // set bigpicture to always be upright
		sr = GetComponent<SpriteRenderer> ();
	}

	void SetBigPicture (bool state)
	{
		picGameObject.GetComponent<SpriteRenderer> ().enabled = state;
	}

	public Painting GrabPicture ()
	{
		sr.enabled = !sr.enabled; // Toggle the sprite display state
		return painting;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.CompareTag ("Player")) // if this is the Player
		{
			pc = col.GetComponent<PlayerController> ();
		}
	}

	void OnTriggerStay2D (Collider2D col)
	{
		if (pc != null)
		{
			if (pc.LookingAtPicture () && pc.flashlight.isOn && sr.enabled)
			{
				SetBigPicture (true);
			}
			else
			{
				SetBigPicture (false);
			}
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		SetBigPicture (false);
		pc = null;
	}

}
