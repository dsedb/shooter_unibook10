using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]
public class Investigator : MonoBehaviour {

    private Vector2 GetForce(Rigidbody2D rb, ref Vector2 player_pos)
    {
        var pos = rb.position;
        var vel = rb.velocity;
        return GetForce(ref pos, ref vel, ref player_pos, 1f, 10f);
    }

    private Vector2 GetForce(ref Vector2 pos, ref Vector2 vel, ref Vector2 player_pos, float level, float scale)
    {
        var diff = player_pos - pos;
        var inner_product = Vector2.Dot(diff.normalized, vel.normalized);
        var dist = diff.magnitude/scale;
        dist *= (1f - inner_product) + 0.5f;
        if (dist == 0f) {
            return new Vector2(level, 0f);
        }
        var dir = diff.normalized;
        float force = 0f;
        if (dist < 1f) {
            force = level*(1f - dist)*(1f - dist);
        }
        if (force < 0f) {
            return new Vector2(0f, 0f);
        }
        return dir * force;
    }

    private Vector2 CollectInformation(GameObject[] gos,
                                        ref Vector2 player_pos,
                                        bool forEnemy,
                                        out bool exist_target)
    {
        bool exist = false;
        var force = Vector2.zero;
        for (var i = 0; i < gos.Length; ++i) {
            var rb = gos[i].GetComponent<Rigidbody2D>();
            force += GetForce(rb, ref player_pos);
            if (forEnemy) {
                var diff = rb.position - player_pos;
                if (diff.y > diff.x*2f && diff.y > -diff.x*2f) {
                    exist = true;
                }
            }                
        }
        exist_target = exist;
        return force;
    }

    private Vector2 GetTotalForce(out bool exist_target)
    {
        exist_target = false;
        var force = Vector2.zero;

        var player_pos3 = transform.position;
        var player_pos = new Vector2(player_pos3.x, player_pos3.y);
        var ebs = GameObject.FindGameObjectsWithTag("EnemyBullet");
        if (ebs != null) {
            force += CollectInformation(ebs, ref player_pos, false /* forEnemy */, out exist_target);
        }
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null) {
            force += CollectInformation(enemies, ref player_pos, true /* forEnemy */, out exist_target) * 2f;
        }
        const float G = 0.02f;
        const float R = 5f;
        if (player_pos.x > R) {
            force.x += -(player_pos.x-R) * G;
        } else if (player_pos.x < -R) {
            force.x += -(player_pos.x+R) * G;
        }
        if ((player_pos.y+10f) > R) {
            force.y += (-10f-(player_pos.y-R)) * G;
        } else if ((player_pos.y+10f) < -R) {
            force.y += (-10f-(player_pos.y+R)) * G;
        }
        // force.x += -player_pos.x * G;
        // force.y += (-10f-player_pos.y) * G;

        if (force.magnitude > 0.01f) {
            force = force.normalized;
        }
        return force;
    }

    private float m_FireTime;
    private bool m_CanFire = false;

    void Update()
    {
        bool exist_target;
        var force = GetTotalForce(out exist_target);
        var player = GetComponent<Player>();
        bool fire = false;
        if (m_CanFire && exist_target) {
            if (Time.time - m_FireTime > 0.1f) {
                fire = true;
                m_FireTime = Time.time;
            }
        }
        player.UpdateKey(force.x, force.y, fire);
    }

}
