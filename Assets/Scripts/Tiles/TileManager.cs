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

	protected List<TileStart> tileStarts;
	protected List<TileEnd> tileEnds;


	// ----------
	// UNITY
	// ----------

	void Start ()
	{
		tileStarts = new List<TileStart>();
		tileEnds = new List<TileEnd>();
		tiles = gameObject.GetComponentsInChildren<Tile>();
		foreach(Tile tile in tiles)
		{
			tile.LoadAvailableTiles(tiles);
			if(tile.GetType() == typeof(TileStart))
				tileStarts.Add((TileStart)tile);
			if(tile.GetType() == typeof(TileEnd))
				tileEnds.Add((TileEnd)tile);
		}
	}


	// ----------
	// UTILITIES
	// ----------

	public TileStart GetTileStart (int _player)
	{
		foreach(TileStart tileStart in tileStarts)
		{
			if(tileStart.player == _player)
				return tileStart;
		}
		return null;
	}

	public TileEnd GetTileEnd (int _player)
	{
		foreach(TileEnd tileEnd in tileEnds)
		{
			if(tileEnd.player == _player)
				return tileEnd;
		}
		return null;
	}

}
