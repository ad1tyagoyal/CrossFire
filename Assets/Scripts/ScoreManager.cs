using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    [SerializeField] private ScoreManagerData m_Data;

    [HideInInspector] public static ScoreManager instance;

    void Awake() {
        if(instance == null) instance = this;
        else if(instance != this) Destroy(gameObject);        
    }

    private void Start() {
        m_Data.score = 0;
        m_Data.scoreText.text = m_Data.score + ""; 
        m_Data.scoreMultiplyingFactor = 1;       
    }

    public void IncreaseScore(int points) {
        int change = m_Data.scoreMultiplyingFactor * points;
        m_Data.score += change;
        m_Data.scoreText.text = m_Data.score + "";
    }

    public void ChangeScoreMultiplyingFactor(int factor) {
        m_Data.scoreMultiplyingFactor = factor;
    }
    
}
