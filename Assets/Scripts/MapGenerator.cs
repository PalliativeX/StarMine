using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapSize
{
	Small, Medium, Large
}

public class MapGenerator : MonoBehaviour
{
	public bool useFixedSeed;
	public int seed;

	[Range(0f, 1f)]
	public float diamondsPercentHigh = 0f;
	[Range(0f, 1f)]
	public float diamondsPercentLow = 0.2f;

	[Range(0f, 1f)]
	public float earthPercentHigh = 0.6f;
	[Range(0f, 1f)]
	public float earthPercentLow = 0f;

	public Voxel earthVoxelPrefab, stoneVoxelPrefab, diamondVoxelPrefab, lavaVoxelPrefab;

	public void GenerateMap(MapChunk[] chunks, Voxel[] voxels, int width, int length, int height)
	{
		Random.State originalRandomState = Random.state;
		Random.InitState(seed);

		int totalVoxelsNumber = width * length * height;
		int chunkSize = chunks[0].Size;
		int chunkDimension = chunks[0].DimensionSize;

		for (int y = 0; y < height; y++)
		{
			for (int z = 0; z < length; z++)
			{
				for (int x = 0; x < width; x++)
				{ 
					Voxel newVoxel;

					if (y == height - 1)
					{
						newVoxel = Instantiate(lavaVoxelPrefab);
					}
					else
					{
						float currentHeight = (float)y / (float)height;
						float diamondsPercent = Mathf.Lerp(diamondsPercentHigh, diamondsPercentLow, currentHeight);
						float earthPercent = Mathf.Lerp(earthPercentHigh, earthPercentLow, currentHeight);

						float random = Random.Range(0f, 1f);
						if (random < diamondsPercent)
						{
							newVoxel = Instantiate(diamondVoxelPrefab);
						}
						else if (random < earthPercent)
						{
							newVoxel = Instantiate(earthVoxelPrefab);
						}
						else
						{
							newVoxel = Instantiate(stoneVoxelPrefab);
						}
					}

					newVoxel.transform.localPosition = new Vector3(x, -y, z);
					newVoxel.transform.SetParent(transform, false);

					int currentChunk = (x + y * chunkDimension + z * chunkDimension * chunkDimension)/totalVoxelsNumber;
					chunks[currentChunk].Set(newVoxel, new Vector3(x/chunkDimension, y, z/chunkDimension));

					voxels[x + y * width + z * width * height] = newVoxel;
				}
			}
		}

		Random.state = originalRandomState;
	}

}
