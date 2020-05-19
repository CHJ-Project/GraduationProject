using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScenePlug_in : EditorWindow {

	[MenuItem("MyEditor/ScenePlug-in")]
    static void AddWindow()
    {
        /*
        Rect wr = new Rect(0, 0, 500, 500);
        ScenePlug_in window = (ScenePlug_in)EditorWindow.GetWindowWithRect(typeof(ScenePlug_in), wr, true, "CreatWindow");
        window.Show();
         * */
        ScenePlug_in.CreateInstance<ScenePlug_in>().Show();
    }

    Vector2 scrollPos;
    Rect rect = new Rect(0,0,200,200);
    string[] str = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    int gridIndex = 0;
    bool isFit = true;
	public bool isEdit = false;

    //绘制窗口时调用
    void OnGUI()
    {
        //开启横向布局
        EditorGUILayout.BeginHorizontal();
        //开启纵向布局
        EditorGUILayout.BeginVertical();

        if(GUILayout.Button("加载预设",GUILayout.Width(130),GUILayout.Height(25)))
        {
            //加载预设
        }
        GUILayout.Space(3);
        if (GUILayout.Button("将当前场景保存为预设", GUILayout.Width(130), GUILayout.Height(25)))
        {
            //将当前场景保存为预设
        }
        GUILayout.Space(3);
        if (GUILayout.Button("保存场景", GUILayout.Width(130), GUILayout.Height(25)))
        {
            //保存场景
        }
        GUILayout.Space(3);
        if (GUILayout.Button("关闭窗口", GUILayout.Width(130), GUILayout.Height(25)))
        {
            this.Close();
        }
        GUILayout.Space(3);
        isFit = GUILayout.Toggle(isFit, "自动贴合");
        GUILayout.Space(3);
		isEdit = GUILayout.Toggle(isEdit, "快速复制选中对象");
		GUILayout.Space (3);
        //结束纵向布局
        EditorGUILayout.EndVertical();
        //滚动列表
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        //GUILayout.Box(new GUIContent("一个BOX"), new[] { GUILayout.Height(50), GUILayout.Width(50) });
        GUILayout.BeginHorizontal();
        gridIndex = GUILayout.SelectionGrid(gridIndex, str, 6);
        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        //结束横向布局
        EditorGUILayout.EndHorizontal();
    }
}