using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInterfaceManager : MonoBehaviour
{
	public GameObject PlayerOneInterface;
	public GameObject PlayerTwoInterface;
	public GameObject PlayerThreeInterface;
	public GameObject PlayerFourInterface;
	public GameObject PlayerOneTimer;
	public GameObject PlayerTwoTimer;
	public GameObject PlayerThreeTimer;
	public GameObject PlayerFourTimer;
	
	void Start()
	{
		Game.Instance.PlayerInterfaceManager = this;
		PlayerOneInterface.SetActive(false);
		PlayerTwoInterface.SetActive(false);
		PlayerThreeInterface.SetActive(false);
		PlayerFourInterface.SetActive(false);
	}
	
	public void ChangeState(State state, object args = null)
	{
		switch (state)
		{
		case State.TacticalPlayerOne:
			PlayerOneInterface.SetActive(true);
			PlayerOneTimer.SetActive(true);
			PlayerOneTimer.GetComponent<TacticalTimerMotor>().Reset();
			break;
		case State.TacticalPlayerTwo:
			PlayerTwoInterface.SetActive(true);
			PlayerTwoTimer.SetActive(true);
			PlayerTwoTimer.GetComponent<TacticalTimerMotor>().Reset();
			break;
		case State.TacticalPlayerThree:
			PlayerThreeInterface.SetActive(true);
			PlayerThreeTimer.SetActive(true);
			PlayerThreeTimer.GetComponent<TacticalTimerMotor>().Reset();
			break;
		case State.TacticalPlayerFour:
			PlayerFourInterface.SetActive(true);
			PlayerFourTimer.SetActive(true);
			PlayerFourTimer.GetComponent<TacticalTimerMotor>().Reset();
			break;
		case State.Action:
			PlayerOneInterface.SetActive(true);
			PlayerTwoInterface.SetActive(true);
			PlayerThreeInterface.SetActive(true);
			PlayerFourInterface.SetActive(true);
			PlayerOneTimer.SetActive(false);
			PlayerTwoTimer.SetActive(false);
			PlayerThreeTimer.SetActive(false);
			PlayerFourTimer.SetActive(false);
			break;
		default: return;
		}
	}

	public void DisableAll()
	{
		PlayerOneInterface.SetActive(false);
		PlayerTwoInterface.SetActive(false);
		PlayerThreeInterface.SetActive(false);
		PlayerFourInterface.SetActive(false);
	}
	
	public enum State
	{
		TacticalPlayerOne,
		TacticalPlayerTwo,
		TacticalPlayerThree,
		TacticalPlayerFour,
		Action
	}
}
