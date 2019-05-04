using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour 
{
	public void Offer()
	{
		DialogueController.instance.OfferItem(name);
		DialogueController.instance.bagScrollView.SetActive (false);
		DialogueController.instance.bagButton.SetActive (false);
		DialogueController.instance.inputField.text = "";
		DialogueController.instance.inputField.gameObject.SetActive (true);
	}
}