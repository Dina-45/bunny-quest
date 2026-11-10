using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;

    [Header("Vertical")]
    public float verticalDeadZone = 1.5f;  
    public float maxHeightAbove = 6f;    

    float startY;

    void Start()
    {
        startY = transform.position.y;    
    }

    void LateUpdate()
    {
        if (target == null) return;

        float goalY = startY;
        float above = target.position.y - (startY + verticalDeadZone);
        if (above > 0f)
            goalY = startY + Mathf.Min(above, maxHeightAbove);

        Vector3 goal = new Vector3(target.position.x, goalY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, goal, smooth * Time.deltaTime);
    }
}