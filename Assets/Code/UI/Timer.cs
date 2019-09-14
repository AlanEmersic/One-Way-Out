using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI text;
    static float time;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (time <= 0)
        {
            time = 0;
            text.text = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(time / 60), Mathf.FloorToInt(time - Mathf.FloorToInt(time / 60) * 60));
        }
        else
        {
            time -= Time.deltaTime;
            text.text = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(time / 60), Mathf.FloorToInt(time % 60));
        }
    }

    public static void StartTimer()
    {
        time = 120f;
    }
}

