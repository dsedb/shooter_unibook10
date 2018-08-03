using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {

    [SerializeField] GameObject m_EnemyBulletPrefab;
    Rigidbody2D m_Rb;
    Player[] m_Players;
    Vector2 m_TargetPos;

	void Start()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        var gos = GameObject.FindGameObjectsWithTag("Player");
        m_Players = new Player[gos.Length];
        for (var i = 0; i < gos.Length; ++i) {
            m_Players[i] = gos[i].GetComponent<Player>();
        }
        StartCoroutine(loop());
	}
	
    private void AddSpringForce(Vector2 targetPos)
    {
        var pos = m_Rb.position;
        var diff = targetPos;
        diff.x -= pos.x;
        diff.y -= pos.y;
        m_Rb.AddForce(diff * 3f);
    }

	IEnumerator loop()
    {
        for (var i = 0; i < 4; ++i) {
            var randomPos = Random.onUnitSphere;
            var targetPos = new Vector2(0f, 0f);
            m_TargetPos.x = targetPos.x + randomPos.x * 16;
            m_TargetPos.y = targetPos.y + randomPos.y * 8;
            yield return new WaitForSeconds(Random.Range(3f, 4f));
        }
        var rpos = Random.insideUnitCircle;
        m_TargetPos.x = rpos.x;
        m_TargetPos.y = rpos.y - 32f;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
	}

    void FixedUpdate()
    {
        AddSpringForce(m_TargetPos);
    }

    void Update()
    {
        if (Random.Range(0f, 1f) < 1f * Time.deltaTime) {
            for (var i = 0; i < m_Players.Length; ++i) {
                var player = m_Players[i];
                var diff = player.transform.position - transform.position;
                var rad = Mathf.Atan2(diff.x, diff.y);
                var rot = Quaternion.Euler(0f, 0f, -rad*Mathf.Rad2Deg);
                var go = Instantiate(m_EnemyBulletPrefab, transform.position, rot);
                var enemy_bullet = go.GetComponent<EnemyBullet>();
                var life = 10f;
                enemy_bullet.Fire(diff.normalized * 16f, life);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        ExplosionEmitter.Emit(transform.position);
        Destroy(gameObject);
        BeatValue.AddScore(1);

        var bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet != null) {
            bullet.GetPlayer().AddReward();
        }
    }
}
