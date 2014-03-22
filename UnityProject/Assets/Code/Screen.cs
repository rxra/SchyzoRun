using UnityEngine;
using System.Collections;

public class Screen : MonoBehaviour {

	public int level = 0;
	public Color color = Color.blue;

	public void Split(bool horizontal)
	{
		Debug.Log ("++ Split: " + gameObject.name);
		Screen s1 = GameObject.Instantiate(this) as Screen;
		Screen s2 = GameObject.Instantiate(this) as Screen;

		s1.level = level+1;
		s2.level = level+1;
		s1.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));
		s2.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));

		if (horizontal) {
			s1.gameObject.name = "Screen_"+s1.level+"_up";
			s1.transform.localScale = new Vector3(
				transform.localScale.x,
				transform.localScale.y/2f,
				transform.localScale.z
			);
			s1.transform.position = new Vector3(
				transform.position.x,
				transform.position.y + s1.transform.localScale.y/2f,
				transform.position.z
			);


			s2.gameObject.name = "Screen_"+s1.level+"_down";
			s2.transform.localScale = new Vector3(
				transform.localScale.x,
				transform.localScale.y/2f,
				transform.localScale.z
				);
			s2.transform.position = new Vector3(
				transform.position.x,
				transform.position.y - s2.transform.localScale.y/2f,
				transform.position.z
				);
		} else {
			s1.gameObject.name = "Screen_"+s1.level+"_right";
			s1.transform.localScale = new Vector3(
				transform.localScale.x/2f,
				transform.localScale.y,
				transform.localScale.z
				);
			s1.transform.position = new Vector3(
				transform.position.x + s1.transform.localScale.x/2f,
				transform.position.y,
				transform.position.z
				);

			s2.gameObject.name = "Screen_"+s1.level+"_left";
			s2.transform.localScale = new Vector3(
				transform.localScale.x/2f,
				transform.localScale.y,
				transform.localScale.z
				);
			s2.transform.position = new Vector3(
				transform.position.x - s2.transform.localScale.x/2f,
				transform.position.y,
				transform.position.z
				);
		}

		ScreenManager.instance.AddScreen(s1);
		ScreenManager.instance.AddScreen(s2);
		ScreenManager.instance.RemoveScreen(this,true);

		Debug.Log ("-- Split");
	}

	void Start () {
		renderer.material.color = color;
	}
	
}
