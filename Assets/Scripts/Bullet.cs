using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

    private Rigidbody2D m_Rb;
    private float m_Time;
    private float m_Life;
    private Player m_Player;

    public void Fire(Vector3 impulse, float life, Player player)
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_Rb.AddForce(impulse, ForceMode2D.Impulse);
        m_Time = Time.time;
        m_Life = life;
        m_Player = player;
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

    public Player GetPlayer()
    {
        return m_Player;
    }
}
