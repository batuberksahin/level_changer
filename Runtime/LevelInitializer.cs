using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInitializer : MonoBehaviour
{
    [Header("Events")] 
    [SerializeField] private IntGameEvent onGameOpened;
    [SerializeField] private IntGameEvent onLevelSceneLoaded;
    [SerializeField] private IntGameEvent onLevelStarted;

    private Scene m_InitialScene;
    private Scene m_CurrentScene;
    
    private void Start()
    {
        m_InitialScene = SceneManager.GetActiveScene();
        m_CurrentScene = SceneManager.GetActiveScene();
        
        Application.targetFrameRate = 240;
        QualitySettings.vSyncCount = 0;
        
        onGameOpened.Raise(0);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelCoroutine());
    }
    
    public void LoadRestartLevel()
    {
        StartCoroutine(LoadLastLevelCoroutine());
    }

    public void LoadLastLevel()
    {
        if (m_CurrentScene.name != LevelProvider.InitialSceneName)
        {
            onLevelSceneLoaded.Raise(LevelProvider.LevelRaw);
            return;
        }
        
        StartCoroutine(LoadLastLevelCoroutine());
    }
    
    private IEnumerator LoadNextLevelCoroutine()
    {
        var asyncOperation = LevelProvider.LoadNextLevel();
        while (!asyncOperation.isDone) yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        m_CurrentScene = SceneManager.GetActiveScene();
        
        onLevelSceneLoaded.Raise(LevelProvider.LevelRaw);
    }

    private IEnumerator LoadLastLevelCoroutine()
    {
        var asyncOperation = LevelProvider.LoadLastLevel();
        while (!asyncOperation.isDone) yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        m_CurrentScene = SceneManager.GetActiveScene();
        
        onLevelSceneLoaded.Raise(LevelProvider.LevelRaw);
    }
}