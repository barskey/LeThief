using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumManager : MonoBehaviour {

	GameManager gm;
	PlayerController player;

	// Use this for initialization
	void Start () {
		gm = Object.FindObjectOfType<GameManager> ();
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
	}

	public void GoToOffice ()
	{
		gm.carriedPainting = player.carriedPainting;
		gm.GoToOffice ();
	}

}
