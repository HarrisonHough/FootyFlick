using System;
using UnityEngine;

public class WindControl : MonoBehaviour
{
    public static WindControl Instance;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float windStrengthRange = 6f; 
    [SerializeField] private float windChangeInterval = 5f;
    private WindData windData;
    
    public static Action<WindData> OnWindChanged;
    public float WindForce => windData.Force;
    public Vector3 WindDirection => windData.Direction;
    
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
            return;
        }
    }
    
    public void RandomizeWindStrength()
    {
        windData.Force = UnityEngine.Random.Range(-windStrengthRange, windStrengthRange);
        windData.Direction = cameraTransform.right.normalized;
        OnWindChanged?.Invoke(windData);
    }
}
