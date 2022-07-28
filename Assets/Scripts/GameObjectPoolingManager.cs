using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolingManager : MonoBehaviour {
    [SerializeField] private List<GameObject> m_PoolingObjects;
    [SerializeField] private List<int> m_PoolingObjectsCount;
    [SerializeField] private List<GameObject> m_PoolingObjectsParentGameObject;    
    private int[] m_Counter;

    [HideInInspector] public static GameObjectPoolingManager instance;
    
    void Awake() {
        if(instance == null) instance = this;
        else if(instance != this) Destroy(gameObject);  

        m_Counter = new int[m_PoolingObjects.Count];
        //m_Pool = new List<List<GameObject>>(m_PoolingObjects.Count);

        CreateObjectPool();
    }


    private void CreateObjectPool() {
        for(int i = 0; i < m_PoolingObjects.Count; i++) {
            //List<GameObject> m_TempPool = new List<GameObject>(m_PoolingObjectsCount[i]);
            for(int j = 0; j < m_PoolingObjectsCount[i]; j++) {
                InstantiateObject(i);
            }

            m_Counter[i] = m_PoolingObjectsCount[i] - 1;
            //m_Pool.Add(m_TempPool);
        }
    }

    private void InstantiateObject(int index) {
        GameObject newObject = Instantiate(m_PoolingObjects[index]);
        newObject.transform.SetParent(m_PoolingObjectsParentGameObject[index].transform);
        newObject.SetActive(false);
        //m_TempPool.Add(newObject);
    }

    public GameObject GetGameObject(int objectIndex) {
        return GetPooledObject(objectIndex);
    }

    private GameObject GetPooledObject(int objectIndex) {
        int endIndex = m_Counter[objectIndex];

        for(int i = 0; i < endIndex; i++) {
            if(!m_PoolingObjectsParentGameObject[objectIndex].transform.GetChild(i).gameObject.activeInHierarchy) {
                return m_PoolingObjectsParentGameObject[objectIndex].transform.GetChild(i).gameObject;
            }
        }

        //create a new object and return it
        InstantiateObject(objectIndex);
        m_Counter[objectIndex]++;
        return m_PoolingObjectsParentGameObject[objectIndex].transform.GetChild(m_Counter[objectIndex]).gameObject;
    }

}
