using DG.Tweening;
using UnityEngine;

public class CarouselElement : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float scaleActive = 1f;
    [SerializeField] private float scaleInactive = 0.85f;
    [SerializeField] private float fadeInactive = 0.5f;
    [SerializeField] private float animDuration = 0.3f;
    [SerializeField] private Ease animEase = Ease.OutCubic;
    
    private RectTransform rect;
    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public virtual void AnimateToActive(bool isActive)
    {
        var targetScale = isActive ? scaleActive : scaleInactive;
        var targetAlpha = isActive ? 1f : fadeInactive;

        rect.DOScale(targetScale, animDuration).SetEase(animEase);
        canvasGroup.DOFade(targetAlpha, animDuration).SetEase(animEase);
    }
}
