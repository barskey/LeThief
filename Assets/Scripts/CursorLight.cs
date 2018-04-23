using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLight : MonoBehaviour {

	void Update () {
		
		Vector2 target = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector3 (target.x, target.y, transform.position.z);
	}
}
