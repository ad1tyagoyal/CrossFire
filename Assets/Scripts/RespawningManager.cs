using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningManager : MonoBehaviour {
    [SerializeField] private Bounds m_SpawningBounds;

    [HideInInspector] public static RespawningManager instance;

    
    void Awake() {
        if(instance == null) instance = this;
        else if(instance != this) Destroy(gameObject); 
    }

    public void ShowEnemyRespawningGFx() {

    }

    public void SpawnEnemy(ref Transform enemyTransform) {
        float xCoor = Random.Range(m_SpawningBounds.minX, m_SpawningBounds.maxX);
        float yCoor = Random.Range(m_SpawningBounds.minY, m_SpawningBounds.maxY);
        enemyTransform.position = new Vector3(xCoor, yCoor, 0f);
    }
}
