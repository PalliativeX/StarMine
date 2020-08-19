using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class Selectable : MonoBehaviour
{
	public GameObject selectionCircle;
	public Image healthBar;
	public float maxHealth;

	protected bool chosen;
	protected Team team;
	protected float health;
	protected Player player;

	protected virtual void Awake()
	{
		bool hasRenderer = false;
		if (TryGetComponent(out MeshRenderer ownRenderer))
		{
			hasRenderer = true;
		}
		MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

		chosen = false;

		player = GetComponentInParent<Player>();
		team = player.Team;

		if (team == Team.RED)
		{
			if (hasRenderer) ownRenderer.material.color += BaseMetrics.redTeamColorAdd;
			foreach (MeshRenderer renderer in childRenderers)
			{
				renderer.material.color += BaseMetrics.redTeamColorAdd;
			}
		}
		else
		{
			if (hasRenderer) ownRenderer.material.color += BaseMetrics.blueTeamColorAdd;
			foreach (MeshRenderer renderer in childRenderers)
			{
				renderer.material.color += BaseMetrics.blueTeamColorAdd;
			}
		}

		health = maxHealth;
	}

	protected virtual void Start() { }

	protected virtual void Update() { }

	public void Remove()
	{
		player.Remove(this);
	}

	public bool Chosen
	{
		get { return chosen; }
		set {
			if (value)
			{
				selectionCircle.SetActive(true);
			}
			else
			{
				selectionCircle.SetActive(false);
			}

			chosen = value;
		}
	}

	public Team Team
	{
		get { return team; }
		set {
			team = value;
		}
	}

	public float Health
	{
		get { return health; }
		set {
			health = value;
			healthBar.fillAmount = health / maxHealth;
		}
	}
}
