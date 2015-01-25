using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game : DesignPattern.Singleton<Game>
{
	protected Game() {}

	private State _state;

	public StatusTextManager StatusTextManager;
	public PlayerInterfaceManager PlayerInterfaceManager;

	public List<PlayerMotor> Players = new List<PlayerMotor>();
	private int _currentPlayer;

	private List<TacticalData> tacticalDatas;

	public int NbRounds = 5;
	private int _currentRound;

	public float TactiqueTime;

	// Use this for initialization
	void Start ()
	{
		_currentRound = 1;

		tacticalDatas = new List<TacticalData>();
		for(int player = 1; player <= Players.Count; player++)
		{
			TacticalData tacticalData = TacticalData.New();
			tacticalData.Player = player;
			tacticalDatas.Add(tacticalData);
		}

		StatusTextManager.ChangeState(StatusTextManager.State.GameStart);
	}

	void RoundInit()
	{
		_currentPlayer = 1;

		foreach (PlayerMotor player in Players)
		{
			player.gameObject.SetActive(false);
			player.Revive();
		}

		StatusTextManager.ChangeState(StatusTextManager.State.RoundStart, _currentRound);
	}
	
	void TacticalInit()
	{
		StatusTextManager.ChangeState(StatusTextManager.State.TacticalStart, _currentPlayer);
	}
	
	void Tactical()
	{
		PlayerInterfaceManager.State state;
		switch (_currentPlayer)
		{
		case 1: state = PlayerInterfaceManager.State.TacticalPlayerOne; break;
		case 2: state = PlayerInterfaceManager.State.TacticalPlayerTwo; break;
		case 3: state = PlayerInterfaceManager.State.TacticalPlayerThree; break;
		case 4: state = PlayerInterfaceManager.State.TacticalPlayerFour; break;
		default: return;
		}
		tacticalDatas[_currentPlayer-1].StartTacticalPhase();
		PlayerInterfaceManager.ChangeState(state);
	}
	
	void TacticalTimerEnd()
	{
		tacticalDatas[_currentPlayer-1].EndTacticalPhase();
		StatusTextManager.ChangeState(StatusTextManager.State.TacticalEnd);
	}
	
	void TacticalNextPlayer()
	{
		_currentPlayer++;
		if (_currentPlayer > Players.Count)
			ActionInit();
		else
			TacticalInit();
	}
	
	void ActionInit()
	{
		foreach (PlayerMotor player in Players)
			player.gameObject.SetActive(true);
		StatusTextManager.ChangeState(StatusTextManager.State.ActionStart);
	}

	void Play()
	{
		for(int player = 1; player <= Players.Count; player++)
		{
			Players[player-1].SetPath(tacticalDatas[player-1].waypoints);
		}
		PlayerInterfaceManager.ChangeState(PlayerInterfaceManager.State.Action);
	}

	void Update()
	{
		if (_state == State.Play)
		{
			if (isSynchroOnWait())
			{
				HandleActionEvents();
				WakeUpAll();
			}
		}
	}

	bool isSynchroOnWait()
	{
		foreach(PlayerMotor player in Players)
			if (!player.IsWaiting)
				return false;

		return true;
	}

	void HandleActionEvents()
	{
		for (int i = 0; i < Players.Count; i++)
			for (int j = 0; j < Players.Count; j++)
				if (i != j
				    && (Players[i].transform.position == Players[j].transform.position
				    || (Players[i].transform.position == Players[j].LastCase
				    	&& Players[j].transform.position == Players[i].LastCase)))
				{
					PlayerKilled(i);
					PlayerKilled(j);
				}
	}

	void WakeUpAll()
	{
		foreach(PlayerMotor player in Players)
			player.WakeUp();
	}

	void PlayerKilled(int idPlayer)
	{
		Players[idPlayer].Death();
		if (Players.All(p => p.IsDead))
			ActionEnd();
	}
	
	void PlayerWin()
	{
		ActionEnd();
	}

	void ActionEnd()
	{
		PlayerInterfaceManager.DisableAll();
		StatusTextManager.ChangeState(StatusTextManager.State.ActionEnd);
	}

	void NextRound()
	{
		_currentRound++;
		if (_currentRound > NbRounds)
			GameEnd();
		else
			RoundInit();
	}

	void GameEnd()
	{
		StatusTextManager.ChangeState(StatusTextManager.State.GameEnd);
	}
	
	public void ChangeState(State state, object args = null)
	{
		_state = state;
		
		switch (state)
		{
		case State.GameStart: Start(); break;
		case State.RoundStart: RoundInit(); break;
		case State.TacticalStart: TacticalInit(); break;
		case State.Tactical: Tactical(); break;
		case State.TacticalTimerEnd: TacticalTimerEnd(); break;
		case State.TacticalNextPlayer: TacticalNextPlayer(); break;
		case State.Play: Play(); break;
		case State.PlayerKilled: PlayerKilled((int)args); break;
		case State.PlayerWin: PlayerWin(); break;
		case State.NextRound: NextRound(); break;
		case State.GameEnd: GameEnd(); break;
		}
	}

	public enum State
	{
		GameStart,
		RoundStart,
		TacticalStart,
		Tactical,
		TacticalTimerEnd,
		TacticalNextPlayer,
		Play,
		PlayerKilled,
		PlayerWin,
		NextRound,
		GameEnd
	}
}
