using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public bool musicOn;

	public AudioClip mainTheme;

	private void Awake()
	{
		if (musicOn)
		{
			if (AudioManager.instance != null)
			{
				AudioManager.instance.PlayMusic(mainTheme);
			}
		}
	}

}
