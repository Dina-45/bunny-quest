using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;

    [Header("Vertical")]
    public float verticalDeadZone = 1.5f;
    public float maxHeightAbove = 6f;

    static CameraFollow current;

    float startY;
    Vector3 followPos;        // где камера была бы без тряски
    float shakeTime, shakeDuration, shakeStrength;

    void Awake() { current = this; }

    void Start()
    {
        startY = transform.position.y;
        followPos = transform.position;
    }

    public static void Shake(float strength, float duration)
    {
        if (current == null) return;
        // более сильная тряска не перебивается более слабой
        if (strength >= current.shakeStrength || current.shakeTime <= 0f)
        {
            current.shakeStrength = strength;
            current.shakeDuration = duration;
            current.shakeTime = duration;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        float goalY = startY;
        float above = target.position.y - (startY + verticalDeadZone);
        if (above > 0f)
            goalY = startY + Mathf.Min(above, maxHeightAbove);

        Vector3 goal = new Vector3(target.position.x, goalY, followPos.z);
        followPos = Vector3.Lerp(followPos, goal, smooth * Time.deltaTime);

        Vector3 offset = Vector3.zero;
        if (shakeTime > 0f)
        {
            shakeTime -= Time.unscaledDeltaTime;            // трясёт и во время hit stop
            float k = Mathf.Clamp01(shakeTime / shakeDuration);   // затухает к концу
            offset = (Vector3)(Random.insideUnitCircle * shakeStrength * k);
        }

        transform.position = followPos + offset;
    }
}