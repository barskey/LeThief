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
	public Text clickText;
	[HideInInspector]
	public bool onScreen = false;

	OfficeManager om;
	float offsetX;
	int msgCount;
	Vector3[] msgStartPos;
	bool msgsDone = true;

	void Awake ()
	{
		om = Object.FindObjectOfType<OfficeManager> ();
		offsetX = transform.position.x;

		msgStartPos = new Vector3[msgBubbles.Length];

		for (int i = 0; i < msgBubbles.Length; i++)
		{
			msgStartPos[i] = msgBubbles [i].transform.position;
		}
	}

	void Update ()
	{
		if (onScreen && msgsDone)
		{
			if (Input.GetMouseButtonDown (0))
			{
				SlideOut ();
			}
		}
	}

	public void SlideIn ()
	{
		onScreen = true;
		msgsDone = false;
		transform.DOLocalMoveX (0f, slideInSecs).SetEase (Ease.OutBack).OnComplete (SlideMsgs);
	}

	public void SlideOut ()
	{
		transform.DOLocalMoveX (offsetX, slideInSecs).SetEase (Ease.InBack).OnComplete (Reset);
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
			// first delay is shorter than other delay times
			float waitSecs = i == 0 ? Random.Range (0.5f, 1.5f) : Random.Range (1f, 3f);
			seq.AppendInterval (waitSecs);
			seq.AppendCallback (()=>ShowBubble (bubble, true));
			seq.Append (msgBubbles[i].transform.DOLocalMoveY (msgYPositions[i], 0.5f).SetEase (Ease.OutBack));
		}
		seq.AppendCallback (()=>ShowClickText (true));
		seq.Play ();
	}

	void Reset ()
	{
		int i = 0;
		foreach (GameObject msg in msgBubbles)
		{
			ShowBubble (msg, false);
			msg.transform.position = msgStartPos [i];
		}
		ShowClickText (false);
		onScreen = false;
		om.PhoneCallback ();
	}

	void ShowClickText (bool state)
	{
		msgsDone = true;
		clickText.GetComponent<Text> ().enabled = state;
	}

	void ShowBubble (GameObject bubble, bool state)
	{
		bubble.GetComponent<Image> ().enabled = state;
		bubble.GetComponentInChildren<Text> ().enabled = state;
		// TODO play sound at start of each msg
	}
}