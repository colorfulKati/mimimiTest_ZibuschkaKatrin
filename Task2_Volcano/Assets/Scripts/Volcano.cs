using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MimiBehaviour
{
	[SerializeField]
	private Rigidbody m_rigidRockPrefab;
	[SerializeField]
	private float m_fStrengthMin;
	[SerializeField]
	private float m_fStrengthMax;
	[SerializeField]
	private float m_fHorizontalMin;
	[SerializeField]
	private float m_fHorizontalMax;
	[SerializeField]
	private float m_fVerticalMin;
	[SerializeField]
	private float m_fVerticalMax;

	private WaitForSeconds m_wait = new WaitForSeconds(0.05f);

	private void Update()
	{
		if (Input.GetButtonDown("Erupt"))
		{
			erupt();
		}
	}

	private IEnumerator throwRock(Rigidbody _rigidRock)
	{
		yield return m_wait;

		// Generate random direction
		int iSignX = Random.Range(0, 2) * 2 - 1;
		int iSignZ = Random.Range(0, 2) * 2 - 1;
		float fX = iSignX * Random.Range(m_fHorizontalMin, m_fHorizontalMax);
		float fY = Random.Range(m_fVerticalMin, m_fVerticalMax);
		float fZ = iSignZ * Random.Range(m_fHorizontalMin, m_fHorizontalMax);
		Vector3 v3Direction = new Vector3(fX, fY, fZ).normalized;

		// Generate random strength
		float fStrength = Random.Range(m_fStrengthMin, m_fStrengthMax);

		// Apply force
		_rigidRock.AddForce(v3Direction * fStrength);
	}

	public void erupt()
	{
		Rigidbody rigidRockInstance = Instantiate(m_rigidRockPrefab, m_transThis);
		
		this.StartCoroutine(throwRock(rigidRockInstance));
	}
}
