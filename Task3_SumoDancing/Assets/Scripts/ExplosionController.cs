using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MimiBehaviour
{
	/// <summary>
	/// Raised on explosion.
	/// </summary>
	public static event Action OnExplosion;

	/// <summary>
	/// Raised, when the explosion is finished.
	/// </summary>
	public static event Action OnExplosionFinished;

	private SpriteRenderer m_Renderer;
	private Animator m_Animator;

	private Vector3 m_v3InitialPosition;
	private Vector3 m_v3MirroredPosition;
	private bool m_bInitialFlipX;

	private WaitForSeconds m_Wait = new WaitForSeconds(3);

	protected override void Awake()
	{
		base.Awake();

		m_Renderer = this.GetComponent<SpriteRenderer>();
		m_Animator = this.GetComponent<Animator>();

		hide();

		m_v3InitialPosition = m_transThis.position;
		m_v3MirroredPosition = new Vector3(m_v3InitialPosition.x * (-1), m_v3InitialPosition.y, m_v3InitialPosition.z);
		m_bInitialFlipX = m_Renderer.flipX;
	}

	private void Start()
	{
		PlayerController.OnPlayerFinished += onFinished;
	}

	private void OnDestroy()
	{
		PlayerController.OnPlayerFinished -= onFinished;
	}

	private void onFinished(int _iPlayerIndex)
	{
		if(_iPlayerIndex == 1)
		{
			m_transThis.position = m_v3InitialPosition;
			m_Renderer.flipX = m_bInitialFlipX;
		}
		else
		{
			m_transThis.position = m_v3MirroredPosition;
			m_Renderer.flipX = !m_bInitialFlipX;
		}

		m_Renderer.enabled = true;
		m_Animator.enabled = true;
	}

	private void hide()
	{
		m_Renderer.enabled = false;
		m_Animator.enabled = false;
	}

	private void finishExplosion()
	{
		hide();
		OnExplosionFinished?.Invoke();
	}

	private void fireExplosion()
	{
		OnExplosion?.Invoke();
	}
}
