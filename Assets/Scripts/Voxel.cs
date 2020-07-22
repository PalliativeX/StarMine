using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
	Earth, Stone, Diamond, Lava
}

public class Voxel : MonoBehaviour
{
	public ResourceType type;
	public int amount;
	public bool mined;
}
