using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceSequenceGenerator : MonoBehaviour
{
	[SerializeField]
	private int m_iSequenceLength;

	private DanceMoves[] m_arDanceSequence;

	public IReadOnlyList<DanceMoves> listDanceSequence
	{
		get { return System.Array.AsReadOnly(m_arDanceSequence); }
	}

	private void Awake()
	{
		m_arDanceSequence = new DanceMoves[m_iSequenceLength];

		for (int i = 0; i < m_iSequenceLength; i++)
		{
			m_arDanceSequence[i] = (DanceMoves)Random.Range(0, 4);
		}
	}
}

public enum DanceMoves
{
	Left = 0,
	Right = 1,
	Up = 2,
	Down = 3,
	None = -1
}
