using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

namespace Code.Scripts
{
    public class WaterPitcher : Interactable
    {
        [SerializeField] private float water = 0f;
        [SerializeField] private VisualEffect vfx;
    
        private bool pouring = false;
        private readonly float pouringAngle = -55;
        private readonly float maxWaterAmount = 100f;
        private readonly float waterPourAmount = 0.1f;

        private LemonadePitcher lemonadePitcher;

        private void Start()
        {
            vfx.Stop();
        }

        public void AddWater(float w = 25f)
        {
            // If the current water level is not at the max yet
            if (water < maxWaterAmount)
            {
                // Say max water is 100. Our current water is 97. There's still room, but not enough for +25 water
                // Then only add the different between 100 - 97 which is 3 water. 
                if (maxWaterAmount - water <= w)
                {
                    water += maxWaterAmount - water;
                }
            
                // Otherwise add the full amount
                else
                {
                    water += w;
                }
            
                Debug.Log("Adding water!"+water);
            }
            else
            {
                Debug.Log("Can't add water, it's already full!");
            }
        
        }

        public float GetWaterAmount()
        {
            return water;
        }

        public float GetWaterPourAmount()
        {
            return waterPourAmount;
        }

        public bool HasWater()
        {
            return water > 0;
        }

        public void ToggleWaterPour(LemonadePitcher l)
        {
            lemonadePitcher = l;
            // If not pouring, pour water
            if (!pouring)
            {
                pouring = true;
                Quaternion q = transform.rotation;
                transform.DOLocalRotate(new Vector3(pouringAngle, q.y, q.z), 0.5f);
                vfx.Play();
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
            vfx.Stop();
        }

        public bool IsPouring()
        {
            return pouring;
        }

        private void Update()
        {
            // Only decrease water count if currently pouring and there is water to pour
            if (pouring && water > 0 )
            {
                water -= waterPourAmount;
                lemonadePitcher.AddWater(waterPourAmount);
            }
            else
            {
                // optimization issue?
                vfx.Stop();
            }
        }
    }
}
