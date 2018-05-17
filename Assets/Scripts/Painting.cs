using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Pic", menuName = "Painting")]
public class Painting : ScriptableObject
{
	public new string name = "Picture Title"; // used for scoring; make sure this is unique
	public Sprite smSprite; // for showing when flashlight is over
	public Sprite bigSprite; // for Heist poster in Office
	
}
