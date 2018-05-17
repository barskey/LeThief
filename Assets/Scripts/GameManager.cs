using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	public Museum[] museums; // stores array of Museum data containers with info on each level (museum) to load
	public Text highScoreText;
	[HideInInspector]
	public Painting carriedPainting;
	[HideInInspector]
	public int score = 0;
	public string[] newLevelTexts;
	[HideInInspector]
	public string newLevelText;

	public enum LevelState
	{
		NewGame,
		NewLevel,
		ForgotPainting,
		WrongPainting,
		RightPainting
	}

	public LevelState levelState;

	private int museumIndex = 0;
	
	void Awake ()
	{
		if (instance == null) // check if instance already exists
		{
			instance = this; // if not, set instance to this
		}
		else if (instance != this) // if it already exists and it's not this
		{
			Destroy (gameObject); // then destroy this
		}
		
		DontDestroyOnLoad (gameObject); // sets to not be destroyed when reloading scene
		
		InitGame ();
	}
	
	void InitGame ()
	{
		// load high score
		int highScore = LoadHighScore ();
		// set high score text
		highScoreText.text = highScore.ToString ("C0");
	}
	
	int LoadHighScore ()
	{
		if (PlayerPrefs.HasKey ("highScore"))
		{
			return PlayerPrefs.GetInt ("highScore");
		}
		return 0;
	}
	
	public void NewGame ()
	{
		museumIndex = 0;
		score = 0;
		carriedPainting = null;
		levelState = LevelState.NewGame;
		newLevelText = "Meanwhile, at the Office...";
		GoToOffice ();
	}
	
	public void GoToOffice ()
	{
		if (carriedPainting == null)
		{
			if (levelState != LevelState.NewGame || levelState != LevelState.NewLevel)
			{
				levelState = LevelState.ForgotPainting;
			}
		}
		else
		{
			levelState = CheckPainting () ? LevelState.RightPainting : LevelState.WrongPainting;
		}

		SceneManager.LoadScene ("Levels/Office");
	}

	public bool CheckPainting ()
	{
		if (carriedPainting == museums [museumIndex].picToSteal)
		{
			return true;
		}
		else
		{
			return false;
		}

	}
	
	public int CollectPoints ()
	{
		int points;

		if (!CheckPainting ()) // wrong painting
		{
			points = Random.Range (1, 6) * 50; // gives score bet 50 and 300
		}
		else
		{
			points = museums [museumIndex].pointsToSteal;
		}
		score += points;
		return points;
	}
	
	public void GoToMuseum ()
	{
		if (museumIndex >= museums.Length)
		{
			Debug.Log ("No more levels. To Be continued.");
			//SceneManager.LoadScene ("Levels/Level_TBC"); // load to be contined
		}
		else
		{
			SceneManager.LoadScene ("Levels/" + museums [museumIndex].level);
		}
	}
	
	public void NextLevel ()
	{
		carriedPainting = null;
		museumIndex++;
		levelState = LevelState.NewLevel;
		int index = Random.Range (1, newLevelTexts.Length - 1);
		newLevelText = newLevelTexts [index];
		GoToOffice ();
	}

	public Museum GetCurrentLevel ()
	{
		return museums [museumIndex];
	}
	
	public void GameOver ()
	{
		Debug.Log ("Caught you!");
		Time.timeScale = 0; // freeze the game
		
		// save high score
		if (score > LoadHighScore ())
		{
			PlayerPrefs.SetInt ("highScore", score);
		}
		// play jailbar sound
		// show jailbars, restart button
		// save score if higher than high score
	}
	
}