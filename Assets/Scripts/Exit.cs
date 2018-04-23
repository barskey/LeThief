using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

	MuseumManager mm;

	// Use this for initialization
	void Start () {
		mm = Object.FindObjectOfType<MuseumManager> ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.isTrigger && col.CompareTag ("Player"))
		{
			mm.GoToOffice ();
		}
	}
}
