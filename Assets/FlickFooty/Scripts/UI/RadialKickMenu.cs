using System;
using UnityEngine;
using UnityEngine.UI;

public class RadialKickMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menuRoot;
    [SerializeField] private Image directionUp;
    [SerializeField] private Image directionLeft;
    [SerializeField] private Image directionRight;

    private Vector2 holdStartPosition;
    private bool isActive;

    public enum KickType { None, DropPunt, SnapLeft, SnapRight }
    public Action<KickType> OnKickSelected;

    public void Show(Vector2 screenPosition)
    {
        isActive = true;
        holdStartPosition = screenPosition;
        menuRoot.gameObject.SetActive(true);
        menuRoot.position = screenPosition;
    }

    public void Hide()
    {
        isActive = false;
        menuRoot.gameObject.SetActive(false);
        ResetHighlights();
    }

    private void Update()
    {
        if (!isActive || Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        Vector2 direction = touch.position - holdStartPosition;
        float distance = direction.magnitude;

        if (touch.phase == TouchPhase.Ended)
        {
            KickType selected = GetDirection(direction);
            OnKickSelected?.Invoke(selected);
            Hide();
        }
        else
        {
            Highlight(GetDirection(direction));
        }
    }

    private KickType GetDirection(Vector2 delta)
    {
        if (delta.magnitude < 50f) return KickType.None;

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        if (angle >= 45f && angle < 135f) return KickType.DropPunt;     // Up
        if (angle >= 135f && angle < 225f) return KickType.SnapLeft;    // Left
        if (angle >= 315f || angle < 45f) return KickType.SnapRight;    // Right

        return KickType.None;
    }

    private void Highlight(KickType type)
    {
        ResetHighlights();
        switch (type)
        {
            case KickType.DropPunt: directionUp.color = Color.yellow; break;
            case KickType.SnapLeft: directionLeft.color = Color.yellow; break;
            case KickType.SnapRight: directionRight.color = Color.yellow; break;
        }
    }

    private void ResetHighlights()
    {
        directionUp.color = Color.white;
        directionLeft.color = Color.white;
        directionRight.color = Color.white;
    }
}
