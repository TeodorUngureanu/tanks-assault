using UnityEngine;
using System.Collections;

public static class Noise {

	public enum NormalizeMode {Local, Global};

	static float[,] noiseMapDiamondSquare;
	static int GRAIN = 25;
	static int widthplusheight = 482;

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode) {
		float[,] noiseMap = new float[mapWidth,mapHeight];

		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + offset.x;
			float offsetY = prng.Next (-100000, 100000) - offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= persistance;
		}

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;


		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				amplitude = 1;
				frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x-halfWidth + octaveOffsets[i].x) / scale * frequency;
					float sampleY = (y-halfHeight + octaveOffsets[i].y) / scale * frequency;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxLocalNoiseHeight) {
					maxLocalNoiseHeight = noiseHeight;
				} else if (noiseHeight < minLocalNoiseHeight) {
					minLocalNoiseHeight = noiseHeight;
				}
				noiseMap [x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				if (normalizeMode == NormalizeMode.Local) {
					noiseMap [x, y] = Mathf.InverseLerp (minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap [x, y]);
				} else {
					float normalizedHeight = (noiseMap [x, y] + 1) / (maxPossibleHeight/0.9f);
					noiseMap [x, y] = Mathf.Clamp(normalizedHeight,0, int.MaxValue);
				}
			}
		}

		return noiseMap;

		/*

		DIAMOND-SQUARE CALL
		
		noiseMapDiamondSquare = new float[mapWidth,mapHeight];
		drawPlasma (mapWidth, mapHeight);

		float maxNoiseH = float.MinValue;
		float minNoiseH = float.MaxValue;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				if (noiseMapDiamondSquare [x, y] > maxNoiseH) {
					maxNoiseH = noiseMapDiamondSquare [x, y];
				} else if (noiseMapDiamondSquare [x, y] < minNoiseH) {
					minNoiseH = noiseMapDiamondSquare [x, y];
				}
			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMapDiamondSquare [x, y] = Mathf.InverseLerp (minNoiseH, maxNoiseH, noiseMapDiamondSquare [x, y]);
			}
		}

		return noiseMapDiamondSquare;
		*/
	}

	static void drawPlasma(float width, float height) {
		float c1, c2, c3, c4;

		c1 = Random.value;
		c2 = Random.value;
		c3 = Random.value;
		c4 = Random.value;

		divideGrid(0.0f, 0.0f, width, height, c1, c2, c3, c4);
	}

	static float displace(float num) {
		float max = num / widthplusheight * GRAIN;
		return Random.Range (-0.5f, 0.5f) * max;
	}

	static void divideGrid(float x, float y, float w, float h, float c1, float c2, float c3, float c4) {

		float newWidth = w * 0.5f;
		float newHeight = h * 0.5f;

		if (w < 1.0f || h < 1.0f) {
			//The four corners of the grid piece will be averaged and drawn as a single pixel.
			float c = (c1 + c2 + c3 + c4) * 0.25f;
			noiseMapDiamondSquare[(int)x, (int)y] = c;
		}
		else
		{
			float middle =(c1 + c2 + c3 + c4) * 0.25f + displace(newWidth + newHeight);      //Randomly displace the midpoint!
			float edge1 = (c1 + c2) * 0.5f; //Calculate the edges by averaging the two corners of each edge.
			float edge2 = (c2 + c3) * 0.5f;
			float edge3 = (c3 + c4) * 0.5f;
			float edge4 = (c4 + c1) * 0.5f;

			//Make sure that the midpoint doesn't accidentally "randomly displaced" past the boundaries!
			if (middle <= 0) {
				middle = 0;
			} else if (middle > 1.0f) {
			}

			//Do the operation over again for each of the four new grids.                 
			divideGrid(x, y, newWidth, newHeight, c1, edge1, middle, edge4);
			divideGrid(x + newWidth, y, newWidth, newHeight, edge1, c2, edge2, middle);
			divideGrid(x + newWidth, y + newHeight, newWidth, newHeight, middle, edge2, c3, edge3);
			divideGrid(x, y + newHeight, newWidth, newHeight, edge4, middle, edge3, c4);
		}
	}

}