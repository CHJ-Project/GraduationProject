using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenAdaption : MonoBehaviour {

    //是否需要进行刘海屏适配(目前暂时使用的方法为分辨率判断，正确方法是获取设备信息)
    private bool _isNeedAdaption = false;
    //为了适配让UI缩进的距离
    public readonly float offset = 60;
    //大于这个比例则代表是刘海屏
    public float normalRatio = 1.90f;

    private static ScreenAdaption uniqueInstance;

    public static ScreenAdaption Instance
    {
        get
        {
            return uniqueInstance;
        }
    }

    public bool IsNeedAdaption
    {
        get
        {
            return _isNeedAdaption;
        }
    }

    RectTransform rectTransform;

    Transform rootTransform;

    private void Awake()
    {
        if (uniqueInstance == null)
        {
            uniqueInstance = this;
        }
        else if (uniqueInstance != this)
        {
            throw new InvalidOperationException("Cannot have two instances of a SingletonMonoBehaviour : " + typeof(ScreenAdaption).ToString() + ".");
        }
    }

    void Start()
    {
        rootTransform = GameObject.Find("UIROOT").transform;
		//print (rootTransform);
        int width = Screen.width;
        int height = Screen.height;
		//print (width + "  " + height);
        float proportion = (float)width / (float)height;
        //print(proportion);
        if (proportion > normalRatio)
            _isNeedAdaption = true;
        else
            _isNeedAdaption = false;
        //print("需要屏幕适配：" + _isNeedAdaption);
		if (_isNeedAdaption) {
			foreach (Transform transform in rootTransform) {
				rectTransform = transform.GetComponent<RectTransform> ();
				rectTransform.offsetMin = new Vector2 (offset, 0);
				rectTransform.offsetMax = new Vector2 (-offset, 0);
			}
		}
    }

    private void OnDestroy()
    {
        if (uniqueInstance == this)
        {
            uniqueInstance = null;
        }
    }
}
