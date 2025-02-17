using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private Image img;

    private float duration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = GetComponentInChildren<Image>();
        img.color = Color.black;
        FadeInto();
        
    }

    private void FadeInto()
    {
        img.DOFade(0, duration);
    }

    private void FadeOut()
    {
        img.DOFade(1, duration);
    }
    
}
