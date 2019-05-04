using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollItens : MonoBehaviour 
{
    private ScrollRect scrollRect;
    private RectTransform contentRect;
	private VerticalLayoutGroup layout;
    private int childCount;
    private float height, scrollViewHeight;
    private Vector2 newAnchoredPosition;

	// Use this for initialization
	void Start () 
    {
		contentRect = gameObject.GetComponent<RectTransform>();
		foreach (ScriptableItem item in Database.instance.player.items)
		{
			item.UiObject (Instantiate (Database.instance.UiItemPrefab, contentRect.transform) as GameObject);
			item.UiObject().name = item.name;
			item.UiObject().GetComponent<Image> ().sprite = item.icon;
			Text text = item.UiObject().GetComponentInChildren<Text> ();
			text.color = item.quality;
			text.text = item.name;
		}
		childCount = transform.childCount;
        scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
        scrollViewHeight = scrollRect.GetComponent<RectTransform>().rect.height;
        layout = GetComponent<VerticalLayoutGroup>();
        height = layout.padding.top + layout.padding.bottom + ((layout.spacing + 200f) * childCount);
        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (transform.childCount != childCount)
        {
			childCount = transform.childCount;
            height = layout.padding.top + layout.padding.bottom + ((layout.spacing + 200f) * childCount);
            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            if (height <= scrollViewHeight)
            {
                scrollRect.movementType = ScrollRect.MovementType.Clamped;
            }else
            {
                scrollRect.movementType = ScrollRect.MovementType.Elastic;
            }
        }
	}
}