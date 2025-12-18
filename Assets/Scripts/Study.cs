using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;
//using Input = UnityEngine.Input; // 声明默认使用UnityEngine下的Input

public class Study : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float h = 0;
    private float v = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move();
    }

    public void Move()
    {

        h = Input.GetAxis("Horizontal");//获取水平轴（左右按钮）的输入值
        v = Input.GetAxis("Vertical");//获取垂直轴（上下按钮）的输入值
        transform.Translate(new Vector3(h, v, 0) * moveSpeed * Time.deltaTime);
        //Debug.Log("h:" + h + "v:" + v);
    }
}
