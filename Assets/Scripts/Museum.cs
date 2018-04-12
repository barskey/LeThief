using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Museum_01", menuName = "Museum")]
public class Museum : ScriptableObject
{
	public new string name = "Museum Name";
	public Scene level; // which scene to load
	public Painting[] picsToSteal; // which painting(s) are worth money?
	public int[] pointsToSteal; // how much money for each painting to steal
	public Painting[] picsInMuseum; // stores all paintings used in this level
	public string[] newHeistMsgs; // text messages - format 'c:blah' or 'j:blah' for client or jacques
	public string[] forgotItMsgs;
	public string[] gotItMsgs;
	
}
