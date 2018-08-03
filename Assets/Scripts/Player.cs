using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    const float LIMIT_X = 52f;
    const float LIMIT_Y = 26f;

    [SerializeField] GameObject m_BulletPrefab;
    Rigidbody2D m_rb;
    float m_StickX;
    float m_StickY;
    bool m_Fire;
    bool m_FireDown;
    bool m_FirePrev;

    int m_DamageCount;
    float m_BeatEnemyRewardAtThisFrame;
    float m_DamageRewardAtThisFrame;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    public void Reset()
    {
        m_DamageCount = 0;
        m_BeatEnemyRewardAtThisFrame = 0f;
        m_DamageRewardAtThisFrame = 0f;
        m_rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
    }

    void fire()
    {
        var impulse = transform.TransformVector(new Vector3(0f, 100f, 0f));
        var pos0 = transform.TransformPoint(new Vector3(1f, 0f, 0f));
        var pos1 = transform.TransformPoint(new Vector3(-1f, 0f, 0f));
        var life = 3f;
        Instantiate(m_BulletPrefab, pos0, transform.rotation).GetComponent<Bullet>().Fire(impulse, life, this);
        Instantiate(m_BulletPrefab, pos1, transform.rotation).GetComponent<Bullet>().Fire(impulse, life, this);
    }

	void FixedUpdate()
    {
        const float force_scale = 640f;
        var hori = m_StickX * force_scale;
        var vert = m_StickY * force_scale;
        m_rb.AddForce(new Vector2(hori, vert));
    }

    void UpdateKey()
    {
        m_StickX = Input.GetAxisRaw("Horizontal");
        m_StickY = Input.GetAxisRaw("Vertical");
        m_Fire = Input.GetButtonDown("Fire1");
    }
    
    public void UpdateKey(bool x0, bool x1, bool y0, bool y1, bool fire)
    {
        m_StickX = x0 ? -1f : (x1 ? 1f : 0f);
        m_StickY = y0 ? 1f : (y1 ? -1f : 0f);
        m_FirePrev = m_FireDown;
        m_FireDown = fire;
        m_Fire = (!m_FirePrev && m_FireDown);
    }

    public void UpdateKey(float x, float y, bool fire)
    {
        m_StickX = x;
        m_StickY = y;
        m_FirePrev = m_FireDown;
        m_FireDown = fire;
        m_Fire = (!m_FirePrev && m_FireDown);
    }

    void Update()
    {
        m_BeatEnemyRewardAtThisFrame = 0f;
        m_DamageRewardAtThisFrame = 0f;

        // UpdateKey();
        if (m_Fire) {
            fire();
        }
	}

    void LateUpdate()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -LIMIT_X, LIMIT_X);
        pos.y = Mathf.Clamp(pos.y, -LIMIT_Y, LIMIT_Y);
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        ++m_DamageCount;
        DamageValue.AddScore(1);
        m_DamageRewardAtThisFrame += -0.2f;
    }

    public void AddReward()
    {
        m_BeatEnemyRewardAtThisFrame += 0.1f;
    }

    public float GetReward()
    {
        return m_DamageRewardAtThisFrame  + m_BeatEnemyRewardAtThisFrame;
    }

    public int GetDamageCount()
    {
        return m_DamageCount;
    }

    public Vector2 GetNormalizedPosition()
    {
        Vector3 pos = transform.position;
        return new Vector2(pos.x/LIMIT_X, pos.y/LIMIT_Y);
    }

}
