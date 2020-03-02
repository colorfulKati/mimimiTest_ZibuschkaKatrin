using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
	/// <summary>
	/// Raised, when the game state changes
	/// Parameters: previous game state, new game state
	/// </summary>
	public static event Action<GameState, GameState> OnGameStateChanged;

	[SerializeField]
	private DanceSequenceGenerator m_DanceSequenceGenerator;

	private static GameState m_eCurrentState = GameState.None;
	private static GameState m_ePrevState;
	public static GameState eCurrentState
	{
		get { return m_eCurrentState; }
		private set
		{
			m_ePrevState = m_eCurrentState;
			m_eCurrentState = value;

			OnGameStateChanged?.Invoke(m_ePrevState, m_eCurrentState);
		}
	}

	private void Awake()
	{
		m_eCurrentState = GameState.BeforeGame;
	}

	private void Start()
	{
		PlayerController.OnPlayerFinished += onPlayerFinished;
		ExplosionController.OnExplosionFinished += onExplosionFinished;
	}

	private void OnDestroy()
	{
		PlayerController.OnPlayerFinished -= onPlayerFinished;
		ExplosionController.OnExplosionFinished -= onExplosionFinished;
	}

	private void Update()
	{
		switch (m_eCurrentState)
		{
			case GameState.BeforeGame:
				if (Input.GetKeyUp(KeyCode.Space))
				{
					m_DanceSequenceGenerator.generateMoveSequences();
					eCurrentState = GameState.InGame;
				}
				break;
			case GameState.AfterGame:
				if (Input.GetKeyUp(KeyCode.Space))
				{
					eCurrentState = GameState.BeforeGame;
				}
				break;
		}
	}
		
	private void onPlayerFinished(int _iPlayerIndex)
	{
		eCurrentState = GameState.Finish;
	}

	private void onExplosionFinished()
	{
		eCurrentState = GameState.AfterGame;
	}
}

public enum GameState
{
	BeforeGame,
	InGame,
	Finish,
	AfterGame,
	None
}
