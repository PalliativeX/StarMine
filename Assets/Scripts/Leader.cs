using UnityEngine;

public class Leader : Unit
{
	public static KeyCode speedupKey = KeyCode.S;

	SpeedupAbility speedupAbility;

	private void Start()
	{
		speedupAbility = new SpeedupAbility(5, 7, this);
	}

	protected override void Update()
	{
		base.Update();

		speedupAbility.UpdateState();

	}

	public void ActivateSpeedupAbility()
	{
		speedupAbility.Activate();
	}
}


// TODO: Think whether it shouldn't be a part of Leader class
class SpeedupAbility : Ability
{
	public float increasedSpeed;
	public float normalSpeed;

	public Leader abilityHolder;

	public SpeedupAbility(float duration, float cooldown, Leader abilityHolder) : base(duration, cooldown)
	{
		this.abilityHolder = abilityHolder;
		normalSpeed = abilityHolder.speed;
		increasedSpeed = normalSpeed + 2;
	}

	public override bool Activate()
	{
		if (!base.Activate())
		{
			return false;
		}

		abilityHolder.speed = increasedSpeed;

		return true;
	}

	public override void UpdateState()
	{
		base.UpdateState();

		if (durationLeft <= 0f)
		{
			abilityHolder.speed = normalSpeed;
		}
	}
}
