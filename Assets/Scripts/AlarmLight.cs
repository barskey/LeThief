﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AlarmLight : MonoBehaviour {

	public Color alarmLow;
	public Color alarmHigh;
	public float pulseTime = 2f;

	// Use this for initialization
	void Start () {
		Light light = GetComponent<Light> ();
		light.color = alarmLow;
		light.DOColor (alarmHigh, pulseTime).SetLoops (-1, LoopType.Yoyo);
	}
}
