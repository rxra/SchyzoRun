using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {
	
	public Screen screenPrefab;
	public Player playerFantasyPrefab;
	public Player playerRealityPrefab;
	public GameObject princessFantasyPrefab;
	public GameObject princessRealityPrefab;
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
	public GUIText text;

	public Material fantasyObstacle1;
	public Material realityObstacle1;
	public Material fantasyObstacle2;
	public Material realityObstacle2;
	public Material fantasyObstacle3;
	public Material realityObstacle3;
	public Material heart;

	public float invulnerabilityTimer = 1f;
	public AudioSource splitSound;
	public AudioSource unsplitSound;

	AudioSource source;
	AudioSource source0;
	AudioSource source1;
	AudioSource source2;
	AudioSource source3;
	AudioSource source4;

	public int screenCount
	{
		get
		{
			return _screens.Count;
		}
	}

	public static ScreenManager instance
	{
		get
		{
			return s_Instance;
		}
	}
	
	public void HeartHitted(bool big)
	{
		_heart++;
		text.text = _heart.ToString();
	}
	
	public void ObsctableHitted()
	{
		if ((Time.time-_startInvulnerability)<invulnerabilityTimer) {
			return;
		}
		if (_screens.Count==1) {
			PlayerPrefs.SetInt("Hearts",_heart);
			Application.LoadLevel(2);
		} else {
			UndivideScreen();
			_flash = true;
			foreach (Screen s in _screens) {
				s.FlashPlayer(true);
			}
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
		
		_generatedBrics++;
		Debug.Log (_generatedBrics);
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
	
	/*public void SplitScreen(Screen s)
	{
		s.SplitDouble(true);
		//s.SplitQuad();
	}*/
	
	static private ScreenManager s_Instance = null;
	
	void turnOffTrack(int i) {
		if (i == 0)
			source0.volume = 0.0f;
		if (i == 1)
			source1.volume = 0.0f;
		if (i == 2)
			source2.volume = 0.0f;
		if (i == 3)
			source3.volume = 0.0f;
		if (i == 4)
			source4.volume = 0.0f;
	}
	
	void turnOnTrack(int i) {
		if (i == 0)
			source0.volume = 1.0f;
		if (i == 1)
			source1.volume = 1.0f;
		if (i == 2)
			source2.volume = 1.0f;
		if (i == 3)
			source3.volume = 1.0f;
		if (i == 4)
			source4.volume = 1.0f;
	}
	
	private int nb_track = 1;
	
	void setTracksOnOff() {
		int i;
		print ("nb_tracks = " + nb_track);
		for (i=0; i<nb_track; i++)
			turnOnTrack(i);
		for (i=nb_track; i<5; i++)
			turnOffTrack(i);
	}

	void Awake () {
		if (s_Instance) {
			Debug.LogError ("Error: an instance of ScreenManager already exist");
			return;
		}
		s_Instance = this;
		_startSpeed = speed;
	}
	
	// Use this for initialization
	void Start () {
		source = gameObject.AddComponent<AudioSource>();
		source0 = gameObject.AddComponent<AudioSource>();
		source1 = gameObject.AddComponent<AudioSource>();
		source2 = gameObject.AddComponent<AudioSource>();
		source3 = gameObject.AddComponent<AudioSource>();
		source4 = gameObject.AddComponent<AudioSource>();
		source.loop = true;
		source0.loop = true;
		source1.loop = true;
		source2.loop = true;
		source3.loop = true;
		source4.loop = true;
		source.clip = Resources.Load("Intro") as AudioClip;
		source.Play();
		source0.clip = Resources.Load("Band_1") as AudioClip;
		source0.Play();
		source1.clip = Resources.Load("Band_2") as AudioClip;
		source1.Play();
		source2.clip = Resources.Load("Band_3") as AudioClip;
		source2.Play();
		source3.clip = Resources.Load("Band_4") as AudioClip;
		source3.Play();
		source4.clip = Resources.Load("Band_5") as AudioClip;
		source4.Play();
		ScreenManager.instance.setTracksOnOff ();

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
		screen.Initialize();
		AddScreen(screen,true);
	}
	
	// Update is called once per frame
	void Update () {
		
		float h = Input.GetAxis("Horizontal");
		if (h > 0) {
			h = 1.0f;
		} else if (h < 0) {
			h = -1.0f;
		} else {
			h = 0;
		}
		foreach (Screen s in _screens) {
			if (!s.InputPlayer(h,hspeed)) {
				break;
			}
		}
		
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			DivideScreen();
		}*/
		
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			Screen s = _screens[0];
			s.SplitDouble(false);
		}*/
		
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			_screens[0].SplitDouble(true);
		}*/
		
		/*if (Input.GetKeyDown(KeyCode.L)) {
			difficulty++;
			if (difficulty>difficultyMax) {
				difficulty = difficultyMax;
			}
		}*/
		
	}
	
	private void LateUpdate()
	{
		if (difficulty==1) {
			if (_generatedBrics==4) {
				DivideScreen();
				difficulty++;
				_generatedBrics = 0;
			}
		} else if (difficulty<5) {
			if (_generatedBrics==4) {
				DivideScreen();
				difficulty++;
				_generatedBrics = 0;
			}
		} else if (_generatedBrics==4) {
			_generatedBrics = 0;
			foreach(Screen s in _screens) {
				if (_currentRealityLevel<RealityLevel.Middle) {
					s.realityLevel = (RealityLevel)((int)s.realityLevel+1);
				}
			}
			if (_screens.Count>1) {
				_currentRealityLevel = _screens[1].realityLevel;
			}
		}

		if (_flash && (Time.time-_startInvulnerability)>invulnerabilityTimer) {
			_flash = false;
			foreach (Screen s in _screens) {
				s.FlashPlayer(false);
			}
		}

	}
	
	private void DivideScreen()
	{
		_startInvulnerability = Time.time;
		int count = _screens.Count+1;
		foreach(Screen s in _screens) {
			s.DestroyScreen();
		}
		_screens.Clear();
		GenerateScreens(count);
		nb_track++;
		ScreenManager.instance.setTracksOnOff ();

		splitSound.Play();

		_flash = true;
		foreach (Screen s in _screens) {
			s.FlashPlayer(true);
		}
	}
	
	private void UndivideScreen()
	{
		_startInvulnerability = Time.time;
		_currentRealityLevel = _screens[1].realityLevel;
		Debug.Log ("realityLevel: " + _currentRealityLevel);
		int count = _screens.Count-1;
		foreach(Screen s in _screens) {
			s.DestroyScreen();
		}
		_screens.Clear();
		GenerateScreens(count);
		nb_track--;
		ScreenManager.instance.setTracksOnOff ();

		unsplitSound.Play();
	}
	
	private void GenerateScreens(int count)
	{
		speed = _startSpeed - (_startSpeed * (count-1) * 0.1f);

		for (int i=0;i<count;i++) {
			Screen screen = GameObject.Instantiate(screenPrefab,Vector3.zero,Quaternion.identity) as Screen;
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
			screen.realityLevel = (RealityLevel)(_currentRealityLevel+i);
			Debug.Log ("screen.realityLevel: " + screen.realityLevel);
			screen.Initialize();
		}
	}
	
	private List<Screen> _screens = new List<Screen>();
	//private RaycastHit _mouseHit = new RaycastHit();
	private BricGenerator _currentBric = null;
	private int _currentScreenGeneration = 0;
	private int _generatedBrics = 0;
	private int _heart = 0;
	private RealityLevel _currentRealityLevel = RealityLevel.None;
	private float _startInvulnerability;
	private float _startSpeed;
	private bool _flash = false;
	
}
