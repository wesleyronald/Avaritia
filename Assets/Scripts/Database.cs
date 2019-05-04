using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
	public ScriptablePlayer player;
	public int playerStartingCurrency = 1000;
	public List <ScriptableItem> playerStartingItems = new List <ScriptableItem>();
	public ScriptableItem[] items;
	public ScriptableNPC[] Npcs;
	public GameObject UiItemPrefab;
	public static Database instance = null;

	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
			player.currency = playerStartingCurrency;
			player.items = playerStartingItems;
		}else if (instance != this)
		{
			Destroy(this);
		}
	}
}