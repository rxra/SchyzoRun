﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Screen : MonoBehaviour {

	public int level = 0;
	public Color color = Color.blue;
	public float screenReality = 0f;
	public bool bricReality = true;
	public RealityLevel realityLevel = RealityLevel.None;

	public void FlashPlayer(bool start)
	{
		if (_player==null) {
			return;
		}
		_player.Flash(start);
	}

	public bool InputPlayer(float h, float hspeed)
	{
		if(_player==null) {
			return false;
		}
		return _player.Input(h,hspeed);
	}

	public void DestroyScreen()
	{
		GameObject.Destroy(_player.gameObject);
		GameObject.Destroy(_princess);
		foreach(BricGenerator bloc in _blocs) {
			GameObject.Destroy(bloc.gameObject);
		}
		GameObject.Destroy(gameObject);
	}

	public void InitSplittedScreen(int idx)
	{
		transform.localScale = new Vector3(
			Camera.main.orthographicSize*Camera.main.aspect*2/level,
			Camera.main.orthographicSize*2,
			1
		);
		transform.position = new Vector3(
			-Camera.main.orthographicSize*Camera.main.aspect + transform.localScale.x/2f + transform.localScale.x*idx,
			0,
			0
		);
	}

	public void SplitDouble(bool addToTheEnd)
	{
		Screen s1 = GameObject.Instantiate(this) as Screen;
		Screen s2 = GameObject.Instantiate(this) as Screen;

		s1.level = level+1;
		s2.level = level+1;
		s1.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));
		s2.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));

		//bool horizontal = transform.localScale.y>transform.localScale.x?true:false;
		bool horizontal = false;

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

		ScreenManager.instance.AddScreen(s2,addToTheEnd);
		ScreenManager.instance.AddScreen(s1,addToTheEnd);
		ScreenManager.instance.RemoveScreen(this,true);
	}

	public void SplitQuad()
	{
		Screen s1 = GameObject.Instantiate(this) as Screen;
		Screen s2 = GameObject.Instantiate(this) as Screen;
		Screen s3 = GameObject.Instantiate(this) as Screen;
		Screen s4 = GameObject.Instantiate(this) as Screen;

		s1.level = level+1;
		s2.level = level+1;
		s3.level = level+1;
		s4.level = level+1;
		s1.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));
		s2.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));
		s3.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));
		s4.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));

		s1.gameObject.name = "Screen_"+s1.level+"_UL";
		s1.transform.localScale = new Vector3(
			transform.localScale.x/2f,
			transform.localScale.y/2f,
			transform.localScale.z
		);
		s1.transform.position = new Vector3(
			transform.position.x - s1.transform.localScale.x/2f,
			transform.position.y + s1.transform.localScale.y/2f,
			transform.position.z
		);
			
			
		s2.gameObject.name = "Screen_"+s2.level+"_UR";
		s2.transform.localScale = new Vector3(
			transform.localScale.x/2f,
			transform.localScale.y/2f,
			transform.localScale.z
		);
		s2.transform.position = new Vector3(
			transform.position.x + s2.transform.localScale.x/2f,
			transform.position.y + s2.transform.localScale.y/2f,
			transform.position.z
		);
				
		s3.gameObject.name = "Screen_"+s3.level+"_BL";
		s3.transform.localScale = new Vector3(
			transform.localScale.x/2f,
			transform.localScale.y/2f,
			transform.localScale.z
		);
		s3.transform.position = new Vector3(
			transform.position.x - s3.transform.localScale.x/2f,
			transform.position.y - s3.transform.localScale.y/2f,
			transform.position.z
		);
		
		s4.gameObject.name = "Screen_"+s3.level+"_BR";
		s4.transform.localScale = new Vector3(
			transform.localScale.x/2f,
			transform.localScale.y/2f,
			transform.localScale.z
		);
		s4.transform.position = new Vector3(
			transform.position.x + s4.transform.localScale.x/2f,
			transform.position.y - s4.transform.localScale.y/2f,
			transform.position.z
		);

		ScreenManager.instance.AddScreen(s1,true);
		ScreenManager.instance.AddScreen(s2,true);
		ScreenManager.instance.AddScreen(s3,true);
		ScreenManager.instance.AddScreen(s4,true);
		ScreenManager.instance.RemoveScreen(this,true);
	}

	public void Initialize () {
		//renderer.material.color = color;
		_idx = 0;
		//Player prefab = bricReality?ScreenManager.instance.playerRealityPrefab:ScreenManager.instance.playerFantasyPrefab;
		Player prefab = ((int)realityLevel>=(int)RealityLevel.Player)?ScreenManager.instance.playerRealityPrefab:ScreenManager.instance.playerFantasyPrefab;
		_player = GameObject.Instantiate(prefab) as Player;
		_player.Initialize(this);

		GameObject prefab2 = ((int)realityLevel>=(int)RealityLevel.Princess)?ScreenManager.instance.princessRealityPrefab:ScreenManager.instance.princessFantasyPrefab;
		//GameObject prefab2 = bricReality?ScreenManager.instance.princessRealityPrefab:ScreenManager.instance.princessFantasyPrefab;
		_princess = GameObject.Instantiate(prefab2) as GameObject;
		_princess.transform.localScale = new Vector3(
			_princess.transform.localScale.x/(float)level,
			_princess.transform.localScale.y/(float)level,
			_princess.transform.localScale.z
			);
		_princess.transform.position = new Vector3(
			transform.position.x,
			transform.position.y + Camera.main.orthographicSize - _princess.transform.localScale.y*3,
			_princess.transform.position.z
			);

		_nextBlocPosition = new Vector3(
			transform.position.x,
			transform.position.y - transform.localScale.y/2f,
			0
		);

		//CreateNextBloc();
		float cumulatedWidth = 0;
		while (cumulatedWidth<Camera.main.orthographicSize*2 && Mathf.Approximately(cumulatedWidth,Camera.main.orthographicSize*2)==false) {
			CreateNextBloc();
			cumulatedWidth += _lastBlockBounds.size.y;
		}

		_firstBloc = _blocs[0];

		_destroyBlocPosition = new Vector3(
			transform.position.x,
			transform.position.y - transform.localScale.y/2f,
			0
		);
		_generateBlocPosition = new Vector3(
			transform.position.x,
			transform.position.y + transform.localScale.y/2f,
			0
			);
	}

	void Update()
	{
		foreach(BricGenerator bloc in _blocs) {
			bloc.transform.Translate(-bloc.transform.up * ScreenManager.instance.speed * Time.deltaTime);
		}

		//if ((Camera.main.transform.position.x+_cameraWidth)>=_nextBlocPosition.x) {
		if (_lastBloc.transform.position.y+_lastBlockBounds.size.y/2f<=_generateBlocPosition.y) {
			/*
			 * Quand la moitié du bloc est sortie du générateur.
			 */
			_nextBlocPosition = new Vector3(
				_generateBlocPosition.x,
				_lastBloc.transform.position.y+_lastBlockBounds.size.y/2f,
				_generateBlocPosition.z
			);
			CreateNextBloc();
		}

		if (_blocs.Count>0 && _firstBloc.transform.position.y<(_destroyBlocPosition.y-_lastBlockBounds.size.y/2f)) {
			_blocs.RemoveAt(0);
			GameObject.Destroy(_firstBloc.gameObject);
			if (_blocs.Count>0) {
				_firstBloc = _blocs[0];
			}
			_nextBlocPosition = _generateBlocPosition;
		}

	}

	private void CreateNextBloc()
	{
		BricGenerator blocPrefab = ScreenManager.instance.NextBloc(ref _idx);
		BricGenerator bloc = GameObject.Instantiate(blocPrefab,_nextBlocPosition,Quaternion.identity) as BricGenerator;
		bloc.reality = bricReality;
		bloc.realityPercent = screenReality;
		bloc.realityLevel = realityLevel;
		bloc.transform.position = _nextBlocPosition;
		bloc.transform.localScale /= level;

		_lastBlockBounds = new Bounds(_nextBlocPosition,Vector3.zero);
		if (bloc.transform.childCount>0) {
			foreach(Transform t in bloc.transform) {
				if (t.gameObject.tag!="Obstacle" && t.renderer!=null) {
					_lastBlockBounds.Encapsulate(t.renderer.bounds);
				}
			}
		} else {
			_lastBlockBounds = bloc.renderer.bounds;
		}

		bloc.transform.Translate(bloc.transform.up * _lastBlockBounds.size.y/2f);

		_nextBlocPosition = new Vector3(
			bloc.transform.position.x,
			bloc.transform.position.y + _lastBlockBounds.size.y/2f,
			bloc.transform.position.z
			);

		_blocs.Add (bloc);
		_lastBloc = bloc;
	}

	private int _idx;
	private Player _player;
	private GameObject _princess;
	private Vector3 _nextBlocPosition = Vector3.zero;
	private Bounds _lastBlockBounds;
	private List<BricGenerator> _blocs = new List<BricGenerator>();
	private BricGenerator _firstBloc;
	private BricGenerator _lastBloc;
	private Vector3 _destroyBlocPosition;
	private Vector3 _generateBlocPosition;

}
