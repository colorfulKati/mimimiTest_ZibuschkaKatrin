using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TerrainDeformationData", menuName = "ScriptableObjects/TerrainDeformationData", order = 1)]
public class TerrainDeformationData : ScriptableObject
{
	[SerializeField]
	private TerrainDeformation.DeformationMode m_eDeformationMode;
	public TerrainDeformation.DeformationMode eDeformationMode
	{
		get { return m_eDeformationMode; }
		private set { m_eDeformationMode = value; }
	}

	[SerializeField]
	private float m_fIndentDepth;
	public float fIndentDepth
	{
		get { return m_fIndentDepth; }
		private set { m_fIndentDepth = value; }
	}
}
