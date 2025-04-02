using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class GameModePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float scaleActive = 1f;
    [SerializeField] private float scaleInactive = 0.85f;
    [SerializeField] private float fadeInactive = 0.5f;
    [SerializeField] private float animDuration = 0.3f;
    [SerializeField] private Ease animEase = Ease.OutCubic;
    [SerializeField] private GameModeEnum gameMode;
    
    private RectTransform rect;
    
    public UnityEvent<GameModeEnum> OnGameModeSelected;
    
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void AnimateToActive(bool isActive)
    {
        var targetScale = isActive ? scaleActive : scaleInactive;
        var targetAlpha = isActive ? 1f : fadeInactive;

        rect.DOScale(targetScale, animDuration).SetEase(animEase);
        canvasGroup.DOFade(targetAlpha, animDuration).SetEase(animEase);
    }
    
    public void SelectGameMode()
    {
        OnGameModeSelected?.Invoke(gameMode);
    }
}
