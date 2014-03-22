using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {

	public Screen screenPrefab;
	public Player playerPrefab;
	public LayerMask screenLayer;
	public BricGenerator[] brics;
	public Color realityColor1;
	public Color realityColor2;
	public Color fantasyColor1;
	public Color fantasyColor2;

	public static ScreenManager instance
	{
		get
		{
			return s_Instance;
		}
	}

	public BricGenerator NextBloc(ref int idx)
	{
		BricGenerator go = brics[idx];
		idx = (idx+1)%brics.Length;
		return go;
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
			s.DestroyScreen();
		}
		Debug.Log ("screens: " + _screens.Count);
	}

	public void SplitScreen(Screen s)
	{
		s.SplitDouble();
		//s.SplitQuad();
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
		screen.transform.localScale = new Vector3(
			Camera.main.orthographicSize*Camera.main.aspect*2,
			Camera.main.orthographicSize*2,
			1
		);
		screen.level = 1;
		screen.reality = 0;
		screen.bricReality = false;
		screen.gameObject.name = "Screen_" + screen.level;
		AddScreen(screen);
	}
	
	// Update is called once per frame
	void Update () {

		float h = Input.GetAxis("Horizontal");
		foreach (Screen s in _screens) {
			if (!s.InputPlayer(h)) {
				break;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			int count = _screens.Count+1;
			foreach(Screen s in _screens) {
				s.DestroyScreen();
			}
			_screens.Clear();
			GenerateScreens(count);
		}

		/*if (Input.GetMouseButtonUp(0)) {
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _mouseHit, screenLayer)) {
				Screen s = _mouseHit.collider.gameObject.GetComponent<Screen>();
				if (s==null) {
					Debug.LogWarning("Cannot find Screen compoment on hitted object");
				} else {
					s.SplitDouble();
				}
			}
		}*/

	}

	private void GenerateScreens(int count)
	{
		for (int i=0;i<count;i++) {
			Screen screen = GameObject.Instantiate(screenPrefab,Vector3.zero,Quaternion.identity) as Screen;
			screen.transform.localScale = new Vector3(
				Camera.main.orthographicSize*Camera.main.aspect*2/count,
				Camera.main.orthographicSize*2,
				1
			);
			screen.level = count;
			screen.gameObject.name = "Screen_" + screen.level;
			AddScreen(screen);
			screen.InitSplittedScreen(i);
			if (i==0) {
				screen.reality = 0;
				screen.bricReality = false;
			} else if (i==count-1) {
				screen.reality = 100;
				screen.bricReality = true;
			} else {
				screen.reality = 100f*i/((float)(count-1));
				screen.bricReality = Random.Range(0f,100f)<screen.reality?true:false;
			}
		}
	}

	private List<Screen> _screens = new List<Screen>();
	private RaycastHit _mouseHit = new RaycastHit();

}
