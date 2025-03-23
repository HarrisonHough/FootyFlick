using UnityEngine;

public class WindPanel : MonoBehaviour
{
    [SerializeField]
    private RectTransform windArrow;
    [SerializeField]
    private WindControl windControl;
    [SerializeField]
    private Animator animator;
    private void Awake()
    {
        WindControl.OnWindChanged += OnWindChanged;
        BallLauncher.OnBallLaunched += OnBallLaunched;
    }

    private void OnDestroy()
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
        var angle = Mathf.Lerp(90, -90, Mathf.InverseLerp(-windControl.WindStrengthRange, windControl.WindStrengthRange, windData.Force));
        windArrow.localEulerAngles = new Vector3(0, 0, angle);
    }
}
