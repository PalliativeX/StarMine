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

class AttackAction : Action
{
	public Transform target;

	public AttackAction(Transform target)
	{
		this.target = target;
	}
}

// TODO: Probably make abstract
public abstract class Unit : Targetable
{
	public float speed;
	public UnitType type;

	public float health;
	Transform target;
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

		TryMove();
	}

	void TryMove()
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

		enemy.gameObject.TryGetComponent(out Unit enemyUnit);
		if (enemyUnit != null)
		{
			enemyUnit.health -= this.damage;
			currentAttackCooldown = attackCooldown;
			currentAction = null;
			if (enemyUnit.health <= 0)
			{
				enemyUnit.Die();
			}
			else
			{
				AddAttackAction(enemy);
			}
		}
		else
		{
			enemy.gameObject.TryGetComponent(out Building enemyBuilding);
			if (enemyBuilding != null)
			{
				enemyBuilding.capacity -= this.damage;
				currentAttackCooldown = attackCooldown;
				currentAction = null;
				if (enemyBuilding.capacity <= 0)
				{
					enemyBuilding.Destroy();
				}
				else
				{
					AddAttackAction(enemy);
				}
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
