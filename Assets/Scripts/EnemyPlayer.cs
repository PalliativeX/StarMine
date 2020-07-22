using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : Player
{

	protected override void Start()
	{
		base.Start();

		team = Team.BLUE;

		// NOTE: Adding a Worker and a Base
		Worker worker = Instantiate(workerPrefab, transform);
		//worker.transform.localPosition = new Vector3(map.Width - 3, 0.5f, map.Length - 3);
		worker.transform.localPosition = new Vector3(6, 0.5f, 6);
		AddUnit(worker);

		Building baseBuilding = Instantiate(basePrefab, transform);
		baseBuilding.transform.localPosition = BaseMetrics.GetFirstOppositeBaseSpawn();
		AddBuilding(baseBuilding);

	}

	protected override void Update()
	{
		base.Update();
	}
}
