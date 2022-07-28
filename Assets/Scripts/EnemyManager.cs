using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour {
    
    public GameObject enemy;
    public GameObject missileEnemy;

    [SerializeField] private EnemyManagerData m_EnemyManagerData; 

    [HideInInspector] public static EnemyManager instance;

    void Awake() {
        if(instance == null) instance = this;
        else if(instance != this) Destroy(gameObject);
    }

    private void Start() {
        m_EnemyManagerData.activeEnimiesTransform = new List<Transform>(m_EnemyManagerData.enemiesCount);
        m_EnemyManagerData.activeEnimiesBarrelTransform = new List<Transform>(m_EnemyManagerData.enemiesCount); 
        ref Bounds bounds = ref m_EnemyManagerData.movementBounds;

        for(int i = 0; i < m_EnemyManagerData.enemiesCount; i++) {
            GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(bounds.minX, bounds.maxX), 
                                                Random.Range(bounds.minY, bounds.maxY), 0f), Quaternion.identity);
            m_EnemyManagerData.activeEnimiesTransform.Add(newEnemy.transform);
            m_EnemyManagerData.activeEnimiesBarrelTransform.Add(newEnemy.transform.GetChild(0));
        }

        StartCoroutine(ShootBulletsToPlayer());
        StartCoroutine(ChangeDistanceBetweenEnemies());
    }

    private void Update() {
        UpdateActiveEnemiesTransform();
        //UpdateActiveMissileEnemiesTransform();
        KeepDistanceBtwEnemies(ref m_EnemyManagerData.activeEnimiesTransform, 
                            ref m_EnemyManagerData.minDistanceBtwEnemies, ref m_EnemyManagerData.maxDistanceBtwEnemies); 
        AddVelocityToBullets();
    }

    private void UpdateActiveEnemiesTransform() {
        foreach(Transform t in m_EnemyManagerData.activeEnimiesTransform) {
            Vector2 enemyToTargetVec = (Vector2) (m_EnemyManagerData.targetTransform.position - t.position);
        //rotate
            float angle = Mathf.Atan2(enemyToTargetVec.y, enemyToTargetVec.x) * Mathf.Rad2Deg;
            Quaternion tempRotation = Quaternion.AngleAxis(angle, t.forward);
            t.rotation = Quaternion.Slerp(t.rotation, tempRotation, m_EnemyManagerData.rotationSpeed * Time.deltaTime);
        //chase
            if(enemyToTargetVec.magnitude >= m_EnemyManagerData.reach && 
                                            enemyToTargetVec.magnitude <= m_EnemyManagerData.lookRadius) { 
                t.position += (Vector3) enemyToTargetVec.normalized * m_EnemyManagerData.movingSpeed * Time.deltaTime;
            }
        //clamp position
            float x = Mathf.Clamp(t.position.x, m_EnemyManagerData.movementBounds.minX, 
                                                                            m_EnemyManagerData.movementBounds.maxX);
            float y = Mathf.Clamp(t.position.y, m_EnemyManagerData.movementBounds.minY, 
                                                                            m_EnemyManagerData.movementBounds.maxY);
                
            t.position = new Vector3(x, y, 0f);
        }
    }

