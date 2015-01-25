using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TacticalData : DesignPattern.Factory<TacticalData> {

	public enum State
	{
		None,
		TacticalPhase
	}

	// ----------
	// VARIABLE
	// ----------

	public int Player;

	public State state = State.None;

	public List<Waypoint> waypoints;
	private Tile lastClicked = null;
	private bool firstClick = true;


	// ----------
	// UNITY
	// ----------

	void Start ()
	{
	
	}
	
	void Update ()
	{
		switch(state)
		{
			case State.None:
				
			break;
			case State.TacticalPhase:
				TacticalPhase();
			break;
		}
	}


	// ----------
	// UTILITIES
	// ----------

	public void StartTacticalPhase ()
	{
		if(state==State.None)
		{
			state = State.TacticalPhase;
			// Init Phase
			waypoints = new List<Waypoint>();
		}
	}

	public void EndTacticalPhase ()
	{
		if(state==State.TacticalPhase)
			state = State.None;
		foreach(Waypoint waypoint in waypoints)
		{
			waypoint.Hide();
		}
	}

	void TacticalPhase ()
	{
		// Use mouse currently
		if(Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] hits = Physics.RaycastAll(ray);
			foreach(RaycastHit hit in hits)
			{
				if(hit.transform.tag == "Tile")
				{
					Tile tile = (hit.transform.gameObject).GetComponent<Tile>();
					if(tile==null)
						break;
					if(waypoints.Count==0)
					{
						if(tile != TileManager.Instance.GetTileStart(Player))
							break;
					}
					else
					{
						Tile lastTile = waypoints[waypoints.Count-1].tile;
						// Restart after 
						if(lastClicked==null)
						{
							if(tile==lastTile && firstClick)
								lastClicked = tile;
							break;
						}
						// If not available
						if(!tile.available)
							break;
						// If same tile
						if(tile==lastTile)
							break;
						// If movement not posible
						if(!lastTile.TileAvailable(tile))
							break;
					}
					string colorPlayer = "";
					switch(Player)
					{
						case 1:
							colorPlayer = "_Blue";
						break;
						case 2:
							colorPlayer = "_Orange";
						break;
						case 3:
							colorPlayer = "_Purple";
						break;
						case 4:
							colorPlayer = "_Red";
						break;
					}
					Waypoint waypoint = Waypoint.New("Prefabs/Waypoints/Waypoint"+colorPlayer);
					waypoint.transform.SetParent(transform);
					waypoint.tile = tile;
					waypoint.state = waypoints.Count+1;
					waypoints.Add(waypoint);
					lastClicked = tile;
				}
			}
			firstClick = false;
		}
		else
		{
			lastClicked = null;
			firstClick = true;
		}
	}

}
