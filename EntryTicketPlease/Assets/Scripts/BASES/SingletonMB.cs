using UnityEngine;

public class SingletonMB<T> : MonoBehaviour where T : SingletonMB<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"An instance of {GetType()} in {gameObject.name} already exists. Destroying this one.");
            Destroy(this);
        }
    }
}
