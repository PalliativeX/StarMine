using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : Player
{

	protected override void Start()
	{
		base.Start();

		team = Team.BLUE;

		AddBaseAndWorker();
	}

	protected override void Update()
	{
		base.Update();
	}
}
