using UnityEngine;

public abstract class Ability
{
	public bool active;

	public float duration;
	public float durationLeft;

	public float cooldown;
	public float cooldownLeft;

	public Ability(float duration, float cooldown)
	{
		this.duration = duration;
		durationLeft = duration;
		this.cooldown = cooldown;
		cooldownLeft = 0;
	}

	public virtual bool Activate()
	{
		if (cooldownLeft > 0 || active)
		{
			return false;
		}

		active = true;
		durationLeft = duration;

		return true;
	}

	public virtual void UpdateState()
	{
		durationLeft -= Time.deltaTime;
		if (durationLeft <= 0f && active)
		{
			active = false;
			cooldownLeft = cooldown;
		}

		cooldownLeft -= Time.deltaTime;
	}
}
