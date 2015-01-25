using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatusTextManager : DesignPattern.Singleton<StatusTextManager>
{
	public GameObject TitleScreenPanel;
	public GameObject GameStartPanel;
	public GameObject RoundStartPanel;
	public Text RoundStartMessage;
	public GameObject TacticalStartPanel;
	public Text TacticalStartMessage;
	public GameObject TacticalEndPanel;
	public GameObject ActionStartPanel;
	public GameObject ActionEndPanel;
	public GameObject GameEndPanel;
	public Text GameEndMessage;

	void Awake()
	{
		TitleScreenPanel.SetActive(false);
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
		case State.TitleScreen: panel = TitleScreenPanel; break;
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
		case State.GameEnd:
			panel = GameEndPanel;

			Dictionary<int, int> scores = args as Dictionary<int,int>;
			List<KeyValuePair<int, int>> scoresList = scores.ToList();
			scoresList.Sort((firstPair,nextPair) =>
			    {
					return firstPair.Value.CompareTo(nextPair.Value);
				}
			);

			string scoreText = string.Format("Player {0} win !\n", scoresList.First().Key);
			foreach (KeyValuePair<int,int> score in scoresList)
				scoreText += string.Format("\nPlayer {0} : {1}", score.Key, score.Value);
			GameEndMessage.text = scoreText;
			break;
		default: return;
		}

		panel.SetActive(true);
		if (panel.GetComponent<Animation>() != null)
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
		GameEnd,
		TitleScreen
	}
}
