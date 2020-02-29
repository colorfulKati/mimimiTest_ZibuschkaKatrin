using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MimiBehaviour
{
	public float fStrength
	{
		get;
		private set;
	}

	public Vector3 v3Direction
	{
		get;
		private set;
	}

	public void Initialize(Vector3 _v3Position, float _fStrength, Vector3 _v3Direction)
	{
		this.m_transThis.position = _v3Position;
		fStrength = _fStrength;
		v3Direction = _v3Direction;
	}
}
