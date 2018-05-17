using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

	public AudioClip click;
	[HideInInspector]
	public Light lightComp;
	AudioSource aud;

	public bool isOn = false; // convenience for player/guard to check state

	void Awake ()
	{
		lightComp = GetComponent<Light> ();
		aud = GetComponent<AudioSource> ();
	}

	public void ToggleLight ()
	{
		isOn = !isOn;
		lightComp.enabled = isOn;

		aud.Play ();
	}

	public void SetDirection (Vector2 dir)
	{
		transform.rotation = Quaternion.LookRotation (new Vector3 (dir.x, dir.y), Vector3.forward);
	}
}
