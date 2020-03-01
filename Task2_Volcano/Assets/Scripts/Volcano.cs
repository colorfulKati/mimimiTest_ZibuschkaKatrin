using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MimiBehaviour
{
	[SerializeField]
	private Rigidbody m_rigidRockPrefab;
	
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
		float fX = iSignX * Random.Range(0.30f, 1.0f);
		float fY = Random.Range(0.30f, 1.0f);
		float fZ = iSignZ * Random.Range(0.30f, 1.0f);
		Vector3 v3Direction = new Vector3(fX, fY, fZ).normalized;

		// Generate random strength
		float fStrength = Random.Range(15000f, 20000f);

		// Apply force
		_rigidRock.AddForce(v3Direction * fStrength);
	}

	public void erupt()
	{
		Rigidbody rigidRockInstance = Instantiate(m_rigidRockPrefab, m_transThis);
		
		this.StartCoroutine(throwRock(rigidRockInstance));
	}
}
