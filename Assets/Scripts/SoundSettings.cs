using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
	private void Start()
	{
		InitialiseSoundSettings();
	}

	public void InitialiseSoundSettings()
	{
		SoundManager.musicOn = PlayerPrefs.GetInt("MusicOn", 1);
		SoundManager.soundOn = PlayerPrefs.GetInt("SoundOn", 1);
		if (SoundManager.soundOn == 0)
		{
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = true;
		}
		else
		{
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = false;
		}
		if (SoundManager.musicOn == 0)
		{
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = true;
		}
		else
		{
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = false;
		}
	}

	public void SoundOnOff()
	{
		if (SoundManager.soundOn == 1)
		{
			SoundManager.soundOn = 0;
			SoundManager.Instance.MuteAllSounds();
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = true;
		}
		else
		{
			SoundManager.soundOn = 1;
			SoundManager.Instance.UnmuteAllSounds();
			SoundManager.Instance.Play_ButtonClick();
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = false;
		}
		PlayerPrefs.SetInt("SoundOn", SoundManager.soundOn);
		PlayerPrefs.SetInt("MusicOn", SoundManager.musicOn);
		PlayerPrefs.Save();
	}

	public void MusicOnOff()
	{
		SoundManager.Instance.Play_ButtonClick();
		if (SoundManager.musicOn == 1)
		{
			SoundManager.Instance.Stop_Music();
			SoundManager.musicOn = 0;
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = true;
		}
		else
		{
			SoundManager.musicOn = 1;
			SoundManager.Instance.Play_Music();
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = false;
		}
		PlayerPrefs.SetInt("SoundOn", SoundManager.soundOn);
		PlayerPrefs.SetInt("MusicOn", SoundManager.musicOn);
		PlayerPrefs.Save();
	}
}
