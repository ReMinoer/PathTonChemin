using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TacticalPhase : DesignPattern.Factory<TacticalPhase> {

	// ----------
	// VARIABLE
	// ----------

	public int Player;

	private bool activate = false;

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
		if(activate)
		{
			// Use mouse currently
			if(Input.GetMouseButton(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] hits = Physics.RaycastAll(ray);
				List<Waypoint> deleteWaypoints = new List<Waypoint>();
				foreach(RaycastHit hit in hits)
				{
					if(hit.transform.tag == "Tile")
					{
						Tile tile = (hit.transform.gameObject).GetComponent<Tile>();
						if(tile==null)
							continue;
						if(waypoints.Count==0)
						{
							if(tile != TileManager.Instance.GetTileStart(Player))
								continue;
						}
						else
						{
							Tile lastTile = waypoints[waypoints.Count-1].tile;
							// Restart after 
							if(lastClicked==null)
							{
								if(tile==lastTile && firstClick)
									lastClicked = tile;
								continue;
							}
							// If not available
							if(!tile.available)
								continue;
							// If same tile
							if(tile==lastTile)
								continue;
							// If movement not posible
							if(!lastTile.TileAvailable(tile))
								continue;
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
					if(hit.transform.tag == "Waypoint")
					{
						if(!firstClick)
							continue;
						Waypoint waypoint = (hit.transform.gameObject).GetComponent<Waypoint>();
						deleteWaypoints.Add(waypoint);
					}
				}
				if(deleteWaypoints.Count>0)
				{
					int max = 0;
					int n = 0;
					foreach(Waypoint waypoint in deleteWaypoints)
					{
						if(waypoint.state>max)
						{
							max = waypoint.state;
							n = waypoints.IndexOf(waypoint);
							lastClicked = waypoint.tile;
						}
					}
					while(n+1 < waypoints.Count)
					{
						Waypoint bufferWaypoint = waypoints[waypoints.Count-1];
						waypoints.Remove(bufferWaypoint);
						Destroy(bufferWaypoint.gameObject);
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


	// ----------
	// UTILITIES
	// ----------

	public void StartTacticalPhase ()
	{
		if(!activate)
		{
			activate = true;
			if(waypoints != null)
			{
				foreach(Waypoint waypoint in waypoints)
				{
					Destroy(waypoint.gameObject);
				}
			}
			// Init Phase
			waypoints = new List<Waypoint>();
		}
	}

	public void EndTacticalPhase ()
	{
		if(activate)
			activate = false;
		foreach(Waypoint waypoint in waypoints)
		{
			waypoint.Hide();
		}
	}

}
