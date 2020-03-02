using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private const int c_iNotStartedIndex = -1;
	private const int c_iFinishedIndex = -2;

	/// <summary>
	/// Raised, when a player's currently wanted dance move changed.
	/// Parameters: player index, next dance move
	/// </summary>
	public static event Action<int, DanceMove> OnPlayerNextDanceMoveChanged;

	/// <summary>
	/// Raised, when a player performed a dance move.
	/// Parameters: player index, true if the performed dance move was correct
	/// </summary>
	public static event Action<int, bool> OnPlayerPerformedMove;
	
	/// <summary>
	/// Raised, when a player is finished.
	/// Parameters: player index
	/// </summary>
	public static event Action<int> OnPlayerFinished;

	[SerializeField]
	private int m_iPlayerIndex;
	[SerializeField]
	private DanceSequenceGenerator m_DanceSequenceGenerator;

	[Header("Key Codes")]
	[SerializeField]
	private KeyCode m_KeyLeft;
	[SerializeField]
	private KeyCode m_KeyRight;
	[SerializeField]
	private KeyCode m_KeyUp;
	[SerializeField]
	private KeyCode m_KeyDown;

	[Header("Sprite Rendering")]
	[SerializeField]
	private SpriteRenderer m_SpriteRenderer;

	[SerializeField]
	private Sprite m_SpriteLeft;
	[SerializeField]
	private Sprite m_SpriteRight;
	[SerializeField]
	private Sprite m_SpriteUp;
	[SerializeField]
	private Sprite m_SpriteDown;
	[SerializeField]
	private Sprite m_SpriteWrongMove;
	
	private Dictionary<KeyCode, Sprite> m_KeyToSprite = new Dictionary<KeyCode, Sprite>();
	private Dictionary<DanceMove, KeyCode> m_MoveToKey = new Dictionary<DanceMove, KeyCode>();

	private KeyCode m_CurrentKeyCode = KeyCode.None;
	private DanceMove m_eCurrentDanceMove = DanceMove.None;
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
				m_eCurrentDanceMove = m_DanceSequenceGenerator.listDanceSequence[m_iCurrentMoveIndex];
				m_CurrentKeyCode = m_MoveToKey[m_eCurrentDanceMove];
				OnPlayerNextDanceMoveChanged?.Invoke(m_iPlayerIndex, m_eCurrentDanceMove);
			}
		}
	}

	private void Start()
	{
		m_MoveToKey.Add(DanceMove.Left, m_KeyLeft);
		m_MoveToKey.Add(DanceMove.Right, m_KeyRight);
		m_MoveToKey.Add(DanceMove.Up, m_KeyUp);
		m_MoveToKey.Add(DanceMove.Down, m_KeyDown);

		m_KeyToSprite.Add(m_KeyLeft, m_SpriteLeft);
		m_KeyToSprite.Add(m_KeyRight, m_SpriteRight);
		m_KeyToSprite.Add(m_KeyUp, m_SpriteUp);
		m_KeyToSprite.Add(m_KeyDown, m_SpriteDown);
	}

	private void Update()
	{
		if (iCurrentMoveIndex == c_iNotStartedIndex)
			iCurrentMoveIndex = 0;

		if (Input.GetKeyDown(m_CurrentKeyCode))
		{
			m_SpriteRenderer.sprite = m_KeyToSprite[m_CurrentKeyCode];
			OnPlayerPerformedMove?.Invoke(m_iPlayerIndex, true);

			iCurrentMoveIndex++;
		}
		else if (Input.GetKeyDown(m_KeyLeft) || Input.GetKeyDown(m_KeyRight) || Input.GetKeyDown(m_KeyUp) || Input.GetKeyDown(m_KeyDown))
		{
			m_SpriteRenderer.sprite = m_SpriteWrongMove;
			OnPlayerPerformedMove?.Invoke(m_iPlayerIndex, false);
		}
	}
}
