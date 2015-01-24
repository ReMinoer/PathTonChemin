using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager :  DesignPattern.Singleton<TileManager>
{

	// ----------
	// SINGLETON
	// ----------

	protected TileManager () {}


	// ----------
	// VARIABLE
	// ----------

	protected Tile[] tiles;


	// ----------
	// UNITY
	// ----------

	void Start ()
	{
		tiles = gameObject.GetComponentsInChildren<Tile>();
		foreach(Tile tile in tiles)
		{
			tile.LoadAvailableTiles(tiles);
		}
	}


	// ----------
	// UTILITIES
	// ----------

}
