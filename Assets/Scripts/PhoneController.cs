using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PhoneController : MonoBehaviour
{
	public GameObject[] msgBubbles;
	public float[] msgYPositions;
	public float slideInSecs = 0.5f;
	[HideInInspector]
	public bool onScreen = false;

	OfficeManager om;
	float offsetX;
	int msgCount;

	void Awake ()
	{
		om = Object.FindObjectOfType<OfficeManager> ();
		offsetX = transform.position.x;
	}

	public void SlideIn ()
	{
		transform.DOLocalMoveX (0f, slideInSecs).SetEase (Ease.OutBack);
	}

	public void SlideOut ()
	{
		transform.DOLocalMoveX (offsetX, slideInSecs).SetEase (Ease.InBack);
	}

	public void SetMsgs (string[] textMsgs)
	{
		msgCount = textMsgs.Length;
		for (int i = 0; i < msgCount; i++)
		{
			msgBubbles [i].GetComponentInChildren<Text> ().text = textMsgs [i];
		}
	}

	public void SlideMsgs ()
	{
		Sequence seq = DOTween.Sequence ();
		for (int i = 0; i < msgCount; i++)
		{
			GameObject bubble = msgBubbles [i];
			seq.AppendInterval (Random.Range (1f, 3f));
			seq.AppendCallback (()=>ShowBubble (bubble, true));
			seq.Append (msgBubbles[i].transform.DOLocalMoveY (msgYPositions[i], 0.5f).SetEase (Ease.OutBack));
		}
		seq.Play ();
	}

	void ShowBubble (GameObject bubble, bool state)
	{
		bubble.GetComponent<Image> ().enabled = state;
		bubble.GetComponentInChildren<Text> ().enabled = state;
	}
}