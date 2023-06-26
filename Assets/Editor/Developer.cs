using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class Developer : MonoBehaviour
{
    [MenuItem("Developer/Open Scene/Game")]
    public static void OpenMainScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        }

    }

    [MenuItem("Developer/Open Scene/Menu")]
    public static void OpenMenuScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity");
        }

    }

   
    [MenuItem("Developer/Player/Full Restore")]
    public static void PlayerFullRestore()
    {
       

    }

    [MenuItem("Developer/Player/Add 100 Exp")]
    public static void PlayerGain100()
    {
     

    }

    [MenuItem("Developer/Player/Add 1000 Exp")]
    public static void PlayerGain1000()
    {
     
    }


    [MenuItem("Developer/Player/Kill")]
    public static void KillPlayer()
    {
       

    }

    [MenuItem("Developer/Clear PlayerPrefs")]
    public static void ClearSaves()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All saves have been cleared!");
    }

}