//yet to call
    private void UpdateActiveMissileEnemiesTransform() {
        foreach(Transform t in m_EnemyManagerData.activeMissileEnemiesTransform) {
            Vector2 enemyToTargetVec = (Vector2) (m_EnemyManagerData.targetTransform.position - t.position);
        //rotate
            float angle = Mathf.Atan2(enemyToTargetVec.y, enemyToTargetVec.x) * Mathf.Rad2Deg;
            Quaternion tempRotation = Quaternion.AngleAxis(angle, t.forward);
            t.rotation = Quaternion.Slerp(t.rotation, tempRotation, m_EnemyManagerData.missileEnemyRotationSpeed
                                                                                     * Time.deltaTime);
        //chase
            if(enemyToTargetVec.magnitude >= m_EnemyManagerData.missileEnemyReach && 
                                        enemyToTargetVec.magnitude <= m_EnemyManagerData.missileEnemyLookRadius) { 
                t.position += (Vector3) enemyToTargetVec.normalized * m_EnemyManagerData.missileEnemyMovingSpeed 
                                                                                                    * Time.deltaTime;
            }
        //clamp position
            float x = Mathf.Clamp(t.position.x, m_EnemyManagerData.movementBounds.minX, 
                                                                            m_EnemyManagerData.movementBounds.maxX);
            float y = Mathf.Clamp(t.position.y, m_EnemyManagerData.movementBounds.minY, 
                                                                            m_EnemyManagerData.movementBounds.maxY);
                
            t.position = new Vector3(x, y, 0f);
        }
    }

    private void KeepDistanceBtwEnemies(ref List<Transform> enemiesList, ref float minDistance, ref float maxDistance) {
        int length = enemiesList.Count;
        for(int i = 0; i < length; i++) {
            //for enemies
            for(int j = i+1; j < length; j++) {
                float distance = i%2 == 0 ? minDistance : maxDistance;
                Vector2 enemyToEnemyVector = (Vector2) (enemiesList[j].position - enemiesList[i].position);
                if(enemyToEnemyVector.magnitude < distance) {
                //too close
                    //float pushBy = 1.5f - enemyToEnemyVector.magnitude;
                    Vector3 pushbyVector = -1 * enemyToEnemyVector.normalized;
                    enemiesList[i].position += (Vector3) pushbyVector * m_EnemyManagerData.movingSpeed * Time.deltaTime;
                }
            }
            //for player        
            Vector2 enemyToPlayerVector = (Vector2) (m_EnemyManagerData.targetTransform.position - 
                                                                                            enemiesList[i].position);
            if(enemyToPlayerVector.magnitude < m_EnemyManagerData.minDistanceFromPlayer) {
            //too close
                //float pushBy = 1.5f - enemyToEnemyVector.magnitude;
                Vector3 pushbyVector = -1 * enemyToPlayerVector.normalized;
                enemiesList[i].position += (Vector3) pushbyVector * m_EnemyManagerData.movingSpeed * Time.deltaTime;
            }
        }
    }

    private IEnumerator ChangeDistanceBetweenEnemies() {
        float minDistance = Random.Range(3.0f, 6.0f);
        float maxDistance = Random.Range(minDistance, (minDistance + 6.0f));
        m_EnemyManagerData.minDistanceBtwEnemies = minDistance;
        m_EnemyManagerData.maxDistanceBtwEnemies = maxDistance;
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(ChangeDistanceBetweenEnemies());
    }
    
    private IEnumerator ShootBulletsToPlayer() {
        for(int i = 0; i < m_EnemyManagerData.activeEnimiesTransform.Count; i++) {
            Transform t = m_EnemyManagerData.activeEnimiesTransform[i];
            Vector2 enemyToTargetVec = (Vector2) (m_EnemyManagerData.targetTransform.position - t.position);
            if(enemyToTargetVec.magnitude <= m_EnemyManagerData.firingRadius) {
            //fire
                GameObject bullet = GameObjectPoolingManager.instance.GetGameObject(EnemyManagerData.BULLET_INDEX);
                m_EnemyManagerData.activeBulletsTransform.Add(bullet.transform);
                bullet.transform.position = m_EnemyManagerData.activeEnimiesBarrelTransform[i].position;
                bullet.transform.up = m_EnemyManagerData.activeEnimiesBarrelTransform[i].up;
                bullet.SetActive(true);
            //add recoil                                                                                                  
            }
            yield return new WaitForSeconds(Random.Range(m_EnemyManagerData.minCoolDownTime, 
                                                                            m_EnemyManagerData.maxCoolDownTime));
        }
        StartCoroutine(ShootBulletsToPlayer());
    }

    private void AddVelocityToBullets() {
        foreach(Transform t in m_EnemyManagerData.activeBulletsTransform) {
            t.position += (t.up * m_EnemyManagerData.bulletVelocity * Time.deltaTime); 
        }
    }

//yet to call
    private IEnumerator SpawnMissileEnemies() {
        Bounds bounds = m_EnemyManagerData.movementBounds;
        GameObject enemy = Instantiate(missileEnemy, new Vector3(Random.Range(bounds.minX, bounds.maxX), 
                                                Random.Range(bounds.minY, bounds.maxY), 0f), Quaternion.identity);
        m_EnemyManagerData.activeMissileEnemiesTransform.Add(enemy.transform);
        yield return new WaitForSeconds(0.0f);
    }

    public void RemoveBullet(ref Transform bullet) {
        m_EnemyManagerData.activeBulletsTransform.Remove(bullet);
    }
    
    public void AddBullet(ref Transform bullet) {
        m_EnemyManagerData.activeBulletsTransform.Add(bullet);
    }

    public void RemoveEnemy(ref Transform enemy) {
    //remove enemy
        m_EnemyManagerData.activeEnimiesTransform.Remove(enemy);
    //remove enemy barrel
        m_EnemyManagerData.activeEnimiesBarrelTransform.Remove(enemy.GetChild(0));
    }
    
    public void AddEnemy(ref Transform enemy) {
    //add enemy
        m_EnemyManagerData.activeEnimiesTransform.Add(enemy);
    //add enemy barrel
        m_EnemyManagerData.activeEnimiesBarrelTransform.Add(enemy.GetChild(0));
    }

    

}

