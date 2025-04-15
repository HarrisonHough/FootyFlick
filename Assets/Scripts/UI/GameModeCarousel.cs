using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CarouselUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform content;
    [SerializeField] private float panelWidth = 800f;
    [SerializeField] private float scrollDuration = 0.3f;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private int currentIndex = 0;
    private int panelCount;

    private Vector2 dragStartPos;

    private void Start()
    {
        panelCount = content.childCount;
        
        var firstPanel = content.GetChild(0).GetComponent<RectTransform>();
        var spacing = content.GetComponent<HorizontalLayoutGroup>()?.spacing ?? 0f;
        panelWidth = firstPanel.rect.width + spacing;

        leftButton.onClick.AddListener(() => Scroll(1));
        rightButton.onClick.AddListener(() => Scroll(-1));

        SnapToIndex(1);
        UpdateButtons();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragEndPos = eventData.position;
        float deltaX = dragEndPos.x - dragStartPos.x;

        if (Mathf.Abs(deltaX) > 50f) // Adjust swipe sensitivity
        {
            if (deltaX < 0 && currentIndex < panelCount - 1)
                Scroll(1); // swipe left = go right
            else if (deltaX > 0 && currentIndex > 0)
                Scroll(-1); // swipe right = go left
        }
    }

    private void Scroll(int direction)
    {
        int newIndex = Mathf.Clamp(currentIndex + direction, 0, panelCount - 1);
        if (newIndex == currentIndex) return;

        currentIndex = newIndex;
        Vector2 target = new Vector2(-panelWidth * currentIndex, 0f);
        UpdatePanelStates();
        UpdateButtons();
        content.DOAnchorPos(target, scrollDuration)
               .SetEase(Ease.OutCubic)
               .OnComplete(() =>
               {
               });
    }

    private void SnapToIndex(int index)
    {
        currentIndex = Mathf.Clamp(index, 0, panelCount - 1);
        content.anchoredPosition = new Vector2(-panelWidth * currentIndex, 0f);
        UpdatePanelStates();
        UpdateButtons();
    }

    private void UpdatePanelStates()
    {
        for (var i = 0; i < panelCount; i++)
        {
            var carouselElement = content.GetChild(i).GetComponent<CarouselElement>();
            if (carouselElement != null)
                carouselElement.AnimateToActive(i == currentIndex);
        }
    }

    private void UpdateButtons()
    {
        rightButton.gameObject.SetActive(currentIndex > 0);
        leftButton.gameObject.SetActive(currentIndex < panelCount - 1);
    }
}
