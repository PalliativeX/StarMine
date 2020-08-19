using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public enum Team
{
	RED, BLUE
}

public abstract class Player : MonoBehaviour
{
	protected Team team;

	[Range(50, 300)]
	public int startingEarth = 100;
	[Range(50, 300)]
	public int startingStones = 50;
	[Range(0, 100)]
	public int startingDiamonds = 0;

	protected int earth, stones, diamonds;

	[SerializeField]
	private Worker workerPrefab;
	[SerializeField]
	private Building basePrefab;

	protected List<Unit> units;
	protected List<Building> buildings;

	protected bool hasLeader;

	protected virtual void Start()
	{
		units = new List<Unit>();
		buildings = new List<Building>();

		earth = startingEarth;
		stones = startingStones;
		diamonds = startingDiamonds;
	}

	protected virtual void Update()
	{
	}

	public void AddBaseAndWorker()
	{
		Worker worker = Instantiate(workerPrefab, transform);
		if (team == Team.RED)
		{
			worker.transform.localPosition = new Vector3(3, 0.6f, 3);
		}
		else
		{
			worker.transform.localPosition = new Vector3(6, 0.6f, 6);
		}
		AddUnit(worker);

		Building baseBuilding = Instantiate(basePrefab, transform);
		if (team == Team.RED)
		{
			baseBuilding.transform.localPosition = BaseMetrics.GetFirstBaseSpawn();
		}
		else
		{
			baseBuilding.transform.localPosition = BaseMetrics.GetFirstOppositeBaseSpawn();
		}
		AddBuilding(baseBuilding);
	}

	public void AddUnit(Unit unit)
	{
		unit.transform.SetParent(transform);
		unit.Team = this.team;
		units.Add(unit);
	}

	public void RemoveUnit(Unit unit)
	{
		units.Remove(unit);
		Destroy(unit.gameObject);
	}

	public void AddBuilding(Building building)
	{
		building.transform.SetParent(transform);
		building.Team = this.team;
		buildings.Add(building);
	}

	public void Remove(Selectable targetable)
	{
		targetable.gameObject.TryGetComponent(out Building building);
		if (building != null)
		{
			buildings.Remove(building);
		}
		else
		{
			units.Remove((Unit)targetable);
		}

		Destroy(targetable.gameObject);
	}

	public void RemoveBuilding(Building building)
	{
		buildings.Remove(building);
		Destroy(building.gameObject);
	}

	public void OnUnitDeath(Unit unit)
	{
		units.Remove(unit);
	}

	public void OnBuildingDestruction(Building building)
	{
		buildings.Remove(building);
	}

	public List<Unit> GetUnitsChosen()
	{
		List<Unit> unitsChosen = new List<Unit>();

		foreach (Unit unit in units)
		{
			if (unit.Chosen)
				unitsChosen.Add(unit);
		}

		return unitsChosen;
	}

	public List<Building> GetBuildingsChosen()
	{
		List<Building> buildingsChosen = new List<Building>();

		foreach (Building building in buildings)
		{
			if (building.Chosen)
				buildingsChosen.Add(building);
		}
		return buildingsChosen;
	}

	public Selectable GetTargetableChosen()
	{
		foreach (Unit unit in units)
		{
			if (unit.Chosen)
				return unit;
		}

		foreach (Building building in buildings)
		{
			if (building.Chosen)
				return building;
		}

		return null;
	}

	public void SetAllChosenFalse()
	{
		foreach (Unit unit in units)
		{
			unit.Chosen = false;
		}

		foreach (Building building in buildings)
		{
			building.Chosen = false;
		}
	}

	public bool BaseChosen()
	{
		foreach (Building building in buildings)
		{
			if (building.GetComponent<Base>() != null && building.Chosen)
			{
				return true;
			}
		}
		return false;
	}

	public List<Selectable> GetSelectables()
	{
		List<Selectable> selectables = new List<Selectable>();

		foreach (Unit unit in units)
		{
			selectables.Add(unit);
		}
		foreach (Building building in buildings)
		{
			selectables.Add(building);
		}

		return selectables;
	}

	public Base GetBase()
	{
		foreach (Building building in buildings)
		{
			if (building.GetComponent<Base>())
			{
				return (Base)building;
			}
		}
		return null;
	}

	public bool UnitChosen()
	{
		foreach (Unit unit in units)
		{
			if (unit.Chosen)
				return true;	
		}
		return false;
	}

	public bool BuildingChosen()
	{
		foreach (Building building in buildings)
		{
			if (building.Chosen)
				return true;
		}
		return false;
	}

	public bool HasEnoughResources(Vector3 requiredResources)
	{
		return Earth    >= requiredResources.x &&
			   Stones   >= requiredResources.y &&
			   Diamonds >= requiredResources.z;
	}

	public void SubtractResources(Vector3 resources)
	{
		Earth -= (int)resources.x;
		Stones -= (int)resources.y;
		Diamonds -= (int)resources.z;
	}

	public void AddResources(Vector3 resources)
	{
		Earth += (int)resources.x;
		Stones += (int)resources.y;
		Diamonds += (int)resources.z;
	}

	public int Earth
	{
		get { return earth; }
		set {
			if (value >= 0)
			{
				earth = value;
			}
		}
	}

	public int Stones
	{
		get { return stones; }
		set {
			if (value >= 0)
			{
				stones = value;
			}
		}
	}

	public int Diamonds
	{
		get { return diamonds; }
		set {
			if (value >= 0)
			{
				diamonds = value;
			}
		}
	}

	public Team Team
	{
		get { return team; }
	}

	public bool HasLeader
	{
		get { return hasLeader; }
		set {
			hasLeader = value;
		}
	}

}
