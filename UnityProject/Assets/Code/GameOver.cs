using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public GUIText text;
	public float timerBeforeKey = 2;

	void Start()
	{
		int h = PlayerPrefs.GetInt("Hearts");
		text.text = h.ToString();
	}

	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad>timerBeforeKey && Input.anyKey) {
			Application.LoadLevel(1);
		}
	}
	
}
