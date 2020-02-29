using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MimiBehaviour
{
	[SerializeField]
	private float m_fStrength;
	[SerializeField]
	private Vector3 m_v3Direction;

	public float fStrength
	{
		get { return m_fStrength; }
		private set { m_fStrength = value; }
	}

	public Vector3 v3Direction
	{
		get { return m_v3Direction; }
		private set { m_v3Direction = value; }
	}

	public void Initialize(Vector3 _v3Position, float _fStrength, Vector3 _v3Direction)
	{
		this.m_transThis.position = _v3Position;
		fStrength = _fStrength;
		v3Direction = _v3Direction;
	}
}
