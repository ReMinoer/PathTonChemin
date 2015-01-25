using UnityEngine;
using System.Collections;

public class TitleScreenBehaviour : MonoBehaviour {

	public GameObject TitleScreenPanel;

	public void LaunchGame()
	{
		TitleScreenPanel.SetActive(false);
		Game.Instance.ChangeState(Game.State.GameStart);
	}
}
