using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{ 
	public bool playMenuMusic; // NOTE: For debugging

	public AudioClip menuMusic;

	public AudioClip buttonClick;

	private void Start()
	{
		if (playMenuMusic)
		{
			AudioManager.instance.PlayMusic(menuMusic);
		}
	}

	public void OnButtonClick()
	{
		AudioManager.instance.PlaySound2D(buttonClick);
	}

}
