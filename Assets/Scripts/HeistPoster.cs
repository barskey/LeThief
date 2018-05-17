using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeistPoster : MonoBehaviour {

	public TextMesh titleText;
	public TextMesh pointsText;
	public SpriteRenderer painting;

	public void UpdateInfo (Painting ptg, int score)
	{
		titleText.text = ptg.name;
		pointsText.text = score.ToString ("C0");
		painting.sprite = ptg.bigSprite;
	}

}
