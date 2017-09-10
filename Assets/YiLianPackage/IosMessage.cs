using UnityEngine;
using System.Collections;

public class IosMessage : MonoBehaviour {
	public string message="没收到";
	public static IosMessage instance;
	void Awake () {
		
		if (instance==null) {
			instance = this;
		} else {
			DestroyObject (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	public void Message (string value) {
		Debug.Log ("接收到Message了:"+value);
		Debug.Log ("接收到Message了:"+value);
		message = value;
	}
}
