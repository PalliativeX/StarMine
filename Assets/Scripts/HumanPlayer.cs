using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		
		List<Unit> unitsChosen = GetUnitsChosen();
		if (unitsChosen.Count > 0)
		{
			Unit unit = unitsChosen[0];
			playerGUI.DisplayLeaderPanel(unit != null && unit.type == UnitType.Leader);
		}
	}

}
