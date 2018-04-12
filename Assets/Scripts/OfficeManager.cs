using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class OfficeManager : MonoBehaviour
{
	public Image fadeImage;
	public Text scoreText;
	public GameObject howToPlayGO;
	public GameObject toolsGO;
	public GameObject paintingGO;
	public GameObject heistGO;
	public GameObject phone;
	[HideInInspector]
	public static clickItem;
	
	static enum Clickable
	{
		None,
		HowToPlay,
		Heist,
		GoToMuseum,
		Flashlight,
		Mask,
		Painting
	}
	
	void Start ()
	{
		clickItem = Clickable.None;
		scoreText.text = GameManager.score.ToString ("C");
		
		StartCoroutine (FadeIn);
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			ClickHandler ();
		}
	}
	
	IEnumerator FadeIn ()
	{
		Tween fadeIn = fadeImage.DOFade (1f, 2f); // fade alpha to 1 in 2s;
		yield return fadeIn.WaitForCompletion ();
		EnteredOffice ();
	}
	
	void EnteredOffice ()
	{
		if (!GameManager.levelStarted)
		{
			// show phone with new painting to steal msgs
		}
		else
		{
			if (GameManager.carriedPainting == null) // player didn't grab a painting
			{
				Debug.Log ("You forgot to grab the painting!");
				// show phone with msg saying you forgot to grab a painting
				// wait for phone to close (be clicked)
				// GameManager.GoToMuseum ();
			}
			else // player grabbed a painting
			{
				int points = GameManager.CollectPoints ();
				string dollars = points.ToString ("C");
				if (points < 500) // player grabbed wrong painting
				{
					Debug.Log ("Oops. You got " + dollars);
					// show phone with msg that you grabbed the wrong painting
					// wait for phone to close (be clicked)
					// GameManager.NextLevel ();
				}
				else
				{
					Debug.Log ("Yay. You got " + dollars);
					// show phone with msg that you grabbed the correct painting
					// wait for phone to close (be clicked)
					// GameManager.NextLevel ();
				}
			}
		}
	}
	
	void TogglePhone ()
	{
		
	}
	
	void ClickHandler ()
	{
		switch (clickItem)
		{
			case Clickable.None:
				break; // do nothing
			case Clickable.ToggleHowToPlay:
				ToggleImage (howToPlayGO);
				break;
			case Clickable.Hesit:
				ToggleImage (heistGO);
				break;
			case Clickable.GoToMuseum:
				StartCoroutine (GoToMuseum);
				break;
			case Clickable.Flashlight:
				ToggleImage (toolsGO);
				break;
			case Clickable.Mask ();
				ToggleImage (toolsGO);
				break;
			case Clickable.Painting:
				ToggleImage (paintingGO);
				break;
		}
	}
	
	void ToggleImage (GameObject img)
	{
		SpriteRenderer sr = img.GetComponent<SpriteRenderer> ();
		float toAlpha = sr.color.alpha < 1 ? 1f : 0f;
		sr.DOFade (toAlpha, 1f);
	}
	
	IEnumerator GoToMuseum ()
	{
		Tween fadeOut = fadeImage.DOFade (0f, 1f); // fade alpha to 0 in 1s
		yield return fadeOut.WaitForCompletion ();
		GameManager.GoToMuseum ();
	}
}