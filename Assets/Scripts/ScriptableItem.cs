using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeholder Item", menuName = "ScriptableObject/Item")]
public class ScriptableItem : ScriptableObject 
{
	public new string name = "Placeholder Name";
	public int value;
	public Color quality = Color.black;
	public Sprite icon;

	private GameObject uiObject;

	public void UiObject(GameObject newUiObject)
	{
		uiObject = newUiObject;
	}

	public GameObject UiObject()
	{
		return uiObject;
	}
}