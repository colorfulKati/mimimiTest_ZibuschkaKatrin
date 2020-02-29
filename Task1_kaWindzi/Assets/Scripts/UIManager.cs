using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Balloon m_Balloon;

	[SerializeField]
	private Transform m_transLivesContainer;
	[SerializeField]
	private Transform m_transStarsContainer;

	[SerializeField]
	private GameObject m_goLifePrefab;
	[SerializeField]
	private GameObject m_goStarPrefab;

	private void Start()
	{
		m_Balloon.OnLivesChanged += onLivesChanged;
		m_Balloon.OnStarsChanged += onStarsChanged;

		onLivesChanged();
	}

	private void OnDestroy()
	{
		if(m_Balloon != null)
		{
			m_Balloon.OnLivesChanged -= onLivesChanged;
			m_Balloon.OnStarsChanged -= onStarsChanged;
		}
	}

	private void onLivesChanged()
	{
		updateDisplay(m_transLivesContainer, m_goLifePrefab, m_Balloon.iCurrentLives);
	}

	private void onStarsChanged()
	{
		updateDisplay(m_transStarsContainer, m_goStarPrefab, m_Balloon.iCurrentStars);
	}

	private void updateDisplay(Transform _transParent, GameObject _goChildPrefab, int _iCurrentNumber)
	{
		int iPrevNumber = _transParent.childCount;

		if (iPrevNumber > _iCurrentNumber)
		{
			for (int i = iPrevNumber - 1; i >= _iCurrentNumber; i--)
			{
				Transform transOldLife = _transParent.GetChild(i);
				transOldLife.SetParent(null);
				Destroy(transOldLife.gameObject);
			}

		}
		else if (iPrevNumber < _iCurrentNumber)
		{
			for (int i = 0; i < _iCurrentNumber - iPrevNumber; i++)
			{
				GameObject goNewLife = Instantiate(_goChildPrefab, _transParent);
			}
		}
	}
}
