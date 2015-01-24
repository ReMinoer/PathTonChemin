using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerMotor : MonoBehaviour
{
	public List<PlayerMove> Path = new List<PlayerMove>();

	public Vector2 CurrentCase = Vector2.zero;
	public float SizeCase = 1;
	public float LevelWidth = 10;
	public float LevelHeight = 10;

	public int Score = 0;

	public float Speed = 2f;

	private Vector2 LevelOrigin
	{ get { return new Vector2(LevelWidth, LevelHeight) / 2; } }

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Path.Any())
		{
			PlayerMove move = Path.First();
			Vector3 direction = MoveToDir(move);
			this.transform.position += direction * Speed * Time.deltaTime;

			Vector3 destination = (XYtoX0Z(CurrentCase) + direction) * SizeCase - XYtoX0Z(LevelOrigin) + XYtoX0Z(Vector2.one * 0.5f);
			Vector3 levelPosition = this.transform.position;
			Vector3 diff = destination - levelPosition;
			if (Vector3.Dot(direction, diff) < 0)
			{
				CurrentCase += new Vector2(direction.x, -direction.z);
				this.transform.position = XYtoX0Z((CurrentCase * SizeCase) - LevelOrigin + Vector2.one * 0.5f);
				Path.RemoveAt(0);
			}
		}
	}

	public void AddScore(int score)
	{
		Score += score;
	}

	private static Vector3 XYtoX0Z(Vector2 vector)
	{
		return new Vector3(vector.x, 0.5f, -vector.y);
	}

	private static Vector2 X0ZtoXY(Vector3 vector)
	{
		return new Vector2(vector.x, -vector.z);
	}

	private static Vector3 MoveToDir(PlayerMove move)
	{
		switch (move)
		{
		case PlayerMove.UP: return Vector3.forward;
		case PlayerMove.RIGHT: return Vector3.right;
		case PlayerMove.DOWN: return Vector3.back;
		case PlayerMove.LEFT: return Vector3.left;
		}
		return Vector3.zero;
	}

	public enum PlayerMove
	{
		UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3
	}
}
