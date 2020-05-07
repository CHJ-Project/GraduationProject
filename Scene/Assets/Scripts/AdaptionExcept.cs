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
        //如果当前分辨率不需要对屏幕进行“刘海屏”适配，则返回
        if (!ScreenAdaption.Instance.IsNeedAdaption)
        {
            return;
        }
        //获取内压的像素大小
        var minPos = rectTransform.offsetMin;
        var maxPos = rectTransform.offsetMax;
        //向外推相同的像素以恢复图片原本的大小
        rectTransform.offsetMin = new Vector2(minPos.x - ScreenAdaption.Instance.offset, minPos.y);
        rectTransform.offsetMax = new Vector2(maxPos.x + ScreenAdaption.Instance.offset, maxPos.y);
    }
}
