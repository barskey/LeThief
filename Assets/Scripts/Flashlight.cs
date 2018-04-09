using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

	[HideInInspector]
	public Light lightComp;

	public bool isOn = false;

	void Awake()
	{
		lightComp = GetComponent<Light> ();
	}

	public void ToggleLight()
	{
		isOn = !isOn;
		lightComp.enabled = isOn;

		// TODO play click sound
	}

	public void SetDirection(Vector2 dir)
	{
		transform.localRotation = Quaternion.LookRotation (new Vector3 (dir.x, dir.y), Vector3.forward);
	}
}
