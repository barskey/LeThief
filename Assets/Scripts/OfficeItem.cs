using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OfficeItem : MonoBehaviour {

	public GameObject gameObjectToShow;
	public Color pulseColor;
	public Color highlightColor;
	static public float pulseTime = 0.8f;

	OfficeManager om;

	Light highlight;
	MeshRenderer mr; // renders text description
	bool itemIsOn;
	Tween pulseTween;
	float startTime;
	float hoverTime = 2f;

	public enum ClickFunction
	{
		None,
		MapClick,
		PhoneClick
	}
	public ClickFunction clickFunction;

	void Awake ()
	{
		om = Object.FindObjectOfType<OfficeManager> ();
	}

	// Use this for initialization
	void Start ()
	{
		highlight = GetComponentInChildren<Light> ();
		mr = GetComponentInChildren<MeshRenderer> ();

		itemIsOn = false;

		highlight.color = pulseColor;

		if (gameObjectToShow != null)
		{
			gameObjectToShow.SetActive (false);
		}
	}
	
	void OnMouseDown ()
	{
		if (itemIsOn)
		{
			if (clickFunction == ClickFunction.MapClick)
			{
				om.StartCoroutine ("GoToMuseum");
				StopPulse ();
			}
			else if (clickFunction == ClickFunction.PhoneClick)
			{
				om.phone.SlideIn ();
				StopPulse ();
			}
		}
	}

	void OnMouseEnter ()
	{
		startTime = Time.time;

		if (itemIsOn && !om.phone.onScreen)
		{
			mr.enabled = true; // turn on text

			if (gameObjectToShow != null)
			{
				gameObjectToShow.SetActive (true); // show big version of item
			}
		}
	}

	void OnMouseExit ()
	{
		float waited = Time.time - startTime;
		// don't stop pulse for items that can be clicked or when phone is onscreen
		if (waited >= hoverTime && clickFunction == ClickFunction.None && !om.phone.onScreen)
		{
			StopPulse ();
			if (gameObject == om.heistInfo.gameObject)
			{
				om.map.TurnOn ();
				om.map.Pulse ();
			}
		}

		mr.enabled = false; // turn off the text

		if (gameObjectToShow != null)
		{
			gameObjectToShow.SetActive (false);
		}
	}

	public void TurnOn ()
	{
		itemIsOn = true;
		highlight.color = Color.black;
		highlight.enabled = true;
		highlight.DOColor (highlightColor, 0.3f);
	}

	public void TurnOff ()
	{
		itemIsOn = false;
		mr.enabled = false; // turns off text
	}

	public void Pulse ()
	{
		highlight.color = pulseColor;
		pulseTween = highlight.DOColor (Color.black, pulseTime).SetLoops (-1, LoopType.Yoyo);
	}

	void StopPulse ()
	{
		if (pulseTween != null)
		{
			pulseTween.Kill ();
			highlight.color = highlightColor;
		}
	}
}
