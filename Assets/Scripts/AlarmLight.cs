using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AlarmLight : MonoBehaviour {

	public Color alarmLow;
	public Color alarmHigh;
	public float pulseTime = 2f;

	// Use this for initialization
	void Start () {
		Light lightComp = GetComponent<Light> ();
		lightComp.color = alarmLow;
		lightComp.DOColor (alarmHigh, pulseTime).SetLoops (-1, LoopType.Yoyo);
	}
}
