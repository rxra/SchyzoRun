using UnityEngine;
using System.Collections;

public class BricGenerator : MonoBehaviour {

	public bool reality = true;

	void Start() {
		foreach(Transform t in transform) {
			BoxCollider b = t.GetComponent<BoxCollider>();
			if (b) {
				b.isTrigger = true;
				b.size = new Vector3(b.size.x,b.size.y,20);
			}
			if (t.gameObject.name=="Middle") {
				t.gameObject.renderer.material.color = reality?ScreenManager.instance.realityColor2:ScreenManager.instance.fantasyColor2;
			} else 	if (t.gameObject.name=="Left" || t.gameObject.name=="Right") {
				t.gameObject.renderer.material.color = reality?ScreenManager.instance.realityColor1:ScreenManager.instance.fantasyColor1;
			}
		}
	}
}
