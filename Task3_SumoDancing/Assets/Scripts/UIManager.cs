using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Text[] m_arPlayerText;

	private void Start()
	{
		PlayerController.OnPlayerNextDanceMoveChanged += onPlayerNextDanceMoveChanged;
		PlayerController.OnPlayerFinished += onPlayerFinished;
	}

	private void OnDestroy()
	{
		PlayerController.OnPlayerNextDanceMoveChanged -= onPlayerNextDanceMoveChanged;
		PlayerController.OnPlayerFinished -= onPlayerFinished;
	}

	private void onPlayerFinished(int _iPlayerIndex)
	{
		updateText(_iPlayerIndex - 1, "Finished!");
	}

	private void onPlayerNextDanceMoveChanged(int _iPlayerIndex, string _strKeyCode)
	{
		updateText(_iPlayerIndex - 1, _strKeyCode);
	}

	private void updateText(int _iTextIndex, string _strText)
	{
		m_arPlayerText[_iTextIndex].text = _strText;
	}
}
