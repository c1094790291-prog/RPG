using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorImage : MonoBehaviour
{
    public Image cursorImage;
    // 鼠标样式配置
    public Sprite defaultCursorSprite;   // 默认鼠标图片
    public Sprite swordCursorSprite;     // 悬浮敌人时的剑图片
    [Tooltip("优先使用手动指定的相机，未指定则自动查找MainCamera/2D正交相机")]
    public Camera mainCamera;            // 可选：手动指定相机（非必需）
    public string enemyTagName = "Enemy"; // 敌人的标签名称

    // 记录原始状态，避免重复赋值
    private bool isOverEnemy = false;

    void Start()
    {
        // 隐藏系统默认鼠标
        Cursor.visible = false;

        // 核心优化：自动查找主相机（优先级：手动指定 > Tag=MainCamera > 第一个正交相机）
        AutoFindMainCamera();

        // 初始化鼠标图片为默认样式
        if (defaultCursorSprite != null)
        {
            cursorImage.sprite = defaultCursorSprite;
        }

        // 安全检查：如果还是没找到相机，给出提示
        if (mainCamera == null)
        {
            Debug.LogWarning("未找到主相机！请确保场景中有Tag为MainCamera的相机，或手动指定相机。", this);
        }
    }

    void Update()
    {
        // 保留原有逻辑：鼠标跟随+点击交互
        transform.position = Input.mousePosition;
        HandleMouseClick();

        // 新增逻辑：检测鼠标是否悬浮在敌人上（加相机判空，避免空引用）
        if (mainCamera != null)
        {
            CheckMouseOverEnemyByTag();
            SwitchCursorSprite();
        }
    }

    /// <summary>
    /// 新增：自动查找2D场景的主相机
    /// </summary>
    private void AutoFindMainCamera()
    {
        // 1. 如果手动指定了相机，直接使用（保留手动赋值的灵活性）
        if (mainCamera != null)
        {
            return;
        }

        // 2. 查找Tag为"MainCamera"的相机（Unity默认主相机的Tag）
        Camera tagCamera = Camera.main;
        if (tagCamera != null)
        {
            mainCamera = tagCamera;
            return;
        }

        // 3. 如果没找到MainCamera，查找场景中第一个正交相机（2D场景常用）
        Camera[] allCameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in allCameras)
        {
            if (cam.orthographic) // 正交相机是2D场景的特征
            {
                mainCamera = cam;
                return;
            }
        }

        // 4. 最后尝试找任意相机（兜底）
        if (allCameras.Length > 0)
        {
            mainCamera = allCameras[0];
        }
    }

    /// <summary>
    /// 保留原有：处理鼠标点击的颜色和缩放逻辑
    /// </summary>
    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cursorImage.color = Color.red;
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cursorImage.color = Color.white;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    /// <summary>
    /// 保留：通过标签检测敌人
    /// </summary>
    private void CheckMouseOverEnemyByTag()
    {
        // 将屏幕鼠标坐标转换为2D世界坐标
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // 发射2D射线检测
        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero, 0f);

        // 判断标签
        isOverEnemy = (hit.collider != null && hit.collider.CompareTag(enemyTagName));
    }

    /// <summary>
    /// 保留：切换鼠标图片
    /// </summary>
    private void SwitchCursorSprite()
    {
        if (isOverEnemy)
        {
            if (cursorImage.sprite != swordCursorSprite && swordCursorSprite != null)
            {
                cursorImage.sprite = swordCursorSprite;
            }
        }
        else
        {
            if (cursorImage.sprite != defaultCursorSprite && defaultCursorSprite != null)
            {
                cursorImage.sprite = defaultCursorSprite;
            }
        }
    }

    // 可选：退出时恢复系统鼠标
    private void OnDestroy()
    {
        Cursor.visible = true;
    }
}