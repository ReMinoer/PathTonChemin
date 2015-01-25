using UnityEngine;
using System.Collections;

public class AudioManager : DesignPattern.Singleton<AudioManager> {

	protected AudioManager() {}

	public AudioClip[] TacticalSongs;
	public AudioClip[] ActionSongsA;
	public AudioClip[] ActionSongsB;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeSong(SongType type, int id)
	{
		audio.Stop();
		switch (type)
		{
		case SongType.Tactical: audio.clip = TacticalSongs[id]; break;
		case SongType.ActionA: audio.clip = ActionSongsA[id]; break;
		case SongType.ActionB: audio.clip = ActionSongsB[id]; break;
		}
		audio.Play();
	}

	public enum SongType
	{
		Tactical,
		ActionA,
		ActionB
	}
}
