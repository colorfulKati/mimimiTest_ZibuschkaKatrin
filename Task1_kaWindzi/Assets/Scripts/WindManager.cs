using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MimiBehaviour
{
	[SerializeField]
	private Wind m_WindPrefab;

	[SerializeField]
	private float m_fDestroyTime;

	[SerializeField]
	private float m_fStrength;

	private Camera m_Camera;

	private Vector3 m_v3PositionMouseDown = Vector3.zero;
	private Vector3 m_v3PositionMouseUp = Vector3.zero;

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
		}

		if (Input.GetMouseButtonUp(0))
		{
			m_v3PositionMouseUp = Input.mousePosition;
			createWind();
		}
	}

	private void createWind()
	{
		Wind windInstance = Instantiate(m_WindPrefab, this.m_transThis);

		windInstance.Initialize(v3GetWindPosition(), m_fStrength, v3GetWindDirection());
		m_DestroyCoroutines.Add(this.StartCoroutine(DestroyCooldown(windInstance.gameObject)));
	}

	private Vector3 v3GetWindPosition()
	{
		Vector3 v3Position = m_Camera.ScreenToWorldPoint(m_v3PositionMouseUp);
		v3Position.y = 0;
		return v3Position;
	}

	private Vector3 v3GetWindDirection()
	{
		Vector3 v3From = m_Camera.ScreenToWorldPoint(m_v3PositionMouseDown);
		v3From.y = 0;

		Vector3 v3To = m_Camera.ScreenToWorldPoint(m_v3PositionMouseUp);
		v3To.y = 0;

		return v3To - v3From;
	}

	private IEnumerator DestroyCooldown(GameObject _go)
	{
		yield return new WaitForSeconds(m_fDestroyTime);

		if(_go != null)
			Destroy(_go);
	}
}
