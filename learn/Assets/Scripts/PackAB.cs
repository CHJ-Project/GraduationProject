using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class PackAB {

    public class AssetBundle
    {
        //编辑器扩展，在菜单栏Assets下生成Build AssetBundles菜单
        [MenuItem("Assets/Build AssetBundles")]
        //进行资源打包
        static void BuildAllAssetBundles()
        {
            //打包之前要保证文件夹存在，不存在的话会报错
            //使用相对路径保存
            string dir = "AssetBundle";
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }
            //BuildPipeline是UnityEditor中用于打包的类，其中的BuildAssetBundle用于AssetBundle打包
            //dir:存储的路径
            //BuildAssetBundleOptions:表示压缩式的算法   None为LZMA算法
            //BuildTarget: 表示用于什么平台
            BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
    }
}
