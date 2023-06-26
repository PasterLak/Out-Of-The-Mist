using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    public static T Instance { get; private set; }
    
    public static bool Exists => Instance != null;

    public static bool TryGetInstance(out T instance)
    {
        instance = Instance;
        return Exists;
    }
 
    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else
        {
            Debug.LogError("More than one Singleton object!");
            Destroy(gameObject);
        }
            
    }

}