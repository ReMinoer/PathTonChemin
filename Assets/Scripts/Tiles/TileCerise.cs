using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileCerise : Tile {

	// ----------
	// VARIABLE
	// ----------

	private bool take = false;

	public Animator ceriseAnimator;

	// ----------
	// UNITY
	// ----------


	// ----------
	// TILE
	// ----------

	override public void RoundInit ()
	{
		if(take)
			ceriseAnimator.SetTrigger("Reset");
		take = false;
	}

	override public void Action (PlayerMotor _player)
	{
		// If cerise give return
		if(take)
			return;

		ceriseAnimator.SetTrigger("Take");
		take = true;
		_player.AddScore(10);
	}

	// ----------
	// UTILITIES
	// ----------

}