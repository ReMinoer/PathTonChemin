﻿using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using DesignPattern;

public class Game : Singleton<Game>
{
	protected Game() {}

	public StatusTextManager StatusTextManager;

	public int NbPlayers = 4;
	private int _currentPlayer;
	private bool[] _playerAlive;

	public int NbRounds = 5;
	private int _currentRound;

	// Use this for initialization
	void Start ()
	{
		_currentRound = 1;
		StatusTextManager.ChangeState(StatusTextManager.State.GameStart);
	}

	void RoundInit()
	{
		_currentPlayer = 1;
		_playerAlive = new bool[NbPlayers];
		for (int i = 0; i < NbPlayers; i++)
			_playerAlive[i] = true;

		StatusTextManager.ChangeState(StatusTextManager.State.RoundStart, _currentRound);
	}
	
	void TacticalInit()
	{
		StatusTextManager.ChangeState(StatusTextManager.State.TacticalStart, _currentPlayer);
	}
	
	void TacticalTimerEnd()
	{
		StatusTextManager.ChangeState(StatusTextManager.State.TacticalEnd);
	}
	
	void TacticalNextPlayer()
	{
		_currentPlayer++;
		if (_currentPlayer > NbPlayers)
			ActionInit();
		else
			TacticalInit();
	}
	
	void ActionInit()
	{
		StatusTextManager.ChangeState(StatusTextManager.State.ActionStart);
	}

	void Play()
	{
		PlayerWin();
	}

	void PlayerKilled(int idPlayer)
	{
		_playerAlive[idPlayer] = false;
		if (_playerAlive.All(p => !p))
			ActionEnd();
	}
	
	void PlayerWin()
	{
		ActionEnd();
	}

	void ActionEnd()
	{
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
		switch (state)
		{
		case State.GameStart: Start(); break;
		case State.RoundStart: RoundInit(); break;
		case State.TacticalStart: TacticalInit(); break;
		case State.TacticalEnd: TacticalTimerEnd(); break;
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
		TacticalEnd,
		TacticalNextPlayer,
		Play,
		PlayerKilled,
		PlayerWin,
		NextRound,
		GameEnd
	}
}
