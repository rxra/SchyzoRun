using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject title;
	public GameObject restart;

	public void GameOver()
	{
		_started = false;
		Application.LoadLevel(2);
	}

	void Start() {
		if (_started && title!=null && title.activeSelf) {
			title.SetActive(true);
		}
		_started = false;
	}

	// Update is called once per frame
	void Update () {
		if (!_started && Input.anyKey) {
			Application.LoadLevel(1);
			_started = true;
		}
	}

	private static bool _started = false;

}
