using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private GameObject m_EnemyPrefab;

    void Awake()
    {
        Random.InitState(12345);
    }

	void Start()
    {
        // Instantiate(m_PlayerPrefab);
        StartCoroutine(loop());
	}
	
    public void Reset()
    {
        var eb_gos = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (var i = 0; i < eb_gos.Length; ++i) {
            Destroy(eb_gos[i]);
        }
        var e_gos = GameObject.FindGameObjectsWithTag("Enemy");
        for (var i = 0; i < e_gos.Length; ++i) {
            Destroy(e_gos[i]);
        }
        var b_gos = GameObject.FindGameObjectsWithTag("Bullet");
        for (var i = 0; i < b_gos.Length; ++i) {
            Destroy(b_gos[i]);
        }
        var p_gos = GameObject.FindGameObjectsWithTag("Player");
        for (var i = 0; i < p_gos.Length; ++i) {
            p_gos[i].GetComponent<Player>().Reset();
        }
    }

	IEnumerator loop()
    {
        yield return new WaitForSeconds(1f);
        for (;;) {
            var pos = new Vector2(Random.Range(-50f, 50f), Random.Range(29f, 31f));
            Instantiate(m_EnemyPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
            yield return new WaitForSeconds(4f);
        }
	}
}
