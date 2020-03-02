using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject m_goBeforeGamePanel;
	[SerializeField]
	private GameObject m_goAfterGamePanel;
	[SerializeField]
	private GameObject m_goSmoke;

	[SerializeField]
	private Text m_TextWinner;

	private void Awake()
	{
		setBeforeGame();
	}

	private void Start()
	{
		GameStateController.OnGameStateChanged += onGameStateChanged;
		PlayerController.OnPlayerFinished += updateWinner;
	}

	private void OnDestroy()
	{
		GameStateController.OnGameStateChanged -= onGameStateChanged;
		PlayerController.OnPlayerFinished -= updateWinner;
	}

	private void onGameStateChanged(GameState _ePrevState, GameState _eCurrentState)
	{
		switch (_eCurrentState)
		{
			case GameState.BeforeGame:
				setBeforeGame();
				break;
			case GameState.InGame:
				setInGame();
				break;
			case GameState.AfterGame:
				setAfterGame();
				break;
		}
	}

	private void setBeforeGame()
	{
		m_goBeforeGamePanel.SetActive(true);
		m_goAfterGamePanel.SetActive(false);
		m_goSmoke.SetActive(true);
	}

	private void setAfterGame()
	{
		m_goBeforeGamePanel.SetActive(false);
		m_goAfterGamePanel.SetActive(true);
		m_goSmoke.SetActive(true);
	}

	private void setInGame()
	{
		m_goBeforeGamePanel.SetActive(false);
		m_goAfterGamePanel.SetActive(false);
		m_goSmoke.SetActive(false);
	}
	
	private void updateWinner(int _iPlayerIndex)
	{
		m_TextWinner.color = PlayerController.m_PlayerColor[_iPlayerIndex - 1];
		m_TextWinner.text = "player" + _iPlayerIndex;
	}
}
