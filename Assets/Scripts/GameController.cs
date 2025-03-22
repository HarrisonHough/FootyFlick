using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float windStrengthRange = 6f; 
    [SerializeField] private float windChangeInterval = 5f;
    
    public static Action<WindData> OnWindChanged;
    
    private void Start()
    {

    }

    private void RandomizeWindStrength()
    {
        var windData = new WindData()
        {
            Force = UnityEngine.Random.Range(-windStrengthRange, windStrengthRange), 
            Direction = cameraTransform.right.normalized
        };
        OnWindChanged?.Invoke(windData);
    }
}

