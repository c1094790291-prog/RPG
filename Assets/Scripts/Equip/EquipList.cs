using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EquipList
{
    //遗物介绍文本
    public static string GetText(int comboNum)
    {
        string _text = "";
        switch (comboNum)
        {
            case 0:
                _text = "金剑\n战斗开始时，获得1点力量";
                break;
            case 1:
                _text = "银胸甲\n第一回合开始时，获得10点格挡";
                break;
            case 2:
                _text = "极速鞋\n战斗开始时，获得1点能量";
                break;
        }
        return _text;
    }
}
