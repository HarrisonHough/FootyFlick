using UnityEngine;

public class WindPanel : MonoBehaviour
{
    [SerializeField]
    private RectTransform windArrow;
    [SerializeField]
    private Animator animator;
    
    private void OnEnable()
    {
        WindControl.OnWindChanged += OnWindChanged;
        BallLauncher.OnBallLaunched += OnBallLaunched;
    }

    private void OnDisable()
    {
        WindControl.OnWindChanged -= OnWindChanged;
        BallLauncher.OnBallLaunched -= OnBallLaunched;
    }

    private void OnBallLaunched()
    {
        animator.speed = 0f;
    }

    public void OnWindChanged(WindData windData)
    {
        animator.speed = 1f;
        var angle = Mathf.Lerp(90, -90, Mathf.InverseLerp(-WindControl.Instance.WindStrengthRange, WindControl.Instance.WindStrengthRange, windData.Force));
        windArrow.localEulerAngles = new Vector3(0, 0, angle);
    }
}
