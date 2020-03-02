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

	[SerializeField]
	private Sprite[] m_arIntensity;

	private Dictionary<DanceMove, Vector3> m_MoveToRotation = new Dictionary<DanceMove, Vector3>()
	{
		{ DanceMove.Up, Vector3.zero },
		{ DanceMove.Left, new Vector3(0,0,90) },
		{ DanceMove.Down, new Vector3(0,0,180) },
		{ DanceMove.Right, new Vector3(0,0,270) }
	};

	private GameObject[] m_arDisplayGameObjects;
	private Transform[] m_arDirectionTransforms;

	private int m_iNumberOfSequencesPerIntensity;

	private void Awake()
	{
		m_iNumberOfSequencesPerIntensity = Mathf.CeilToInt((float)SlimeController.c_iNumberOfStages / m_arIntensity.Length);
		int i;

		m_arDirectionTransforms = new Transform[m_arDirections.Length];
		for (i = 0; i < m_arDirections.Length; i++)
		{
			m_arDirections[i].sprite = m_arIntensity[0];
			m_arDirectionTransforms[i] = m_arDirections[i].transform;
		}

		m_arDisplayGameObjects = new GameObject[m_arDisplayBackgrounds.Length];
		for (i = 0; i < m_arDisplayBackgrounds.Length; i++)
		{
			m_arDisplayGameObjects[i] = m_arDisplayBackgrounds[i].gameObject;
			m_arDisplayGameObjects[i].SetActive(false);
		}
	}

	private void Start()
	{
		PlayerController.OnNextDanceMoveChanged += updateDirection;
		PlayerController.OnPlayerFinishedSequence += updateIntensity;
		GameStateController.OnGameStateChanged += onGameStateChanged;
	}

	private void OnDestroy()
	{
		PlayerController.OnNextDanceMoveChanged -= updateDirection;
		PlayerController.OnPlayerFinishedSequence -= updateIntensity;
		GameStateController.OnGameStateChanged -= onGameStateChanged;
	}

	private void updateDirection(int _iPlayerIndex, DanceMove _eDanceMove)
	{
		if (_eDanceMove == DanceMove.None)
		{
			m_arDisplayGameObjects[_iPlayerIndex - 1].SetActive(false);
		}
		else
		{
			m_arDisplayGameObjects[_iPlayerIndex - 1].SetActive(true);
			m_arDirectionTransforms[_iPlayerIndex - 1].eulerAngles = m_MoveToRotation[_eDanceMove];
		}
	}

	private void updateIntensity(int _iPlayerIndex, int _iFinishedSequenceIndex)
	{
		int iCurrentIntensity = (_iFinishedSequenceIndex + 1) / m_iNumberOfSequencesPerIntensity;
		if (iCurrentIntensity >= 0 && iCurrentIntensity < m_arIntensity.Length)
			m_arDirections[_iPlayerIndex - 1].sprite = m_arIntensity[iCurrentIntensity];
	}
		
	private void onGameStateChanged(GameState _ePrevState, GameState _eCurrentState)
	{
		// Only show next move display during InGame state
		if (_eCurrentState != GameState.InGame)
		{
			foreach (GameObject go in m_arDisplayGameObjects)
			{
				go.SetActive(false);
			}
		}
		else
		{
			foreach (SpriteRenderer renderer in m_arDirections)
			{
				renderer.sprite = m_arIntensity[0];
			}
		}
	}
}
