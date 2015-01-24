using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	// ----------
	// VARIABLE
	// ----------

	public bool available = true;

	public bool right = false;
	public bool left = false;
	public bool up = false;
	public bool down = false;

	private List<Tile> availableTiles;


	// ----------
	// UNITY
	// ----------

	void Start ()
	{

	}
	
	void Update ()
	{

	}


	// ----------
	// UTILITIES
	// ----------

	public bool TileAvailable (Tile _tile)
	{
		return availableTiles.Contains(_tile);
	}

	public void LoadAvailableTiles (Tile[] _tiles)
	{
		availableTiles = new List<Tile>();

		foreach(Tile tile in _tiles)
		{
			if(right)
			{
				if(transform.position+Vector3.right==tile.transform.position)
				{
					availableTiles.Add(tile);
					continue;
				}
			}
			if(left)
			{
				if(transform.position+Vector3.left==tile.transform.position)
				{
					availableTiles.Add(tile);
					continue;
				}
			}
			if(up)
			{
				if(transform.position+Vector3.forward==tile.transform.position)
				{
					availableTiles.Add(tile);
					continue;
				}
			}
			if(down)
			{
				if(transform.position+Vector3.back==tile.transform.position)
				{
					availableTiles.Add(tile);
					continue;
				}
			}			
		}
	}

}