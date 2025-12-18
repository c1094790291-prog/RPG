using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//这是管理界面上卡片数量显示的脚本
public class CardCounter : MonoBehaviour
{
    public Text counterText;//链接外部文本
    private int counter = 0;

    public bool SetCounter(int _value)
    {
        counter += _value;
        OnCounterChange();
        if (counter == 0)
        {
            //gameObject 是一个内置的属性，它默认指向当前脚本所挂载的游戏对象。
            Destroy(gameObject);
            return false;
        }
        return true;
    }

    private void OnCounterChange()
    {
        counterText.text = counter.ToString();
    }
}
