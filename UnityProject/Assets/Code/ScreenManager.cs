using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {

	public Screen screenPrefab;
	public LayerMask screenLayer;

	public static ScreenManager instance
	{
		get
		{
			return s_Instance;
		}
	}

	public void AddScreen(Screen s)
	{
		_screens.Add (s);
		Debug.Log ("screens: " + _screens.Count);
	}

	public void RemoveScreen(Screen s, bool destroy)
	{
		_screens.Remove(s);
		if (destroy) {
			GameObject.Destroy(s.gameObject);
		}
		Debug.Log ("screens: " + _screens.Count);
	}

	static private ScreenManager s_Instance = null;

	void Awake () {
		if (s_Instance) {
			Debug.LogError ("Error: an instance of ScreenManager already exist");
			return;
		}
		s_Instance = this;
	}

	// Use this for initialization
	void Start () {
		Screen screen = GameObject.Instantiate(screenPrefab,Vector3.zero,Quaternion.identity) as Screen;
		screen.level = 0;
		screen.gameObject.name = "Screen_" + screen.level;
		AddScreen(screen);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonUp (0)) {
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _mouseHit, screenLayer)) {
				Screen s = _mouseHit.collider.gameObject.GetComponent<Screen>();
				if (s==null) {
					Debug.LogWarning("Cannot find Screen compoment on hitted object");
				} else {
					s.Split(_horizontal);
					_horizontal = !_horizontal;
				}
			}
		}	

	}

	private List<Screen> _screens = new List<Screen>();
	private RaycastHit _mouseHit = new RaycastHit();
	private bool _horizontal = true;

}
