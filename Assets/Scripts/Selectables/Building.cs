using UnityEngine;

public class Building : Selectable
{

	protected override void Update()
	{
		base.Update();
	}

	public void Destroy()
	{
		player.RemoveBuilding(this);
	}

}
