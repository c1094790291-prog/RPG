using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State_FreshText : MonoBehaviour
{
    public Text count;

    public void FreshCount(int _count)
    {
        count.text = _count.ToString();
    }
}
