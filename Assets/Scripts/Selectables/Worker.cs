using UnityEngine;
using System.Collections;

class MineAction : Action
{
	public Voxel targetVoxel;

	public MineAction(Voxel targetVoxel)
	{
		this.targetVoxel = targetVoxel;
	}
}

class BuildAction : Action
{
	public Building buildingPrefab;
	public Vector3 buildPos;

	public BuildAction(Building prefab, Vector3 buildPos)
	{
		buildingPrefab = prefab;
		this.buildPos = buildPos;
	}
}

/*
 * Selection   - in worker's building menu
 * Selected    - chosen a building but hasn't placed it yet
 * InProgress  - building right now
 */
public enum BuildingStatus
{
	None,
	Selection,
	Selected,
	InProgress
}

public enum BuildingBuilt
{
	Turret,
	Barracks
}

public class Worker : Unit
{
	// TODO: Probably incapsulate this into some data class
	public Building turretPrefab;
	public static float turretBuildingTime = 10;
	public static Vector3 turretCost = new Vector3(100, 0, 0);

	public Building barracksPrefab;
	public static float barracksBuildingTime = 15;
	public static Vector3 barracksCost = new Vector3(150, 30, 0);

	public static KeyCode miningKey = KeyCode.G;
	public static KeyCode selectionModeKey = KeyCode.B;
	public static KeyCode selectTurretKey = KeyCode.T;
	public static KeyCode selectBarracksKey = KeyCode.C;
	public static KeyCode cancelKey = KeyCode.Escape;

	public float miningRange = 2f;
	public float buildingRange = 5f;

	Vector3 currentCost;

	// NOTE: Mining time may be changed with better gear
	float earthMiningTime = 1.2f;
	float stoneMiningTime = 2.3f;
	float diamondMiningTime = 3.5f;
	float miningTimeLeft = 0f;

	float buildingTimeLeft;
	BuildingStatus status;
	BuildingBuilt buildingBuilt;
	Building selectedBuilding;

	Voxel targetVoxel;

	Camera mainCamera;

	EngineerActions animationActions;

	protected override void Start()
	{
		base.Start();
		mainCamera = Camera.main;
		status = BuildingStatus.None;
		animationActions = GetComponent<EngineerActions>();
		animationActions.Stay();
		GetComponent<EngineerController>().SetArsenal("OneHand");
	}

	protected override void Update()
	{
		base.Update();

		//Animation
		if (currentAction is null || currentAction is BuildAction)
		{
			animationActions.Stay();
		}
		if (currentAction is MoveAction)
		{
			animationActions.Walk();
		}
		if (currentAction is AttackAction)
		{
			animationActions.Attack();
		}

		if (currentAction != null && !currentAction.started)
		{
			if (currentAction is MineAction && targetVoxel == null)
			{
				currentAction.started = true;
				Voxel targetVoxel = ((MineAction)currentAction).targetVoxel;

				if (Vector3.Distance(targetVoxel.transform.position, transform.position) < miningRange)
				{
					SetTargetVoxel(targetVoxel);
				}
				else
				{
					currentAction = null;
					Vector3 targetPoint = BaseMetrics.GetDest(transform.position, targetVoxel.transform.position, miningRange);
					targetPoint += (targetVoxel.transform.position - transform.position).normalized * 0.08f;
					AddMoveAction(targetPoint);
					AddMineAction(targetVoxel);
				}
			}
			else if (currentAction is BuildAction && status != BuildingStatus.InProgress)
			{
				currentAction.started = true;

				BuildAction current = (BuildAction)currentAction;
				if (Vector3.Distance(current.buildPos, transform.position) < buildingRange)
				{
					StartCoroutine(Build(current.buildingPrefab, current.buildPos));
				}
				else
				{
					currentAction = null;
					Vector3 targetPoint = BaseMetrics.GetDest(transform.position, current.buildPos, buildingRange);
					targetPoint += (current.buildPos - transform.position).normalized * 0.1f;
					AddMoveAction(targetPoint);
					AddBuildAction(current.buildPos); // TODO: Check this
				}
			}
		}

		if (selectedBuilding != null && status == BuildingStatus.Selected)
		{
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				selectedBuilding.transform.position = hit.transform.position + Vector3.up * BaseMetrics.VoxelDimension * 0.5f;
			}

		}

