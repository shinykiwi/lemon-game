using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ArrayVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private TextMeshProUGUI vertGrid;
    [SerializeField] private GameObject initialVertGrid;
    [SerializeField] private GameObject triGrid;

    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        //canvas.enabled = false;
    }

    public void PopulateInitialVertGrid(Vector3[] array)
    {
        vertGrid.text += "\n\nInitVertGrid\n";
        for (int i = 0; i < array.Length; i++)
        {
            vertGrid.text += "(" + array[i].x.ToString("0.00") + ", " + array[i].y.ToString("0.00") + ", " + array[i].z.ToString("0.00") + "), ";
        }
    }

    public void PopulateVertGrid(Vector3[] array)
    {
        vertGrid.text += "\n\nVertGrid\n";
        for (int i = 0; i < array.Length; i++)
        {
            
            vertGrid.text += "(" + array[i].x.ToString("0.00") + ", " + array[i].y.ToString("0.00") + ", " + array[i].z.ToString("0.00") + "), ";
        }
        
    }

    public void PopulateTriGrid(int[] array)
    {
        vertGrid.text += "\n\nTriGrid\n";
        foreach (var t in array)
        {
            vertGrid.text += t.ToString() + ", ";
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Toggle showing canvas or not
            canvas.enabled = !canvas.enabled;
        }
    }
}
