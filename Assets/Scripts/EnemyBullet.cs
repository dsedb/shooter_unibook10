using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour {

    private Rigidbody2D m_Rb;
    private float m_Time;
    private float m_Life;

    public void Fire(Vector3 impulse, float life)
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_Rb.AddForce(impulse, ForceMode2D.Impulse);
        m_Time = Time.time;
        m_Life = life;
    }

    void Update()
    {
        if (Time.time - m_Time > m_Life) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(gameObject);
    }
}
