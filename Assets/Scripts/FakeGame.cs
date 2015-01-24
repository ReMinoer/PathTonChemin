using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FakeGame : MonoBehaviour {

	// ----------
	// VARIABLE
	// ----------

	public int NbPlayers = 4;
	private List<TacticalData> tacticalDatas;


	// ----------
	// UNITY
	// ----------

	void Start () {
		tacticalDatas = new List<TacticalData>();
		for(int player = 1; player <= NbPlayers; player++)
		{
			TacticalData tacticalData = TacticalData.New();
			tacticalData.Player = player;
			tacticalDatas.Add(tacticalData);
		}
		tacticalDatas[0].StartTacticalPhase();
	}

}
