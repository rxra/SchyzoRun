﻿using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {


	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			Application.LoadLevel(1);
		}
	}
}
