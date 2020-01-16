using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DeleteUnusingThings : Editor
{
    [MenuItem("Tools/一键删除没有引用的东西(需要手动改路径，删除后无法恢复)", false, 10)]
    private static void DeleteUnusingThing()
    {
        //添加包含依赖文件的文件夹，文件夹下所有的文件及其依赖文件都会被保存
        //路径格式以Assets开头，注意不要重复
        string[] directoryPath = {
			"Assets\\Scenes",
            "Assets\\Prefabs"
		};
        //添加依赖文件，此文件以及文件的依赖文件都会被保存
        //路径格式以Assets开头，注意不要重复
        string[] filePath = {
			""
		};
        //删除文件的路径，此路径下没有被引用到的资源会被删除
        string deletePath = "Assets\\Arctic_Castle";
        //添加受保护的文件列表，其中所有的文件都不会被删除
        List<string> protectFileList = new List<string>();
        protectFileList.Add("Assets\\Invector-3rdPersonController\\Scripts\\DeleteUnusingThings.cs");
        protectFileList.Add("Assets\\Invector-3rdPersonController\\Scripts\\TXTFileHelper.cs");
        //保存所有依赖文件名的txt文本的路径
        string dependentSavePath = "C:\\Users\\chenhaijian\\Desktop\\dependencies.txt";
        //保存所有文件名的txt文本路径
        string allSavePath = "C:\\Users\\chenhaijian\\Desktop\\all.txt";
        //保存被删除的文件名的txt文本路径
        string deleteSavePath = "C:\\Users\\chenhaijian\\Desktop\\delete.txt";
        //保存所有依赖文件名列表
        List<string> allDependenciesList = new List<string>();
        //保存去除重复的依赖文件名列表
        List<string> noRepeatDependenciesList = new List<string>();
        //保存已被删除的文件名列表
        List<string> deleteList = new List<string>();
        //保存工程中所有文件的列表
        List<string> allList = new List<string>();
        //将文件依赖源列表的所有项添加到allDependenciesList中
        foreach (var i in filePath)
        {
            if (File.Exists(i))
            {
                allDependenciesList.Add(i);
            }
            else
            {
                Debug.Log("文件路径不存在: " + i);
            }
        }
        //将文件夹依赖源列表中包含的所有文件添加到allDependenciesList中
        foreach (var i in directoryPath)
        {
            if (Directory.Exists(i))
            {
                DirectoryInfo directory = new DirectoryInfo(i);
                GetAllFiles(directory, allDependenciesList);
            }
            else
            {
                Debug.Log("文件夹路径不存在: " + i);
            }
        }
        //根据allDependenciesList列表，添加所有依赖文件名到列表中，并去掉重复项
        foreach (var i in allDependenciesList)
        {
            string[] dependencies = AssetDatabase.GetDependencies(i);
            foreach (var d in dependencies)
            {
                if (!noRepeatDependenciesList.Contains(d))
                {
                    noRepeatDependenciesList.Add(d.Replace("/", "\\"));
                }
            }
        }
        //将去除重复的依赖文件列表输出成txt文档
        TXTFileHelper.Write(noRepeatDependenciesList, dependentSavePath);
        //获取被删除文件夹中所有的文件名
        GetAllFiles(new DirectoryInfo(deletePath), allList);
        //将工程中所有的文件列表输出成txt文档
        TXTFileHelper.Write(allList, allSavePath);
        //对比工程文件列表和依赖文件列表，删除没有引用到的资源
        foreach (var i in allList)
        {
            if (!noRepeatDependenciesList.Contains(i))
            {
				if(!new FileInfo(i).Extension.Equals(".cs") || !new FileInfo(i).Extension.Equals(".asset") || !new FileInfo(i).Extension.Equals(".unity") || !new FileInfo(i).Extension.Equals(".mat"))
				{
					deleteList.Add(i);
					//删除操作(慎用)
                	File.Delete(i);
				}
            }
        }
        //将工程中所有的文件列表输出成txt文档
        TXTFileHelper.Write(deleteList, deleteSavePath);
        //TODO 删除空文件夹
        /*
        DirectoryInfo assets = new DirectoryInfo("Assets");
        DirectoryInfo[] allDirectories = assets.GetDirectories();
        foreach (var i in allDirectories)
        {
            FileInfo[] f = i.GetFiles();
        }
         * */
    }

    //保存传递的文件夹中所有的文件路径到列表中
    private static void GetAllFiles(DirectoryInfo directory, List<string> list)
    {
        FileInfo[] files = directory.GetFiles();
        DirectoryInfo[] directorys = directory.GetDirectories();
        foreach (var i in files)
        {
            if (!i.Extension.Equals(".meta"))
            {
                list.Add(i.FullName.Replace(Application.dataPath.Replace("/", "\\"), "Assets"));
            }
        }
        foreach (var i in directorys)
        {
            GetAllFiles(i, list);
        }
    }
}