using UnityEngine;

public class Building : Selectable
{
	
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
