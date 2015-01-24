using UnityEngine;
using System.Collections;

public class StatusTextEvent : MonoBehaviour {

	public Game.State GameStateToInvoke;

	void OnAnimationEnd()
	{
		this.gameObject.SetActive(false);
		Game.Instance.ChangeState(GameStateToInvoke);
	}
}
