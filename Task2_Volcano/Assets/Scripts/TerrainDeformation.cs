using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDeformation : MonoBehaviour
{
	private Terrain m_Terrain;
	private TerrainData m_TerrainData;

	private float[,] m_arInitialHeightmap;
	private int m_iHeightmapResolution;

	private void Awake()
	{
		m_Terrain = this.GetComponent<Terrain>();
		m_TerrainData = m_Terrain.terrainData;
		m_iHeightmapResolution = m_TerrainData.heightmapResolution;
		m_arInitialHeightmap = m_TerrainData.GetHeights(0, 0, m_iHeightmapResolution, m_iHeightmapResolution);
	}

	private void OnDestroy()
	{
		if (m_TerrainData != null)
			m_TerrainData.SetHeights(0, 0, m_arInitialHeightmap);
	}

	private void OnCollisionEnter(Collision _collision)
	{
		Debug.Log("Something collided with Terrain.");
		
		float[,] arHeights = m_TerrainData.GetHeights(0, 0, m_iHeightmapResolution, m_iHeightmapResolution);

		arHeights[m_iHeightmapResolution / 2 + 10, m_iHeightmapResolution / 2 + 10] *= 0.5f;

		m_TerrainData.SetHeights(0, 0, arHeights);
	}
}
