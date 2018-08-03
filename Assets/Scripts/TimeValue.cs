using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeValue : MonoBehaviour {

    UnityEngine.UI.Text m_Text;
    float m_Time;

	void Start()
    {
        m_Text = GetComponent<UnityEngine.UI.Text>();
        m_Time = Time.time;
	}
	
    void Update()
    {
        var elapsed = Time.time - m_Time;
        m_Text.text = string.Format("{0:f2}", elapsed);
    }
}
