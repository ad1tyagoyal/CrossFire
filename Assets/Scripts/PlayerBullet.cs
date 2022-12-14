using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    private Transform m_Transform;
    
    void Start() {
        m_Transform = GetComponent<Transform> ();    
    }
 
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("bounds")) {
            PlayerAbilitiesManager.instance.RemovePlayerBullet(ref m_Transform);
            gameObject.SetActive(false);
        } 
        else if(other.gameObject.CompareTag("shield")) {
            transform.up *= -1.0f;
        }
    }
    
}
