using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelProvider
{
    public const string InitialSceneName = "InitialScene";

    public static int LevelRaw
    {
        get => PlayerPrefs.GetInt("level");
        set => PlayerPrefs.SetInt("level", value);
    }
    
    public static AsyncOperation LoadLastLevel()
    {
        ClearLevels();
        var asyncOperation = LoadLevel(LevelRaw);

        return asyncOperation;
    }
    
    public static AsyncOperation LoadNextLevel()
    {
        ClearLevels();
            
        LevelRaw += 1;
        var asyncOperation = LoadLevel(LevelRaw);
        
        return asyncOperation;
    }

    private static AsyncOperation LoadLevel(int level)
    {
        var initialSceneIndex = 0;
        
        for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (SceneManager.GetSceneByBuildIndex(i).name == InitialSceneName)
                initialSceneIndex = i;
        }
        
        var modularOffset = initialSceneIndex + 1;
        var loadSceneIndex = ((level % (SceneManager.sceneCountInBuildSettings - modularOffset)) + modularOffset);

        return SceneManager.LoadSceneAsync(loadSceneIndex, LoadSceneMode.Additive);
    }

    private static void ClearLevels()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (scene.name != InitialSceneName)
                SceneManager.UnloadSceneAsync(scene.buildIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }
    }
}
