using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class ScrollButton : MonoBehaviour,IPointerClickHandler
{
    public GameObject Scrollbar;
    private int switchState = 1;
    private float localValue = 0;
    Tween tweener;
    bool isComplete = true;

    public  UnityEvent<bool> OnCompleted = new UnityEvent<bool>();

    private Vector3 InitPoisition = Vector3.zero;
    private Vector3 EndPoisition = Vector3.zero;
    private void Awake()
    {
        switchState = -1;
        InitPoisition.x = Scrollbar.transform.localPosition.x;
        EndPoisition.x = -Scrollbar.transform.localPosition.x;
        ChangeState(1);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isComplete)
            return;
        tweener = Scrollbar.transform.DOLocalMoveX(-Scrollbar.transform.localPosition.x, 0.2f);
        switchState = Math.Sign(-Scrollbar.transform.localPosition.x);
        isComplete = false;
        tweener.OnComplete(() =>
        {
            isComplete = true;
           
            //Debug.LogError(switchState);
            OnCompleted?.Invoke(switchState==1?true:false);
          
        });

    }

    public void ChangeState(int state)
    {
        if (state==-1)
        {
            tweener = Scrollbar.transform.DOLocalMoveX(InitPoisition.x, 0.0f);
        }
        else
        {
            tweener = Scrollbar.transform.DOLocalMoveX(EndPoisition.x, 0.0f);
        }
        switchState = state;
    }

}
