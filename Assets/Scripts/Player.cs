using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Windows;
//using Input = UnityEngine.Input; // 声明默认使用UnityEngine下的Input

public class Player : MonoBehaviour
{
    [Header("基础属性")]
    public float speed = 5f;
    public int MAXHP = 100;
    private int HP = 100;
    public int ATK = 10;
    private float h, v;
    [Header("组件")]
    public Rigidbody2D rb;//调用物理引擎
    public Animator animPlayer;
    public Animator animSword;
    public Animator animSlash;
    public SpriteRenderer sr;
    public GameObject swordTri;//创建剑触发器
    public Transform swordTriPos;
    //public Slider hpbar;
    //[Header("玩家死亡")]
    //public GameObject deadVFX;
    //public bool isDead = false;
    //public GameObject deadUI;
    [Header("攻击")]
    public AudioSource audioVFX;//音频播放器
    public AudioClip attackSound;//音频素材
    [Header("时间管理器")]
    private SceneTimeMarker sceneMarker;
    public bool playerStop = false;

    void Awake()
    {
        //寻找当前场景中第一个时间管理器实例（最后会找到SceneChanger对象身上）
        //sceneMarker = FindObjectOfType<SceneTimeMarker>();
        sceneMarker = SceneTimeMarker.Instance;
    }
    void Start()
    {
        HP = MAXHP;
    }

    void Update()
    {
        //查看时间管理器是否暂停
        if ((sceneMarker != null && sceneMarker.isPaused) || playerStop)
        {
            //人物立刻停在原地
            rb.velocity = Vector2.zero;
            //关闭跑步动画
            animPlayer.SetBool("IsRun", false);
            return;
        }
        Move();
        Attack();
    }

    public void Move()
    {
        //这里加了Raw，物体就不会有惯性，松开按键立刻刹车
        h = Input.GetAxisRaw("Horizontal");//获取水平轴（左右按钮）的输入值
        v = Input.GetAxisRaw("Vertical");//获取垂直轴（上下按钮）的输入值
        rb.velocity = new Vector2(h * speed, v * speed);
        //原运动控制方法
        //transform.Translate(new Vector3(h, v, 0) * moveSpeed * Time.deltaTime);
        //Debug.Log("h:" + h + "v:" + v);
        if(h!=0 ||  v!=0)
        {
            animPlayer.SetBool("IsRun", true);//跑步动画
            if (h>0)
            {
                sr.flipX=false;//镜像转身
            }
            else if (h<0)
            {
                sr.flipX=true;
            }
        }
        else
        {
            animPlayer.SetBool("IsRun", false);
        }
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //通过触发Attack1，播放挥剑与剑气的动画
            animSword.SetTrigger("Attack1");
            animSlash.SetTrigger("Attack1");
            //创建物体（预制体（碰撞器）、坐标、旋转、父类）
            GameObject go = Instantiate(swordTri, swordTriPos.position,
                swordTriPos.rotation, swordTriPos);
            //设置本次攻击造成的伤害，以及自身坐标
            go.GetComponent<PlayerAttackTrigger>().
                SetDamage(ATK,transform);
            //使用音频播放器audioVFX播放一次素材attackSound
            audioVFX.PlayOneShot(attackSound);
        }
    }
}
