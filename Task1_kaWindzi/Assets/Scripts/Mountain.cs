using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : MonoBehaviour
{
	[SerializeField]
	private float m_fSpeedThresholdForDamage;

	public float fSpeedThresholdForDamage
	{
		get { return m_fSpeedThresholdForDamage; }
		private set { m_fSpeedThresholdForDamage = value; }
	}
}