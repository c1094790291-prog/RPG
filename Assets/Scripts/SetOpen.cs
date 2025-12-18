using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOpen : MonoBehaviour
{
    public GameObject MiniWindow;//迷你窗口
    
    public void CilckSetButton()
    {
        if (MiniWindow != null)
        {
            bool setActive = MiniWindow.activeSelf;
            MiniWindow.SetActive(!setActive);
            if (setActive)//由于全局暂停会关闭Canvas,所以这里只暂停玩家
            {
                GameObject.Find("Player").GetComponent<Player>().playerStop = false;//恢复
            }
            else
            {
                GameObject.Find("Player").GetComponent<Player>().playerStop = true;//暂停
            }
        }
    }

    //如果自身被销毁，确保窗口关闭
    private void OnDestroy()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            Player playerScript = playerObj.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.playerStop = false;
            }
        }
    }

}
