using UnityEngine;

namespace Code.Scripts.Holdables
{
    public class Knife : Holdable
    {
        public override void PickUp()
        {
            base.PickUp();

            LemonSlicer l = FindFirstObjectByType<LemonSlicer>();
            if (l)
            {
                if (l.HasLemon())
                {
                    InteractHelper.EnableInteractablesByType<LemonSlicer>();
                }
            }
                
        }
    }
}
