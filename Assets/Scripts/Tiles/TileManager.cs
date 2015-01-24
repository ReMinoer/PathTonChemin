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

	public List<TileStart> tileStart;


	// ----------
	// UNITY
	// ----------

	void Start ()
	{
		tiles = gameObject.GetComponentsInChildren<Tile>();
		foreach(Tile tile in tiles)
		{
			tile.LoadAvailableTiles(tiles);
			if(tile.GetType() == typeof(TileStart))
				Debug.Log("c'est trop cool Path ton chemin #VROOOUM!");
		}
	}


	// ----------
	// UTILITIES
	// ----------

}
