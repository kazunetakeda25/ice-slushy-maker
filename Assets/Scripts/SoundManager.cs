using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static int musicOn = 1;

	public static int soundOn = 1;

	public static bool forceTurnOff;

	public AudioSource gameplayMusic;

	public AudioSource ButtonClick;

	public AudioSource ButtonClick2;

	public AudioSource PopUpShow;

	public AudioSource PopUpHide;

	public AudioSource ShowCup;

	public AudioSource FillCup;

	public AudioSource UnlockFlavor;

	public AudioSource EmptyFlavor;

	public AudioSource ActionCompleted;

	public AudioSource Decoration;

	public AudioSource Straw;

	public AudioSource DrinkSlush;

	public AudioSource CameraSound;

	public AudioSource MoldSelected;

	public AudioSource WaterSound;

	public AudioSource ShowFreezer;

	public AudioSource FreezerDoorOpen;

	public AudioSource FreezerDoorClose;

	public AudioSource FreezerOnSound;

	public AudioSource KnifeCutSound;

	public AudioSource InsertIce;

	public AudioSource InsertFruit;

	public AudioSource Blender;

	public AudioSource FillFlavor;

	public AudioSource Coins;

	public float OriginalMusicVolume;

	private static SoundManager instance;

	public List<AudioSource> listStopSoundOnExit = new List<AudioSource>();

	public static SoundManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (UnityEngine.Object.FindObjectOfType(typeof(SoundManager)) as SoundManager);
			}
			return instance;
		}
	}

	private void Start()
	{
		OriginalMusicVolume = gameplayMusic.volume;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (PlayerPrefs.HasKey("SoundOn"))
		{
			soundOn = PlayerPrefs.GetInt("SoundOn", 1);
			if (soundOn == 0)
			{
				MuteAllSounds();
			}
			else
			{
				UnmuteAllSounds();
			}
		}
		else
		{
			SetSound(bEnabled: true);
		}
		musicOn = PlayerPrefs.GetInt("MusicOn", 1);
		if (musicOn == 1)
		{
			Play_Music();
		}
		else
		{
			Stop_Music();
		}
		Screen.sleepTimeout = -1;
	}

	public void SetSound(bool bEnabled)
	{
		if (bEnabled)
		{
			PlayerPrefs.SetInt("SoundOn", 1);
			UnmuteAllSounds();
		}
		else
		{
			PlayerPrefs.SetInt("SoundOn", 0);
			MuteAllSounds();
		}
		soundOn = PlayerPrefs.GetInt("SoundOn");
	}

	public void Play_ButtonClick()
	{
		if (ButtonClick.clip != null && soundOn == 1)
		{
			ButtonClick.Play();
		}
	}

	public void Play_Music()
	{
		if (gameplayMusic.clip != null && musicOn == 1 && !gameplayMusic.isPlaying)
		{
			gameplayMusic.volume = OriginalMusicVolume;
			gameplayMusic.Play();
		}
	}

	public void Stop_Music()
	{
		if (gameplayMusic.clip != null && musicOn == 1)
		{
			StartCoroutine(FadeOut(gameplayMusic, 0.1f));
		}
	}

	public void Play_PopUpShow(float time = 0f)
	{
		if (PopUpShow.clip != null && soundOn == 1)
		{
			StartCoroutine(PlayClip(PopUpShow, time));
		}
	}

	public void Play_PopUpHide(float time = 0f)
	{
		if (PopUpHide.clip != null && soundOn == 1)
		{
			StartCoroutine(PlayClip(PopUpHide, time));
		}
	}

	private IEnumerator PlayClip(AudioSource Clip, float time)
	{
		yield return new WaitForSeconds(time);
		Clip.Play();
	}

	private IEnumerator FadeOut(AudioSource sound, float time)
	{
		float originalVolume = sound.volume;
		if (sound.name == gameplayMusic.name)
		{
			originalVolume = OriginalMusicVolume;
		}
		while (sound.volume > 0.05f)
		{
			sound.volume = Mathf.MoveTowards(sound.volume, 0f, time);
			yield return null;
		}
		sound.Stop();
		sound.volume = originalVolume;
	}

	public void MuteAllSounds()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.GetComponent<AudioSource>().mute = true;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void UnmuteAllSounds()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.GetComponent<AudioSource>().mute = false;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void Play_Sound(AudioSource sound)
	{
		if (!sound.isPlaying && soundOn == 1)
		{
			sound.Play();
		}
	}

	public void StopAndPlay_Sound(AudioSource sound)
	{
		if (sound.isPlaying)
		{
			sound.Stop();
		}
		if (soundOn == 1)
		{
			sound.Play();
		}
	}

	public void Stop_Sound(AudioSource sound)
	{
		if (sound.isPlaying)
		{
			sound.Stop();
		}
	}

	public void ChangeSoundVolume(AudioSource sound, float time, float value)
	{
		if (value > 1f)
		{
			value = 1f;
		}
		if (value < 0f)
		{
			value = 0f;
		}
		if ((musicOn == 1 && sound.name == gameplayMusic.name) || (soundOn == 1 && sound.name != gameplayMusic.name))
		{
			StartCoroutine(_ChangeVolume(sound, time, value));
		}
	}

	private IEnumerator _ChangeVolume(AudioSource sound, float time, float value)
	{
		float _time = 0f;
		yield return new WaitForFixedUpdate();
		while (_time < 1f)
		{
			_time += Time.fixedDeltaTime / time;
			sound.volume = Mathf.Lerp(sound.volume, value, _time);
			yield return new WaitForFixedUpdate();
		}
	}

	public void StopActiveSoundsOnExitAndClearList()
	{
		foreach (AudioSource item in listStopSoundOnExit)
		{
			item.Stop();
		}
		listStopSoundOnExit.Clear();
	}

	public void StopAndPlay_Sound(AudioSource sound, float time)
	{
		if (soundOn == 1)
		{
			StartCoroutine(StopAndPlay_SoundDly(sound, time));
		}
	}

	private IEnumerator StopAndPlay_SoundDly(AudioSource sound, float time)
	{
		yield return new WaitForSeconds(time);
		if (sound.isPlaying)
		{
			sound.Stop();
		}
		if (soundOn == 1)
		{
			sound.Play();
		}
	}

	public void Play_Sound(AudioSource sound, float time)
	{
		if (soundOn == 1)
		{
			StartCoroutine(Play_SoundDly(sound, time));
		}
	}

	private IEnumerator Play_SoundDly(AudioSource sound, float time)
	{
		yield return new WaitForSeconds(time);
		if (!sound.isPlaying && soundOn == 1)
		{
			sound.Play();
		}
	}
}
