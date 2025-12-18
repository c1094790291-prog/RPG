using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CardEffect : MonoBehaviour
{
    public BattleManager BattleManager;
    private void Awake()
    {
        BattleManager = BattleManager.Instance;
    }
    public void Effect(Card attackCard, EnemyState enemyState, PlayerState playerState)
    {
        //发起剩余效果
        int effect_id = attackCard.id;
        if (attackCard.upgrade)
        {
            effect_id += 1000;//已升级的卡延后1000位
        }
        switch (effect_id)
        {
            case 0://盾击
            case 1000:
                break;
            case 1://铁壁
            case 1001:
                break;
            case 2://转守为攻
                Anim_Attack();
                enemyState.TakeDamage(playerState.armor + playerState.strength);
                playerState.GetArmor(-playerState.armor);
                break;
            case 1002:
                Anim_Attack();
                enemyState.TakeDamage(playerState.armor + playerState.strength);
                playerState.GetArmor(-(playerState.armor/2));
                break;
            case 3://戳刺
            case 1003:
                break;
            case 4://防御
            case 1004:
                break;
            case 5://暗器
            case 1005:
                break;
            case 6://极速突袭
            case 1006:
                DrawCard(2);
                break;
            case 7://燃烧
                playerState.GetStrength(2);
                break;
            case 1007:
                playerState.GetStrength(3);
                break;
            case 8://烈焰打击
            case 1008:
                break;
            case 9://爆燃
            case 1009:
                enemyState.FireAnim();//触发敌人的燃烧动画
                enemyState.TakeDamage(enemyState.fire);
                enemyState.fire = 0;
                enemyState.GetFire(0);//使其刷新一次燃烧值
                break;
            case 10://无情之阳
            case 1010:
                break;
            case 11://涅槃
                playerState.Heal(15);
                playerState.GetFire(15);
                break;
            case 1011:
                playerState.Heal(20);
                playerState.GetFire(20);
                break;
            case 12://毒刺
            case 1012:
                break;
            case 13://以毒攻毒
                playerState.GetToxin(3);
                break;
            case 1013:
                playerState.GetToxin(4);
                break;
            case 14://瘟疫手雷
            case 1014:
                break;
            case 15://雷光斩
            case 1015:
                DrawCard(1);
                break;
            case 16://电能释放
            case 1016:
                break;
            case 17://雷枪
            case 1017:
                TurnEnd();
                break;
            case 18://火球术
            case 1018:
                break;
            case 19://荆棘之甲
            case 1019:
                break;
            case 20://元素补剂
                playerState.Heal(4);
                break;
            case 1020:
                playerState.Heal(6);
                break;
            case 21://提纯
            case 1021:
                if (BattleManager.TargetCard != null)
                {
                    GameObject targetCard = BattleManager.TargetCard;//获取被选中的卡牌
                    CardConsume(targetCard);//消耗
                }
                if (effect_id == 1021)
                {
                    DrawCard(1);
                }
                break;
            case 22://穿透打击
                if (enemyState.armor > 0)
                {
                    enemyState.TakeDamage(5);
                }
                break;
            case 1022:
                if (enemyState.armor > 0)
                {
                    enemyState.TakeDamage(7);
                }
                break;
        }
    }

    public void EnterEffect(Card attackCard, PlayerState playerState)
    {

    }

    public void OverEffect(Card attackCard, PlayerState playerState)
    {

    }

    //回调BattleManager的方法
    //抽牌
    public void DrawCard(int count)
    {
        BattleManager.DrawCard(count);
    }

    //攻击动画
    public void Anim_Attack()
    {
        BattleManager.Anim_Attack();
    }

    //回合结束
    public void TurnEnd()
    {
        BattleManager.TurnEnd();
    }

    //消耗指定卡牌
    public void CardConsume(GameObject targetCard)
    {
        Card _cardObj = targetCard.GetComponent<CardDisplay>().card;//获取卡牌脚本
        BattleManager.ConsumeList.Add(_cardObj);//放入消耗牌堆
        Destroy(targetCard.gameObject);//销毁对象
        BattleManager.HandCount -= 1;//别忘了修改手牌数量
    }



}
