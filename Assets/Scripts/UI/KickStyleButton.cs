using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KickStyleButton : MonoBehaviour
{
    [SerializeField]
    private KickStyle kickStyle;
    private Button button;
    public UnityEvent<KickStyle> OnSelectKickStyle;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        OnSelectKickStyle?.Invoke(kickStyle);
    }
}
