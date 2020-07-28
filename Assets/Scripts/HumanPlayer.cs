using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HumanPlayer : Player
{
	PlayerGUI playerGUI;

	protected override void Start()
	{
		base.Start();
		playerGUI = GetComponent<PlayerGUI>();

		team = Team.RED;

		AddBaseAndWorker();
	}

	protected override void Update()
	{
		base.Update();

		HandlePanelDisplay();
	}

	void HandlePanelDisplay()
	{
		// Displaying Base panel if a Base is chosen
		playerGUI.DisplayBasePanel(BaseChosen());


		Unit unit = GetUnitChosen();
		playerGUI.DisplayLeaderPanel(unit != null && unit.type == UnitType.Leader);


		playerGUI.CurrentKeyPressed = KeyCode.None;
	}

	public bool IsPointerOverUIObject()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
		{
			position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
		};
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	public KeyCode CurrentKeyPressed()
	{
		return playerGUI.CurrentKeyPressed;
	}

	public void SetCurrentKeyPressed(KeyCode keyCode)
	{
		playerGUI.CurrentKeyPressed = keyCode;
	}

}
