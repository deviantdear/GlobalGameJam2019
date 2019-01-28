using UnityEngine;

// Modified from http://wiki.unity3d.com/index.php/Toolbox
/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	private static object _lock = new object();
	private static bool applicationIsQuitting = false;

	public static bool Exists { get { return _instance != null && !applicationIsQuitting; } }

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				Debug.LogWarningFormat("[{0}] Instance already destroyed on application quit.", typeof(T));
				return null;
			}

			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();
					if (!_instance)
					{
						var prefabs = Resources.LoadAll<T>("");
						if (prefabs.Length > 0)
						{
							_instance = Instantiate(prefabs[0]) as T;
						}
						else
						{
							Debug.LogErrorFormat("[{0}] Singleton prefab could not be loaded. Make sure there is a prefab of type '{0}' in the Assets/Resources folder!", typeof(T));
							return null;
						}
					}
					if (FindObjectsOfType<T>().Length > 1)
						Debug.LogWarningFormat("[{0}] Multiple Singleton instances found. There should never be more than 1 singleton!", typeof(T));
					Debug.LogFormat("[{0}] Using instance: {1}", typeof(T), _instance.gameObject.name);
					DontDestroyOnLoad(_instance);
				}

				return _instance;
			}
		}
	}

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it has been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	///
	///	Jeff's addition:
	///		Changed from OnDestroy() to OnApplicationQuit() to prevent need to mark OnDestroy as virtual
	/// </summary>
	private void OnApplicationQuit()
	{
		applicationIsQuitting = true;
	}

	public static void Load()
	{
		var instance = Instance;
	}
}
