using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BattleCardState
{
    inHand, inBlock
}

public class BattleCard : MonoBehaviour, IPointerDownHandler
{
    public bool choosed = false;//标记该卡牌是否被选中（这个由战斗管理器设置）
    //在点击卡牌时
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!choosed)
        {
            //向战斗管理器发送出牌请求（由于BattleManager是单例，所以只需要加个Instance就能直接找到）
            BattleManager.Instance.AttackRequest(gameObject);
        }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
