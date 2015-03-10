using UnityEngine;

public class MonoStatic<T> : MonoBehaviour where T : MonoStatic<T>
{
    private static GameObject _instance;

    protected static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T find = FindObjectOfType<T>();
                _instance = find == null ? new GameObject(typeof (T).Name, typeof (T)) : find.gameObject;
            }
            return _instance.GetComponent<T>();
        }
    }
}