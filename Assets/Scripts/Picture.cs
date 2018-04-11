using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour {

	public GameObject picGameObject;
	public Sprite picSprite;

	private SpriteRenderer sr;
	private PlayerController pc;

	void Awake ()
	{
		picGameObject.GetComponent<SpriteRenderer> ().sprite = picSprite;
		picGameObject.transform.rotation = Quaternion.Euler (0, 0, 0); // set bigpicture to always be upright
		sr = GetComponent<SpriteRenderer> ();
	}

	void SetBigPicture (bool state)
	{
		picGameObject.GetComponent<SpriteRenderer> ().enabled = state;
	}

	public Sprite GrabPicture ()
	{
		sr.enabled = !sr.enabled; // Toggle the sprite display state
		return picSprite;
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
