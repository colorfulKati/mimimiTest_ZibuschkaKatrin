using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDeformation : MimiBehaviour
{
	[SerializeField]
	private bool m_bDebugLog = false;
	[SerializeField]
	private TerrainDeformationData m_DeformationData;

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
		// Calcucalte collision point
		ContactPoint contactPoint = _Collision.GetContact(0);
		Vector3 v3WorldPoint = contactPoint.point;
		Vector3 v3LocalPoint = v3GetCollisionPointInLocalSpace(v3WorldPoint);
		Vector2Int v2HeightmapPoint = v2GetPointInHeightmapSpace(v3LocalPoint);

		if (m_bDebugLog)
		{
			Debug.Log("World contact point: " + v3WorldPoint);
			Debug.Log("Local contact point: " + v3LocalPoint);
			Debug.Log("Heightmap contact point: " + v2HeightmapPoint);
		}

		// Get current heightmap
		float[,] arHeights = m_TerrainData.GetHeights(0, 0, m_iHeightmapResolution, m_iHeightmapResolution);

		// Calculate deformation depending on mode
		if (m_DeformationData.eDeformationMode == DeformationMode.Point)
		{
			deformHeightsPointMode(arHeights, v2HeightmapPoint);
		}
		else if (m_DeformationData.eDeformationMode == DeformationMode.Rect)
		{
			Vector2Int v2ColliderDimension = v2GetPointInHeightmapSpace(contactPoint.otherCollider.bounds.size);
			deformHeightsRectMode(arHeights, v2HeightmapPoint, v2ColliderDimension);
		}
		else if(m_DeformationData.eDeformationMode == DeformationMode.HalfRect)
		{
			Vector2Int v2ColliderDimension = v2GetPointInHeightmapSpace(contactPoint.otherCollider.bounds.size) / 2;
			deformHeightsRectMode(arHeights, v2HeightmapPoint, v2ColliderDimension);
		}
	
		// Apply deformation
		m_TerrainData.SetHeights(0, 0, arHeights);
	}

	private Vector3 v3GetCollisionPointInLocalSpace(Vector3 _v3WorldPoint)
	{
		return this.m_transThis.InverseTransformPoint(_v3WorldPoint);
	}

	private Vector2Int v2GetPointInHeightmapSpace(Vector3 _v3LocalPoint)
	{
		int iX = (int)(_v3LocalPoint.x * m_iHeightmapResolution / m_v3TerrainSize.x);
		int iY = (int)(_v3LocalPoint.z * m_iHeightmapResolution / m_v3TerrainSize.z);

		Vector2Int v2HeightmapSpace = new Vector2Int(iY, iX);
		return v2HeightmapSpace;
	}

	private void deformHeightsPointMode(float[,] _arHeights, Vector2Int _v2HeightmapPoint)
	{
		_arHeights[_v2HeightmapPoint.x, _v2HeightmapPoint.y] -= m_DeformationData.fIndentDepth;
	}

	private void deformHeightsRectMode(float[,] _arHeights, Vector2Int _v2HeightmapPoint, Vector2Int _v2ColliderDimension)
	{
		int iStartX = Mathf.Max(_v2HeightmapPoint.x - _v2ColliderDimension.x / 2, 0);
		int iStartY = Mathf.Max(_v2HeightmapPoint.y - _v2ColliderDimension.y / 2, 0);
		int iEndX = Mathf.Min(_v2HeightmapPoint.x + _v2ColliderDimension.x / 2, m_iHeightmapResolution);
		int iEndY = Mathf.Min(_v2HeightmapPoint.y + _v2ColliderDimension.y / 2, m_iHeightmapResolution);

		float fHeight;
		for (int iX = iStartX; iX < iEndX; iX++)
		{
			for (int iY = iStartY; iY < iEndY; iY++)
			{
				fHeight = _arHeights[iX, iY];
				fHeight -= m_DeformationData.fIndentDepth;
				_arHeights[iX, iY] = Mathf.Max(fHeight, 0f);
			}
		}
	}

	public enum DeformationMode
	{
		Point,
		Rect,
		HalfRect
	}
}
