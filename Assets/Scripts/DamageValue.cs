using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageValue : MonoBehaviour {

    static UnityEngine.UI.Text s_Text;
    static int s_Value;

	void Start()
    {
        s_Text = GetComponent<UnityEngine.UI.Text>();
        s_Value = 0;
	}
	
    public static void AddScore(int score)
    {
        s_Value += score;
        s_Text.text = string.Format("{0}", s_Value);
	}
}
