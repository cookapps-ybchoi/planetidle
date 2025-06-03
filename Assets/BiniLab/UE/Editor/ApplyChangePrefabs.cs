using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ApplyChangePrefabs {

	[MenuItem("GameObject/Apply Prefab Changes", false, 0)]
	public static void ApplyChanges()
	{
		foreach (GameObject obj in Selection.gameObjects)
		{
			GameObject prefab_root = PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
			Object prefab_src = PrefabUtility.GetCorrespondingObjectFromSource(prefab_root);
			if(prefab_src != null)
			{
				string prefabPath = AssetDatabase.GetAssetPath(prefab_src);
				PrefabUtility.SaveAsPrefabAssetAndConnect(prefab_root, prefabPath, InteractionMode.AutomatedAction);
				Debug.Log("Updating prefab : "+prefabPath);
			}
			else
			{
				Debug.Log("Selected has no prefab");
			}
		}
	}
}
