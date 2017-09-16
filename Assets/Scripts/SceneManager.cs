using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager
{
    private static SceneManager _instance;

    public static SceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SceneManager();
            }
            return _instance;
        }
    }

    public SceneManager()
    {
        _listeners = new List<ISceneEventListener>();
    }

    public interface ISceneEventListener
    {
        void SceneLoaded(SceneManager manager, Scene sceneLoaded);
    }

    private readonly List<ISceneEventListener> _listeners;

    public Scene CurrentScene { get; private set; }

    public void AddListener(ISceneEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(ISceneEventListener listener)
    {
        _listeners.Remove(listener);
    }

    public IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = Application.LoadLevelAsync(sceneName);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            yield return 0;
        }

        System.GC.Collect();
        async.allowSceneActivation = true;
    }

    public void NotifySceneLoaded(Scene scene)
    {
        CurrentScene = scene;

        foreach (ISceneEventListener listener in _listeners)
        {
            listener.SceneLoaded(this, CurrentScene);
        }
    }
}
