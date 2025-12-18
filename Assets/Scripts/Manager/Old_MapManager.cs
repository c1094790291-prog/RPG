using UnityEngine;
using System.Collections.Generic;



//原方法可行，但过于复杂，所以暂时封存



/*
// 房间开口状态类
public class RoomOpenState
{
    public bool top;
    public bool bottom;
    public bool left;
    public bool right;

    public RoomOpenState()
    {
        top = false;
        bottom = false;
        left = false;
        right = false;
    }
}

public class MapManager : MonoBehaviour
{
    [Header("围墙生成器引用")]
    public GameObject WallMap; // 围墙地图对象
    private Tilemap_Wall wallGenerator; // 引用围墙生成脚本

    [Header("地图设置")]
    public int mapSize = 5; // 地图尺寸5×5
    public int roomInterval = 30; // 房间之间的间隔坐标

    private int[] mapArray; // 房间启用状态数组（1启用，0未启用）
    private Vector2Int[] roomCenterPositions; // 每个房间的中心点坐标数组
    private List<Vector2Int> pathRooms; // 存储路径上的房间坐标（x,y）
    private Dictionary<Vector2Int, RoomOpenState> roomOpenStates; // 房间开口状态容器

    private void Awake()
    {
        // 获取围墙生成脚本引用
        if (WallMap != null)
        {
            wallGenerator = WallMap.GetComponent<Tilemap_Wall>();
        }
        else
        {
            Debug.LogError("未设置WallMap对象引用！");
        }
    }

    /// <summary>
    /// 生成地图和房间围墙（主函数）
    /// </summary>
    /// <param name="seed">随机种子</param>
    public void GenerateMapAndWalls(int seed)
    {
        // 初始化数据结构
        int totalRooms = mapSize * mapSize;
        mapArray = new int[totalRooms];
        roomCenterPositions = new Vector2Int[totalRooms];
        pathRooms = new List<Vector2Int>();
        roomOpenStates = new Dictionary<Vector2Int, RoomOpenState>();

        // 初始化房间状态为0，并初始化开口状态
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int index = GetRoomIndex(x, y);
                mapArray[index] = 0;
                roomOpenStates[new Vector2Int(x, y)] = new RoomOpenState();
            }
        }

        // 设置随机种子
        Random.InitState(seed);

        // 生成房间路径（包含开口状态记录）
        GenerateRoomPath();

        // 计算每个房间的中心点坐标
        CalculateRoomCenterPositions();

        // 一次性生成所有房间的围墙
        GenerateAllRoomWallsOnce();
    }

    /// <summary>
    /// 生成从起点到终点的房间路径（随机DFS），并记录开口状态
    /// </summary>
    private void GenerateRoomPath()
    {
        int startX = 0, startY = 0;
        int endX = mapSize - 1, endY = mapSize - 1;

        bool[,] visited = new bool[mapSize, mapSize];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parentMap = new Dictionary<Vector2Int, Vector2Int>(); // 记录父节点

        // 初始化起点
        stack.Push(new Vector2Int(startX, startY));
        visited[startX, startY] = true;
        mapArray[GetRoomIndex(startX, startY)] = 1;
        pathRooms.Add(new Vector2Int(startX, startY));
        parentMap[new Vector2Int(startX, startY)] = new Vector2Int(-1, -1); // 起点无父节点

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),  // 上
            new Vector2Int(0, -1), // 下
            new Vector2Int(-1, 0), // 左
            new Vector2Int(1, 0)   // 右
        };

        bool foundEnd = false;
        while (stack.Count > 0 && !foundEnd)
        {
            Vector2Int current = stack.Peek();
            int x = current.x;
            int y = current.y;

            if (x == endX && y == endY)
            {
                mapArray[GetRoomIndex(x, y)] = 1;
                if (!pathRooms.Contains(current)) pathRooms.Add(current);
                foundEnd = true;
                break;
            }

            ShuffleDirections(directions);
            bool hasUnvisited = false;

            foreach (Vector2Int dir in directions)
            {
                int newX = x + dir.x;
                int newY = y + dir.y;

                if (newX >= 0 && newX < mapSize && newY >= 0 && newY < mapSize && !visited[newX, newY])
                {
                    visited[newX, newY] = true;
                    stack.Push(new Vector2Int(newX, newY));
                    mapArray[GetRoomIndex(newX, newY)] = 1;
                    pathRooms.Add(new Vector2Int(newX, newY));
                    parentMap[new Vector2Int(newX, newY)] = current;
                    hasUnvisited = true;

                    // 记录当前房间与新房间的开口状态（双向）
                    SetRoomOpenState(current, new Vector2Int(newX, newY), dir);
                    break;
                }
            }

            if (!hasUnvisited)
            {
                stack.Pop();
            }
        }

        // 确保终点在路径中（极端情况处理）
        if (!foundEnd)
        {
            mapArray[GetRoomIndex(endX, endY)] = 1;
            if (!pathRooms.Contains(new Vector2Int(endX, endY))) pathRooms.Add(new Vector2Int(endX, endY));

            // 找到终点最近的路径房间，并建立开口
            Vector2Int nearest = FindNearestPathRoom(endX, endY);
            Vector2Int dir = new Vector2Int(endX - nearest.x, endY - nearest.y);

            // 手动将Vector2Int转换为单位向量（仅保留单一方向，处理浮点转整数）
            int absX = Mathf.Abs(dir.x);
            int absY = Mathf.Abs(dir.y);
            if (absX > absY)
            {
                // 取x方向，Sign返回float转int
                int signX = (int)Mathf.Sign(dir.x);
                dir = new Vector2Int(signX, 0);
            }
            else if (absY > absX)
            {
                // 取y方向，Sign返回float转int
                int signY = (int)Mathf.Sign(dir.y);
                dir = new Vector2Int(0, signY);
            }
            else
            {
                // 斜向时优先x方向（或随机选择，此处处理整数转换）
                int signX = (int)Mathf.Sign(dir.x);
                dir = new Vector2Int(signX, 0);
            }

            SetRoomOpenState(nearest, new Vector2Int(endX, endY), dir);
        }
    }

    /// <summary>
    /// 设置两个相邻房间的开口状态（双向）
    /// </summary>
    /// <param name="fromRoom">源房间</param>
    /// <param name="toRoom">目标房间</param>
    /// <param name="direction">从源到目标的方向向量</param>
    private void SetRoomOpenState(Vector2Int fromRoom, Vector2Int toRoom, Vector2Int direction)
    {
        if (direction == new Vector2Int(0, 1)) // 向上
        {
            roomOpenStates[fromRoom].top = true;
            roomOpenStates[toRoom].bottom = true;
        }
        else if (direction == new Vector2Int(0, -1)) // 向下
        {
            roomOpenStates[fromRoom].bottom = true;
            roomOpenStates[toRoom].top = true;
        }
        else if (direction == new Vector2Int(-1, 0)) // 向左
        {
            roomOpenStates[fromRoom].left = true;
            roomOpenStates[toRoom].right = true;
        }
        else if (direction == new Vector2Int(1, 0)) // 向右
        {
            roomOpenStates[fromRoom].right = true;
            roomOpenStates[toRoom].left = true;
        }
    }

    /// <summary>
    /// 计算每个房间的中心点坐标
    /// </summary>
    private void CalculateRoomCenterPositions()
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int index = GetRoomIndex(x, y);
                int centerX = x * roomInterval;
                int centerY = y * roomInterval;
                roomCenterPositions[index] = new Vector2Int(centerX, centerY);
            }
        }
    }

    /// <summary>
    /// 一次性生成所有房间的围墙
    /// </summary>
    private void GenerateAllRoomWallsOnce()
    {
        if (wallGenerator == null)
        {
            Debug.LogError("未引用Tilemap_Wall脚本！");
            return;
        }

        // 清空原有围墙
        wallGenerator.targetTilemap.ClearAllTiles();

        // 遍历所有启用的房间生成围墙
        foreach (Vector2Int room in pathRooms)
        {
            int index = GetRoomIndex(room.x, room.y);
            if (mapArray[index] != 1) continue;

            Vector2Int center = roomCenterPositions[index];
            RoomOpenState openState = roomOpenStates[room];

            // 调用围墙生成函数
            wallGenerator.GenerateWall(
                center.x,
                center.y,
                openState.top,
                openState.bottom,
                openState.left,
                openState.right
            );
        }
    }

    /// <summary>
    /// 找到离指定房间最近的路径房间
    /// </summary>
    private Vector2Int FindNearestPathRoom(int x, int y)
    {
        Vector2Int target = new Vector2Int(x, y);
        Vector2Int nearest = pathRooms[0];
        float minDist = Vector2Int.Distance(target, nearest);

        foreach (Vector2Int room in pathRooms)
        {
            float dist = Vector2Int.Distance(target, room);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = room;
            }
        }

        return nearest;
    }

    /// <summary>
    /// 打乱方向数组顺序
    /// </summary>
    private void ShuffleDirections(Vector2Int[] directions)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            int randomIndex = Random.Range(i, directions.Length);
            Vector2Int temp = directions[i];
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 获取房间的一维数组索引
    /// </summary>
    private int GetRoomIndex(int x, int y)
    {
        return y * mapSize + x;
    }

    // 测试方法
    [ContextMenu("测试生成地图和围墙")]
    private void TestGenerateMapAndWalls()
    {
        int seed = Random.Range(0, 1000);
        Debug.Log("生成种子：" + seed);
        GenerateMapAndWalls(seed);

        // 打印地图数组
        string mapStr = "地图数组：\n";
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                mapStr += mapArray[GetRoomIndex(x, y)] + " ";
            }
            mapStr += "\n";
        }
        Debug.Log(mapStr);
    }
}
*/