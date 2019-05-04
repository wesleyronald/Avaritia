using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Typewriter : MonoBehaviour 
{
	public float typingSpeed = 40f;
	public Text text;

	private char[] charArray;
	private WaitForSeconds delay;
	private IEnumerator coroutine;
	private string fullDialogue = "", itemName, colorHex;
	private bool completedTyping = false, ready = false;

	// Use this for initialization
	void Start () 
	{
		delay = new WaitForSeconds ( typingSpeed == 0f ? 0f: 1f/typingSpeed);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			if (completedTyping)
			{
				ready = true;
			} else
			{
				ShowAll ();
			}
		}
	}

	public bool Ready()
	{
		return ready;
	}

	public void Write(int itemId = -1, string dialogueState = "", int npcId = -1)
	{
		completedTyping = false;
		ready = false;
		switch (dialogueState)
		{
		case "buy":
			fullDialogue = Database.instance.Npcs[npcId].buy;
			break;
		case "sell":
			fullDialogue = Database.instance.Npcs[npcId].sell;
			break;
		case "acceptHappy":
			fullDialogue = Database.instance.Npcs[npcId].acceptHappy;
			break;
		case "acceptNeutral":
			fullDialogue = Database.instance.Npcs[npcId].acceptNeutral;
			break;
		case "acceptUnhappy":
			fullDialogue = Database.instance.Npcs[npcId].acceptUnhappy;
			break;
		case "refuseNeutral":
			fullDialogue = Database.instance.Npcs[npcId].refuseNeutral;
			break;
		case "refuseUnhappy":
			fullDialogue = Database.instance.Npcs[npcId].refuseUnhappy;
			break;
		case "otherItemAccept":
			fullDialogue = Database.instance.Npcs[npcId].otherItemAccept;
			break;
		case "otherItemRefuse":
			fullDialogue = Database.instance.Npcs[npcId].otherItemRefuse;
			break;
		case "gossip":
			fullDialogue = Database.instance.Npcs[npcId].gossip;
			break;
		default:
			fullDialogue = "";
			break;
		}
		coroutine = TypeText (itemId);
		StartCoroutine (coroutine);
	}

	private IEnumerator TypeText(int itemId = -1)
	{
		itemName = Database.instance.items [itemId].name;
		colorHex = "#" + ColorUtility.ToHtmlStringRGBA (Database.instance.items [itemId].quality) + ">";
		int position = fullDialogue.IndexOf ("@");
		text.text = fullDialogue.Replace ("@", itemName);

		charArray = text.text.ToCharArray ();
		text.text = "";

		int i = -1;
		foreach (char letter in charArray) {
			yield return delay;
			i++;
			if (position >= 0 && i >= position && i <= position + itemName.Length) {
				text.text += "<color=" + colorHex + letter + "</color>";
			} else {
				text.text += letter;
			}
		}
		completedTyping = true;
	}

	private void ShowAll()
	{
		StopCoroutine(coroutine);
		text.text = fullDialogue.Replace ("@", "<color="+colorHex+itemName+"</color>");
		completedTyping = true;
	}
}