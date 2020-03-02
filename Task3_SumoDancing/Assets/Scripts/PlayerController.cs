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
	public static event System.Action<int, DanceMove> OnNextDanceMoveChanged;

	/// <summary>
	/// Raised, when a player performed a dance move.
	/// Parameters: player index, true if the performed dance move was correct
	/// </summary>
	public static event System.Action<int, bool> OnPlayerPerformedMove;
	
	/// <summary>
	/// Raised, when a player is completely finished.
	/// Parameters: player index
	/// </summary>
	public static event System.Action<int> OnPlayerFinished;

	/// <summary>
	/// Raised, when a player finished a sequence.
	/// Parameters: player index, finished sequence index
	/// </summary>
	public static event System.Action<int, int> OnPlayerFinishedSequence;

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
	[SerializeField]
	private Sprite m_SpriteLoser;

	[SerializeField]
	private Vector3 m_v3OffsetSpriteLeft;
	[SerializeField]
	private Vector3 m_v3OffsetSpriteRight;
	[SerializeField]
	private Vector3 m_v3OffsetSpriteUp;
	[SerializeField]
	private Vector3 m_v3OffsetSpriteDown;

	private bool m_bIsWinner = false;
	private bool m_bOtherPlayerFinishedFirst = false;

	private Dictionary<KeyCode, Sprite> m_KeyToSprite = new Dictionary<KeyCode, Sprite>();
	private Dictionary<KeyCode, Vector3> m_KeyToSpriteOffset = new Dictionary<KeyCode, Vector3>();
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

				if (!m_bOtherPlayerFinishedFirst)
				{
					m_bIsWinner = true;
					OnPlayerFinished?.Invoke(m_iPlayerIndex);
				}
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

		m_KeyToSpriteOffset.Add(m_KeyLeft, m_v3OffsetSpriteLeft);
		m_KeyToSpriteOffset.Add(m_KeyRight, m_v3OffsetSpriteRight);
		m_KeyToSpriteOffset.Add(m_KeyUp, m_v3OffsetSpriteUp);
		m_KeyToSpriteOffset.Add(m_KeyDown, m_v3OffsetSpriteDown);
	}

	private void Start()
	{
		GameStateController.OnGameStateChanged += onGameStateChanged;
		OnPlayerFinished += onPlayerFinished;
		ExplosionController.OnExplosion += onExplosion;
	}

	private void OnDestroy()
	{
		GameStateController.OnGameStateChanged -= onGameStateChanged;
		OnPlayerFinished -= onPlayerFinished;
		ExplosionController.OnExplosion -= onExplosion;
	}

	private void Update()
	{
		if (GameStateController.eCurrentState != GameState.InGame)
			return;
		
		if (eCurrentDanceMove == DanceMove.None)
			return;

		if (Input.GetKeyDown(m_CurrentKeyCode))
		{
			Vector3 v3RandomRotation = new Vector3(0, 0, Random.Range(-3f, 3f));

			m_SpriteRenderer.sprite = m_KeyToSprite[m_CurrentKeyCode];
			m_SpriteRenderer.transform.localPosition = m_KeyToSpriteOffset[m_CurrentKeyCode];
			m_SpriteRenderer.transform.localEulerAngles = v3RandomRotation;
			OnPlayerPerformedMove?.Invoke(m_iPlayerIndex, true);

			eCurrentDanceMove = DanceMove.None;

			this.StartCoroutine(goToNextMoveDelayed());

		}
		else if (Input.GetKeyDown(m_KeyLeft) || Input.GetKeyDown(m_KeyRight) || Input.GetKeyDown(m_KeyUp) || Input.GetKeyDown(m_KeyDown))
		{
			Vector3 v3RandomRotation = new Vector3(0, 0, Random.Range(-5f, 5f));
			m_SpriteRenderer.sprite = m_SpriteWrongMove;
			m_SpriteRenderer.transform.localPosition = m_v3OffsetSpriteLeft;
			m_SpriteRenderer.transform.localEulerAngles = v3RandomRotation;
			OnPlayerPerformedMove?.Invoke(m_iPlayerIndex, false);
			
			this.StartCoroutine(blockCurrentMoveDelayed(eCurrentDanceMove));
		}
	}

	private IEnumerator goToNextMoveDelayed()
	{
		yield return m_WaitForSeconds;

		iCurrentMoveIndex++;
	}

	private IEnumerator blockCurrentMoveDelayed(DanceMove _ePreviousMove)
	{
		eCurrentDanceMove = DanceMove.None;

		yield return m_WaitForSeconds;

		DanceMove eNewMove;
		do
		{
			eNewMove = (DanceMove)Random.Range(0, 4);
		} while (eNewMove == _ePreviousMove);

		eCurrentDanceMove = eNewMove;
		m_CurrentKeyCode = m_MoveToKey[eCurrentDanceMove];
	}

	private void onGameStateChanged(GameState _ePrevState, GameState _eCurrentState)
	{
		if (_eCurrentState == GameState.InGame)
		{
			iCurrentSequenceIndex = 0;
		}
		if (_eCurrentState == GameState.Finish)
		{
			m_SpriteRenderer.sprite = m_SpriteDown;
			m_SpriteRenderer.transform.localPosition = m_v3OffsetSpriteDown;
			m_SpriteRenderer.transform.localEulerAngles = Vector3.zero;
		}
		if (_eCurrentState == GameState.BeforeGame)
		{
			m_bIsWinner = false;
			m_bOtherPlayerFinishedFirst = false;
			m_SpriteRenderer.sprite = m_SpriteDown;
			m_SpriteRenderer.transform.localPosition = m_v3OffsetSpriteDown;
			m_SpriteRenderer.transform.localEulerAngles = Vector3.zero;
		}
	}

	private void onExplosion()
	{
		if (!m_bIsWinner)
		{
			m_SpriteRenderer.sprite = m_SpriteLoser;
			m_SpriteRenderer.transform.localPosition = m_v3OffsetSpriteDown;
			m_SpriteRenderer.transform.localEulerAngles = Vector3.zero;
		}
	}

	private void onPlayerFinished(int _iPlayerIndex)
	{
		if (_iPlayerIndex != m_iPlayerIndex)
		{
			m_bOtherPlayerFinishedFirst = true;
		}
	}
}
