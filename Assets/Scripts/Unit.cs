using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
	Worker, Scout, Leader
}

class MoveAction : Action
{
	public Vector3 destination;

	public MoveAction(Vector3 destination)
	{
		this.destination = destination;
	}
}

class FollowAction: Action
{
	public Transform target;

	public FollowAction(Transform target)
	{
		this.target = target;
	}
}

class AttackAction : Action
{
	public Transform target;

	public AttackAction(Transform target)
	{
		this.target = target;
	}
}

public abstract class Unit : Selectable
{
	public float speed;
	public UnitType type;

	Transform target;

	Transform friendlyTarget; // NOTE: A friendly unit to follow
	public int damage;
	public float attackRange;
	public float attackCooldown = 0.5f;

	float currentAttackCooldown;

	protected Queue<Action> actions;
	protected Action currentAction;

	protected Vector3 destination;

	protected override void Awake()
	{
		base.Awake();
		
		destination = Vector3.zero;
		actions = new Queue<Action>();
	}

	protected override void Update()
	{
		base.Update();

		if (currentAction == null && actions.Count > 0)
		{
			currentAction = actions.Dequeue();
			if (currentAction is MoveAction)
			{
				destination = ((MoveAction)currentAction).destination;
			}
			else if (currentAction is FollowAction)
			{
				friendlyTarget = ((FollowAction)currentAction).target;
			}
			else if (currentAction is AttackAction)
			{
				Transform enemy = ((AttackAction)currentAction).target;
				Vector3 targetPos = BaseMetrics.GetDest(transform.position, enemy.position, attackRange);
				Vector3 enemyPos = enemy.position;
				// TODO: Add Y coord handling
				if (Mathf.Abs(transform.position.x - enemyPos.x) > attackRange ||
					Mathf.Abs(transform.position.z - enemyPos.z) > attackRange)
				{
					currentAction = null;
					AddMoveAction(targetPos);
					AddAttackAction(enemy);
				}
				else
				{
					Attack(enemy);
				}
			}
		}

		currentAttackCooldown -= Time.deltaTime;

		if (friendlyTarget != null)
		{

		}

		Move();
	}

	void Move()
	{
		if (destination != Vector3.zero)
		{
			if (Mathf.Abs(transform.position.x - destination.x) <= 0.02f || Mathf.Abs(transform.position.z - destination.z) <= 0.02f)
			{
				destination = Vector3.zero;
				currentAction = null;
				return;
			}
			transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
		}
	}

	// Make it a coroutine
	void Attack(Transform enemy)
	{
		if (currentAttackCooldown > 0f)
		{
			currentAction = null;
			AddAttackAction(enemy);
			return;
		}

		enemy.gameObject.TryGetComponent(out Selectable target);
		if (target != null)
		{
			target.health -= damage;
			currentAttackCooldown = attackCooldown;
			currentAction = null;
			if (target.health <= 0)
			{
				target.Remove();
			}
			else
			{
				AddAttackAction(enemy);
			}
		}
	}


	public void AddMoveAction(Vector3 destination)
	{
		actions.Enqueue(new MoveAction(destination));
	}

	public void AddAttackAction(Transform target)
	{
		actions.Enqueue(new AttackAction(target));
	}

	public void RemoveAllActions()
	{
		actions.Clear();
		currentAction = null;
	}

	public void Die()
	{
		player.RemoveUnit(this);
	}

	public Vector3 Destination
	{
		get { return destination;  }
		set { destination = value; }
	}

}
