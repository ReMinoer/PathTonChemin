using UnityEngine;
using System.Collections;

public class StatusTextEvent : MonoBehaviour {

	public Game.State GameStateToInvoke;

	void OnAnimationEnd()
	{
		Game.Instance.ChangeState(GameStateToInvoke);
	}
}
