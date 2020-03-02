using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindLine : MimiBehaviour
{
	[SerializeField]
	public Vector3 m_v3Direction;

	[SerializeField]
	public float m_fStrength;

	protected override void Awake()
	{
		base.Awake();

		LineRenderer lineRenderer = this.GetComponent<LineRenderer>();
		MeshCollider meshCollider = this.GetComponent<MeshCollider>();

		Mesh mesh = new Mesh();
		lineRenderer.BakeMesh(mesh, true);
		meshCollider.sharedMesh = mesh;
	}
}
