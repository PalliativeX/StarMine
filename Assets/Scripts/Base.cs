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

	private void Start()
	{
		
	}

	private void Update()
	{
	}

	public Worker CreateWorker()
	{
		Worker worker = Instantiate(workerPrefab, transform);
		worker.transform.localPosition = new Vector3(2, 0.5f, 2);

		return worker;
	}

	public Leader CreateLeader()
	{
		Leader leader = Instantiate(leaderPrefab, transform);
		leader.transform.localPosition = new Vector3(2, 0.5f, 2);

		return leader;
	}

	public void StopProduction()
	{
		inProduction = false;
		productionTimeLeft = 0f;
	}


}
