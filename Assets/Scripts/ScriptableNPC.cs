using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeholder NPC", menuName = "ScriptableObject/NPC")]
public class ScriptableNPC : ScriptableObject
{
	public string buy = "Placeholder buy";
	public string sell = "Placeholder sell";
	public string acceptHappy = "Placeholder acceptHappy";
	public string acceptNeutral = "Placeholder acceptNeutral";
	public string acceptUnhappy = "Placeholder acceptUnhappy";
	public string refuseNeutral = "Placeholder refuseNeutral";
	public string refuseUnhappy = "Placeholder refuseUnhappy";
	public string otherItemAccept = "Placeholder otherItemAccept";
	public string otherItemRefuse = "Placeholder otherItemRefuse";
	public string gossip = "Placeholder gossip";
	public Sprite portrait;
}