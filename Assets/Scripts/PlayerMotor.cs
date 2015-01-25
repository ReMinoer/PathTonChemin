using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerMotor : MonoBehaviour
{
	public List<Waypoint> Path { get; private set; }
	private int _pathIndex = 0;
	public Vector3 LastCase { get; private set; }
	public bool IsWaiting { get; private set; }
	public bool IsDead { get; private set; }

	private const float _delaiStartPeriod = 5;
	private float _delaiStartElapsed;

	public int Score = 0;

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

		if (_pathIndex < Path.Count && !IsWaiting)
		{
			Waypoint waypoint = Path[_pathIndex];
			Vector3 destination = waypoint.Position + Vector3.up * 0.5f;
			Vector3 direction = (destination - this.transform.position).normalized;
			this.transform.position += direction * Speed * Time.deltaTime;
			
			Vector3 diff = waypoint.Position - LastCase;

			if (Vector3.Dot(direction, diff) < -0.5)
			{
				IsWaiting = true;
				LastCase = destination;
				this.transform.position = destination;
				_pathIndex++;
			}
		}
	}

	public void WakeUp()
	{
		IsWaiting = false;
	}
	
	public void Death()
	{
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
	}

	public void SetPath(List<Waypoint> path)
	{
		Path = path;
		LastCase = Path[0].Position;
		if (Path.Count > 0)
			_pathIndex++;
	}

	public void AddScore(int score)
	{
		Score += score;
	}
}
