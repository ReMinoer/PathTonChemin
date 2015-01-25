using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerMotor : DesignPattern.Factory<PlayerMotor>
{
	public List<Waypoint> Path { get; private set; }
	private int _pathIndex = 0;
	public Tile CurrentTile
	{
		get
		{
			return _pathIndex < Path.Count ? Path[_pathIndex].tile : null;
		}
	}
	public Vector3 LastCase { get; private set; }

	public bool IsWaiting { get; private set; }
	public bool IsDead { get; private set; }
	public bool IsFreeze { get; private set; }

	private const float _delaiStartPeriod = 5;
	private float _delaiStartElapsed;

	public int Score = 0;
	public int bufferScore = 0;

	public float Speed = 2f;

	// Use this for initialization
	void Start ()
	{
		IsWaiting = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		_delaiStartElapsed += Time.deltaTime;
		if (_delaiStartElapsed < _delaiStartPeriod)
			return;
		
		if (IsFreeze)
			return;
		
		if (Path.Count == 0)
		{
			Death();
			Game.Instance.CheckAllPlayersDead();
		}

		if (_pathIndex < Path.Count && !IsWaiting)
		{
			Waypoint waypoint = Path[_pathIndex];
			Vector3 destination = waypoint.Position;
			Vector3 direction = (destination - this.transform.position).normalized;

			if (direction == Vector3.left) this.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
			if (direction == Vector3.right) this.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
			if (direction == Vector3.forward) this.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
			if (direction == Vector3.back) this.transform.rotation = Quaternion.AngleAxis(270, Vector3.up);

			this.transform.position += direction * Speed * Time.deltaTime;
			
			Vector3 diff = waypoint.Position - LastCase;

			if (destination == this.transform.position || Vector3.Dot(direction, diff) < -0.5)
			{
				IsWaiting = true;
				LastCase = destination;
				this.transform.position = destination;
				_pathIndex++;
			}
		}
		else if (!IsWaiting)
		{
			Death();
			Game.Instance.CheckAllPlayersDead();
		}
	}

	public void WakeUp()
	{
		IsWaiting = false;
	}
	
	public void Death()
	{
		IsWaiting = true;
		IsDead = true;
		gameObject.SetActive(false);
	}

	public void Revive()
	{
		IsDead = false;
		_delaiStartElapsed = 0;
		Path = new List<Waypoint>();
		_pathIndex = 0;
		IsWaiting = false;
		IsFreeze = false;
		bufferScore = 0;
	}

	public void Freeze()
	{
		IsFreeze = true;
	}

	public void SetPath(List<Waypoint> path, int playerId)
	{
		Path = path;

		if (Path.Count == 0)
			return;

		LastCase = Path[0].Position;
		//Vector3 direction = (TileManager.Instance.GetTileEnd(playerId).transform.position - this.transform.position).normalized;
		this.transform.LookAt(TileManager.Instance.GetTileEnd(playerId).transform.position);
		if (Path.Count > 0)
			_pathIndex++;
	}

	public void AddScore(int score)
	{
		bufferScore += score;
	}

	public void ValidateScore (PlayerMotor _winner)
	{
		if(this == _winner)
			bufferScore*=3;
		Score = bufferScore;
	}
}
