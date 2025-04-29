using Unity.VisualScripting;
using UnityEngine;

public abstract class Singleton<T> :  MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance ??= FindAnyObjectByType(typeof(T)).GetComponent<T>();
}
