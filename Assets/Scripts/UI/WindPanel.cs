using System;
using UnityEngine;

public class WindPanel : MonoBehaviour
{
    [SerializeField]
    private RectTransform windArrow;
    [SerializeField]
    private WindControl windControl;
    private void Awake()
    {
        WindControl.OnWindChanged += OnWindChanged;
    }

    private void OnDestroy()
    {
        WindControl.OnWindChanged -= OnWindChanged;
    }

    public void OnWindChanged(WindData windData)
    {
        var angle = Mathf.Lerp(90, -90, Mathf.InverseLerp(-windControl.WindStrengthRange, windControl.WindStrengthRange, windData.Force));
        windArrow.localEulerAngles = new Vector3(0, 0, angle);
    }
}
