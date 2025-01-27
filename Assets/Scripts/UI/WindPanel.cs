using UnityEngine;

public class WindPanel : MonoBehaviour
{
    [SerializeField]
    private RectTransform windArrow;

    private void Awake()
    {
        BallLauncher.OnWindChanged += OnWindChanged;
    }

    public void OnWindChanged(float windStrength)
    {
        // remap wind strength to arrow rotation
        var angle = Mathf.Lerp(90, -90, Mathf.InverseLerp(-10, 10, windStrength));
        windArrow.localEulerAngles = new Vector3(0, 0, angle);
    }
}
