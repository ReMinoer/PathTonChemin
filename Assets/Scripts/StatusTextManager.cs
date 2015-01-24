using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusTextManager : DesignPattern.Singleton<StatusTextManager>
{
	public GameObject GameStartPanel;
	public GameObject RoundStartPanel;
	public Text RoundStartMessage;
	public GameObject TacticalStartPanel;
	public Text TacticalStartMessage;
	public GameObject TacticalEndPanel;
	public GameObject ActionStartPanel;
	public GameObject ActionEndPanel;
	public GameObject GameEndPanel;

	void Awake()
	{
		GameStartPanel.SetActive(false);
		RoundStartPanel.SetActive(false);
		TacticalStartPanel.SetActive(false);
		TacticalEndPanel.SetActive(false);
		ActionStartPanel.SetActive(false);
		ActionEndPanel.SetActive(false);
		GameEndPanel.SetActive(false);
	}

	public void ChangeState(State state, object args = null)
	{
		GameObject panel;
		switch (state)
		{
		case State.GameStart: panel = GameStartPanel; break;
		case State.RoundStart:
			panel = RoundStartPanel;
			RoundStartMessage.text = string.Format("Round n°{0}", (int)args);
			break;
		case State.TacticalStart:
			panel = TacticalStartPanel;
			TacticalStartMessage.text = string.Format("Player {0}", (int)args);
			break;
		case State.TacticalEnd: panel = TacticalEndPanel; break;
		case State.ActionStart: panel = ActionStartPanel; break;
		case State.ActionEnd: panel = ActionEndPanel; break;
		case State.GameEnd: panel = GameEndPanel; break;
		default: return;
		}

		panel.SetActive(true);
		panel.animation.Play();
	}

	public enum State
	{
		GameStart,
		RoundStart,
		TacticalStart,
		TacticalEnd,
		ActionStart,
		ActionEnd,
		GameEnd
	}
}
