using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HumanPlayer : Player
{
	PlayerGUI playerGUI;

	int terrainLayer = 8;
	int unitLayer = 9;
	int buildingLayer = 10;

	protected override void Start()
	{
		base.Start();
		playerGUI = GetComponent<PlayerGUI>();

		team = Team.RED;

		// NOTE: Adding a Worker and a Base
		Worker worker = Instantiate(workerPrefab, transform);
		worker.transform.localPosition = new Vector3(3, 0.5f, 3);
		AddUnit(worker);

		Building baseBuilding = Instantiate(basePrefab, transform);
		baseBuilding.transform.localPosition = BaseMetrics.GetFirstBaseSpawn();
		AddBuilding(baseBuilding);
	}

	protected override void Update()
	{
		base.Update();

		if (Input.GetMouseButtonDown(0))
		{
			HandleUnitBuildingChoosing();
		}
		else if (Input.GetMouseButtonDown(1))
		{
			HandleUnitInput();
		}


		Unit unitChosen = GetUnitChosen();
		if (unitChosen != null)
		{
			if (unitChosen.type == UnitType.Worker)
			{
				HandleWorkerInput((Worker)unitChosen);
			}
			else if (unitChosen.type == UnitType.Leader)
			{
				HandleLeaderInput((Leader)unitChosen);
			}
		}

		Base baseB = GetBase();
		if (baseB != null)
		{
			HandleBaseInput(baseB);
		}


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

	void HandleUnitBuildingChoosing()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(inputRay, out RaycastHit hit))
		{
			if (!IsPointerOverUIObject())
			{
				if (hit.collider != null)
				{
					// Unit
					if (hit.collider.gameObject.layer == unitLayer)
					{
						SetAllChosenFalse();

						Unit unit = hit.collider.gameObject.GetComponent<Unit>();
						if (unit.Team == this.team)
						{
							unit.Chosen = true;
						}
					}
					// Building
					else if (hit.collider.gameObject.layer == buildingLayer)
					{
						SetAllChosenFalse();

						Building building = hit.collider.gameObject.GetComponent<Building>();
						if (building.Team == this.team)
						{
							building.Chosen = true;
						}
					}
					else
					{
						SetAllChosenFalse();
					}
				}
			}
		}
	}

	// TODO: Simplify and rename
	void HandleUnitInput()
	{
		Unit unit = GetUnitChosen();
		if (unit != null)
		{
			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(inputRay, out RaycastHit hit);

			// NOTE: Validating destination
			if (hit.collider != null)
			{
				if (hit.collider.gameObject.layer == terrainLayer)
				{
					Vector3 dest = hit.point;
					if (dest.x >= 0 || dest.x < map.Width || dest.z >= 0 || dest.z < map.Length)
					{
						if (Input.GetKey(KeyCode.LeftShift))
						{
							unit.AddMoveAction(dest);
						}
						else
						{
							unit.RemoveAllActions();
							unit.AddMoveAction(dest);
						}
						//unit.Destination = dest;
					}
				}
				else if (hit.collider.gameObject.layer == unitLayer && hit.collider.gameObject != unit.gameObject)
				{
					if (Input.GetKey(KeyCode.LeftShift))
					{
						unit.AddAttackAction(hit.collider.gameObject.transform);
					}
					else
					{
						unit.RemoveAllActions();
						unit.AddAttackAction(hit.collider.gameObject.transform);
					}
				}
			}



		}
	}

	private bool IsPointerOverUIObject()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
		{
			position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
		};
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	void HandleWorkerInput(Worker worker)
	{
		if (Input.GetKeyDown(Worker.miningKey))
		{
			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(inputRay, out RaycastHit hit))
			{
				if (hit.collider != null && hit.collider.gameObject.layer == terrainLayer)
				{
					Voxel targetVoxel = hit.collider.gameObject.GetComponent<Voxel>();

					if (Input.GetKey(KeyCode.LeftShift))
					{
						worker.AddMineAction(targetVoxel);
					}
					else
					{
						worker.RemoveAllActions();
						worker.AddMineAction(targetVoxel);
					}
				}
			}
		}
	}

	void HandleBaseInput(Base baseB)
	{
		if (Input.GetKeyDown(Base.makeWorkerKey) || playerGUI.CurrentKeyPressed == Base.makeWorkerKey)
		{
			AddUnit(baseB.CreateWorker());
		}
		else if (Input.GetKeyDown(Base.makeLeaderKey) || playerGUI.CurrentKeyPressed == Base.makeLeaderKey)
		{
			AddUnit(baseB.CreateLeader());
			hasLeader = true;
		}
		else if (Input.GetKeyDown(Base.stopProductionKey) || playerGUI.CurrentKeyPressed == Base.stopProductionKey)
		{
			baseB.StopProduction();
		}
	}

	void HandleLeaderInput(Leader leader)
	{
		if (Input.GetKeyDown(Leader.speedupKey) || playerGUI.CurrentKeyPressed == Leader.speedupKey)
		{
			leader.ActivateSpeedupAbility();
		}
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
