using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room
{
    public int id;
    public Vector2Int roomPos;//房间的逻辑坐标
    public bool MainRoom;//是否主线房间
    public bool Clear;
    public List<int> Source;//是否有资源点（0为空，1为火堆，2为删卡，3为宝箱，4为商店）
    
    public Room(int _id, Vector2Int _roomPos, bool _MainRoom, List<int> _Source)
    {
        this.id = _id;
        this.MainRoom = _MainRoom;
        this.roomPos = _roomPos;
        this.Clear = true;
        this.Source = _Source;
    }
}
