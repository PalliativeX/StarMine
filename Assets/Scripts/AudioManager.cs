using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	float masterVolumePercent;
	float sfxVolumePercent;
	float musicVolumePercent;

	public static AudioManager instance;

	AudioSource musicSource;
	AudioSource sfx2DSource;

	private void Awake()
	{
		if (instance != null)
			Destroy(gameObject);

		instance = this;

		musicSource = new AudioSource();
		GameObject newMusicSource = new GameObject("Music source 1");
		musicSource = newMusicSource.AddComponent<AudioSource>();
		newMusicSource.transform.parent = transform;

		GameObject newSfx2Dsource = new GameObject("2D sfx source");
		sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();
		newSfx2Dsource.transform.parent = transform;

		masterVolumePercent = PlayerPrefs.GetFloat("master");
		musicVolumePercent = PlayerPrefs.GetFloat("music");
		sfxVolumePercent = PlayerPrefs.GetFloat("sfx");
	}

	public void PlayMusic(AudioClip clip)
	{
		musicSource.Stop();
		musicSource.clip = clip;
		musicSource.volume = masterVolumePercent * musicVolumePercent;
		musicSource.Play();
	}

	public void AdjustMusicVolume()
	{
		musicSource.volume = masterVolumePercent * musicVolumePercent;
	}

	// NOTE: For short sounds 
	public void PlaySound(AudioClip clip, Vector3 pos)
	{
		if (clip != null)
		{
			AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
		}
	}

	// NOTE: For short sounds 
	public void PlaySound2D(AudioClip clip)
	{
		if (clip != null)
		{
			sfx2DSource.PlayOneShot(clip, sfxVolumePercent * masterVolumePercent);
		}
	}

	public float MasterVolumePercent
	{
		get { return masterVolumePercent; }
		set {
			if (value >= 0f && value <= 1f)
			{
				masterVolumePercent = value;
				AdjustMusicVolume();
			}
		}
	}

	public float MusicVolumePercent
	{
		get { return musicVolumePercent; }
		set {
			if (value >= 0f && value <= 1f)
			{
				musicVolumePercent = value;
				AdjustMusicVolume();
			}
		}
	}

	public float SFXVolumePercent
	{
		get { return sfxVolumePercent; }
		set {
			if (value >= 0f && value <= 1f)
			{
				sfxVolumePercent = value;
			}
		}
	}

}
