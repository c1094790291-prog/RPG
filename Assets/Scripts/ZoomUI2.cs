using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomUI2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float zoomSize;//设定放大倍数
    public float moveUpDistance;//设定上移距离

    private Vector3 originalPosition; // 记录卡牌初始位置
    private int originalSiblingIndex;//记录卡牌层级
    private GridLayoutGroup parentGridLayout; // 父对象的GridLayoutGroup组件
    private bool isGridLayoutEnabled; // 记录GridLayoutGroup的初始启用状态

    // 初始化获取父对象的GridLayoutGroup
    private void Awake()
    {
        // 获取父对象的GridLayoutGroup组件
        parentGridLayout = transform.parent.GetComponent<GridLayoutGroup>();

        // 容错处理：如果父对象没有GridLayoutGroup，尝试向上查找
        if (parentGridLayout == null)
        {
            parentGridLayout = transform.parent.GetComponentInParent<GridLayoutGroup>();
        }

        // 记录初始状态（防止重复操作）
        if (parentGridLayout != null)
        {
            isGridLayoutEnabled = parentGridLayout.enabled;
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] 未找到父对象的GridLayoutGroup组件！");
        }
    }
    //这里的函数名必须这个才能正常接入鼠标事件
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // 如果找到GridLayoutGroup，暂停自动布局
        if (parentGridLayout != null && parentGridLayout.enabled)
        {
            parentGridLayout.enabled = false;
        }
        //鼠标位于按钮上方时放大按钮
        transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
        //记录原位置
        originalPosition = transform.localPosition;
        //记录层级
        originalSiblingIndex = transform.GetSiblingIndex();
        //上移
        transform.localPosition = new Vector3(
            originalPosition.x, // X轴保持不变
            originalPosition.y + moveUpDistance, // Y轴上移
            originalPosition.z  // Z轴保持不变
        );
        //移动到最顶层
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //离开时变回原来大小
        transform.localScale = Vector3.one;
        //回到原来位置
        transform.localPosition = originalPosition;
        //恢复层级
        transform.SetSiblingIndex(originalSiblingIndex);
        // 恢复GridLayoutGroup的自动布局（只恢复初始启用的）
        if (parentGridLayout != null && isGridLayoutEnabled)
        {
            parentGridLayout.enabled = true;
        }
    }
    // 防止对象被销毁时布局未恢复
    private void OnDestroy()
    {
        if (parentGridLayout != null && isGridLayoutEnabled && !parentGridLayout.enabled)
        {
            parentGridLayout.enabled = true;
        }
    }
}
