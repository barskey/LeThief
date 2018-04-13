using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OfficeManager : MonoBehaviour
{
	public Image fadeImage;
	public Text scoreText;
	public GameObject phone;

	GameManager gm;

	void Awake ()
	{
		gm = Object.FindObjectOfType<GameManager> ();
	}

	void Start ()
	{
		scoreText.text = gm.score.ToString ("C0");

		// set the fadeimage to active and alpha 1
		fadeImage.gameObject.SetActive (true);
		Color c = fadeImage.color;
		c.a = 1f;
		fadeImage.color = c;
		
		StartCoroutine ("FadeIn");
	}

	IEnumerator FadeIn ()
	{
		Tween fadeIn = fadeImage.DOFade (0f, 2f); // fade alpha to 0 in 2s;
		yield return fadeIn.WaitForCompletion ();
		EnteredOffice ();
	}
	
	void EnteredOffice ()
	{
		if (!gm.levelStarted)
		{
			Debug.Log ("Level not started. Show phone.");
			// show phone with new painting to steal msgs
		}
		else
		{
			if (gm.carriedPainting == null) // player didn't grab a painting
			{
				Debug.Log ("You forgot to grab the painting!");
				// show phone with msg saying you forgot to grab a painting
				// wait for phone to close (be clicked)
				// GameManager.GoToMuseum ();
			}
			else // player grabbed a painting
			{
				int points = gm.CollectPoints ();
				string dollars = points.ToString ("C0");
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
	
	IEnumerator GoToMuseum ()
	{
		Tween fadeOut = fadeImage.DOFade (0f, 1f); // fade alpha to 0 in 1s
		yield return fadeOut.WaitForCompletion ();
		gm.GoToMuseum ();
	}
}