using UnityEngine;

public class Building : MonoBehaviour
{
	public int capacity; // NOTE: Health

	protected Player player;
	
	private bool chosen;

	Color defaultColor;

	protected Team team;

	private void Awake()
	{
		chosen = false;
		defaultColor = GetComponent<MeshRenderer>().material.color;

		if (team == Team.RED)
		{
			player = GameObject.FindGameObjectsWithTag("HumanPlayer")[0].GetComponent<Player>();
		}
		else
		{
			player = GameObject.FindGameObjectsWithTag("EnemyPlayer")[0].GetComponent<Player>();
		}
	}

	public void Destroy()
	{
		player.RemoveBuilding(this);
	}

	public Team Team
	{
		get { return team; }
		set {
			team = value;
		}
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
