﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HumanPlayerInputController : MonoBehaviour
{
	HumanPlayer player;
	PlayerGUI playerGUI;
	Camera mainCamera;

	public bool isSelecting;
	Vector3 initialMousePos;

	void Start()
	{
		player = GetComponent<HumanPlayer>();
		playerGUI = GetComponent<PlayerGUI>();
		mainCamera = Camera.main;
	}

	void Update()
	{
		HandleSelection();

		HandleUnitMovement();

		List<Unit> unitsChosen = player.GetUnitsChosen();
		foreach (Unit unitChosen in unitsChosen)
		{
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
		}

		HandleBaseInput();

		playerGUI.CurrentKeyPressed = KeyCode.None;
	}

	void HandleSelection()
	{
		List<Selectable> selectables = player.GetSelectables();

		if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
		{
			isSelecting = true;
			initialMousePos = Input.mousePosition;
			HandleUnitBuildingSelection();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			isSelecting = false;
		}

		// NOTE: Selecting units and buildings in the selection box
		if (isSelecting)
		{
			List<Selectable> selected = new List<Selectable>();
			bool hasBuildingSelected = false;
			foreach (Selectable selectable in selectables)
			{
				if (IsWithinSelectionBounds(selectable))
				{
					selectable.Chosen = true;
					if (selectable is Building)
						hasBuildingSelected = true;
					selected.Add(selectable);
				}
			}

			// NOTE: We select only buildings if any
			if (hasBuildingSelected)
			{
				foreach (Selectable current in selected)
				{
					if (current is Unit)
					{
						current.Chosen = false;
					}
				}
			}
		}
	}

	void HandleUnitBuildingSelection()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(inputRay, out RaycastHit hit))
		{
			if (!IsPointerOverUIObject())
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

					if (colliderMask == BaseMetrics.terrainMask)
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

	void HandleBaseInput()
	{
		Base mainBase = player.GetBase();
		if (mainBase == null || !mainBase.Chosen)
		{
			return;
		}

		if (Input.GetKeyDown(Base.makeWorkerKey) || playerGUI.CurrentKeyPressed == Base.makeWorkerKey)
		{
			mainBase.CreateWorker();
		}
		else if (Input.GetKeyDown(Base.makeLeaderKey) || playerGUI.CurrentKeyPressed == Base.makeLeaderKey)
		{
			mainBase.CreateLeader();
			//player.HasLeader = true;
		}
		else if (Input.GetKeyDown(Base.stopProductionKey) || playerGUI.CurrentKeyPressed == Base.stopProductionKey)
		{
			mainBase.StopProduction();
		}
	}

	void HandleLeaderInput(Leader leader)
	{
		if (Input.GetKeyDown(Leader.speedupKey) || playerGUI.CurrentKeyPressed == Leader.speedupKey)
		{
			leader.ActivateSpeedupAbility();
		}
	}

	void HandleUnitMovement()
	{
		if (Input.GetMouseButtonDown(1))
		{
			List<Unit> unitsChosen = player.GetUnitsChosen();

			Vector3 prevUnitPos = Vector3.zero;
			foreach (Unit unit in unitsChosen)
			{
				if (unit != null)
				{
					Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
					Physics.Raycast(inputRay, out RaycastHit hit);

					if (hit.collider != null)
					{
						LayerMask colliderMask = 1 << hit.collider.gameObject.layer;

						if (colliderMask == BaseMetrics.terrainMask)
						{
							Vector3 dest = hit.point;
							dest.y += 0.3f;
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
						else if ((colliderMask == BaseMetrics.unitMask || colliderMask == BaseMetrics.buildingMask) &&
								  hit.collider.gameObject != unit.gameObject)
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
	}

	private void OnGUI()
	{
		if (isSelecting)
		{
			var rect = GuiUtils.GetScreenRect(initialMousePos, Input.mousePosition);
			GuiUtils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
			GuiUtils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
		}
	}

	public bool IsWithinSelectionBounds(Selectable selectable)
	{
		if (!isSelecting)
			return false;

		var viewportBounds =
			GuiUtils.GetViewportBounds(mainCamera, initialMousePos, Input.mousePosition);

		return viewportBounds.Contains(
			mainCamera.WorldToViewportPoint(selectable.transform.position));
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


}
