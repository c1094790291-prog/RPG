using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //卡槽
    public GameObject CardBlock;
    public GameObject BigBlock;
    //卡牌
    public GameObject DeCard_Prefab;
    public GameObject BigCard_Prefab;
    //数据管理器
    public GameObject DataManager;
    private PlayerData PlayerData;
    private CardStore CardStore;
    //卡牌类暂存
    private Card card;
    //动画
    public Animator anim1;

    void Awake()
    {
        PlayerData = PlayerData.Instance;
        CardStore = CardStore.Instance;
    }

    //在两个卡槽中创建指定卡牌
    public void CreateCard(Card _card)
    {
        card = _card;
        //清除原有卡牌
        Destroy(CardBlock.GetComponent<Block>().obj);
        //找到容器生成自己的复制品
        GameObject newCard = Instantiate(DeCard_Prefab, CardBlock.transform);
        //给复制品赋予card类实例
        newCard.GetComponent<CardDisplay>().card = card;
        //卡牌与卡槽关联
        CardBlock.GetComponent<Block>().obj = newCard;
    }

    //带权重的随机数（随机卡牌的稀有度）
    public int GetWeightedRandom()
    {
        // 1. 定义权重数组（顺序对应选项A、B、C）
        int[] weights = { 6, 3, 1 };
        // 2. 计算总权重
        int totalWeight = 0;
        foreach (int w in weights) totalWeight += w;
        // 3. 生成0~总权重的随机数（左闭右开，所以用totalWeight）
        int randomValue = Random.Range(0, totalWeight);

        // 4. 遍历权重，找随机数落在的区间
        int currentSum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            currentSum += weights[i];
            if (randomValue < currentSum)
            {
                return i + 1; // 找到对应索引，最终返回1-3的值
            }
        }

        // 兜底（理论上不会走到）
        return 0;
    }

    public void FinishUp()
    {
        //保存数据
        PlayerData.SavePlayerData();
        //返回主城
        Exit();
    }

    //退出按钮
    public void Exit()
    {
        SceneChanger.Instance.GetMajorCity();
    }


}
