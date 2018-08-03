using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEmitter : MonoBehaviour {

    static ParticleSystem s_PS;

	void Start()
    {
        s_PS = GetComponent<ParticleSystem>();
	}
	
    public static void Emit(Vector3 pos)
    {
		var ep = new ParticleSystem.EmitParams();
		ep.position = pos;
		s_PS.Emit(ep, 1);
    }
}
