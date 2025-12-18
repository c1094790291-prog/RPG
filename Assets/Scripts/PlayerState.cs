using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [Header("基本属性")]
    public int id;
    public int hp;//生命值
    public int maxhp;//最大生命值
    public bool life = true;//生存状况
    public int armor = 0;//护甲
    public int strength = 0;//力量
    public int fire = 0;//燃烧层数
    public int toxin = 0;//毒素层数
    public int electricity = 0;//雷电层数
    [Header("UI")]
    public Text nameText;
    public Text hpText;
    public Text maxhpText;
    public Text armorText;
    public Slider hpbar;
    public Image enemyImage;
    public Transform State;//状态栏
    //图标储存处（方便未来删除）
    private GameObject Strength_State;//力量状态
    private GameObject Fire_State;//燃烧状态
    private GameObject Toxin_State;//中毒状态
    private GameObject Ele_State;//雷电状态
    [Header("特效")]
    public Transform EffectPlace; //特效区域
    public GameObject BoomAnim_Prefab;//燃烧特效
    public GameObject ToxinAnim_Prefab;//中毒特效
    public GameObject HurtAnim_Prefab;//受伤特效
    [Header("其它")]
    //public GameObject DataManager;//从数据管理器获取玩家数据
    private PlayerData PlayerData;
    public GameObject StateManager;//状态管理器
    //public PlayerType playerType;

    void Awake() // 用 Awake() 初始化，比 OnEnable() 早，避免时机问题
    {
        PlayerData = PlayerData.Instance;
    }
    void Start()
    {
        Debug.Log("所有信息加载完成，进入准备阶段");
        StartShow();//初始化信息
        Refresh();//刷新界面
    }
    void Update()
    {

    }

    //void OnEnable()
    //{
    //    //当脚本PlayerData的DataLoaded事件被触发后，执行OnScriptADataLoaded
    //    PlayerData.PlayerLoaded += OnDataLoaded;
    //}

    //void OnDataLoaded()
    //{
    //    Debug.Log("所有信息加载完成，进入准备阶段");
    //    StartShow();//初始化信息
    //    Refresh();//刷新界面
    //    PlayerData.PlayerLoaded -= OnDataLoaded;//取消事件订阅释放内存
    //}

    //初始化显示层
    public void StartShow()
    {
        id = Global_PlayerData.id;//加载id
        nameText.text = GetName(id);//显示名字
        maxhpText.text = Global_PlayerData.maxhp.ToString();//显示最大生命
        hp = Global_PlayerData.hp;//加载初始生命值
        maxhp = Global_PlayerData.maxhp;
    }
    //更新显示层
    public void Refresh()
    {
        hpText.text = hp.ToString();//更新当前生命
        hpbar.value = (float)hp / (float)maxhp;//更新血条
        armorText.text = armor.ToString();
    }

    public string GetName(int _id)
    {
        string name;
        switch (_id)
        {
            case 0:
                name = "无名";
                break;
            case 1:
                name = "重甲兵";
                break;
            case 2:
                name = "刺客";
                break;
            case 3:
                name = "元素使";
                break;
            default:
                name = "";
                break;
        }
        return name;
    }

    //伤害函数
    public void TakeDamage(int damage)
    {
        //雷电附加
        if (electricity > 0)
        {
            damage += electricity;
            GetElectricity(-1);
            if (electricity > 5) { GetElectricity(-1); }//大于5层额外扣减
        }
        //播放受伤特效
        GameObject hurt_count = Instantiate(HurtAnim_Prefab, EffectPlace);
        hurt_count.GetComponent<Text>().text = damage.ToString();
        //伤害结算
        armor -= damage;//掉甲
        if (armor < 0)
        {
            hp += armor;//扣血
            armor = 0;
        }
        if (hp < 0) hp = 0;
        Refresh();//更新显示层
        //死亡
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            life = false;
            //通知战斗管理器验证一次是否胜利
            BattleManager.Instance.CheckWin();
        }
    }

    //治疗函数
    public void Heal(int _count)
    {
        hp += _count;//回血
        if (hp > maxhp) { hp = maxhp; }
        Refresh();//更新显示层
    }

    //起甲函数
    public void GetArmor(int _armor)
    {
        armor += _armor;
        Refresh();
    }

    //清空格挡函数
    public void CleanArmor()
    {
        armor = 0;
        Refresh();
    }

    //修改力量函数
    public void GetStrength(int _strength)
    {
        strength += _strength;
        FreshState(0);
    }

    //修改燃烧函数
    public void GetFire(int _count)
    {
        fire += _count;
        if (fire < 0) { fire = 0; }
        FreshState(1);
    }

    //修改毒素函数
    public void GetToxin(int _count)
    {
        toxin += _count;
        if (toxin < 0) { toxin = 0; }
        FreshState(2);
    }

    //修改雷电函数
    public void GetElectricity(int _count)
    {
        electricity += _count;
        if (electricity < 0) { electricity = 0; }
        FreshState(3);
    }

    //刷新状态栏（出于性能考虑，每次调用只刷新一种状态）
    public void FreshState(int _state)
    {
        int state_Count = strength;
        ref GameObject state_Object = ref Strength_State;
        switch (_state)
        {
            case 0://力量
                state_Count = strength;
                state_Object = ref Strength_State;
                break;
            case 1://燃烧
                state_Count = fire;
                state_Object = ref Fire_State;
                break;
            case 2://中毒
                state_Count = toxin;
                state_Object = ref Toxin_State;
                break;
            case 3://雷电
                state_Count = electricity;
                state_Object = ref Ele_State;
                break;
            default://传入未知变量则用默认值
                //state_Count = strength;
                //state_Object = ref Strength_State;
                Debug.Log("状态栏刷新异常！");
                break;
        }
        if (state_Count != 0)//检测数值
        {
            if (state_Object == null)//检测是否有图标
            {
                //调用状态管理器创建图标
                state_Object = StateManager.GetComponent<StateManager>().AddState(_state, State.transform);
            }
            state_Object.GetComponent<State_FreshText>().FreshCount(state_Count);//更新数值文本
        }
        else if (state_Count == 0)
        {
            Destroy(state_Object);
        }
    }

    //中毒结算
    public void ToxinSolve()
    {
        TakeDamage(toxin);//结算一次毒素伤害
        GetToxin(-3);//减少三层毒素
    }

    //燃烧结算
    public void FireSolve()
    {
        int damage = fire - (fire / 2);
        TakeDamage(damage);//结算一次燃烧伤害
        GetFire(-damage);//减少一半燃烧
    }

    //播放燃烧动画
    public void FireAnim()
    {
        Instantiate(BoomAnim_Prefab, EffectPlace);
        Invoke("FireSolve", 0.7f);
    }

    //播放中毒动画
    public void ToxinAnim()
    {
        Instantiate(ToxinAnim_Prefab, EffectPlace);
        Invoke("ToxinSolve", 0.7f);
    }

}
