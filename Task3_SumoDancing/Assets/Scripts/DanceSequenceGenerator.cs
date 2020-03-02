using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New DanceSequenceGenerator", menuName = "ScriptableObjects/DanceSequenceGenerator")]
public class DanceSequenceGenerator : MonoBehaviour
{
	[SerializeField]
	private int m_iSequenceLength;

	private DanceMove[] m_arDanceSequence;

	public IReadOnlyList<DanceMove> listDanceSequence
	{
		get { return System.Array.AsReadOnly(m_arDanceSequence); }
	}

	private void Awake()
	{
		m_arDanceSequence = new DanceMove[m_iSequenceLength];

		for (int i = 0; i < m_iSequenceLength; i++)
		{
			m_arDanceSequence[i] = (DanceMove)Random.Range(0, 4);
		}
	}
}

public enum DanceMove
{
	Left = 0,
	Right = 1,
	Up = 2,
	Down = 3,
	None = -1
}
