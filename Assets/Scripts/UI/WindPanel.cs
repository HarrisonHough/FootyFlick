using UnityEngine;

public class WindPanel : MonoBehaviour
{
    [SerializeField]
    private RectTransform windArrow;

    private void Awake()
    {
        WindControl.OnWindChanged += OnWindChanged;
    }

    public void OnWindChanged(WindData windData)
    {
        var angle = Mathf.Lerp(90, -90, Mathf.InverseLerp(-6, 6, windData.Force));
        windArrow.localEulerAngles = new Vector3(0, 0, angle);
    }
}
