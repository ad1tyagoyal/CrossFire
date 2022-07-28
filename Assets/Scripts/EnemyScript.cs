using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    
    [SerializeField] private GameObject gFx;
    private Transform m_Transform;
    
    
    void Start() {
        gFx.SetActive(true);
        m_Transform = GetComponent<Transform> ();
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("crusher")) {
            StartCoroutine(OnHit());
        }   
    }

    private IEnumerator OnHit() {
    //remove enemy from active list
        EnemyManager.instance.RemoveEnemy(ref m_Transform);
        gFx.SetActive(false);
    //respawning gfx
        RespawningManager.instance.ShowEnemyRespawningGFx();
        yield return new WaitForSeconds(1.0f);
    //spwaning enemy
        RespawningManager.instance.SpawnEnemy(ref m_Transform);
        yield return new WaitForSeconds(1.0f);
    //add enemy to active list
        EnemyManager.instance.AddEnemy(ref m_Transform);
        gFx.SetActive(true);
    }

}
