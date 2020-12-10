using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MaskFade : MonoBehaviour
{
    enum TestType { a, b, c };
    [SerializeField] TestType test = TestType.a;
    [SerializeField] float duration;
    Image selfImg;

    public void DOFade(float value)
    {
        selfImg.DOFade(value, duration).SetUpdate(true);
    }
    public void DOFade(float value, System.Action callback)
    {
        selfImg.DOFade(value, duration).SetUpdate(true).OnComplete(() => callback());
    }
    private void Awake()
    {
        selfImg = GetComponent<Image>();
    }

}
