using UnityEngine;
using System.Collections;

public class AudioManager : DesignPattern.Singleton<AudioManager> {

	protected AudioManager() {}

	public AudioClip[] TacticalSongs;
	public AudioClip[] ActionSongsA;
	public AudioClip[] ActionSongsB;
	
	private float _fade = -1;
	private float _elapsedFade;

	private SongType _nextType;
	private int _nextId;
	private bool _stopRequest;

	// Use this for initialization
	void Start ()
	{
		_fade = -1;
		_elapsedFade = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_fade != -1)
		{
			_elapsedFade += Time.deltaTime;

			audio.volume = 1.0f - 1.0f * (_elapsedFade / _fade);

			if (_elapsedFade >= _fade)
			{
				if (_stopRequest)
				{
					audio.loop = false;
					audio.Stop();
					_stopRequest = false;
				}
				else
				{
					switch (_nextType)
					{
					case SongType.Tactical: audio.clip = TacticalSongs[_nextId]; break;
					case SongType.ActionA: audio.clip = ActionSongsA[_nextId]; break;
					case SongType.ActionB: audio.clip = ActionSongsB[_nextId]; break;
					}
					audio.loop = true;
					audio.PlayDelayed(1);
				}
				_fade = -1;
				audio.volume = 1;
			}
		}
	}

	public void ChangeSong(SongType type, int id, float fade = 0)
	{
		_nextType = type;
		_nextId = id;

		_elapsedFade = 0;
		_fade = fade;
	}
	
	public void Stop(float fade = 0)
	{
		_stopRequest = true;

		_elapsedFade = 0;
		_fade = fade;
	}

	public enum SongType
	{
		Tactical,
		ActionA,
		ActionB
	}
}
