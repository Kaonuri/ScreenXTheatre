using UnityEngine;

public abstract class Scene : MonoBehaviour
{
    public abstract string sceneName { get; }

    protected virtual void Start()
    {
        SceneManager.Instance.NotifySceneLoaded(this);        
    }
}
