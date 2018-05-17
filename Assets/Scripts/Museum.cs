using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu (fileName = "Museum_01", menuName = "Museum")]
public class Museum : ScriptableObject
{
	public new string name = "Museum Name";
	public string level = "Level_00";
	public Painting picToSteal; // target painting to steal
	public int pointsToSteal; // how much money for painting to steal
	public Painting[] picsInMuseum; // stores all paintings used in this level
	public string[] newHeistMsgs; // text messages - first 3 are client, 4 is jacques
	public string[] forgotItMsgs;
	public string[] gotItMsgs;
	public string[] wrongOneMsgs;
	
}
