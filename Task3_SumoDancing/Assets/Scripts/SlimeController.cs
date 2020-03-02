using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
	public const int c_iNumberOfStages = 5;

	[SerializeField]
	private SpriteRenderer[] m_arSlimePlayer1;
	[SerializeField]
	private SpriteRenderer[] m_arSlimePlayer2;

	private SpriteRenderer[][] m_arSlimePerPlayer = new SpriteRenderer[2][];

	/// <summary>
	/// Raised, when more slime is added.
	/// </summary>
	public static event Action OnAddedSlime;

	private void Awake()
	{
		m_arSlimePerPlayer[0] = m_arSlimePlayer1;
		m_arSlimePerPlayer[1] = m_arSlimePlayer2;

		hideAll();
	}

	private void Start()
	{
		PlayerController.OnPlayerFinishedSequence += updateSlime;
		GameStateController.OnGameStateChanged += onGameStateChanged;
	}

	private void OnDestroy()
	{
		PlayerController.OnPlayerFinishedSequence -= updateSlime;
		GameStateController.OnGameStateChanged -= onGameStateChanged;
	}

	private void updateSlime(int _iPlayerIndex, int _iFinishedSequenceIndex)
	{
		if (_iFinishedSequenceIndex >= m_arSlimePerPlayer[_iPlayerIndex - 1].Length)
			return;

		m_arSlimePerPlayer[_iPlayerIndex - 1][_iFinishedSequenceIndex].enabled = true;
		OnAddedSlime?.Invoke();
	}

	private void onGameStateChanged(GameState _ePrevState, GameState _eCurrentState)
	{
		if (_eCurrentState == GameState.BeforeGame)
		{
			hideAll();
		}
	}

	private void hideAll()
	{
		for (int i = 0; i < m_arSlimePerPlayer.Length; i++)
		{
			for (int j = 0; j < m_arSlimePerPlayer[i].Length; j++)
			{
				m_arSlimePerPlayer[i][j].enabled = false;
			}
		}
	}
}
