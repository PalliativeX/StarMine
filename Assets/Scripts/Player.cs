using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
	RED, BLUE
}


public abstract class Player : MonoBehaviour
{
	// TODO: A player should not have a direct access to the map, 
	// only to what he already discovered
	public Map map;

	protected Team team;

	[Range(50, 300)]
	public int startingEarth = 100;
	[Range(50, 300)]
	public int startingStones = 50;
	[Range(0, 100)]
	public int startingDiamonds = 0;

	public Worker workerPrefab;
	public Building basePrefab;

	protected int earth, stones, diamonds;

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

	public Unit GetUnitChosen()
	{
		foreach (Unit unit in units)
		{
			if (unit.Chosen)
				return unit;
		}
		return null;
	}

	public Building GetBuildingChosen()
	{
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

}
