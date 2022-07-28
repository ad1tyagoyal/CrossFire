using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Abilities:
//     1. Ghost
//     2. Crusher
//     3. Shield
//     4. Shooting
//     5. Score Multiplier    





public class PlayerAbilitiesManager : MonoBehaviour {

    [SerializeField] private PlayerAbilitiesManagerData m_Data;


    [HideInInspector] public static PlayerAbilitiesManager instance;


    void Awake() {
        if(instance == null) instance = this;
        else if(instance != this) Destroy(gameObject);
    } 

    
    void Start() {
        m_Data.polygonCollider = GetComponent<PolygonCollider2D> ();
    }


    void Update() {
        AddVelocityToBullets();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("ability_ghost"))
            StartCoroutine(ActivateGhostAbility());
        else if(other.gameObject.CompareTag("ability_crusher")) 
            StartCoroutine(ActivateCrusherAbility());
        else if(other.gameObject.CompareTag("ability_shield")) 
            StartCoroutine(ActivateShieldAbility());
        else if(other.gameObject.CompareTag("ability_shooting")) 
            StartCoroutine(ActivateShootingAbility());
        else if(other.gameObject.CompareTag("ability_score_multiply")) 
            StartCoroutine(ActivateScoreMultiplyingAbility());
        else if(other.gameObject.CompareTag("ability_speed_decreser")) 
            StartCoroutine(ActivateScoreMultiplyingAbility());
    }

//Ghost Ability
    private IEnumerator ActivateGhostAbility() {
        Debug.Log("Ghost");
        m_Data.polygonCollider.enabled = false;
        m_Data.spriteRenderer.color = m_Data.ghostAbilityColor;
        yield return new WaitForSeconds(m_Data.abilityTime);
        BackToNormal();
    }

//Crusher Ability
    private IEnumerator ActivateCrusherAbility() {
        Debug.Log("Crusher");
        m_Data.crusherGameObject.SetActive(true);
        m_Data.spriteRenderer.color = m_Data.crusherAbilityColor;
        yield return new WaitForSeconds(m_Data.abilityTime);
        BackToNormal();
    }

//Shield Ability 
    private IEnumerator ActivateShieldAbility() {
        Debug.Log("Shield");
        m_Data.shieldGameObject.SetActive(true);
        m_Data.spriteRenderer.color = m_Data.shieldAbilityColor;
        yield return new WaitForSeconds(m_Data.abilityTime);
        BackToNormal();
    }

//Shooting Ability
    private IEnumerator ActivateShootingAbility() {
        Debug.Log("Shooting");
        m_Data.shootingGunsTransform.gameObject.SetActive(true);
        m_Data.spriteRenderer.color = m_Data.shootingAbilityColor;
        StartCoroutine(ShootEnemies());
        yield return new WaitForSeconds(m_Data.abilityTime);
        BackToNormal();
    }

    private IEnumerator ShootEnemies() {
        GameObject bullet = GameObjectPoolingManager.instance
                                                    .GetGameObject(PlayerAbilitiesManagerData.PLAYER_BULLET_INDEX);
        Transform bulletTransform = bullet.transform;
        AddPlayerBullet(ref bulletTransform);
        bulletTransform.position = m_Data.shootingGunsTransform.position;
        bullet.transform.up = m_Data.shootingGunsTransform.up;
        bullet.SetActive(true);
        yield return new WaitForSeconds(m_Data.shootingCoolDownTime);
        StartCoroutine(ShootEnemies());
    }

    public void RemovePlayerBullet(ref Transform bulletTransform) {
        m_Data.activePlayerBullets.Add(bulletTransform);
    }

    public void AddPlayerBullet(ref Transform bulletTransform) {
        m_Data.activePlayerBullets.Add(bulletTransform);
    } 

    private void AddVelocityToBullets() {
        foreach(Transform t in m_Data.activePlayerBullets) {
            t.position += (t.up * m_Data.playerBulletSpeed * Time.deltaTime); 
        }
    }

//Score Multiplyer Ability
    private IEnumerator ActivateScoreMultiplyingAbility() {
        Debug.Log("Score Multiplier");
        ScoreManager.instance.ChangeScoreMultiplyingFactor(m_Data.scoreMultiplyingFactor);
        yield return new WaitForSeconds(m_Data.abilityTime);
        ScoreManager.instance.ChangeScoreMultiplyingFactor(1);
        BackToNormal();
    }


    private void BackToNormal() {
        m_Data.spriteRenderer.color = m_Data.normalColor;
        m_Data.crusherGameObject.SetActive(false);
        m_Data.shieldGameObject.SetActive(false);
        m_Data.shootingGunsTransform.gameObject.SetActive(false);
        StopCoroutine(ShootEnemies());
        m_Data.polygonCollider.enabled = true;
    }
    
}
