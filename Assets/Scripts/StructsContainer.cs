using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StructsContainer{ }

[System.Serializable]
public struct PlayerMovementData {
[Header ("Constants")]

    public float maxRadiusDistance;
    public float rotationSpeed;
    public float movingSpeed;
    public Transform gFxTransform;
    public Transform playerTransform;
    public int startingChances;
    public Bounds movementBounds;

//var for internal use
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public Vector2 tempAimDirection;
    [HideInInspector] public int m_Chances;

}

[System.Serializable] 
public struct EnemyManagerData {

    public int enemiesCount;

[Space]

[Header ("Movement")]

    public float movingSpeed;
    public float rotationSpeed;
    public float reach;
    public float lookRadius;
    public Transform targetTransform;
    public float minDistanceBtwEnemies;
    public float maxDistanceBtwEnemies;
    public float minDistanceFromPlayer;
    public Bounds movementBounds;

    public float missileEnemyMovingSpeed;
    public float missileEnemyRotationSpeed;
    public float missileEnemyReach;
    public float missileEnemyLookRadius;

[Space]

[Header ("Shooting")]

    public float firingRadius;
    public float minCoolDownTime;
    public float maxCoolDownTime;
    public float bulletVelocity;
    public float recoilAfterFiring;
    [HideInInspector] public const int BULLET_INDEX = 0;

[Space]    

    [HideInInspector] public List<Transform> activeEnimiesTransform;
    [HideInInspector] public List<Transform> activeEnimiesBarrelTransform;
    [HideInInspector] public List<Transform> activeBulletsTransform;
    [HideInInspector] public List<Transform> activeMissileEnemiesTransform;
}

[System.Serializable] 
public struct SlowMoManagerData {
[Header("Constants")]
    public float slowMoFraction;
    public float maxSlowMoTime;

[Space]

[Header("UI")]
    public Slider remainingSlowMotimeSlider;

//var for internal use
    [HideInInspector] public bool slowMoActivated;
    [HideInInspector] public float remainingSlowMoTime;
}

[System.Serializable]
public struct Bounds {
    public float minY, maxY;
    public float minX, maxX;
}

[System.Serializable]
public struct CameraData {
    public Transform camTransform;
    public Transform targetTransform;
    public float movingSpeed;
}

[System.Serializable]
public struct PlayerAbilitiesManagerData {

[Header("Constants")]
    public float abilityTime;
    public float shootingCoolDownTime;
    public float playerBulletSpeed;
    [HideInInspector] public const int PLAYER_BULLET_INDEX = 1;
    public SpriteRenderer spriteRenderer;
    public int scoreMultiplyingFactor;

[Header("Abilities GameObjects")]
    public GameObject crusherGameObject;
    public GameObject shieldGameObject;
    public Transform shootingGunsTransform;
    public GameObject vulnerabilityGameObject;
    public GameObject scoreMultiplierGameObject;  

[Space]

[Header("Abilities Color")]
    public Color normalColor;
    public Color ghostAbilityColor;
    public Color crusherAbilityColor;
    public Color shieldAbilityColor;
    public Color shootingAbilityColor;
    public Color vulnerabilityAbilityColor;
    public Color scoreMultiplierAbilityColor;

//for internal use
    [HideInInspector] public float currAbilityTimer;
    [HideInInspector] public PolygonCollider2D polygonCollider;
    [HideInInspector] public List<Transform> activePlayerBullets;
}

[System.Serializable] 
public struct ScoreManagerData {
    public Text scoreText;
    [HideInInspector] public int score; 
    [HideInInspector] public int scoreMultiplyingFactor; 
}




