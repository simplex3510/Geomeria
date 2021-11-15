using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform startParent;
    Image perkIcon;

    private void Awake()
    {
        perkIcon = GetComponent<Image>();
        #region perkIcon alpha값 처리
        if (perkIcon.sprite == null)
        {
            Color temp = new Color(0, 0, 0, 0);
            perkIcon.color = temp;
            // perkIcon.color.a = 0f; 가 왜 안 되는지 질문하기
        }
        #endregion

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        transform.SetParent(GameObject.FindGameObjectWithTag("UI Canvas").transform);
        startParent.localScale = Vector3.one;

        GetComponent<Image>().raycastTarget = false;
        eventData.selectedObject = this.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.selectedObject != null)
        {
            transform.SetParent(startParent);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            GetComponent<Image>().raycastTarget = true;
        }
        
    }
}
