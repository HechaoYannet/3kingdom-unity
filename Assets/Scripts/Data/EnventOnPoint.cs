using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class EnventOnPoint : MonoBehaviour,
    IPointerDownHandler,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,
    IBeginDragHandler, IDragHandler,IEndDragHandler
{
    #region 拖拽事件
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
       
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        
    }

    #endregion


    #region 点击事件
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }

    #endregion


    #region 按下
    public virtual void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        
    }
    #endregion
}
