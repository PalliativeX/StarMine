using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
	Player player;

	public Text earthText, stoneText, diamondsText;

	public Transform basePanel;
	public Transform leaderPanel;

	KeyCode currentlyPressedKey;

	private void Awake()
	{
		player = GetComponent<Player>();
		currentlyPressedKey = KeyCode.None;
		basePanel.gameObject.SetActive(false);
		leaderPanel.gameObject.SetActive(false);
	}

	private void Update()
	{
		// TODO: Change to update when necessary
		UpdateResourcesPanel();
	}

	public void UpdateResourcesPanel()
	{
		earthText.text = player.Earth.ToString();
		stoneText.text = player.Stones.ToString();
		diamondsText.text = player.Diamonds.ToString();
	}

	public void DisplayBasePanel(bool toggle)
	{
		basePanel.gameObject.SetActive(toggle);
	}

	public void DisplayLeaderPanel(bool toggle)
	{
		leaderPanel.gameObject.SetActive(toggle);
	}

	public void SetKeyCode(string keyStr)
	{
		KeyCode key = (KeyCode)Enum.Parse(typeof(KeyCode), keyStr);
		currentlyPressedKey = key;
	}

	public KeyCode CurrentKeyPressed
	{
		get { return currentlyPressedKey; }
		set { currentlyPressedKey = value; }
	}


}
