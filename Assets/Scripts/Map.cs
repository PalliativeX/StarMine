using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public enum MapType
{
	Small, Medium, Large
}

public class Map : MonoBehaviour
{
	public MapGenerator mapGenerator;
	public MapType mapType;
	int width, length, height;

	int chunkSize = 512;
	MapChunk[] chunks;

	Voxel[] voxels;

	[SerializeField]
	LayerMask terrainMask, unitMask, buildingMask;

	private void Awake()
	{
		GenerateMap();
		//mapGenerator.GenerateSetting(width, length);//, height);
	}

	public void GenerateMap()
	{
		switch (mapType)
		{
			case MapType.Small:
				width = 32; length = 32; height = 8;
				break;
			case MapType.Medium:
				width = 64; length = 64; height = 8;
				break;
			case MapType.Large:
				width = 128; length = 128; height = 8;
				break;
		}

		chunkSize = 512;

		int chunksCount = (width * length * height) / chunkSize;
		chunks = new MapChunk[chunksCount];
		for (int i = 0; i < chunks.Length; i++)
		{
			chunks[i] = new MapChunk();
		}

		voxels = new Voxel[width * length * height];

		mapGenerator.GenerateMap(chunks, voxels, width, length, height);

		BaseMetrics.Width = width;
		BaseMetrics.Length = length;
		BaseMetrics.terrainMask = terrainMask;
		BaseMetrics.unitMask = unitMask;
		BaseMetrics.buildingMask = buildingMask;
	}

	// TODO: Only update when a Voxel has been destroyed
	private void Update()
	{
		int totalVoxelsNumber = width * length * height;
		int chunkSize = chunks[0].Size;
		int chunkDimension = chunks[0].DimensionSize;

		for (int z = 0; z < length; z++)
		{
			for (int x = 0; x < width; x++)
			{
				bool occluderFound = false;

				for (int y = 0; y < height; y++)
				{
					Voxel currentVoxel = voxels[x + y * width + z * width * height];

					if (occluderFound && !currentVoxel.hasNeighborMined)
					{
						currentVoxel.gameObject.SetActive(false);
					}
					else if (!currentVoxel.mined)
					{
						occluderFound = true;
						currentVoxel.gameObject.SetActive(true);
					}
					else if (currentVoxel.mined)
					{
						currentVoxel.gameObject.SetActive(false);

						// FIXME: A temp solution
						Voxel[] neighborVoxels =
						{
							voxels[(x-1) + y * width + z* width * height],
							voxels[x + y * width + (z+1) * width * height],
							voxels[(x+1) + y * width + z * width * height],
							voxels[x + y * width + (z-1) * width * height]
						};

						foreach (Voxel neighbor in neighborVoxels)
						{
							neighbor.gameObject.SetActive(true);
							neighbor.hasNeighborMined = true;
						}
					}
				}
			}
		}
	}

	public int Width { get { return width; } }
	public int Length { get { return length; } }
	public int Height { get { return height; } }

}
