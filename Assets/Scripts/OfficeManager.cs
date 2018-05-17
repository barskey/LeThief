using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OfficeManager : MonoBehaviour
{
	public Image fadeImage;
	public Text scoreText;
	public Text levelText;
	public PhoneController phone;
	public OfficeItem heistInfo;
	public HeistPoster heistPoster;
	public OfficeItem map;
	public OfficeItem howToPlay;
	public OfficeItem deskPhone;
	public OfficeItem[] otherItems;

	GameManager gm;
	int currentScore;
	int displayedScore;

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

		PlayLevelText ();
	}

	void Update ()
	{
		scoreText.text = displayedScore.ToString ("C0");
	}

	void PlayLevelText ()
	{
		// TODO Make a sequence that will fade out at the end
		Sequence seq = DOTween.Sequence ();
		seq.Append (levelText.DOText (gm.newLevelText, 3f, false));
		Color fadeToColor = levelText.color;
		fadeToColor.a = 0f;
		seq.Append (DOTween.To (() => levelText.color, x => levelText.color = x, fadeToColor, 0.5f));
		seq.AppendCallback (()=>FadeIn());
		seq.Play ();
	}
		
	void FadeIn ()
	{
		fadeImage.DOFade (0f, 2f).OnComplete (EnterOffice); // fade alpha to 0 in 2s;
	}
	
	void EnterOffice ()
	{
		heistPoster.UpdateInfo (gm.GetCurrentLevel ().picToSteal, gm.GetCurrentLevel ().pointsToSteal);

		howToPlay.TurnOn ();
		deskPhone.TurnOn ();
		deskPhone.Pulse ();

		switch (gm.levelState)
		{
		case GameManager.LevelState.NewGame:
			phone.SetMsgs (gm.GetCurrentLevel ().newHeistMsgs);
			howToPlay.Pulse ();
			break;
		case GameManager.LevelState.NewLevel:
			phone.SetMsgs (gm.GetCurrentLevel ().newHeistMsgs);
			break;
		case GameManager.LevelState.ForgotPainting:
			phone.SetMsgs (gm.GetCurrentLevel ().forgotItMsgs);
			break;
		case GameManager.LevelState.WrongPainting:
			phone.SetMsgs (gm.GetCurrentLevel ().wrongOneMsgs);
			break;
		case GameManager.LevelState.RightPainting:
			phone.SetMsgs (gm.GetCurrentLevel ().gotItMsgs);
			break;
		}
	}

	IEnumerator GoToMuseum ()
	{
		Tween fadeOut = fadeImage.DOFade (1f, 1f); // fade alpha to 1 in 1s
		yield return fadeOut.WaitForCompletion ();
		gm.GoToMuseum ();
	}

	// called from office manager after phone has displayed text msgs and slides offscreen
	public void AfterPhoneSlideOut ()
	{
		foreach (OfficeItem item in otherItems)
		{
			item.TurnOn ();
		}

		switch (gm.levelState)
		{
		case GameManager.LevelState.NewGame:
			gm.levelState = GameManager.LevelState.NewLevel;
			heistInfo.TurnOn ();
			heistInfo.Pulse ();
			deskPhone.TurnOff ();
			break;
		case GameManager.LevelState.NewLevel:
			heistInfo.TurnOn ();
			heistInfo.Pulse ();
			deskPhone.TurnOff ();
			break;
		case GameManager.LevelState.ForgotPainting:
			heistInfo.TurnOn ();
			heistInfo.Pulse ();
			map.TurnOn ();
			map.Pulse ();
			deskPhone.TurnOff ();
			break;
		case GameManager.LevelState.WrongPainting:
			AddScore (gm.CollectPoints ());
			break;
		case GameManager.LevelState.RightPainting:
			AddScore (gm.CollectPoints ());
			break;
		}
	}

	void AddScore(int points)
	{
		currentScore += points;
		DOTween.To (() => displayedScore, x => displayedScore = x, currentScore, 1).OnComplete (gm.NextLevel);
		//scoreText.rectTransform.DOPunchScale (new Vector3 (1.1f, 1.1f, 1f), 1f, 1); // doesn't look right yet
	}
}