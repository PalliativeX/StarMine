using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunk
{
	int dimensionSize;
	Voxel[] voxels;

	public MapChunk(int dimensionSize = 8)
	{
		this.dimensionSize = dimensionSize;

		voxels = new Voxel[Size];
	}

	public Voxel Get(int x, int y, int z)
	{
		return voxels[x + y*dimensionSize + z*dimensionSize*dimensionSize];
	}

	public Voxel Get(Vector3 pos)
	{
		return Get((int)pos.x, (int)pos.y, (int)pos.z);
	}

	public void Set(Voxel voxel, int x, int y, int z)
	{
		voxels[x + y * dimensionSize + z * dimensionSize * dimensionSize] = voxel;
	}

	public void Set(Voxel voxel, Vector3 pos)
	{
		Set(voxel, (int)pos.x, (int)pos.y, (int)pos.z);
	}

	public void Destroy(Vector3 pos)
	{
		Voxel voxelToBeDestroyed = Get(pos);
		voxelToBeDestroyed.gameObject.SetActive(false);
	}

	public void Destroy(int x, int y, int z)
	{
		Voxel voxelToBeDestroyed = Get(x, y, z);
		voxelToBeDestroyed.gameObject.SetActive(false);
	}

	public int DimensionSize
	{
		get { return dimensionSize; }
	}

	public int Size
	{
		get { return dimensionSize*dimensionSize*dimensionSize; }
	}

}
