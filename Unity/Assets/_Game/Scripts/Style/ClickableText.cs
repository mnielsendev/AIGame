using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class ClickableText : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		var text = GetComponent<TextMeshProUGUI>();
		if (eventData.button == PointerEventData.InputButton.Left) // Could replace this with input manager
		{
			int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
			// When an actual link is clicked
			if (linkIndex > -1)
			{
				// textInfo has all the TMP data for a given text
				var linkInfo = text.textInfo.linkInfo[linkIndex];
				var linkId = linkInfo.GetLinkID();
				Debug.Log("Clicked " + linkId);

				// TODO
				//access the object via it's ID from a list/arry of all possible spwnable objects
				//get object's available interactions
				//display them as UI


				//var objectData = FindObjectOfType<WorldObject>().ID;

				
			}
		}
	}

}
