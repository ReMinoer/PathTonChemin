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

	public int nbPlayer = 1;
	private List<PlayerMotor> Players;
	private int _currentPlayer;

	private List<TacticalPhase> TacticalPhases;

	public int NbRounds = 5;
	private int _currentRound;

	public float TactiqueTime;

	private bool _musicPack = true;

	// Use this for initialization
	void Start ()
	{
		_currentRound = 1;

		TacticalPhases = new List<TacticalPhase>();
		Players = new List<PlayerMotor>();
		for(int player = 1; player <= nbPlayer; player++)
		{
			// Player Creation
			string playerColor = "Blue";
			if(player==2)
				playerColor = "Orange";
			else if(player==3)
				playerColor = "Purple";
			else if(player==4)
				playerColor = "Red";
			PlayerMotor playerMotor = PlayerMotor.New("Prefabs/Players/Player_"+playerColor);
			playerMotor.gameObject.SetActive(false);
			Players.Add(playerMotor);
			// TactialPhase Creation
			TacticalPhase tacticalPhase = TacticalPhase.New();
			tacticalPhase.Player = player;
			TacticalPhases.Add(tacticalPhase);
		}

		TitleScreen();
	}

	void TitleScreen ()
	{
		_musicPack = !_musicPack;
		AudioManager.Instance.ChangeSong(AudioManager.SongType.ActionA, 0, 1);
		
		StatusTextManager.ChangeState(StatusTextManager.State.TitleScreen);
	}

	void GameStart()
	{
		StatusTextManager.ChangeState(StatusTextManager.State.GameStart);
	}

	void RoundInit()
	{
		_currentPlayer = 1;

		for (int i = 0; i < Players.Count; i++)
		{
			Players[i].gameObject.SetActive(false);
			Players[i].Revive();
			Players[i].transform.position = TileManager.Instance.GetTileStart(i+1).transform.position;
		}

		TileManager.Instance.RoundInit();

		StatusTextManager.ChangeState(StatusTextManager.State.RoundStart, _currentRound);
	}
	
	void TacticalInit()
	{
		Players[_currentPlayer-1].gameObject.SetActive(true);
		Players[_currentPlayer-1].Freeze();
		AudioManager.Instance.ChangeSong(AudioManager.SongType.Tactical, _currentPlayer-1, 1);
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
		TacticalPhases[_currentPlayer-1].StartTacticalPhase();
		PlayerInterfaceManager.ChangeState(state);
	}
	
	void TacticalTimerEnd()
	{
		Players[_currentPlayer-1].gameObject.SetActive(false);
		Players[_currentPlayer-1].Revive();
		TacticalPhases[_currentPlayer-1].EndTacticalPhase();
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
		AudioManager.Instance.ChangeSong(_musicPack ? AudioManager.SongType.ActionA : AudioManager.SongType.ActionB, _currentRound-1, 1);
		foreach (PlayerMotor player in Players)
			player.gameObject.SetActive(true);
		StatusTextManager.ChangeState(StatusTextManager.State.ActionStart);
	}

	void Play()
	{
		for(int player = 1; player <= Players.Count; player++)
			Players[player-1].SetPath(TacticalPhases[player-1].waypoints, player);
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
		{
			if (Players[i].IsDead)
				continue;

			// Check contect between players
			for (int j = 0; j < Players.Count; j++)
			{
				if (Players[j].IsDead)
					continue;

				if (i != j
				    && (Players[i].transform.position == Players[j].transform.position
				    || (Players[i].transform.position == Players[j].LastCase
				    	&& Players[j].transform.position == Players[i].LastCase)))
				{
					PlayerKilled(i);
					PlayerKilled(j);
				}
			}

			// Check current tile action
			if (Players[i].CurrentTile != null)
				Players[i].CurrentTile.Action(Players[i]);

			// Check player on end tile
			if (Players[i].transform.position == TileManager.Instance.GetTileEnd(i+1).transform.position)
				PlayerWin(i);
		}
	}

	void WakeUpAll()
	{
		foreach(PlayerMotor player in Players)
			if (!player.IsDead)
				player.WakeUp();
	}

	void PlayerKilled(int idPlayer)
	{
		Players[idPlayer].Death();
		CheckAllPlayersDead();
	}

	public void CheckAllPlayersDead()
	{
		if (Players.All(p => p.IsDead))
		{
			foreach(PlayerMotor player in Players)
				player.ValidateScore(null);
			ActionEnd();
		}
	}
	
	void PlayerWin(int _winner)
	{
		foreach(PlayerMotor player in Players)
		{
			player.ValidateScore(Players[_winner]);
			if (!player.IsDead)
				player.Freeze();
		}

		ActionEnd();
	}

	void ActionEnd()
	{
		AudioManager.Instance.Stop (1);
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
		Dictionary<int,int> scores = new Dictionary<int, int>();
		for(int i = 0; i < Players.Count; i++)
			scores.Add(i+1, Players[i].Score);
		StatusTextManager.ChangeState(StatusTextManager.State.GameEnd, scores);
	}
	
	public void ChangeState(State state, object args = null)
	{
		_state = state;
		
		switch (state)
		{
		case State.TitleScreen: TitleScreen(); break;
		case State.GameStart: GameStart(); break;
		case State.RoundStart: RoundInit(); break;
		case State.TacticalStart: TacticalInit(); break;
		case State.Tactical: Tactical(); break;
		case State.TacticalTimerEnd: TacticalTimerEnd(); break;
		case State.TacticalNextPlayer: TacticalNextPlayer(); break;
		case State.Play: Play(); break;
		case State.PlayerKilled: PlayerKilled((int)args); break;
		// case State.PlayerWin: PlayerWin(); break;
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
		GameEnd,
		TitleScreen
	}
}
