﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MimiBehaviour
{
	private const float c_fWindPosY = -0.5f;

	[SerializeField]
	private Wind m_WindPrefab;

	[SerializeField]
	private bool m_bDestroyWind;
	[SerializeField]
	private float m_fDestroyTime;

	[SerializeField]
	private float m_fStrengthMultiplier;
	[SerializeField]
	private float m_fMaxStrength;

	private Camera m_Camera;

	private Vector3 m_v3PositionMouseDown = Vector3.zero;
	private Vector3 m_v3PositionMouseUp = Vector3.zero;

	private DateTime m_DateTimeDown;
	private DateTime m_DateTimeUp;

	private List<Coroutine> m_DestroyCoroutines = new List<Coroutine>();

	protected override void Awake()
	{
		base.Awake();
		m_Camera = Camera.main;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_v3PositionMouseDown = Input.mousePosition;
			m_DateTimeDown = DateTime.Now;
		}

		if (Input.GetMouseButtonUp(0))
		{
			m_v3PositionMouseUp = Input.mousePosition;
			m_DateTimeUp = DateTime.Now;
			createWind();
		}
	}

	private void createWind()
	{
		Vector3 v3Direction = v3GetWindDirection();
		float fStrength = fGetWindStrength(v3Direction.magnitude);
		Vector3 v3Position = v3GetWindPosition(v3Direction);

		Wind windInstance = Instantiate(m_WindPrefab, this.m_transThis);

		windInstance.Initialize(v3Position, fStrength, v3Direction);

		if(m_bDestroyWind)
			m_DestroyCoroutines.Add(this.StartCoroutine(destroyCooldown(windInstance.gameObject)));
	}

	private Vector3 v3GetWindPosition(Vector3 _v3Direction)
	{
		Vector3 v3Position = m_Camera.ScreenToWorldPoint(m_v3PositionMouseDown);
		v3Position.y = c_fWindPosY;
		v3Position += _v3Direction / 2;
		return v3Position;
	}

	private float fGetWindStrength(float _fDistance)
	{
		TimeSpan timeDiff = m_DateTimeUp - m_DateTimeDown;

		return Mathf.Min((float)(_fDistance / timeDiff.TotalSeconds) * m_fStrengthMultiplier, m_fMaxStrength);
	}

	private Vector3 v3GetWindDirection()
	{
		Vector3 v3From = m_Camera.ScreenToWorldPoint(m_v3PositionMouseDown);
		v3From.y = c_fWindPosY;

		Vector3 v3To = m_Camera.ScreenToWorldPoint(m_v3PositionMouseUp);
		v3To.y = c_fWindPosY;

		return v3To - v3From;
	}

	private IEnumerator destroyCooldown(GameObject _go)
	{
		yield return new WaitForSeconds(m_fDestroyTime);

		if(_go != null)
			Destroy(_go);
	}
}
