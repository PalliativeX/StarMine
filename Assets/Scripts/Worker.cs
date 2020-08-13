using UnityEngine;

class MineAction : Action
{
	public Voxel targetVoxel;

	public MineAction(Voxel targetVoxel)
	{
		this.targetVoxel = targetVoxel;
	}
}

public class Worker : Unit
{
	public float miningRange = 2f;

	public static KeyCode miningKey = KeyCode.G;

	// NOTE: Mining time may be changed with better gear
	float earthMiningTime   = 1.2f;
	float stoneMiningTime   = 2.3f;
	float diamondMiningTime = 3.5f;

	Voxel targetVoxel;

	float miningTimeLeft = 0f;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		if (currentAction is MineAction && targetVoxel == null)
		{
			Voxel targetVoxel = ((MineAction)currentAction).targetVoxel;

			if (Vector3.Distance(targetVoxel.transform.position, transform.position) < miningRange)
			{
				SetTargetVoxel(targetVoxel);
			}
			else
			{
				currentAction = null;
				Vector3 targetPoint = BaseMetrics.GetDest(this.transform.position, targetVoxel.transform.position, miningRange);
				//targetPoint += (targetVoxel.transform.position - this.transform.position).normalized * 0.08f;
				AddMoveAction(targetPoint);
				AddMineAction(targetVoxel);
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

	public void AddMineAction(Voxel targetVoxel)
	{
		actions.Enqueue(new MineAction(targetVoxel));
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

}
