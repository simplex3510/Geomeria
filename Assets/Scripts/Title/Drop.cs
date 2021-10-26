using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
	public void OnDrop(PointerEventData eventData)
	{
		// Debug.Log("OnDrop item :" + eventData.selectedObject.name);
		if (eventData.selectedObject)
		{
			eventData.selectedObject.transform.SetParent(this.transform);
			eventData.selectedObject.transform.localPosition = Vector3.zero;
			eventData.selectedObject.transform.localScale = Vector3.one;
			eventData.selectedObject.GetComponent<Image>().raycastTarget = true;

			eventData.selectedObject = null;
		}
	}

}
