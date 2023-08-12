using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static Scene targetScene;
    public enum Scene
    {
        MainMenuScene,
        LoadingScene,
        GameScene
    }
    public static void LoadScene(Scene scene)
    {
        if (scene == Scene.LoadingScene) return;
        targetScene = scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString(), LoadSceneMode.Single);
    }

    public static void SceneLoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }
}