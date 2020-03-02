using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private AudioSource m_SFXSource;
	[SerializeField]
	private AudioSource m_MusicSource;

	[SerializeField]
	private AudioClip m_ClipFinished;
	[SerializeField]
	private AudioClip m_ClipCorrect;
	[SerializeField]
	private AudioClip m_ClipWrong;
	
	private void Start()
	{
		PlayerController.OnPlayerPerformedMove += onPlayerPerformedMove;
		PlayerController.OnPlayerFinished += onPlayerFinished;
	}

	private void OnDestroy()
	{
		PlayerController.OnPlayerPerformedMove -= onPlayerPerformedMove;
		PlayerController.OnPlayerFinished -= onPlayerFinished;
	}

	private void onPlayerFinished(int _iPlayerIndex)
	{
		m_SFXSource.PlayOneShot(m_ClipFinished);
	}

	private void onPlayerPerformedMove(int _iPlayerIndex, bool _bCorrect)
	{
		m_SFXSource.PlayOneShot(_bCorrect ? m_ClipCorrect : m_ClipWrong);
	}
}
