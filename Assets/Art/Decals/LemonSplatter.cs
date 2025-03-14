using System;
using Code.Scripts.Holdables;
using UnityEngine;

public class LemonSplatter : MonoBehaviour
{
    [SerializeField] private GameObject sprayDecalPrefab;

    public void SplatterLemon(LemonSlice lemon)
    {
        Debug.Log("Splattering");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Quaternion rotation = Quaternion.LookRotation(hit.normal);
            Instantiate(sprayDecalPrefab, hit.point + hit.normal * 0.01f, rotation);
        }
    }
}
