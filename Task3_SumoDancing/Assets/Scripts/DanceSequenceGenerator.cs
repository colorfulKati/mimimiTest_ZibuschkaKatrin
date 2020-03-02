using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DanceSequenceGenerator", menuName = "ScriptableObjects/DanceSequenceGenerator")]
public class DanceSequenceGenerator : ScriptableObject
{
	[SerializeField]
	private int m_iSequenceLength;

	public List<DanceMove[]> m_listMoveSequences = new List<DanceMove[]>();

	public void generateMoveSequences()
	{
		m_listMoveSequences.Clear();

		m_listMoveSequences.Add(new DanceMove[m_iSequenceLength]);
		m_listMoveSequences[0][0] = (DanceMove)Random.Range(0, 4);
		DanceMove eNext;
		DanceMove ePrev = m_listMoveSequences[0][0];
		for (int i = 0; i < SlimeController.c_iNumberOfStages; i++)
		{
			if(i != 0)
				m_listMoveSequences.Add(new DanceMove[m_iSequenceLength]);

			for (int j = 0; j < m_iSequenceLength; j++)
			{
				if (i == 0 && j == 0)
					continue;

				do
				{
					eNext = (DanceMove)Random.Range(0, 4);
				} while (eNext == ePrev);

				m_listMoveSequences[i][j] = eNext;
				ePrev = eNext;
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
