using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    [SerializeField]
    public bool destroyOnLoad;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    var obj = new GameObject
                    {
                        name = typeof(T).Name
                    };
                    instance = obj.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (!destroyOnLoad)
            {
                //Debug.Log($"{gameObject.name} Will NOT destroy on load");
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                //Debug.Log($"{gameObject.name} Will destroy on load");
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
}