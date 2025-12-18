using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    //默认10伤害
    public int damage = 10;
    public Transform owner;//攻击者的坐标

    private void Start()
    {
        //0.2秒后销毁
        Destroy(gameObject, 0.2f);
    }
    //当玩家攻击触发器碰到2D对象时
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果目标有“敌人”标签
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //调用敌人伤害函数
            //collision.gameObject.GetComponent<EnemyBase>().TakeDamage(damage, owner);
        }
    }

    //新增一个设置伤害的接口
    public void SetDamage(int num, Transform Owner)
    {
        damage = num;
        owner = Owner;
    }
}
