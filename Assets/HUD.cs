using System;
using Code.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Sprite leftClick;
    [SerializeField] private Sprite rightClick;
    private Image image;
    private TextMeshProUGUI text;
    [SerializeField] private GameObject container;

    private void Start()
    {
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetRightClick()
    {
        image.sprite = rightClick;
    }

    public void SetLeftClick()
    {
        image.sprite = leftClick;
    }

    private void SetText(string s)
    {
        text.text = s;
    }

    public void Set(Interactable interactable)
    {
        SetText(interactable.ToString());
        SetRightClick();
    }

    public void Hide()
    {
        container.SetActive(false);
    }

    public void Show()
    {
        container.SetActive(true);
    }

    public void Toggle()
    {
        container.SetActive(!container.activeSelf);
    }
    
}
