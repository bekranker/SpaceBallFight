using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveManager : MonoBehaviour
{
    private static int keyID = Shader.PropertyToID("_WaveDistanceFromCenter");
    public Material Material;
    [SerializeField, Range(0.05f, 3)] private float _ShockWaveSpeed;

    public void CallShockWave()
    {
        StartCoroutine(CallShockWaveIE());
    }

    private IEnumerator CallShockWaveIE()
    {
        float lerpedAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < 1)
        {
            elapsedTime += Time.unscaledDeltaTime;
            lerpedAmount = Mathf.Lerp(0, 1, (elapsedTime / _ShockWaveSpeed));
            Material.SetFloat(keyID, lerpedAmount);
            yield return null;
        }
    }

}
