using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemap_Wall : MonoBehaviour
{
    [Header("围墙瓦片配置")]
    public Tilemap targetTilemap; // 目标瓦片地图
    public TileBase topLeftCorner; // 左上角围墙瓦片
    public TileBase bottomLeftCorner; // 左下角围墙瓦片
    public TileBase topRightCorner; // 右上角围墙瓦片
    public TileBase bottomRightCorner; // 右下角围墙瓦片
    public TileBase topEdge; // 上边围墙瓦片
    public TileBase bottomEdge; // 下边围墙瓦片
    public TileBase leftEdge; // 左边围墙瓦片
    public TileBase rightEdge; // 右边围墙瓦片

    [Header("地图设置")]
    private Vector2Int centerPos; // 地图中心点（瓦片坐标）
    public int mapSize = 24; // 地图边长

    [ContextMenu("生成围墙测试")]
    public void GenerateWallTest()
    {
        targetTilemap.ClearAllTiles();// 清空原有瓦片
        GenerateWall(0, 0, true, true, true, true);
    }

    /// <summary>
    /// 生成围墙，支持指定边开缺口
    /// </summary>
    /// <param name="cenx">中心点x坐标</param>
    /// <param name="ceny">中心点y坐标</param>
    /// <param name="topGap">上边是否开缺口</param>
    /// <param name="bottomGap">下边是否开缺口</param>
    /// <param name="leftGap">左边是否开缺口</param>
    /// <param name="rightGap">右边是否开缺口</param>
    public void GenerateWall(int cenx, int ceny, bool topGap, bool bottomGap, bool leftGap, bool rightGap)
    {
        //targetTilemap.ClearAllTiles();// 清空原有瓦片
        centerPos = new Vector2Int(cenx, ceny);//设置中心点
        // 校验参数
        if (targetTilemap == null ||
            topLeftCorner == null || bottomLeftCorner == null ||
            topRightCorner == null || bottomRightCorner == null ||
            topEdge == null || bottomEdge == null || leftEdge == null || rightEdge == null)
        {
            Debug.LogError("请配置所有围墙瓦片和目标Tilemap！");
            return;
        }

        // 计算地图边界坐标（30×30正方形，中心点为centerPos）
        int halfSize = mapSize / 2;
        int minX = centerPos.x - halfSize;
        int maxX = centerPos.x + halfSize - 1; // 偶数尺寸修正，保证30个单元格
        int minY = centerPos.y - halfSize;
        int maxY = centerPos.y + halfSize - 1;

        // 1. 生成四个角落的围墙瓦片
        SetWallTile(minX, maxY, topLeftCorner); // 左上角
        SetWallTile(minX, minY, bottomLeftCorner); // 左下角
        SetWallTile(maxX, maxY, topRightCorner); // 右上角
        SetWallTile(maxX, minY, bottomRightCorner); // 右下角

        // 计算缺口的起始和结束坐标（长度为6，居中）
        int gapLength = 6;
        int halfGap = gapLength / 2;

        // 2. 生成上边的围墙瓦片（排除角落，处理缺口）
        int topMidX = centerPos.x;
        int topGapStartX = topMidX - halfGap;
        int topGapEndX = topMidX + halfGap - 1; // 修正长度为6
        for (int x = minX + 1; x < maxX; x++)
        {
            if (topGap && x >= topGapStartX && x <= topGapEndX)
            {
                continue; // 跳过缺口区域
            }
            SetWallTile(x, maxY, topEdge); // 上边
        }

        // 3. 生成下边的围墙瓦片（排除角落，处理缺口）
        int bottomMidX = centerPos.x;
        int bottomGapStartX = bottomMidX - halfGap;
        int bottomGapEndX = bottomMidX + halfGap - 1;
        for (int x = minX + 1; x < maxX; x++)
        {
            if (bottomGap && x >= bottomGapStartX && x <= bottomGapEndX)
            {
                continue; // 跳过缺口区域
            }
            SetWallTile(x, minY, bottomEdge); // 下边
        }

        // 4. 生成左边的围墙瓦片（排除角落，处理缺口）
        int leftMidY = centerPos.y;
        int leftGapStartY = leftMidY - halfGap;
        int leftGapEndY = leftMidY + halfGap - 1;
        for (int y = minY + 1; y < maxY; y++)
        {
            if (leftGap && y >= leftGapStartY && y <= leftGapEndY)
            {
                continue; // 跳过缺口区域
            }
            SetWallTile(minX, y, leftEdge); // 左边
        }

        // 5. 生成右边的围墙瓦片（排除角落，处理缺口）
        int rightMidY = centerPos.y;
        int rightGapStartY = rightMidY - halfGap;
        int rightGapEndY = rightMidY + halfGap - 1;
        for (int y = minY + 1; y < maxY; y++)
        {
            if (rightGap && y >= rightGapStartY && y <= rightGapEndY)
            {
                continue; // 跳过缺口区域
            }
            SetWallTile(maxX, y, rightEdge); // 右边
        }
    }

    /// <summary>
    /// 设置指定坐标的围墙瓦片
    /// </summary>
    /// <param name="x">瓦片x坐标</param>
    /// <param name="y">瓦片y坐标</param>
    /// <param name="tile">要设置的瓦片</param>
    private void SetWallTile(int x, int y, TileBase tile)
    {
        Vector3Int tilePos = new Vector3Int(x, y, 0);
        targetTilemap.SetTile(tilePos, tile);
    }
}