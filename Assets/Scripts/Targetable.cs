using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : MonoBehaviour
{
	protected bool chosen;
	protected Team team;

	Color defaultColor;

	protected Player player;

	private MeshRenderer ownMeshRenderer;

	protected virtual void Awake()
	{
		ownMeshRenderer = GetComponent<MeshRenderer>();

		defaultColor = ownMeshRenderer.material.color;
		chosen = false;

		player = GetComponentInParent<Player>();
		team = player.Team;

		if (team == Team.RED)
		{
			defaultColor += BaseMetrics.redTeamColorAdd;
		}
		else
		{
			defaultColor += BaseMetrics.blueTeamColorAdd;
		}
		ownMeshRenderer.material.color = defaultColor;
	}

	protected virtual void Start()
    {
        
    }

	protected virtual void Update()
    {
        
    }

	public bool Chosen
	{
		get { return chosen; }
		set {
			if (value)
			{
				ownMeshRenderer.material.color = Color.green;
			}
			else
			{
				ownMeshRenderer.material.color = defaultColor;
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
}
