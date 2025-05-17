using System;
using UnityEngine;

public class WindControl : MonoBehaviour
{
    public static WindControl Instance;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float windStrengthRange = 4f; 

    
    public float WindStrengthRange => windStrengthRange;
    public static Action<WindData> OnWindChanged;
    public float WindForce => windData.Force;
    public Vector3 WindDirection => windData.Direction;
    private WindData windData;
    
    private void Start()
    {
        windData = new WindData();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void RandomizeWindStrength()
    {
        windData.Force = UnityEngine.Random.Range(-windStrengthRange, windStrengthRange);
        SetWindDirection();
        OnWindChanged?.Invoke(windData);
    }
    
    public void SetWindStrength(float strength)
    {
        windData.Force = strength;
        SetWindDirection();
        OnWindChanged?.Invoke(windData);
    }
    
    private void SetWindDirection()
    {
        windData.Direction = cameraTransform.right.normalized;
        OnWindChanged?.Invoke(windData);
    }
}
