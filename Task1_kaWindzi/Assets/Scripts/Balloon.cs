using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	private Rigidbody m_rigidBody;
	private Wind m_wind;

	private void Awake()
	{
		m_rigidBody = this.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if(m_wind != null)
		{
			m_rigidBody.AddForce(m_wind.v3Direction * m_wind.fStrength);
		}
	}

	private void OnTriggerEnter(Collider _col)
	{
		Wind wind = _col.GetComponent<Wind>();

		if(wind != null)
		{
			m_wind = wind;
		}
	}

	private void OnTriggerExit(Collider _col)
	{
		Wind wind = _col.GetComponent<Wind>();

		if (wind != null && wind == m_wind)
		{
			m_wind = null;
		}
	}
}
