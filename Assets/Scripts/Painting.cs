using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Paintings/New Pic", menuName = "Painting")]
public class Painting : ScriptableObject {

	public new string name = "Picture Title";
	public Sprite bigSprite;
	
}
