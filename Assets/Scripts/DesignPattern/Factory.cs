using UnityEngine;
using System.Collections;

namespace DesignPattern
{

	public class Factory<T> : MonoBehaviour where T : Factory<T>
	{
		
		public static T New ()
		{
			GameObject gameObject = new GameObject (typeof(T).ToString());
			T component = gameObject.AddComponent<T>();
			return component;
		}
		
		public static T New (string _prefabPath)
		{
			GameObject gameObject = Instantiate(Resources.Load(_prefabPath)) as GameObject;
			T component = gameObject.GetComponent<T>();
			if(component == null)
				Destroy(gameObject);
			return component;
		}

	}

}