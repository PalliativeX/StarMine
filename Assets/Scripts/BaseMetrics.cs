using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Prob rename
public class BaseMetrics
{
	static int width, length;

	static Vector3[] baseSpawns = 
	{
		new Vector3(10, 0f, 10),
		new Vector3(10, 0f, length - 10),
		new Vector3(width - 10, 0f, length - 10),
		new Vector3(width - 10, 0f, 10)
	};

	public static Vector3 GetFirstBaseSpawn()
	{
		return baseSpawns[0];
	}

	public static Vector3 GetFirstOppositeBaseSpawn()
	{
		return baseSpawns[2];
	}

	public static Vector3 GetSecondBaseSpawn()
	{
		return baseSpawns[1];
	}

	public static Vector3 GetSecondOppositeBaseSpawn()
	{
		return baseSpawns[3];
	}

	public static Vector3 ClosestPointOnSquare(Vector3 currentLoc, Vector3 sphereLoc, float radius)
	{
		radius -= 0.1f;

		Vector3 point = new Vector3();
		point.x = sphereLoc.x + radius * ((currentLoc.x - sphereLoc.x) / Mathf.Sqrt(Mathf.Pow(currentLoc.x - sphereLoc.x, 2) + Mathf.Pow(currentLoc.z - sphereLoc.z, 2) + Mathf.Pow(currentLoc.y - sphereLoc.y, 2)));
		point.y = sphereLoc.y + radius * ((currentLoc.y - sphereLoc.y) / Mathf.Sqrt(Mathf.Pow(currentLoc.x - sphereLoc.x, 2) + Mathf.Pow(currentLoc.z - sphereLoc.z, 2) + Mathf.Pow(currentLoc.y - sphereLoc.y, 2)));
		point.z = sphereLoc.z + radius * ((currentLoc.z - sphereLoc.z) / Mathf.Sqrt(Mathf.Pow(currentLoc.x - sphereLoc.x, 2) + Mathf.Pow(currentLoc.z - sphereLoc.z, 2) + Mathf.Pow(currentLoc.y - sphereLoc.y, 2)));


		return point;
	}

	public static Vector3 GetDest(Vector3 currentLoc, Vector3 sphereLoc, float radius)
	{
		if (Mathf.Abs(sphereLoc.y - currentLoc.y) > 1f)
		{
			return sphereLoc;
		}
		else
		{
			return ClosestPointOnSquare(currentLoc, sphereLoc, radius);
		}
	}

	public static int Width
	{
		set {
			width = value;
		}
	}

	public static int Length
	{
		set {
			length = value;
		}
	}
}
