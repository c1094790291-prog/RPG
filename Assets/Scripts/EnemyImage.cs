using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImage : MonoBehaviour
{
    public Animator anim;//动画
    public GameObject parent;//父对象

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //播放动画
    public void Donghua()
    {
        anim.SetBool("Action", true);
    }

    //这个函数由动画事件触发
    public void Act()
    {
        //向父对象的EnemyState脚本激活Action函数
        parent.GetComponent<EnemyState>().Action();
    }
}
