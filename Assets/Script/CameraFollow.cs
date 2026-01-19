using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offsetX = 3f; 
    public float cameraFixedY = 0f; 
    public float cameraFixedZ = -10f; 

    void LateUpdate()
    {
        if (target != null)
        {
           
            Vector3 newPosition = new Vector3(target.position.x + offsetX, cameraFixedY, cameraFixedZ);

            transform.position = newPosition;

            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}