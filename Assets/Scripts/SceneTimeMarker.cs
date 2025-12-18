using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTimeMarker : MonoSingleton<SceneTimeMarker>
{
    public bool isPaused = false; // 标记该场景是否暂停
}
