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
	[SerializeField]
	private AudioClip m_ClipExplosion;

	private void Awake()
	{
		m_MusicSource.clip = m_MusicIntro;
		m_MusicSource.Play();
	}

	private void Start()
	{
		GameStateController.OnGameStateChanged += onGameStateChanged;
		PlayerController.OnPlayerPerformedMove += onPlayerPerformedMove;
		SlimeController.OnAddedSlime += onAddedSlime;
		ExplosionController.OnExplosion += onExplosion;
	}

	private void OnDestroy()
	{
		GameStateController.OnGameStateChanged -= onGameStateChanged;
		PlayerController.OnPlayerPerformedMove -= onPlayerPerformedMove;
		SlimeController.OnAddedSlime -= onAddedSlime;
		ExplosionController.OnExplosion -= onExplosion;
	}

	private void onGameStateChanged(GameState _ePrevState, GameState _eCurrentState)
	{
		if (_eCurrentState == GameState.InGame)
		{
			m_MusicSource.clip = m_MusicInGame;
			m_MusicSource.Play();
		}
		else if (_eCurrentState == GameState.Finish)
		{
			m_MusicSource.Stop();
		}
		else if(_ePrevState == GameState.Finish)
		{
			m_MusicSource.clip = m_MusicIntro;
			m_MusicSource.Play();

		}

		if(_eCurrentState == GameState.AfterGame)
		{
			m_SFXSource.PlayOneShot(m_ClipFinished, 0.3f);
		}
	}

	private void onPlayerPerformedMove(int _iPlayerIndex, bool _bCorrect)
	{
		m_SFXSource.PlayOneShot(_bCorrect ? m_ClipCorrect : m_ClipWrong);
	}

	private void onAddedSlime()
	{
		m_SFXSource.PlayOneShot(m_ClipSlime);
	}

	private void onExplosion()
	{
		m_SFXSource.PlayOneShot(m_ClipExplosion);
	}
}
