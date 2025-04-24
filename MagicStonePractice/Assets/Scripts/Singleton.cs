using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> :  MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType(typeof(T)).GetComponent<T>();
            }
            
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
}
