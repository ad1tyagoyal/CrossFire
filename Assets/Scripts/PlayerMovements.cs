using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour {
    //use mouse to rotate and move
    [SerializeField] private PlayerMovementData m_PlayerMovementData;

    [HideInInspector] public static PlayerMovements instance;

    

//unity methods

    private void Awake() {
        if(instance == null) instance = this;
        else if(instance != this) Destroy(gameObject);
    }

    private void Start() {
        m_PlayerMovementData.mainCamera = Camera.main;
        m_PlayerMovementData.m_Chances = m_PlayerMovementData.startingChances;
    }
    
    private void Update() {
        m_PlayerMovementData.tempAimDirection = m_PlayerMovementData.mainCamera.ScreenToWorldPoint(Input.mousePosition) 
                                                                            - m_PlayerMovementData.gFxTransform.position;
        RotationLook(ref m_PlayerMovementData.tempAimDirection, ref m_PlayerMovementData.rotationSpeed);    
        FollowMouse(ref m_PlayerMovementData.tempAimDirection, ref m_PlayerMovementData.movingSpeed);

        if(!IsPlayerInBounds(m_PlayerMovementData.playerTransform.position, 
                                        ref m_PlayerMovementData.movementBounds)) {
            BringPlayerBackInBounds(ref m_PlayerMovementData.playerTransform, 
                                        ref m_PlayerMovementData.movementBounds);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Bullet")) {
            other.gameObject.SetActive(false);
            OnHit();
        }
    }

//user defined methods

    private void RotationLook(ref Vector2 aimDirection, ref float speed) {
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        Quaternion tempRotation = Quaternion.AngleAxis(angle, m_PlayerMovementData.gFxTransform.forward);
        m_PlayerMovementData.gFxTransform.rotation = Quaternion.Slerp(m_PlayerMovementData.gFxTransform.rotation, tempRotation, 
                                                                        m_PlayerMovementData.rotationSpeed * Time.deltaTime);
    }

    private void FollowMouse(ref Vector2 moveDirection, ref float speed) {
        float distance = moveDirection.magnitude;
        if(distance > m_PlayerMovementData.maxRadiusDistance) {
            m_PlayerMovementData.playerTransform.Translate(moveDirection.normalized * speed * Time.deltaTime);
        }
    }

    private void OnHit() {
        m_PlayerMovementData.m_Chances--;
        if(m_PlayerMovementData.m_Chances <= 0) {
            Debug.Log("Player Die!!!");
        }
    }

    private bool IsPlayerInBounds(Vector2 playerPos, ref Bounds bounds) {
        return ((playerPos.x >= bounds.minX && playerPos.x <= bounds.maxX) 
                            && (playerPos.y >= bounds.minY && playerPos.y <= bounds.maxY));
    }

    private void BringPlayerBackInBounds(ref Transform player, ref Bounds bounds) {
        float x = Mathf.Clamp(player.position.x, bounds.minX, bounds.maxX);
        float y = Mathf.Clamp(player.position.y, bounds.minY, bounds.maxY);
        player.position = new Vector3(x, y, 0f);
    }
//changing rotation and moving speed so that player move at same rate even in slow-mo

    public void OnSlowMoStart(ref float slowMoFraction) {
    //change rotation and moving speed
        m_PlayerMovementData.movingSpeed /= (2 * slowMoFraction);
        m_PlayerMovementData.rotationSpeed /= (2 * slowMoFraction);
    }

    public void OnSlowMoEnd(ref float slowMoFraction) {
    //change rotation and moving speed back to normal
        m_PlayerMovementData.movingSpeed *= (2 * slowMoFraction);
        m_PlayerMovementData.rotationSpeed *= (2 * slowMoFraction);
    }

}

