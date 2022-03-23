using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class InitialSceneAdder
{
    static InitialSceneAdder()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
#if UNITY_EDITOR
        const string initialSceneName = LevelProvider.InitialSceneName;
        
        if (PlayModeStateChange.EnteredPlayMode != obj) return;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (scene.name == initialSceneName) return;
        }

        SceneManager.LoadSceneAsync(initialSceneName, LoadSceneMode.Additive).completed += operation =>
        {
            Debug.Log("LevelChanger: Scene initializer added!");
        };
#endif
    }

}