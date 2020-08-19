using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UnitType
{
	Worker, Scout, Leader, Warrior
}

class MoveAction : Action
{
	public Vector3 destination;

	public MoveAction(Vector3 destination)
	{
		this.destination = destination;
	}
}

class FollowAction : Action
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
	public Image manaBar;

	public float initialMana;
	float mana;
	public float speed;
	public UnitType type;

	public Transform canvasTransform;

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
		mana = initialMana;
	}

	protected override void Update()
	{
		base.Update();

		if (currentAction == null && actions.Count > 0)
		{
			currentAction = actions.Dequeue();
		}

		// TODO: Fix this to be called only once!!!
		if (currentAction != null && !currentAction.started)
		{
			if (currentAction is MoveAction)
			{
				currentAction.started = true;
				destination = ((MoveAction)currentAction).destination;

				StartCoroutine(Turn(destination));
			}
			else if (currentAction is FollowAction)
			{
				currentAction.started = true;
				friendlyTarget = ((FollowAction)currentAction).target;
			}
			else if (currentAction is AttackAction)
			{
				currentAction.started = true;
				Transform enemy = ((AttackAction)currentAction).target;
				Vector3 targetPos = BaseMetrics.GetDest(transform.position, enemy.position, attackRange);
				Vector3 enemyPos = enemy.position;
				// TODO: Add Y coord handling
				if (Mathf.Abs(transform.position.x - enemyPos.x) > attackRange ||
					Mathf.Abs(transform.position.z - enemyPos.z) > attackRange)
				{
					currentAction = null;
					AddMoveAction(targetPos);
					targetPos += (targetPos - transform.position).normalized * 0.1f;
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

	IEnumerator Turn(Vector3 point)
	{
		point.y = transform.localPosition.y;
		Quaternion fromRotation = transform.localRotation;
		Quaternion toRotation = Quaternion.LookRotation(point - transform.localPosition);

		float rotationSpeed = 250f;
		float angle = Quaternion.Angle(fromRotation, toRotation);

		if (angle > 0f)
		{
			float speed = rotationSpeed / angle;

			for (float t = Time.deltaTime * speed; t < 1f; t += Time.deltaTime * speed)
			{
				transform.localRotation = Quaternion.Slerp(fromRotation, toRotation, t);
				// NOTE: We have to rotate our Canvas in the opposite direction to the object, so that it doesn't actually rotate
				canvasTransform.localRotation = Quaternion.Euler(55, -transform.localRotation.eulerAngles.y, 0);

				yield return null;
			}

		}
	}

	void Move()
	{
		if (currentAction is MoveAction && destination != Vector3.zero)
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
			target.Health -= damage;
			currentAttackCooldown = attackCooldown;
			currentAction = null;
			if (target.Health <= 0)
			{
				target.Remove();
				target = null;
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
		destination = Vector3.zero;
	}

	public void Die()
	{
		player.RemoveUnit(this);
	}

	public Vector3 Destination
	{
		get { return destination; }
		set { destination = value; }
	}

	public float Mana
	{
		get { return mana; }
		set {
			mana = value;
			manaBar.fillAmount = mana / initialMana;
		}
	}

}
