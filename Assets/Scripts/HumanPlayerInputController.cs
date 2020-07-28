using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerInputController : MonoBehaviour
{
	public HumanPlayer player;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HandleUnitBuildingSelection();
		}
		else if (Input.GetMouseButtonDown(1))
		{
			HandleUnitMovement();
		}

		Unit unitChosen = player.GetUnitChosen();
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

		Base mainBase = player.GetBase();
		if (mainBase != null)
		{
			HandleBaseInput(mainBase);
		}
	}

	void HandleWorkerInput(Worker worker)
	{
		if (Input.GetKeyDown(Worker.miningKey))
		{
			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(inputRay, out RaycastHit hit))
			{
				if (hit.collider != null)
				{
					LayerMask colliderMask = 1 << hit.collider.gameObject.layer;

					if (colliderMask != BaseMetrics.terrainMask)
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
	}

	void HandleBaseInput(Base mainBase)
	{
		if (Input.GetKeyDown(Base.makeWorkerKey) || player.CurrentKeyPressed() == Base.makeWorkerKey)
		{
			player.AddUnit(mainBase.CreateWorker());
		}
		else if (Input.GetKeyDown(Base.makeLeaderKey) || player.CurrentKeyPressed() == Base.makeLeaderKey)
		{
			player.AddUnit(mainBase.CreateLeader());
			player.HasLeader = true;
		}
		else if (Input.GetKeyDown(Base.stopProductionKey) || player.CurrentKeyPressed() == Base.stopProductionKey)
		{
			mainBase.StopProduction();
		}
	}

	void HandleLeaderInput(Leader leader)
	{
		if (Input.GetKeyDown(Leader.speedupKey) || player.CurrentKeyPressed() == Leader.speedupKey)
		{
			leader.ActivateSpeedupAbility();
		}
	}

	void HandleUnitBuildingSelection()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(inputRay, out RaycastHit hit))
		{
			if (!player.IsPointerOverUIObject())
			{
				if (hit.collider != null)
				{
					LayerMask colliderMask = 1 << hit.collider.gameObject.layer;
					// Unit
					if (colliderMask == BaseMetrics.unitMask)
					{
						player.SetAllChosenFalse();

						Unit unit = hit.collider.gameObject.GetComponent<Unit>();
						if (unit.Team == player.Team)
						{
							unit.Chosen = true;
						}
					}
					// Building
					else if (colliderMask == BaseMetrics.buildingMask)
					{
						player.SetAllChosenFalse();

						Building building = hit.collider.gameObject.GetComponent<Building>();
						if (building.Team == player.Team)
						{
							building.Chosen = true;
						}
					}
					else
					{
						player.SetAllChosenFalse();
					}
				}
			}
		}
	}

	// TODO: Simplify and rename
	void HandleUnitMovement()
	{
		Unit unit = player.GetUnitChosen();
		if (unit != null)
		{
			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(inputRay, out RaycastHit hit);

			// NOTE: Validating destination
			if (hit.collider != null)
			{
				LayerMask colliderMask = 1 << hit.collider.gameObject.layer;

				if (colliderMask == BaseMetrics.terrainMask)
				{
					Vector3 dest = hit.point;
					if (BaseMetrics.IsDestValid(dest))
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
					}
				}
				else if (colliderMask == BaseMetrics.unitMask && hit.collider.gameObject != unit.gameObject)
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
}
