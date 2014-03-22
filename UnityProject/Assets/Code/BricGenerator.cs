using UnityEngine;
using System.Collections;

public class BricGenerator : MonoBehaviour {

	public bool reality = true;
	public float realityPercent = 0f;

	void Start() {

		//bool generateBonus = Random.Range(0f,100f)<realityPercent?false:true;

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
			} else if (t.gameObject.tag=="Obstacle") {
				if (Random.Range(0f,100f)<50) {
					GameObject.Destroy(t.gameObject);
				}
			} else {
				//if (!generateBonus && (t.gameObject.tag=="Eart" || t.gameObject.tag=="BigEart")) {
				if (realityPercent>0f && (t.gameObject.tag=="Eart" || t.gameObject.tag=="BigEart")) {
					bool generateBonus = Random.Range(0f,100f)<realityPercent?false:true;
					if (!generateBonus) {
						GameObject.Destroy(t.gameObject);
					}
				}
			}
		}
	}
}
