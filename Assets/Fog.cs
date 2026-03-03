using UnityEngine;

public class FogController : MonoBehaviour
{
    public Color fogColor = Color.gray;
    public float fogDensity = 0.02f;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential; // Linear / Exponential / ExponentialSquared
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
    }
}