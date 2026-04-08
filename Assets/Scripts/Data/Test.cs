using AssetBundleFormWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test :EnventOnPoint
{

  
    //    private RectTransform rectTransform;
    //    void Start()
    //    {
    //        rectTransform = GetComponent<RectTransform>();
    //    }


    //    public GameObject go;
    //    private Vector3 mousePos;                       //鼠标初始位置
    //    private Vector3 pos;                            //控件初始位置
    //    public void Init()
    //    {

    //    }
    //    public override void OnBeginDrag(PointerEventData eventData)
    //    {
    //        base.OnBeginDrag(eventData);
    //        Debug.Log("开始拖拽");
    //        pos = this.GetComponent<RectTransform>().position;
    //        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out mousePos);
    //    }

    //    public override void OnDrag(PointerEventData eventData)
    //    {
    //        Vector3 newVec;
    //        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out newVec);
    //        Vector3 offset = new Vector3(newVec.x - mousePos.x, newVec.y - mousePos.y, 0);
    //        rectTransform.position = pos + offset;

    //    }
    //    public override void OnEndDrag(PointerEventData eventData)
    //    {
    //        Debug.Log("结束拖拽");
    //    }

    //    /***
    //     * 
    //     * PointerClick：在UI元素上点下在抬起时执行。
    //     * PointerDown：在UI元素上点下执行。
    //     */
    //    public override void OnPointerClick(PointerEventData eventData)
    //    {
    //        rectTransform.position = new Vector3(400, 0, 0);
    //    }

    //    public override void OnPointerDown(PointerEventData eventData)
    //    {
    //        rectTransform.position = pos;
    //    }
}
