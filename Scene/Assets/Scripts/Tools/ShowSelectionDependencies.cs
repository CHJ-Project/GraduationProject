using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tools{
	public class ShowSelectionDependencies : Editor {
		[MenuItem("Tools/显示选中物体的依赖关系",false,20)]
		private static void ShowSelectionDependency(){
			UnityEngine.Object selectedObject = Selection.activeObject;
			if(selectedObject == null) {
				Debug.Log ("没有选择任何物体！");
				return;
			}
			UnityEngine.Object[] roots = new UnityEngine.Object[]{ selectedObject };
			var objs = EditorUtility.CollectDependencies (roots);
			string path = AssetDatabase.GetAssetPath (selectedObject);
			string[] depList = AssetDatabase.GetDependencies (path);
			foreach (var dep in depList) {
				Debug.Log (dep);
			}
		}
	}
}