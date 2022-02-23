using System;
using UnityEditor;

#if  UNITY_EDITOR
using UnityEditor.SceneManagement;

// 에디터에서 씬을 빠르고 쉽게 열어서 작업할수 있도록 도와줍니다.

public class EditorSceneOpen
{
    [MenuItem("Scenes/1.IntroScene")]
    public static void OpenScene_Intro()
    {
        OpenScene("Assets/1_Scenes/Intro.unity");
    }
    
    [MenuItem("Scenes/2.LobbyScene")]
    public static void OpenScene_Lobby()
    {
        OpenScene("Assets/1_Scenes/Lobby.unity");
    }
    
    [MenuItem("Scenes/3.MapScene")]
    public static void OpenScene_Map()
    {
        OpenScene("Assets/1_Scenes/Map.unity");
    }
    
    [MenuItem("Scenes/4.IngameScene")]
    public static void OpenScene_Ingame()
    {
        OpenScene("Assets/1_Scenes/Ingame.unity");
    }   
    
    [MenuItem("Scenes/5.ShopScene")]
    public static void OpenScene_Shop()
    {
        OpenScene("Assets/1_Scenes/Shop.unity");
    }
    
    [MenuItem("Scenes/6.TutorialScene")]
    public static void OpenScene_Tutorial()
    {
        OpenScene("Assets/1_Scenes/Tutorial.unity");
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
