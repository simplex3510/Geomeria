using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    Image perkIcon;
    public void OnDrop(PointerEventData eventData)
    {
        perkIcon = GetComponentInChildren<Image>();
    }
}
