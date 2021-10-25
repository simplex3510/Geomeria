using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    Transform startParent;
    RectTransform rectTransform;
    Image perkIcon;

    private void Awake()
    {
        perkIcon = GetComponent<Image>();
        #region alpha값 처리
        if(perkIcon.sprite == null)
        {
            Color temp = new Color(0, 0, 0, 0);
            perkIcon.color = temp;
            // perkIcon.color.a = 0f; 가 왜 안 되는지 질문하기
        }
        #endregion

        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        transform.SetParent(GameObject.FindGameObjectWithTag("UI Canvas").transform);
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.transform.position = eventData.position;
        Debug.Log("OnDrop");
    }

    public void OnEndDrag(PointerEventData eventData)
    { 
        transform.SetParent(startParent);
        transform.localPosition = Vector3.zero;

        Debug.Log("OnEndDrag");
    }
}
