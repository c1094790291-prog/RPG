using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeleteSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //播放自身动画
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //由动画调用
    public void Break()
    {
        Destroy(gameObject);
    }
}
