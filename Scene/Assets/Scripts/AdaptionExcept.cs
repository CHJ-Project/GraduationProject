using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptionExcept : MonoBehaviour {

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start()
    {
        if (!ScreenAdaption.Instance.IsNeedAdaption)
        {
            return;
        }
        var minPos = rectTransform.offsetMin;
        var maxPos = rectTransform.offsetMax;
        rectTransform.offsetMin = new Vector2(minPos.x - ScreenAdaption.Instance.offset, minPos.y);
        rectTransform.offsetMax = new Vector2(maxPos.x + ScreenAdaption.Instance.offset, maxPos.y);
    }
}
