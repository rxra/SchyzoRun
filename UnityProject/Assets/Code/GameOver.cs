using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public float timerBeforeKey = 2;

	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad>timerBeforeKey && Input.anyKey) {
			Application.LoadLevel(1);
		}
	}
	
}
