using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//全局变量
public static class Global_PlayerData
{
    public static int coins;//持有的金币数
    public static int maxhp;//玩家最大生命
    public static int hp;//玩家生命
    //public static int count;//玩家卡牌数量
    public static int id;//角色id
    public static int seed = 100;//随机种子
    public static int progress = 0;//游戏进度（已完成的关卡数）
    public static bool newGame = true;//是否为新游戏

    public static Vector2Int currentRoom = new Vector2Int(0, 0);//玩家所在房间的逻辑坐标（默认初始房间）
    public static int CurrentId = 0;
}
