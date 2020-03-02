using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[Header("Music")]
	[SerializeField]
	private AudioSource m_MusicSource;
	[SerializeField]
	private AudioClip m_MusicInGame;
	[SerializeField]
	private AudioClip m_MusicIntro;

	[Header("SFX")]
	[SerializeField]
	private AudioSource m_SFXSource;
	[SerializeField]
	private AudioClip m_ClipFinished;
	[SerializeField]
	private AudioClip m_ClipCorrect;
	[SerializeField]
	private AudioClip m_ClipWrong;
	[SerializeField]
	private AudioClip m_ClipSlime;

	private void Awake()
	{
		m_MusicSource.clip = m_MusicIntro;
		m_MusicSource.Play();
	}

	private void Start()
	{
		GameStateController.OnGameStateChanged += onGameStateChanged;
		PlayerController.OnPlayerPerformedMove += onPlayerPerformedMove;
		PlayerController.OnPlayerFinished += onPlayerFinished;
		SlimeController.OnAddedSlime += onAddedSlime;
	}

	private void OnDestroy()
	{
		GameStateController.OnGameStateChanged -= onGameStateChanged;
		PlayerController.OnPlayerPerformedMove -= onPlayerPerformedMove;
		PlayerController.OnPlayerFinished -= onPlayerFinished;
		SlimeController.OnAddedSlime -= onAddedSlime;
	}

	private void onGameStateChanged(GameState _ePrevState, GameState _eCurrentState)
	{
		if (_eCurrentState == GameState.InGame)
		{
			m_MusicSource.clip = m_MusicInGame;
			m_MusicSource.Play();
		}
		else if (_ePrevState == GameState.InGame)
		{
			m_MusicSource.clip = m_MusicIntro;
			m_MusicSource.Play();
		}
	}

	private void onPlayerFinished(int _iPlayerIndex)
	{
		m_SFXSource.PlayOneShot(m_ClipFinished);
	}

	private void onPlayerPerformedMove(int _iPlayerIndex, bool _bCorrect)
	{
		m_SFXSource.PlayOneShot(_bCorrect ? m_ClipCorrect : m_ClipWrong);
	}

	private void onAddedSlime()
	{
		m_SFXSource.PlayOneShot(m_ClipSlime);
	}
}
