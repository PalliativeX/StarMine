using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{
	public bool isInTestMode;
	PlayerGUI playerGUI;

	protected override void Start()
	{
		base.Start();
		playerGUI = GetComponent<PlayerGUI>();

		team = Team.RED;

		AddBaseAndWorker();

		if (isInTestMode)
		{
			earth = 1000;
			stones = 1000;
			diamonds = 1000;
		}
	}

	protected override void Update()
	{
		base.Update();

		HandlePanelDisplay();
	}

	void HandlePanelDisplay()
	{
		// FIX
		playerGUI.DisplayWorkerPanel(false);
		playerGUI.DisplayWorkerSelectionPanel(false);
		playerGUI.DisplayLeaderPanel(false);

		// Displaying Base panel if a Base is chosen
		playerGUI.DisplayBasePanel(BaseChosen());
		
		List<Unit> unitsChosen = GetUnitsChosen();
		if (unitsChosen.Count > 0)
		{
			Unit unit = unitsChosen[0];

			playerGUI.DisplayLeaderPanel(unit is Leader);
			if (unit is Worker worker)
			{
				if (worker.BuildingStatus == BuildingStatus.None || worker.BuildingStatus == BuildingStatus.InProgress)
				{
					playerGUI.DisplayWorkerPanel(true);
				}
				else if (worker.BuildingStatus == BuildingStatus.Selected || worker.BuildingStatus == BuildingStatus.Selection)
				{
					playerGUI.DisplayWorkerSelectionPanel(true);
				}
			}
		}
	}

}
