using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Building
{
	public Worker workerPrefab;
	public Leader leaderPrefab;

	public static KeyCode makeWorkerKey = KeyCode.W;
	public static KeyCode makeLeaderKey = KeyCode.L;
	public static KeyCode stopProductionKey = KeyCode.Escape;

	public static Vector3 workerCost = new Vector3(0, 50, 0);
	public static Vector3 leaderCost = new Vector3(0, 100, 5);

	readonly int workerProductionTime = 10;
	readonly int leaderProductionTime = 25;
	bool inProduction = false;
	float productionTimeLeft = 0f;
	Vector3 currentCost;

	Vector3 spawnRelativeToBase;
	Vector3 rallyPoint;

	protected override void Start()
	{
		base.Start();

		spawnRelativeToBase = new Vector3(-2.9f, 0.8f, 0.35f);
		rallyPoint = transform.position - new Vector3(3.5f, 0f, 0.35f);
	}

	protected override void Update()
	{
		base.Update();
	}

	public void CreateWorker()
	{
		if (!inProduction && player.HasEnoughResources(workerCost))
		{
			StartCoroutine(ProduceUnit(workerProductionTime, workerPrefab, workerCost));
		}
	}

	public void CreateLeader()
	{
		if (!inProduction && player.HasEnoughResources(leaderCost))
		{
			StartCoroutine(ProduceUnit(leaderProductionTime, leaderPrefab, leaderCost));
		}
	}

	IEnumerator ProduceUnit(float productionTime, Unit prefab, Vector3 unitCost)
	{
		inProduction = true;
		productionTimeLeft = productionTime;
		player.SubtractResources(unitCost);
		currentCost = unitCost;

		while (inProduction && productionTimeLeft > 0f)
		{
			productionTimeLeft -= Time.deltaTime;

			yield return null;
		}

		if (inProduction)
		{
			Unit unit = Instantiate(prefab, player.transform);
			player.AddUnit(unit);
			unit.transform.position = transform.position + spawnRelativeToBase;
			unit.AddMoveAction(rallyPoint);

			inProduction = false;
			productionTimeLeft = 0f;
		}
	}

	public void StopProduction()
	{
		inProduction = false;
		productionTimeLeft = 0f;
		player.AddResources(currentCost);
	}

}
