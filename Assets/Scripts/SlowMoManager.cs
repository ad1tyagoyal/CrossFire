using UnityEngine;

public class SlowMoManager : MonoBehaviour {
    
    [SerializeField] private SlowMoManagerData m_SlowMoManagerData;

    private void Start() {
        m_SlowMoManagerData.slowMoActivated = false;
        m_SlowMoManagerData.remainingSlowMoTime = m_SlowMoManagerData.maxSlowMoTime;
        m_SlowMoManagerData.remainingSlowMotimeSlider.maxValue = m_SlowMoManagerData.maxSlowMoTime;
        m_SlowMoManagerData.remainingSlowMotimeSlider.value = m_SlowMoManagerData.remainingSlowMoTime;
    }

    private void Update() {
        if(Input.GetMouseButton(0) && m_SlowMoManagerData.remainingSlowMoTime > 0f) { 
            if(!m_SlowMoManagerData.slowMoActivated)
                OnSlowMoStart();
            
            m_SlowMoManagerData.slowMoActivated = true;
            m_SlowMoManagerData.remainingSlowMoTime -= Time.deltaTime;
        }
        else {
            if(m_SlowMoManagerData.slowMoActivated)
                OnSlowMoEnd();
            
            m_SlowMoManagerData.slowMoActivated = false;
            
            if(m_SlowMoManagerData.remainingSlowMoTime < m_SlowMoManagerData.maxSlowMoTime)
                m_SlowMoManagerData.remainingSlowMoTime += (Time.deltaTime / 10.0f);
        }

        m_SlowMoManagerData.remainingSlowMotimeSlider.value = m_SlowMoManagerData.remainingSlowMoTime;
    }

    private void OnSlowMoStart() {
        Time.timeScale = m_SlowMoManagerData.slowMoFraction;
        PlayerMovements.instance.OnSlowMoStart(ref m_SlowMoManagerData.slowMoFraction);
    }

    private void OnSlowMoEnd() {
        Time.timeScale = 1f;
        PlayerMovements.instance.OnSlowMoEnd(ref m_SlowMoManagerData.slowMoFraction);
    }
}
