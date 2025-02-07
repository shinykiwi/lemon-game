using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WaterPitcher : Interactable
{
    private float water = 100f;
    private bool pouring = false;
    private float pouringAngle = -55;

    public void AddWater(float w)
    {
        water += w;
    }

    public float GetWaterAmount()
    {
        return water;
    }

    public bool HasWater()
    {
        return water > 0;
    }

    public void ToggleWaterPour()
    {
        // If not pouring, pour water
        if (!pouring)
        {
            pouring = true;
            Quaternion q = transform.rotation;
            transform.DOLocalRotate(new Vector3(pouringAngle, q.y, q.z), 0.5f);
        }
        // If already pouring, stop pouring
        else
        {
            StopPouring();
        }
        
    }

    private void StopPouring()
    {
        pouring = false;
        Quaternion q = transform.rotation;
        transform.DOLocalRotate(new Vector3(0, q.y, q.z), 0.5f);
    }

    private void Update()
    {
        if (pouring)
        {
            water -= 0.01f;
        }
    }
}
