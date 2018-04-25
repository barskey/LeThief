using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OfficeManager : MonoBehaviour
{
	public Image fadeImage;
	public Text scoreText;
	public PhoneController phone;

	GameManager gm;
	int currentScore;
	int displayedScore;

	enum PaintingStatus
	{
		None,
		GotCorrectOne,
		GotWrongOne
	}

	PaintingStatus status = PaintingStatus.None;

	void Awake ()
	{
		gm = Object.FindObjectOfType<GameManager> ();
	}

	void Start ()
	{
		currentScore = gm.score;
		displayedScore = currentScore;
		scoreText.text = displayedScore.ToString ("C0");

		// set the fadeimage to active and alpha 1
		fadeImage.gameObject.SetActive (true);
		Color c = fadeImage.color;
		c.a = 1f;
		fadeImage.color = c;
		
		StartCoroutine ("FadeIn");
	}

	void Update ()
	{
		scoreText.text = displayedScore.ToString ("C0");
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
			Museum currentLevel = gm.GetCurrentLevel ();

			phone.onScreen = true; // disable all office item elements

			// show phone with new painting to steal msgs
			status = PaintingStatus.None; // for determining what to do in phonecallback
			phone.SetMsgs (currentLevel.newHeistMsgs);
			phone.SlideIn (); // slide in the phone and show the text messages
		}
		else
		{
			if (gm.carriedPainting == null) // player didn't grab a painting
			{
				Debug.Log ("You forgot to grab the painting!");

				// show phone with new painting to steal msgs
				status = PaintingStatus.None; // for determining what to do in phonecallback
				phone.SetMsgs (gm.GetCurrentLevel ().forgotItMsgs);
				phone.SlideIn (); // slide in the phone and show the text messages
			}
			else // player grabbed a painting
			{
				if (!gm.CheckPainting ()) // player grabbed wrong painting
				{
					Debug.Log ("Oops. You got wrong one.");
					status = PaintingStatus.GotWrongOne; // for determining what to do in phonecallback
					phone.SetMsgs (gm.GetCurrentLevel ().wrongOneMsgs);
					phone.SlideIn (); // slide in the phone and show the text messages
				}
				else
				{
					Debug.Log ("Yay. You got the right one!");

					// show phone with msgs that you got it
					status = PaintingStatus.GotCorrectOne; // for determining what to do in phonecallback
					phone.SetMsgs (gm.GetCurrentLevel ().gotItMsgs);
					phone.SlideIn (); // slide in the phone and show the text messages
				}
			}
		}
	}

	IEnumerator GoToMuseum ()
	{
		Tween fadeOut = fadeImage.DOFade (1f, 1f); // fade alpha to 1 in 1s
		yield return fadeOut.WaitForCompletion ();
		gm.GoToMuseum ();
	}

	// called from office manager after phone has displayed text msgs and slides offscreen
	public void PhoneCallback ()
	{
		if (status != PaintingStatus.None)
		{
			AddScore (gm.CollectPoints ());
		}
	}

	void NextLevel ()
	{
		gm.NextLevel ();
	}

	void AddScore(int points)
	{
		currentScore += points;
		DOTween.To (() => displayedScore, x => displayedScore = x, currentScore, 1).OnComplete (NextLevel);
		//scoreText.rectTransform.DOPunchScale (new Vector3 (1.1f, 1.1f, 1f), 1f, 1); // doesn't look right yet
	}
}