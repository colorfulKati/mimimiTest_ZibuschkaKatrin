using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New DanceSequenceGenerator", menuName = "ScriptableObjects/DanceSequenceGenerator")]
public class DanceSequenceGenerator : MonoBehaviour
{
	[SerializeField]
	private int m_iSequenceLength;

	public List<DanceMove[]> m_listMoveSequences = new List<DanceMove[]>();

	private void Awake()
	{
		generateMoveSequences();
	}

	public void generateMoveSequences()
	{
		m_listMoveSequences.Clear();
		for (int i = 0; i < SlimeController.c_iNumberOfStages; i++)
		{
			m_listMoveSequences.Add(new DanceMove[m_iSequenceLength]);
			for (int j = 0; j < m_iSequenceLength; j++)
			{
				m_listMoveSequences[i][j] = (DanceMove)Random.Range(0, 4);
			}
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
