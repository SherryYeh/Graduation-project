using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] float duration;
    Camera cam;

    public void DOZoom(float value)
    {
        if (cam == null)
            return;

        cam.DOFieldOfView(value, duration).SetUpdate(true);
    }
    public void DOZoom(float value, System.Action callback)
    {
        if (cam == null)
            return;

        cam.DOFieldOfView(value, duration).SetUpdate(true).OnComplete(() => callback());
    }

    private void Start()
    {
        cam = Camera.main;
        Invoke("Test", 1f);
    }

    void Test()
    {
        DOZoom(0);
    }

}
