using UnityEngine;


public class FpsCounter : MonoBehaviour
{
    float smoothDelta;
    GUIStyle style;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Create()
    {
        Application.targetFrameRate = 60;  
        GameObject go = new GameObject("FpsCounter");
        go.AddComponent<FpsCounter>();
        DontDestroyOnLoad(go);
    }

    void Update()
    {
        smoothDelta = Mathf.Lerp(smoothDelta, Time.unscaledDeltaTime, 0.1f);
    }

    void OnGUI()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.fontSize = Screen.height / 30;
            style.fontStyle = FontStyle.Bold;
        }

        float fps = smoothDelta > 0f ? 1f / smoothDelta : 0f;
        style.normal.textColor = fps >= 30f ? Color.green : Color.red;

        Rect r = new Rect(Screen.width / 2f - 100f, Screen.safeArea.yMin + 10f, 200f, 60f);
        GUI.Label(r, Mathf.RoundToInt(fps) + " FPS", style);
    }
}