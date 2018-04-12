using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeClickable : MonoBehaviour {

	Light highlight;
	MeshRenderer mr;

	// Use this for initialization
	void Start ()
	{
		highlight = GetComponentInChildren<Light> ();
		mr = GetComponentInChildren<MeshRenderer> ();
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
}
