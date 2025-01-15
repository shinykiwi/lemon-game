using System;
using TMPro;
using UnityEngine;

public class Lemon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI xText;
    [SerializeField] private TextMeshProUGUI zText;
    
    [SerializeField] GameObject circle;
    [SerializeField] private float limit = 0.5f;
    [SerializeField] private float speed = 1;
    private Vector3 center;
    private float x = 0;
    private float scale = 0;
    private bool knifeOn = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        center = transform.position;
        scale = circle.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCircle();
    }

    void MoveCircle()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            knifeOn = !knifeOn;
        }

        if (knifeOn)
        {
            Vector3 pos = circle.transform.localPosition;
            x += Time.deltaTime;
            xText.text = x.ToString("0.000");
            zText
        
            float z = limit * Mathf.Sin(speed*x);
            circle.transform.localPosition = new Vector3(pos.x, pos.x, z);

            // scale *= z;
            // //Vector3 circleScale = circle.transform.localScale;
            // circle.transform.localScale = new Vector3(scale, scale, scale);
        }
        
    }
}
