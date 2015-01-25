using UnityEngine;
using System.Collections;

public class Waypoint : DesignPattern.Factory<Waypoint> {

	// ----------
	// VARIABLE
	// ----------

	// public Renderer spriteCenter;
	// public Renderer spriteHalo;

	public TacticalData tacticalData;
	public Tile tile;
	public int state;
	// public Action action;


	// ----------
	// UNITY
	// ----------

	void Start ()
	{
		transform.position = tile.transform.position;
	}
	
	void Update ()
	{

	}


	// ----------
	// UTILITIES
	// ----------

	public void Hide ()
	{
		gameObject.SetActive(false);
	}

}
