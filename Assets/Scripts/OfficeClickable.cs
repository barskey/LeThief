using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OfficeClickable : MonoBehaviour {

	public GameObject gameObjectToShow;

	OfficeManager om;
	
	Light highlight;
	MeshRenderer mr;

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
		ToggleImage ();
	}

	void OnMouseOver ()
	{
		highlight.enabled = true;
		mr.enabled = true;
	}

	void OnMouseExit ()
	{
		highlight.enabled = false;
		mr.enabled = false;
	}
		
	void ToggleImage ()
	{
		if (gameObjectToShow != null)
		{
			gameObjectToShow.SetActive (!gameObjectToShow.activeSelf);
		}
	}

}
