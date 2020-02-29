using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MimiBehaviour
{
	[SerializeField]
	private int m_iInitialLives;
	private int m_iCurrentLives;
	public int iCurrentLives
	{
		get { return m_iCurrentLives; }
		private set
		{
			m_iCurrentLives = value;
			OnLivesChanged?.Invoke();
		}
	}

	private int m_iCurrentStars = 0;
	public int iCurrentStars
	{
		get { return m_iCurrentStars; }
		private set
		{
			m_iCurrentStars = value;
			OnStarsChanged?.Invoke();
		}
	}


	[SerializeField]
	private float m_fDamageCooldown;
	private Coroutine m_runningDamageCooldown;

	private Rigidbody m_rigidBody;
	private Wind m_wind;
	private WindLine m_windLine;

	private Vector3 m_v3InitialPosition;

	/// <summary>
	/// Fired, when the balloon's number of lives changed.
	/// </summary>
	public event Action OnLivesChanged;

	/// <summary>
	/// Fired, when the number of collected stars changed.
	/// </summary>
	public event Action OnStarsChanged;
	
	/// <summary>
	/// Fired, when the balloon got reset.
	/// </summary>
	public static event Action OnReset;

	protected override void Awake()
	{
		base.Awake();
		m_rigidBody = this.GetComponent<Rigidbody>();
		m_iCurrentLives = m_iInitialLives;
		m_v3InitialPosition = m_transThis.position;
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
		Goal goal = _col.GetComponent<Goal>();
		if (goal != null)
		{
			reachGoal();
			return;
		}

		Wind wind = _col.GetComponent<Wind>();
		if(wind != null)
		{
			m_wind = wind;
			return;
		}

		Star star = _col.GetComponent<Star>();
		if(star != null)
		{
			star.collect();
			iCurrentStars++;
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
		if (mountain != null && collision.relativeVelocity.magnitude > mountain.fSpeedThresholdForDamage)
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

	private void reset()
	{
		m_transThis.position = m_v3InitialPosition;
		m_rigidBody.velocity = Vector3.zero;
		iCurrentLives = m_iInitialLives;
		iCurrentStars = 0;

		OnReset?.Invoke();
	}

	private void reachGoal()
	{
		Debug.Log("Balloon reached the goal.");
		reset();
	}

	private void decreaseLives()
	{
		if (m_runningDamageCooldown != null)
			return;

		int iPrevLives = m_iCurrentLives;
		iCurrentLives--;

		Debug.Log("Balloon lost a live. Remaining: " + iCurrentLives);

		if (iCurrentLives <= 0)
			reset();
		else
			m_runningDamageCooldown = this.StartCoroutine(damageCooldown());
	}

	private IEnumerator damageCooldown()
	{
		yield return new WaitForSeconds(m_fDamageCooldown);

		m_runningDamageCooldown = null;
		Debug.Log("Damage cooldown over.");
	}
}
