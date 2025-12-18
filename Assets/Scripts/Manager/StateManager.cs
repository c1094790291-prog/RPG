using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public GameObject Strength_State_Perfab;//力量状态图标预制体
    public GameObject Fire_State_Perfab;//燃烧状态图标预制体
    public GameObject Toxin_State_Perfab;//中毒状态图标预制体
    public GameObject Ele_State_Perfab;//雷电状态图标预制体

    //添加状态图标，接收状态id与添加位置，返回图标对象
    public GameObject AddState(int _state, Transform _stateLab)
    {
        GameObject NewState;
        switch (_state)
        {
            case 0://力量图标
                NewState = Instantiate(Strength_State_Perfab, _stateLab);
                break;
            case 1://燃烧图标
                NewState = Instantiate(Fire_State_Perfab, _stateLab);
                break;
            case 2://中毒图标
                NewState = Instantiate(Toxin_State_Perfab, _stateLab);
                break;
            case 3://雷电图标
                NewState = Instantiate(Ele_State_Perfab, _stateLab);
                break;
            default:
                NewState = Instantiate(Strength_State_Perfab, _stateLab);//默认图标
                break;
        }
        return NewState;
    }
}
