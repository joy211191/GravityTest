using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T m_Instance;
    private static readonly object m_Lock = new object();

    public static T Instance {
        get {
            lock (m_Lock) {
                if (m_Instance != null)
                    return m_Instance;
                m_Instance = (T)FindObjectOfType(typeof(T));
                if (m_Instance != null) return m_Instance;
                var singletonObject = new GameObject();
                m_Instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString();
                return m_Instance;
            }
        }
    }
}