using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private const int c_iNotStartedIndex = -1;
	private const int c_iFinishedIndex = -2;

	/// <summary>
	/// Raised, when a player's currently wanted key code changed.
	/// Parameters: player index, keycode
	/// </summary>
	public static event Action<int, string> OnPlayerKeyCodeChanged;

	/// <summary>
	/// Raised, when a player is finished.
	/// Parameters: player index
	/// </summary>
	public static event Action<int> OnPlayerFinished;

	[SerializeField]
	private int m_iPlayerIndex;

	[SerializeField]
	private DanceSequenceGenerator m_DanceSequenceGenerator;

	[SerializeField]
	private KeyCode m_KeyLeft;
	[SerializeField]
	private KeyCode m_KeyRight;
	[SerializeField]
	private KeyCode m_KeyUp;
	[SerializeField]
	private KeyCode m_KeyDown;

	private Dictionary<KeyCode, DanceMoves> m_KeyToMove = new Dictionary<KeyCode, DanceMoves>();
	private Dictionary<DanceMoves, KeyCode> m_MoveToKey = new Dictionary<DanceMoves, KeyCode>();

	private KeyCode m_CurrentKeyCode = KeyCode.None;
	private int m_iCurrentMoveIndex = c_iNotStartedIndex;
	private int iCurrentMoveIndex
	{
		get { return m_iCurrentMoveIndex; }
		set
		{
			m_iCurrentMoveIndex = value;

			if (m_iCurrentMoveIndex >= m_DanceSequenceGenerator.listDanceSequence.Count)
			{
				m_iCurrentMoveIndex = c_iFinishedIndex;
				m_CurrentKeyCode = KeyCode.None;
				OnPlayerFinished?.Invoke(m_iPlayerIndex);
			}
			else
			{
				m_CurrentKeyCode = m_MoveToKey[m_DanceSequenceGenerator.listDanceSequence[m_iCurrentMoveIndex]];
				OnPlayerKeyCodeChanged?.Invoke(m_iPlayerIndex, m_CurrentKeyCode.ToString());
			}
		}
	}

	private void Start()
	{
		m_KeyToMove.Add(m_KeyLeft, DanceMoves.Left);
		m_KeyToMove.Add(m_KeyRight, DanceMoves.Right);
		m_KeyToMove.Add(m_KeyUp, DanceMoves.Up);
		m_KeyToMove.Add(m_KeyDown, DanceMoves.Down);

		m_MoveToKey.Add(DanceMoves.Left, m_KeyLeft);
		m_MoveToKey.Add(DanceMoves.Right, m_KeyRight);
		m_MoveToKey.Add(DanceMoves.Up, m_KeyUp);
		m_MoveToKey.Add(DanceMoves.Down, m_KeyDown);
	}

	private void Update()
	{
		if (iCurrentMoveIndex == c_iNotStartedIndex)
			iCurrentMoveIndex = 0;

		if (Input.GetKeyDown(m_CurrentKeyCode))
		{
			iCurrentMoveIndex++;
		}
		else if (Input.GetKeyDown(m_KeyLeft) || Input.GetKeyDown(m_KeyRight) || Input.GetKeyDown(m_KeyUp) || Input.GetKeyDown(m_KeyDown))
		{
		}
	}
}