		UpdateMining();
	}

	void UpdateMining()
	{
		if (destination != Vector3.zero)
		{
			targetVoxel = null;
			miningTimeLeft = 0f;
		}

		if (targetVoxel != null)
		{
			if (miningTimeLeft > 0f)
			{
				miningTimeLeft -= Time.deltaTime;
			}
			else
			{
				targetVoxel.gameObject.SetActive(false);
				switch (targetVoxel.type)
				{
					case ResourceType.Earth:
						player.Earth += targetVoxel.amount;
						break;
					case ResourceType.Stone:
						player.Stones += targetVoxel.amount;
						break;
					case ResourceType.Diamond:
						player.Diamonds += targetVoxel.amount;
						break;
				}
				targetVoxel.mined = true;
				targetVoxel = null;
				currentAction = null;
			}
		}
	}

	public void CreateBuildingHologram(KeyCode code)
	{
		status = BuildingStatus.Selected;

		if (code == selectTurretKey)
		{
			buildingBuilt = BuildingBuilt.Turret;
			selectedBuilding = Instantiate(turretPrefab, player.transform);
		}
		else if (code == selectBarracksKey)
		{
			buildingBuilt = BuildingBuilt.Barracks;
			selectedBuilding = Instantiate(barracksPrefab, player.transform);
		}

		selectedBuilding.GetComponent<MeshRenderer>().material.color = new Color(0.1f, 0.9f, 0.1f, 0.4f);
		foreach (MeshRenderer renderer in selectedBuilding.GetComponentsInChildren<MeshRenderer>())
		{
			renderer.material.color = new Color(0.1f, 0.9f, 0.1f, 0.4f);
		}

		selectedBuilding.healthBar.enabled = false;
		selectedBuilding.GetComponent<Collider>().enabled = false;
		selectedBuilding.healthBar.transform.parent.gameObject.SetActive(false);
	}

	public void DestroyHologram()
	{
		selectedBuilding.Destroy();
		Destroy(selectedBuilding.gameObject);
		selectedBuilding = null;
	}

	IEnumerator Build(Building prefab, Vector3 buildPos)
	{
		status = BuildingStatus.InProgress;
		DestroyHologram();
		buildingTimeLeft = 
			(buildingBuilt == BuildingBuilt.Turret) ? turretBuildingTime : barracksBuildingTime;
		player.SubtractResources(turretCost);
		currentCost = turretCost;

		while (status == BuildingStatus.InProgress && buildingTimeLeft >= 0)
		{
			buildingTimeLeft -= Time.deltaTime;

			yield return null;
		}

		if (status == BuildingStatus.InProgress)
		{
			Building building = Instantiate(prefab, player.transform);
			building.transform.position = buildPos;
			player.AddBuilding(building);

			status = BuildingStatus.None;
			buildingTimeLeft = 0f;
			currentAction = null;
		}
	}

	public void AddMineAction(Voxel targetVoxel)
	{
		actions.Enqueue(new MineAction(targetVoxel));
	}

	public void AddBuildAction(Vector3 buildPos)
	{
		if ((buildingBuilt == BuildingBuilt.Turret   && !player.HasEnoughResources(turretCost)) ||
			(buildingBuilt == BuildingBuilt.Barracks && !player.HasEnoughResources(barracksCost)))
		{
			return;
		}

		actions.Enqueue(new BuildAction((buildingBuilt == BuildingBuilt.Turret) ? turretPrefab : barracksPrefab, buildPos));
	}

	public void SetTargetVoxel(Voxel targetVoxel)
	{
		destination = Vector3.zero;

		this.targetVoxel = targetVoxel;
		switch (targetVoxel.type)
		{
			case ResourceType.Earth:
				miningTimeLeft = earthMiningTime;
				break;
			case ResourceType.Stone:
				miningTimeLeft = stoneMiningTime;
				break;
			case ResourceType.Diamond:
				miningTimeLeft = diamondMiningTime;
				break;
			case ResourceType.Lava:
				this.targetVoxel = null; // NOTE: We can't mine lava
				miningTimeLeft = 0f;
				break;
		}
	}

	public float EarthMiningTime
	{
		get { return earthMiningTime; }
		set {
			if (value >= 0f)
				earthMiningTime = value;
		}
	}

	public float StoneMiningTime
	{
		get { return stoneMiningTime; }
		set {
			if (value >= 0f)
				stoneMiningTime = value;
		}
	}

	public float DiamondMiningTime
	{
		get { return diamondMiningTime; }
		set {
			if (value >= 0f)
				diamondMiningTime = value;
		}
	}

	public BuildingStatus BuildingStatus
	{
		get { return status; }
		set {
			status = value;
		}
	}

}
