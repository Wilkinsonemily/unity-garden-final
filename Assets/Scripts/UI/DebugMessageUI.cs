using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugMessageUI : MonoBehaviour
{
    public static DebugMessageUI Instance; 

    [Header("UI Settings")]
    public TextMeshProUGUI debugText;
    public float displayTime = 2f;
    private Coroutine showRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (debugText != null)
            debugText.text = "";
    }

    public void ShowMessage(string message)
    {
        Debug.Log(message); 
        if (showRoutine != null)
            StopCoroutine(showRoutine);

        showRoutine = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        debugText.text = message;
        debugText.alpha = 1f;

        yield return new WaitForSeconds(displayTime);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            debugText.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        debugText.text = "";
    }
}
