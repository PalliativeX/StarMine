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

	readonly int workerProductionTime = 10;
	readonly int leaderProductionTime = 25;
	bool inProduction = false;
	float productionTimeLeft = 0f;

	Vector3 spawnRelativeToBase = new Vector3(-2.9f, 0.7f, 0.35f);
	public Vector3 RallyPoint { get; set; } // Relative to current unit pos

	protected override void Start()
	{
		base.Start();

		RallyPoint = new Vector3(-1.3f, 0f, 0);
	}

	protected override void Update()
	{
		base.Update();
	}

	public Worker CreateWorker()
	{
		Worker worker = Instantiate(workerPrefab, transform);
		worker.transform.localPosition = spawnRelativeToBase;

		worker.AddMoveAction(worker.transform.position + RallyPoint);

		return worker;
	}

	public Leader CreateLeader()
	{
		Leader leader = Instantiate(leaderPrefab, transform);
		leader.transform.localPosition = spawnRelativeToBase;

		leader.AddMoveAction(leader.transform.position + RallyPoint);

		return leader;
	}

	public void StopProduction()
	{
		inProduction = false;
		productionTimeLeft = 0f;
	}

}
