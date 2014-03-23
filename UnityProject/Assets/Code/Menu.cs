using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public static Menu instance
	{
		get
		{
			return s_Instance;
		}
	}

	public void Restart()
	{
		_started = false;
		Application.LoadLevel(0);
	}

	static private Menu s_Instance = null;
	
	void Awake() {
		if (s_Instance==null) {
			s_Instance = this;
		}
		DontDestroyOnLoad(transform.gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (!_started && Input.anyKey) {
			Application.LoadLevel(1);
			_started = true;
		}
	}

	private bool _started = false;

}
