using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private const int c_iNotStartedIndex = -1;
	private const float c_fNextMoveDelay = 0.15f;

	private static WaitForSeconds m_WaitForSeconds = new WaitForSeconds(c_fNextMoveDelay);

	/// <summary>
	/// Raised, when a player's currently wanted dance move changed.
	/// Parameters: player index, next dance move
	/// </summary>
	public static event Action<int, DanceMove> OnNextDanceMoveChanged;

	/// <summary>
	/// Raised, when a player performed a dance move.
	/// Parameters: player index, true if the performed dance move was correct
	/// </summary>
	public static event Action<int, bool> OnPlayerPerformedMove;
	
	/// <summary>
	/// Raised, when a player is completely finished.
	/// Parameters: player index
	/// </summary>
	public static event Action<int> OnPlayerFinished;

	/// <summary>
	/// Raised, when a player finished a sequence.
	/// Parameters: player index, finished sequence index
	/// </summary>
	public static event Action<int, int> OnPlayerFinishedSequence;

	public static Color[] m_PlayerColor = new[] { Color.blue, Color.red };

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

	private DanceMove[] m_arCurrentSequence;
	private KeyCode m_CurrentKeyCode = KeyCode.None;
	private DanceMove m_eCurrentDanceMove = DanceMove.None;
	private DanceMove eCurrentDanceMove
	{
		get { return m_eCurrentDanceMove; }
		set
		{
			m_eCurrentDanceMove = value;
			OnNextDanceMoveChanged?.Invoke(m_iPlayerIndex, m_eCurrentDanceMove);
		}
	}

	private int m_iCurrentSequenceIndex = c_iNotStartedIndex;
	private int iCurrentSequenceIndex
	{ get { return m_iCurrentSequenceIndex; }
		set
		{
			int iPrevSequenceIndex = m_iCurrentSequenceIndex;
			m_iCurrentSequenceIndex = value;

			// Do not count the "not started sequence" as finished sequence on start
			if (m_iCurrentSequenceIndex > iPrevSequenceIndex && iPrevSequenceIndex >= 0)
				OnPlayerFinishedSequence?.Invoke(m_iPlayerIndex, iPrevSequenceIndex);

			// Check whether all sequences were finished, or a new one should begin
			if (m_iCurrentSequenceIndex >= m_DanceSequenceGenerator.m_listMoveSequences.Count)
			{
				m_iCurrentSequenceIndex = c_iNotStartedIndex;
				iCurrentMoveIndex = c_iNotStartedIndex;
				OnPlayerFinished?.Invoke(m_iPlayerIndex);
			}
			else if(m_iCurrentSequenceIndex == c_iNotStartedIndex)
			{
				iCurrentMoveIndex = c_iNotStartedIndex;
			}
			else 
			{
				m_arCurrentSequence = m_DanceSequenceGenerator.m_listMoveSequences[m_iCurrentSequenceIndex];
				iCurrentMoveIndex = 0;
			}
		}
	}


	private int m_iCurrentMoveIndex = c_iNotStartedIndex;
	private int iCurrentMoveIndex
	{
		get { return m_iCurrentMoveIndex; }
		set
		{
			m_iCurrentMoveIndex = value;

			if (m_iCurrentMoveIndex >= m_arCurrentSequence.Length)
			{
				iCurrentSequenceIndex++;
			}
			else if (m_iCurrentMoveIndex == c_iNotStartedIndex)
			{
				m_CurrentKeyCode = KeyCode.None;
			}
			else
			{
				eCurrentDanceMove = m_arCurrentSequence[m_iCurrentMoveIndex];
				m_CurrentKeyCode = m_MoveToKey[eCurrentDanceMove];
			}
		}
	}

	private void Awake()
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

	private void Start()
	{
		GameStateController.OnGameStateChanged += onGameStateChanged;
	}

	private void OnDestroy()
	{
		GameStateController.OnGameStateChanged -= onGameStateChanged;
	}

	private void Update()
	{
		if (GameStateController.eCurrentState != GameState.InGame)
			return;
		
		if (eCurrentDanceMove == DanceMove.None)
			return;

		if (Input.GetKeyDown(m_CurrentKeyCode))
		{
			m_SpriteRenderer.sprite = m_KeyToSprite[m_CurrentKeyCode];
			OnPlayerPerformedMove?.Invoke(m_iPlayerIndex, true);

			eCurrentDanceMove = DanceMove.None;

			this.StartCoroutine(goToNextMoveDelayed());

		}
		else if (Input.GetKeyDown(m_KeyLeft) || Input.GetKeyDown(m_KeyRight) || Input.GetKeyDown(m_KeyUp) || Input.GetKeyDown(m_KeyDown))
		{
			m_SpriteRenderer.sprite = m_SpriteWrongMove;
			OnPlayerPerformedMove?.Invoke(m_iPlayerIndex, false);
		}
	}

	private IEnumerator goToNextMoveDelayed()
	{
		yield return m_WaitForSeconds;

		iCurrentMoveIndex++;
	}

	private void onGameStateChanged(GameState _ePrevState, GameState _eCurrentState)
	{
		if (_eCurrentState == GameState.InGame)
		{
			iCurrentSequenceIndex = 0;
		}
		if (_eCurrentState == GameState.AfterGame)
		{
			m_SpriteRenderer.sprite = m_SpriteDown;
		}
	}
}
