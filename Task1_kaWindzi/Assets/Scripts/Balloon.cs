using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	[SerializeField]
	private int m_iInitialLives;
	private int m_iCurrentLives;

	[SerializeField]
	private float m_fDamageCooldown;
	private Coroutine m_runningDamageCooldown;

	private Rigidbody m_rigidBody;
	private Wind m_wind;
	private WindLine m_windLine;

	/// <summary>
	/// Fired, when the balloon's number of lives changes.
	/// Parameters: previous number of lives, current number of lives
	/// </summary>
	public event Action<int, int> OnLivesChanged;

	private void Awake()
	{
		m_rigidBody = this.GetComponent<Rigidbody>();
		m_iCurrentLives = m_iInitialLives;
	}

	private void FixedUpdate()
	{
		if(m_wind != null)
		{
			m_rigidBody.AddForce(m_wind.v3Direction * m_wind.fStrength);
		}

		//if (m_windLine != null)
		//{
		//	m_rigidBody.AddForce(m_windLine.m_v3Direction * m_windLine.m_fStrength);
		//}
	}

	private void OnTriggerEnter(Collider _col)
	{
		Wind wind = _col.GetComponent<Wind>();
		if(wind != null)
		{
			m_wind = wind;
		}

		//WindLine windLine = _col.GetComponent<WindLine>();
		//if (windLine != null)
		//{
		//	m_windLine = windLine;
		//}
	}

	private void OnTriggerExit(Collider _col)
	{
		Wind wind = _col.GetComponent<Wind>();
		if (wind != null && wind == m_wind)
		{
			m_wind = null;
		}

		//WindLine windLine = _col.GetComponent<WindLine>();
		//if (windLine != null && windLine == m_windLine)
		//{
		//	m_windLine = null;
		//}
	}

	private void OnCollisionEnter(Collision collision)
	{
		Mountain mountain = collision.gameObject.GetComponent<Mountain>();
		if (mountain != null && collision.relativeVelocity.magnitude > mountain.fMaxCollisionSpeed)
		{
			decreaseLives();
			return;
		}
		
		Spike spike = collision.gameObject.GetComponent<Spike>();
		if (spike != null)
		{
			decreaseLives();
		}
	}

	private void decreaseLives()
	{
		if (m_runningDamageCooldown != null)
			return;

		int iPrevLives = m_iCurrentLives;
		m_iCurrentLives--;

		OnLivesChanged?.Invoke(iPrevLives, m_iCurrentLives);
		Debug.Log("Balloon lost a live. Remaining: " + m_iCurrentLives);

		m_runningDamageCooldown = this.StartCoroutine(damageCooldown());
	}

	private IEnumerator damageCooldown()
	{
		yield return new WaitForSeconds(m_fDamageCooldown);

		m_runningDamageCooldown = null;
		Debug.Log("Damage cooldown over.");
	}
}
