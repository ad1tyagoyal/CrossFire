using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField] private CameraData m_CameraData;


    void LateUpdate() {
        Vector3 endPos = m_CameraData.targetTransform.position;
        endPos.z = m_CameraData.camTransform.position.z;
        m_CameraData.camTransform.position = endPos;  
    }

    private void UpdateCameraTransform(ref Transform target, ref Transform camera, ref float speed) {
        Vector2 camToTargeteVec = (Vector2) (target.position - camera.position);
        camera.position += (Vector3) (camToTargeteVec.normalized * speed * Time.deltaTime);
    }
    
}
