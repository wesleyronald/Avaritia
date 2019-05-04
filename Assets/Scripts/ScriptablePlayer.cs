using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObject/Player")]
public class ScriptablePlayer : ScriptableObject
{
	public List <ScriptableItem> items = new List <ScriptableItem>();
	public int currency = 0;
}