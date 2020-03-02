using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceMoveDisplayController : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer[] m_arDisplayBackgrounds;
	[SerializeField]
	private SpriteRenderer[] m_arDirections;

	private Dictionary<DanceMove, Vector3> m_MoveToRotation = new Dictionary<DanceMove, Vector3>()
	{
		{ DanceMove.Up, Vector3.zero },
		{ DanceMove.Left, new Vector3(0,0,90) },
		{ DanceMove.Down, new Vector3(0,0,180) },
		{ DanceMove.Right, new Vector3(0,0,270) }
	};

	private Transform[] m_arDirectionTransforms;

	private void Awake()
	{
		m_arDirectionTransforms = new Transform[m_arDirections.Length];

		for (int i = 0; i < m_arDirections.Length; i++)
		{
			m_arDirectionTransforms[i] = m_arDirections[i].transform;
		}
	}

	private void Start()
	{
		PlayerController.OnPlayerNextDanceMoveChanged += updateDirection;
		PlayerController.OnPlayerFinished += onFinished;
	}

	private void OnDestroy()
	{
		PlayerController.OnPlayerNextDanceMoveChanged -= updateDirection;
		PlayerController.OnPlayerFinished -= onFinished;
	}

	private void updateDirection(int _iPlayerIndex, DanceMove _eDanceMove)
	{
		m_arDirections[_iPlayerIndex - 1].transform.eulerAngles = m_MoveToRotation[_eDanceMove];
	}
	
	private void onFinished(int _iPlayerIndex)
	{
		foreach (SpriteRenderer renderer in m_arDisplayBackgrounds)
		{
			renderer.gameObject.SetActive(false);
		}
	}
}
