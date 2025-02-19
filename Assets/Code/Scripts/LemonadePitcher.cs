using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

namespace Code.Scripts
{
    public class LemonadePitcher : Interactable
    {
        [SerializeField] private CinemachineCamera camera;
        [SerializeField] private Transform lemonSpawnPoint;
        [SerializeField] private Transform woodSpoonSpawnPoint;
        [SerializeField] private Transform waterPitcherSpawnPoint;
        [SerializeField] private Transform sugarSpoonSpawnPoint;

        [SerializeField] private GameObject[] liquidLayers;
        [SerializeField] private VisualEffect vfx;
        private int layerCount = 0;

        private float lemonJuice = 0f;
        private float sugar = 0f;
        private float water = 0f;
        private readonly float maxLiquid = 100f;
    
        [SerializeField] private Color yellowColor = new Color(1f, 0.792f, 0.286f);
        [SerializeField] private Color waterColor = new Color(0.682f, 0.8588f, 1f);

        public void AddLemonJuice()
        {
            lemonJuice += 1f; // amount of juice adding, hardcoded for now
            vfx.Play();
            UpdateLiquid();
        }
    
        public void AddSugar(float s)
        {
            sugar += s;
            UpdateLiquid();
        }

        public void AddWater(float w)
        {
            water += w;
            UpdateLiquid();
        }
   

        public Transform EnterSqueezingMode()
        {
            camera.enabled = true;
            DisableInteract();
        
            return lemonSpawnPoint;
        }

        public Transform EnterPouringMode()
        {
            camera.enabled = true;
            DisableInteract();

            return waterPitcherSpawnPoint;
        }
    
        // ----- Exits -----

        public void ExitSqueezingMode()
        {
            camera.enabled = false;
            EnableInteract();
        }

        public void Exit()
        {
            camera.enabled = false;
            EnableInteract();
        }

    
        private void UpdateLiquid()
        {
            // This function is called when there is an update to the liquid content
            // So we can assume that there's been a change and thus a need to update the visuals

            // Get the total liquid amount that includes both water and lemon juice
            float totalLiquid = water + lemonJuice;
        
            // Get the interval value. (Ex. if there's 4 layers, max liquid is 100, this value would be 0.25)
            float interval = (maxLiquid / liquidLayers.Length) / 100;

            // If there's any liquid at all
            if (totalLiquid > 0)
            {
                // A loop for each layer number
                for (int i = 1; i <= liquidLayers.Length; i++)
                {
                    // Value will get bigger each layer you climb
                    // Ex. Layer 1 will be 0.16, layer 2 will be 0.32 etc.
                    float x = interval * (i);
        
                    // If the current liquid content is less than the liquid mandate for this interval
                    // Right interval to be in
                    if (totalLiquid < (maxLiquid * x))
                    {
                        if (i != layerCount)
                        {
                            AddLayer();
                        }

                        break;
                    }
                 
                    // Otherwise, continue the loop till it finds the right interval
        
                }
            }
         
            UpdateLiquidColour();

            // All three elements are present
            if (sugar > 0 && water > 0 && lemonJuice > 0)
            {
                LemonadeMade();
            }
        
        }

        private void LemonadeMade()
        {
            
        }

        private void UpdateLiquidColour()
        {
            Color color = waterColor;
            // Only water, should only show water colour
            if (water > 0 && lemonJuice == 0)
            {
                color = waterColor;
            }
            
            // Only lemon juice, should only show lemon juice
            else if (water == 0 && lemonJuice > 0)
            {
                color = yellowColor;
            }
        
            // It must be a mix of lemon juice and water
            else
            {
                float lemonPercent = lemonJuice / maxLiquid;
                Debug.Log(lemonPercent);
            
                color = Color.Lerp(yellowColor, waterColor, lemonPercent);
            }
        
            foreach (GameObject layer in liquidLayers)
            {
                Material mat = layer.GetComponent<MeshRenderer>().material;
                mat.DOColor(color, 1f);

            }
        }

        private void AddLayer()
        {
            layerCount++;
            if ((layerCount-1) < liquidLayers.Length)
            {
                liquidLayers[layerCount-1].SetActive(true);
            }
        }
    }
}
