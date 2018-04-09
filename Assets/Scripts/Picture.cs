using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour {

	public GameObject picGameObject;
	public Sprite picSprite;

	void Awake()
	{
		picGameObject.GetComponent<SpriteRenderer> ().sprite = picSprite;
		picGameObject.transform.rotation = Quaternion.Euler (0, 0, 0);
	}

	void SetBigPicture(bool state)
	{
		picGameObject.GetComponent<SpriteRenderer> ().enabled = state;
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.gameObject.CompareTag ("Player")) // if this is the Player
		{
			PlayerController pc = col.GetComponent<PlayerController> ();
			if (pc.LookingAtPicture () && pc.flashlight.isOn)
			{
				SetBigPicture (true);
			}
			else
			{
				SetBigPicture (false);
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.CompareTag ("Player")) // if this is the Player
		{
			SetBigPicture (false);
		}
	}

}
