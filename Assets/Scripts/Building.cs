using UnityEngine;

public class Building : Targetable
{
	public int capacity; // NOTE: Health
	
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void Destroy()
	{
		player.RemoveBuilding(this);
	}

}
