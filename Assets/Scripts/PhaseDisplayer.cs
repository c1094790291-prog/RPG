using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseDisplayer : MonoBehaviour
{
    public Text phaseText;
    // Start is called before the first frame update
    void Awake()
    {
        BattleManager.Instance.phaseChangeEvent.AddListener(UpdateText);//接收回合变化事件
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateText()
    {
        phaseText.text = BattleManager.Instance.GamePhase.ToString();
        Debug.Log("当前游戏阶段："+BattleManager.Instance.GamePhase.ToString());
    }
}
