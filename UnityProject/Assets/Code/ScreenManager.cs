using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {

	public Screen screenPrefab;
	public Player playerFantasyPrefab;
	public Player playerRealityPrefab;
	public LayerMask screenLayer;
	public BricGenerator[] brics;
	public BricGenerator[] easyBrics;
	public BricGenerator[] mediumBrics;
	public BricGenerator[] hardBrics;
	public Color realityColor1;
	public Color realityColor2;
	public Color fantasyColor1;
	public Color fantasyColor2;
	public int difficulty = 1;
	public int difficultyMax = 5;
	public float speed = 5;
	public float hspeed = 5;
	public Material fantasyLeft;
	public Material fantasyMiddle;
	public Material fantasyRight;
	public Material realityLeft;
	public Material realityMiddle;
	public Material realityRight;

	public static ScreenManager instance
	{
		get
		{
			return s_Instance;
		}
	}

	public BricGenerator NextBloc(ref int idx)
	{
		if (_currentBric!=null) {
			_currentScreenGeneration++;
			BricGenerator ogo = _currentBric;
			if(_currentScreenGeneration>=_screens.Count) {
				_currentScreenGeneration = 0;
				_currentBric = null;
			}
			return ogo;
		}
		//BricGenerator go = brics[idx];
		//idx = (idx+1)%brics.Length;
		//return go;

		BricGenerator go = null;

		switch(difficulty) {
		case 1:
			go = easyBrics[Random.Range (0,easyBrics.Length)];
			break;

		case 2:
			{
				int bidx = Random.Range (0,easyBrics.Length + mediumBrics.Length);
				if (bidx<easyBrics.Length) {
					 go = easyBrics[bidx];
				} else {
					bidx -= easyBrics.Length;
					go = mediumBrics[bidx];
				}
			}
			break;

		case 3:
			go = mediumBrics[Random.Range (0,mediumBrics.Length)];
			break;

		case 4:
			{
				int bidx = Random.Range (0,mediumBrics.Length + hardBrics.Length);
				if (bidx<mediumBrics.Length) {
					go = mediumBrics[bidx];
				} else {
					bidx -= mediumBrics.Length;
					go = hardBrics[bidx];
				}
			}
			break;

		case 5:
			go = hardBrics[Random.Range (0,hardBrics.Length)];
			break;
		}

		_currentScreenGeneration++;
		if(_currentScreenGeneration>=_screens.Count) {
			_currentScreenGeneration = 0;
			_currentBric = null;
		} else {
			_currentBric = go;
		}

		return go;
	}

	public void AddScreen(Screen s, bool addToTheEnd)
	{
		if (addToTheEnd) {
			_screens.Add (s);
		} else {
			_screens.Insert (0,s);
		}
	}

	public void RemoveScreen(Screen s, bool destroy)
	{
		_screens.Remove(s);
		if (destroy) {
			s.DestroyScreen();
		}
	}

	public void SplitScreen(Screen s)
	{
		s.SplitDouble(true);
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
		screen.screenReality = 0;
		screen.bricReality = false;
		screen.gameObject.name = "Screen_" + screen.level;
		AddScreen(screen,true);
	}
	
	// Update is called once per frame
	void Update () {

		float h = Input.GetAxis("Horizontal");
		foreach (Screen s in _screens) {
			if (!s.InputPlayer(h,hspeed)) {
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

		/*if (Input.GetKeyDown(KeyCode.Space)) {
			Screen s = _screens[0];
			s.SplitDouble(false);
		}*/

		/*if (Input.GetKeyDown(KeyCode.Space)) {
			_screens[0].SplitDouble(true);
		}*/

		if (Input.GetKeyDown(KeyCode.L)) {
			difficulty++;
			if (difficulty>difficultyMax) {
				difficulty = difficultyMax;
			}
		}
		
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
			AddScreen(screen,true);
			screen.InitSplittedScreen(i);
			if (i==0) {
				screen.screenReality = 0;
				screen.bricReality = false;
			} else if (i==count-1) {
				screen.screenReality = 100;
				screen.bricReality = true;
			} else {
				screen.screenReality = 100f*i/((float)(count-1));
				screen.bricReality = Random.Range(0f,100f)<screen.screenReality?true:false;
			}
		}
	}

	private List<Screen> _screens = new List<Screen>();
	private RaycastHit _mouseHit = new RaycastHit();
	private BricGenerator _currentBric = null;
	private int _currentScreenGeneration = 0;

}
