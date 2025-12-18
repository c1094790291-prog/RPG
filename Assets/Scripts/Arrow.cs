using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector2 StartPoint;
    private Vector2 EndingPoint;
    //arrow是UI图片，所以是RectTransform
    private RectTransform arrow;

    private float ArrowLength;
    private float ArrowTheta;
    private Vector2 ArrowPosition;
    // Start is called before the first frame update
    void Start()
    {
        arrow = transform.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //计算
        //获取鼠标位置，由于鼠标的中心点在左下角、游戏的中心点在正中央，所以需要减去位置差
        //这里应该根据玩家屏幕分辨率计算，暂时先这样
        EndingPoint = Input.mousePosition - new Vector3(960.0f, 540.0f, 0.0f);
        ArrowPosition = new Vector2((EndingPoint.x + StartPoint.x) / 2, (EndingPoint.y + StartPoint.y) / 2);
        //长度使用勾股定理，X坐标差的平方+Y坐标差的平方，开根号
        ArrowLength = Mathf.Sqrt((EndingPoint.x - StartPoint.x) * (EndingPoint.x - StartPoint.x) + (EndingPoint.y - StartPoint.y) * (EndingPoint.y - StartPoint.y));
        //角度使用tan
        ArrowTheta = Mathf.Atan2(EndingPoint.y - StartPoint.y, EndingPoint.x - StartPoint.x);

        //赋值
        arrow.localPosition = ArrowPosition;
        //尺寸改为计算的长度，宽度不变（长度缩短避免挡住鼠标）
        arrow.sizeDelta = new Vector2(ArrowLength - 1f, arrow.sizeDelta.y);
        //角度
        arrow.localEulerAngles = new Vector3(0.0f, 0.0f, ArrowTheta * 180 / Mathf.PI);

        //arrow.localPosition = EndingPoint;
    }

    //输入位置，输出更新后的位置
    public void SetStartPoint(Vector2 _startPoint)
    {
        StartPoint = _startPoint - new Vector2(960.0f, 540.0f);
    }

}
