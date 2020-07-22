using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
	Worker, Scout, Leader  // TODO: To be continued
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
public class Unit : MonoBehaviour
{
	public float speed;
	public UnitType type;

	protected Player player;

	public float health;
	Transform target;
	public int damage;
	public float attackRange;
	public float attackCooldown = 0.5f;

	float currentAttackCooldown;

	protected Team team;

	protected Queue<Action> actions;
	protected Action currentAction;

	protected Vector3 destination;

	private bool chosen;

	Color defaultColor;

	private void Awake()
	{
		defaultColor = GetComponent<Renderer>().material.color;
		chosen = false;
		destination = Vector3.zero;
		actions = new Queue<Action>();
		if (team == Team.RED)
		{
			player = GameObject.FindGameObjectsWithTag("HumanPlayer")[0].GetComponent<Player>();
		}
		else
		{
			player = GameObject.FindGameObjectsWithTag("EnemyPlayer")[0].GetComponent<Player>();
		}
	}

	protected virtual void Update()
	{
		if (currentAction == null && actions.Count > 0)
		{
			currentAction = actions.Dequeue();
			if (currentAction is MoveAction)
			{
				destination = ((MoveAction)currentAction).destination;
			}
			else if (currentAction is AttackAction)
			{
				// TODO: Use attack range
				Transform enemy = ((AttackAction)currentAction).target;
				Vector3 targetPos = BaseMetrics.GetDest(this.transform.position, enemy.position, attackRange);
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

		HandleMovement();
	}

	void HandleMovement()
	{
		// REWRITE THIS SHIT
		if (destination != Vector3.zero)
		{
			Vector3 direction = (destination - transform.position).normalized;

			transform.position += direction * speed * Time.deltaTime;
			if (Mathf.Abs(transform.position.x - destination.x) <= 0.05f || Mathf.Abs(transform.position.z - destination.z) <= 0.05f)
			{
				transform.position.Set(destination.x, transform.position.y, destination.z);
				destination = Vector3.zero;
				currentAction = null;
			}

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
		//this.destination = destination;
	}

	public void AddAttackAction(Transform target)
	{
		actions.Enqueue(new AttackAction(target));
		//this.target = target;
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

	public Team Team
	{
		get { return team; }
		set {
			team = value;
		}
	}

	public Vector3 Destination
	{
		get { return destination;  }
		set { destination = value; }
	}

	public bool Chosen
	{
		get { return chosen; }
		set {
			if (value)
			{
				GetComponent<MeshRenderer>().material.color = Color.green;
			}
			else
			{
				GetComponent<MeshRenderer>().material.color = defaultColor;
			}

			chosen = value;
		}
	}

}
