using UnityEngine;
using System.Collections;

public class MimiBehaviour : MonoBehaviour
{
    protected Transform m_transThis;
    public Transform trans
    {
        get
        {
            return m_transThis;
        }
    }

    protected virtual void Awake()
    {
        m_transThis = this.transform;
    }
}
