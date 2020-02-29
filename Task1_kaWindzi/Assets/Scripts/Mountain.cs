using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : MonoBehaviour
{
	[SerializeField]
	private float m_fMaxCollisionSpeed;

	public float fMaxCollisionSpeed
	{
		get { return m_fMaxCollisionSpeed; }
		private set { m_fMaxCollisionSpeed = value; }
	}
}