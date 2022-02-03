using System;
using UnityEditor;

#if  UNITY_EDITOR
using UnityEditor.SceneManagement;

// 에디터에서 씬을 빠르고 쉽게 열어서 작업할수 있도록 도와줍니다.

public class EditorSceneOpen
{
    [MenuItem("Scenes/1.IngameScene")]
    public static void OpenScene_Title()
    {
        OpenScene("Assets/1_Scenes/Ingame.unity");
    }
    
    public static void OpenScene(string scenepath)
    {
        if(EditorSceneManager.GetActiveScene().isDirty == true)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene(scenepath);
    }
}
#endif
