using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
	[Range(0, 100)]
	public int chanceToCounterOffer = 40;
	[Range(0f, 1f)]
	public float counterOfferChanceRandomizerRate = 0.2f;
	[Range(0, 100)]
	public int chanceToGetAngry = 40;
	[Range(0f, 1f)]
	public float getAngryChanceRandomizerRate = 0.2f;
	public float minGoodValueRate = 0.75f, maxGoodValueRate = 1.5f , minValueRate = 0.5f, maxValueRate = 2f;
	public InputField inputField;
	public Image portraitPanel;
	public GameObject bagButton;
	public GameObject bagScrollView;
	public Text lifeCurrency;

	public static DialogueController instance;

	private Typewriter typewriter;
	private IEnumerator coroutine;
	private int itemId, npcId, priceOffered;
	private string dialogueState, itemOfferedName = "";

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		typewriter = GetComponent<Typewriter> ();
		coroutine = Dialogue ();
		StartCoroutine (coroutine);
		lifeCurrency.text = Database.instance.player.currency.ToString();
	}

	public void Offer()
	{
		priceOffered = int.Parse (inputField.text);
		if (itemOfferedName == "")
		{
			if (priceOffered <= Database.instance.player.currency)
			{
				inputField.gameObject.SetActive (false);
			}
		} else
		{
			inputField.gameObject.SetActive (false);
		}
	}

	public void StopDialogue()
	{
		StopCoroutine (coroutine);
		dialogueState = "";
		gameObject.SetActive (false);
	}

	public void OfferItem(string newOffer)
	{
		itemOfferedName = newOffer;
	}

	private IEnumerator Dialogue()
	{
		itemId = Random.Range (0, Database.instance.items.Length);
		npcId = Random.Range (0, Database.instance.Npcs.Length);
		portraitPanel.sprite = Database.instance.Npcs [npcId].portrait;
		portraitPanel.gameObject.SetActive (portraitPanel.sprite != null);
		dialogueState = Random.Range (0, 2) == 0 ? "buy" : "sell";
		typewriter.Write (itemId, dialogueState, npcId);
		while (!typewriter.Ready())
		{
			yield return null;
		}
		if (dialogueState == "buy")
		{
			//substitute by cancel variable controlled by button
			if (true)
			{
				do
				{
					priceOffered = -1;
					bagButton.SetActive (true);
					while (priceOffered < 0)
					{
						yield return null;
					}
					if (itemOfferedName != Database.instance.items [itemId].name)
					{
						if (Random.Range (0, 10) < 4)
						{
							dialogueState = "otherItemRefuse";
							typewriter.Write (itemId, dialogueState, npcId);
							while (!typewriter.Ready())
							{
								yield return null;
							}
							StopDialogue();
						}else
						{
							dialogueState = "otherItemAccept";
							typewriter.Write (itemId, dialogueState, npcId);
							while (!typewriter.Ready())
							{
								yield return null;
							}
						}
					}
					int value = Database.instance.items[itemId].value;
					float randomRate = Random.Range(0.8f, 1.2f);
					if (priceOffered >= value * minValueRate * randomRate)
					{
						if (priceOffered >= value * minGoodValueRate * randomRate)
						{
							dialogueState = "acceptHappy";
							Database.instance.player.currency += priceOffered;
							lifeCurrency.text = Database.instance.player.currency.ToString();
							Database.instance.player.items.Remove(Database.instance.items[itemId]);
						}else
						{
							float randomChance = chanceToCounterOffer * Random.Range(1f - counterOfferChanceRandomizerRate, 1f + counterOfferChanceRandomizerRate);
							if (Random.Range(0f,100f) > randomChance)
							{
								Database.instance.player.currency += priceOffered;
								lifeCurrency.text = Database.instance.player.currency.ToString();
								Database.instance.player.items.Remove(Database.instance.items[itemId]);
								randomChance = chanceToGetAngry * Random.Range(1f - getAngryChanceRandomizerRate, 1f + getAngryChanceRandomizerRate);
								if (Random.Range(0f,100f) > randomChance)
								{
									dialogueState = "acceptNeutral";
								}else
								{
									dialogueState = "acceptUnhappy";
								}
							}else
							{
								dialogueState = "refuseNeutral";
							}
						}
					}else
					{
						dialogueState = "refuseUnhappy";
					}
					yield return null;
				} while (dialogueState == "refuseNeutral" || dialogueState == "");
			} else
			{
				//dialogueState = "";
				//missing dialogue
			}
			typewriter.Write (itemId, dialogueState, npcId);
			while (!typewriter.Ready())
			{
				yield return null;
			}
			StopDialogue();
		} else
		{
			inputField.text = "";
			inputField.gameObject.SetActive (true);
			priceOffered = -1;
			itemOfferedName = "";
			while (priceOffered < 0)
			{
				yield return null;
			}

			int value = Database.instance.items[itemId].value;
			float randomRate = Random.Range(0.8f, 1.2f);
			if (priceOffered >= value * minValueRate * randomRate)
			{
				if (priceOffered >= value * minGoodValueRate * randomRate)
				{
					dialogueState = "acceptHappy";
					Database.instance.player.currency -= priceOffered;
					lifeCurrency.text = Database.instance.player.currency.ToString();
					Database.instance.player.items.Add(Database.instance.items[itemId]);
				}else
				{
					float randomChance = chanceToCounterOffer * Random.Range(1f - counterOfferChanceRandomizerRate, 1f + counterOfferChanceRandomizerRate);
					if (Random.Range(0f,100f) > randomChance)
					{
						Database.instance.player.currency -= priceOffered;
						lifeCurrency.text = Database.instance.player.currency.ToString();
						Database.instance.player.items.Add(Database.instance.items[itemId]);
						randomChance = chanceToGetAngry * Random.Range(1f - getAngryChanceRandomizerRate, 1f + getAngryChanceRandomizerRate);
						if (Random.Range(0f,100f) > randomChance)
						{
							dialogueState = "acceptNeutral";
						}else
						{
							dialogueState = "acceptUnhappy";
						}
					}else
					{
						dialogueState = "refuseNeutral";
					}
				}
			}else
			{
				dialogueState = "refuseUnhappy";
			}
		}
	}
}