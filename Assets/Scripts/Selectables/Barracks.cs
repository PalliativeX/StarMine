using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: Prototype, prob make a base Producer class
public class Barracks : Building
{
	public Warrior warriorPrefab;
	public Warrior soldierPrefab;

	public static KeyCode makeWarriorKey = KeyCode.W;
	public static KeyCode makeSoldierKey = KeyCode.S;

	Vector3 spawnRelativeToBase;
	Vector3 rallyPoint;


	protected override void Start()
	{
		base.Start();

		spawnRelativeToBase = new Vector3(-2.9f, 0f, 0.35f);
	}

	protected override void Update()
	{
		base.Update();
	}

	public void CreateWarrior()
	{
		Warrior warrior = Instantiate(warriorPrefab, player.transform);
		player.AddUnit(warrior);
		warrior.transform.position = transform.position + spawnRelativeToBase;
		warrior.AddMoveAction(rallyPoint);
	}

	public void CreateSoldier()
	{
		Warrior soldier = Instantiate(soldierPrefab, player.transform);
		player.AddUnit(soldier);
		soldier.transform.position = transform.position + spawnRelativeToBase;
		soldier.AddMoveAction(rallyPoint);
	}




}
