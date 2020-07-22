using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
	public MenuMusicManager musicManager;

	public Transform menuPanel;
	public Transform settingsPanel;

	public Slider masterSlider, musicSlider, sfxSlider;
	public Toggle fullscreenToggle;
	public TMPro.TMP_Dropdown resolutionDropdown;
	public TMPro.TMP_Dropdown qualityDropdown;

	Resolution[] resolutions;

	private void Start()
	{
		masterSlider.value = AudioManager.instance.MasterVolumePercent;
		musicSlider.value = AudioManager.instance.MusicVolumePercent;
		sfxSlider.value = AudioManager.instance.SFXVolumePercent;

		resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions();

		List<string> options = new List<string>();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width  == Screen.currentResolution.width &&
				resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions(options);

		// Loading player settings
		if (PlayerPrefs.HasKey("quality"))
		{
			qualityDropdown.value = PlayerPrefs.GetInt("quality");
		}
		else
		{
			qualityDropdown.value = qualityDropdown.options.Count - 1;
		}
		if (PlayerPrefs.HasKey("fullscreen"))
		{
			fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen") == 1 ? true : false;
		}
		else
		{
			fullscreenToggle.isOn = true;
			ToggleFullscreen(true);
		}
		if (PlayerPrefs.HasKey("resolution"))
		{
			currentResolutionIndex = PlayerPrefs.GetInt("resolution");
		}
		resolutionDropdown.value = currentResolutionIndex;
		SetResolution(currentResolutionIndex);
		resolutionDropdown.RefreshShownValue();
	}

	public void Play()
	{
		SceneManager.LoadSceneAsync("MainScene");
	}

	public void OpenSettingsMenu()
	{
		menuPanel.gameObject.SetActive(false);
		settingsPanel.gameObject.SetActive(true);
	}

	public void QuitSettingsMenu()
	{
		settingsPanel.gameObject.SetActive(false);
		menuPanel.gameObject.SetActive(true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
		PlayerPrefs.SetInt("quality", qualityIndex);
		PlayerPrefs.Save();
	}

	public void ToggleFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
		PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		PlayerPrefs.SetInt("resolution", resolutionIndex);
		PlayerPrefs.Save();
	}

	public void SetMasterVolume(float newVolume)
	{
		AudioManager.instance.MasterVolumePercent = newVolume;
		PlayerPrefs.SetFloat("master", newVolume);
		PlayerPrefs.Save();
	}

	public void SetMusicVolume(float newVolume)
	{
		AudioManager.instance.MusicVolumePercent = newVolume;
		PlayerPrefs.SetFloat("music", newVolume);
		PlayerPrefs.Save();
	}

	public void SetSFXVolume(float newVolume)
	{
		AudioManager.instance.SFXVolumePercent = newVolume;
		PlayerPrefs.SetFloat("sfx", newVolume);
		PlayerPrefs.Save();
	}
}
