using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //设定放大倍数
    public float zoomSize;
    
    //这里的函数名必须这个才能正常接入鼠标事件
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //鼠标位于按钮上方时放大按钮
        transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //离开时变回原来大小
        transform.localScale = Vector3.one;
    }
}
