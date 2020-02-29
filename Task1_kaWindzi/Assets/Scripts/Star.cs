using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
	private Collider m_collider;

	private void Awake()
	{
		m_collider = this.GetComponent<Collider>();
	}

	private void Start()
	{
		Balloon.OnReset += reset;
	}

	private void OnDestroy()
	{
		Balloon.OnReset -= reset;
	}

	public void collect()
	{
		gameObject.SetActive(false);
	}

	private void reset()
	{
		gameObject.SetActive(true);
	}
}
