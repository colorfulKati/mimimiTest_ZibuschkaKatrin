using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDeformation : MimiBehaviour
{
	private Terrain m_Terrain;
	private TerrainData m_TerrainData;

	private float[,] m_arInitialHeightmap;
	private int m_iHeightmapResolution;
	private Vector3 m_v3TerrainSize;

	protected override void Awake()
	{
		base.Awake();
		m_Terrain = this.GetComponent<Terrain>();
		m_TerrainData = m_Terrain.terrainData;
		m_iHeightmapResolution = m_TerrainData.heightmapResolution;
		m_arInitialHeightmap = m_TerrainData.GetHeights(0, 0, m_iHeightmapResolution, m_iHeightmapResolution);
		m_v3TerrainSize = m_TerrainData.size;
	}

	private void OnDestroy()
	{
		if (m_TerrainData != null)
			m_TerrainData.SetHeights(0, 0, m_arInitialHeightmap);
	}

	private void OnCollisionEnter(Collision _Collision)
	{		
		float[,] arHeights = m_TerrainData.GetHeights(0, 0, m_iHeightmapResolution, m_iHeightmapResolution);
		
		Vector3 v3LocalPoint = v3GetCollisionPointInLocalSpace(_Collision);
		Debug.Log("Local contact point: " + v3LocalPoint);

		Vector2Int v2HeightmapSpace = v2GetPointInHeightmapSpace(v3LocalPoint);
		Debug.Log("Heightmap contact point: " + v2HeightmapSpace);

		arHeights[v2HeightmapSpace.x, v2HeightmapSpace.y] *= 0.5f;

		m_TerrainData.SetHeights(0, 0, arHeights);
	}

	private Vector3 v3GetCollisionPointInLocalSpace(Collision _Collision)
	{
		Vector3 v3World = _Collision.GetContact(0).point;
		Debug.Log("World contact point: " + v3World);

		return this.m_transThis.InverseTransformPoint(v3World);
	}

	private Vector2Int v2GetPointInHeightmapSpace(Vector3 _v3LocalPoint)
	{
		int iX = (int)(_v3LocalPoint.x * m_iHeightmapResolution / m_v3TerrainSize.x);
		int iY = (int)(_v3LocalPoint.z * m_iHeightmapResolution / m_v3TerrainSize.z);

		Vector2Int v2HeightmapSpace = new Vector2Int(iY, iX);
		return v2HeightmapSpace;
	}
}
