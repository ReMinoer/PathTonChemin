﻿using UnityEngine;
using System.Collections;

public class Waypoint : DesignPattern.Factory<Waypoint> {

	// ----------
	// VARIABLE
	// ----------

	// public Renderer spriteCenter;
	// public Renderer spriteHalo;

	public TacticalPhase tacticalPhase;
	public Tile tile;
	public int state;
	// public Action action;

	public Vector3 Position
	{
		get { return tile.transform.position; }
	}

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
