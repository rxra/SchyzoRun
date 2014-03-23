using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float scaleFactor = 0.1f;
	public float borderMargin = 20f;

	public void Flash(bool start)
	{
		_flash = start;
		_flashTimer = 0;
		if (!start) {
			renderer.enabled = true;
		}
	}

	public bool Input(float h, float hspeed)
	{
		Vector3 oldPosition = transform.position;
		transform.Translate(transform.right * h * hspeed * Time.deltaTime);

		if (transform.position.x < (_screen.transform.position.x - _screen.transform.localScale.x/2f + renderer.bounds.size.x)) {
			transform.position = oldPosition;
			return false;
		}

		if (transform.position.x > (_screen.transform.position.x + _screen.transform.localScale.x/2f - renderer.bounds.size.x)) {
			transform.position = oldPosition;
			return false;
		}

		return true;
	}

	public void Initialize(Screen screen)
	{
		_screen = screen;
		gameObject.name = "Player_"+screen.name;

		/*float size = Mathf.Min(screen.transform.localScale.x,screen.transform.localScale.y);
		transform.localScale = new Vector3(
			size * scaleFactor,
			size * scaleFactor,
			transform.localScale.z
		);*/

		transform.localScale = new Vector3(
			transform.localScale.x/(float)screen.level,
			transform.localScale.y/(float)screen.level,
			transform.localScale.z
		);

		_startPosition = new Vector3(
			screen.transform.position.x,
			screen.transform.position.y - screen.transform.localScale.y/2f + transform.localScale.y/2f + 7f/(float)screen.level,
			-1f
		);

		transform.position = _startPosition;

		/*_splitPosition = new Vector3(
			screen.transform.position.x,
			screen.transform.position.y + screen.transform.localScale.y/2f - transform.localScale.y/2f,
			-1f
		);*/

		borderMargin /= (float)screen.level;
	}

	void Update()
	{
		if (_flash) {
			_flashTimer += Time.deltaTime;
			if (_flashTimer>=0.1f) {
				_flashTimer = 0;
				renderer.enabled = !renderer.enabled;
			}
		}
	}

	IEnumerator Wait(float waitTime) {
		yield return new WaitForSeconds(waitTime);
	}

	void OnTriggerEnter(Collider collider)
	{
		//Debug.Log (collider.gameObject);
		if (collider.gameObject.tag=="Eart" || collider.gameObject.tag=="BigEart") {
			ScreenManager.instance.HeartHitted(collider.gameObject.tag=="BigEart"?true:false);
			//audio.Play();
			GameObject.Destroy(collider.gameObject);
		} else if (collider.gameObject.tag=="Obstacle") {
			ScreenManager.instance.ObsctableHitted();
		}
	}

	// Update is called once per frame
	/*void Update () {
		float v = Input.GetAxis("Vertical");
		transform.Translate(transform.up * v * speed * Time.deltaTime);

		if (transform.position.y<_startPosition.y) {
			transform.position = _startPosition;
		} else if (transform.position.y>_splitPosition.y) {
			//transform.position = _splitPosition;
			ScreenManager.instance.SplitScreen(_screen);
		}
	}*/

	private Screen _screen;
	private Vector3 _startPosition;
	//private Vector3 _splitPosition;
	private bool _flash = false;
	private float _flashTimer;

}
