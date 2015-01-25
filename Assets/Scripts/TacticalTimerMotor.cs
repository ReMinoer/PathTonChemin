using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TacticalTimerMotor : MonoBehaviour {

	public GameObject PlayerInterface;
	private float _timeleft;

	// Use this for initialization
	void Start () {
	}

	public void Reset()
	{
		_timeleft = Game.Instance.TactiqueTime;
	}
	
	// Update is called once per frame
	void Update () {
		_timeleft -= Time.deltaTime;
		if (_timeleft < 0)
		{
			PlayerInterface.SetActive(false);
			Game.Instance.ChangeState(Game.State.TacticalTimerEnd);
			_timeleft = 0;
		}
		this.GetComponent<Text>().text = FormatTime(_timeleft);
	}

	string FormatTime(float time)
	{
		int intTime = (int)time;
		int seconds = intTime % 60;
		int fraction = (int)(time * 100);
		fraction = fraction % 100;
		return string.Format ("{0:00}:{1:00}", seconds,fraction);
	}
}
