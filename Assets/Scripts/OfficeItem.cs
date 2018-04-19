using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OfficeItem : MonoBehaviour {

	public GameObject gameObjectToShow;
	public bool isMap = false;

	OfficeManager om;

	Light highlight;
	MeshRenderer mr; // renders text description

	void Awake ()
	{
		om = Object.FindObjectOfType<OfficeManager> ();
	}

	// Use this for initialization
	void Start ()
	{
		highlight = GetComponentInChildren<Light> ();
		mr = GetComponentInChildren<MeshRenderer> ();
		if (gameObjectToShow != null)
		{
			gameObjectToShow.SetActive (false);
		}
	}
	
	void OnMouseDown ()
	{
		if (isMap && !om.phone.onScreen)
		{
			om.StartCoroutine ("GoToMuseum");
		}
	}

	void OnMouseOver ()
	{
		if (!om.phone.onScreen)
		{
			highlight.enabled = true;
			mr.enabled = true;

			if (gameObjectToShow != null)
			{
				gameObjectToShow.SetActive (true);
			}
		}
	}

	void OnMouseExit ()
	{
		highlight.enabled = false;
		mr.enabled = false;
		if (gameObjectToShow != null)
		{
			gameObjectToShow.SetActive (false);
		}
	}
}
