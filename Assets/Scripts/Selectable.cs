using UnityEngine;
using UnityEngine.UI;

public abstract class Selectable : MonoBehaviour
{
	public GameObject selectionCircle;
	public Image healthBar;

	protected bool chosen;
	protected Team team;

	public float maxHealth;
	public float health;

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

		health = maxHealth;
	}

	protected virtual void Start() { }

	protected virtual void Update()
	{
		//healthBar.TryGetComponent<SpriteRenderer>().
		healthBar.fillAmount = health / maxHealth;
	}

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
}
