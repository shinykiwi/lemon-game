using Code.Scripts.Holdables;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using static Code.Scripts.InteractHelper;

namespace Code.Scripts
{
    public enum State
    {
        Idle,
        Holding,
        Slicing,
        Squeezing,
        Stirring,
        Pouring
    }
    public class Player : MonoBehaviour
    {
        public Interactable lastInteractable;
        public Interactable itemInHand;
        private LemonSlice currentLemon;
        private LemonSlicer currentLemonSlicer;
        private LemonadePitcher currentLemonadePitcher;
        private WaterPitcher currentWaterPitcher;

        private LemonSplatter lemonSplatter;

        private Throw throwController;
        private PlayerAudio playerAudio;
    
        public State state = State.Idle;

        [SerializeField] private Transform hand;
        [SerializeField] private HUD hud;

        private void Start()
        {
            GetComponentInChildren<ReticleController>();
            playerAudio = GetComponentInChildren<PlayerAudio>();
            throwController = gameObject.AddComponent<Throw>();
            lemonSplatter = GetComponent<LemonSplatter>();
            
            HideNonHoldables();
        }

        /// <summary>
        /// Snaps the object to the players hand and turns off physics so it stays in place. Disables object's interaction.
        /// </summary>
        /// <param name="interactable"></param>
        private void AddToHand(Interactable interactable)
        {
            // Disable interaction for the picked up object
            interactable.DisableInteract();
        
            // Reset transforms
            GameObject obj;
            (obj = interactable.gameObject).transform.SetParent(hand);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
            // Turn off physics
            obj.GetComponent<Rigidbody>().isKinematic = true;
        
            itemInHand = interactable;
        }

        /// <summary>
        /// Detaches the object from the player's hand, turns on physics again so it falls.
        /// </summary>
        private void DropFromHand()
        {
            hand.DetachChildren();
            itemInHand.GetComponent<Rigidbody>().isKinematic = false;
            itemInHand.EnableInteract();
            
            itemInHand.GetComponent<Holdable>().Drop();
            itemInHand = null;
        
            //ResetAll();
        }

        private void SnapToCuttingBoard(Interactable interactable, LemonSlicer slicer)
        {
            SnapToLocation(interactable, slicer.GetObjectSpawn());

            slicer.SetObjectToSlice(interactable);
            
            EnableAllEmptyHand();
        }

        private void SnapToLocation(Interactable interactable, Transform parent)
        {
            // Disable interaction for the object
            interactable.DisableInteract();
        
            // Reset transforms
            GameObject obj;
            (obj = interactable.gameObject).transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
            // Turn off physics
            obj.GetComponent<Rigidbody>().isKinematic = true;

            itemInHand = null;
            
            //ResetAll();

        }

        /// <summary>
        /// Sends out a raycast, hits various objects and decides where to direct the logic.
        /// </summary>
        private void SearchByRaycast()
        {
            // Sending a raycast from the mouse position (which is already locked to middle of screen)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            // If something was hit
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.collider.gameObject;
            
                // If it's something that can have an outline
                if (hitObject.GetComponent<Interactable>() is { } interactable)
                {
                   // Hides outline of last looked at
                    if(lastInteractable) lastInteractable.HideOutline();
                    
                    // If you're able to interact with object, show its outline 
                    if (interactable.CanInteract())
                    {
                        interactable.ShowOutline();
                        hud.SetRightClick();
                        hud.Set(interactable);
                        hud.Show();
                    }
                    
                    // Save the interactable for later
                    lastInteractable = interactable;
                }
                else
                {
                    // Hides outline of last looked at
                    if (lastInteractable) lastInteractable.HideOutline();
                    if (itemInHand)
                    {
                        hud.Set("Throw");
                        hud.SetLeftClick();
                    }
                    else
                    {
                        hud.Hide();
                    }
                    
                    lastInteractable = null;
                }
            }
        }
    
        // Left click
        public void OnThrow(InputValue value)
        {
            switch (state)
            {
                case State.Holding:
                    // If you have an item in your hand
                    if (itemInHand)
                    {
                        if (itemInHand.GetComponent<LemonSlice>() is { } lemon)
                        {
                            lemonSplatter.SplatterLemon(lemon);
                        }
                        // Throw whatever object is in your hand
                        throwController.ThrowObject(itemInHand);
                        itemInHand.GetComponent<Holdable>().Drop();
                        itemInHand = null;
                        
                        //ResetAll();
                        
                        state = State.Idle;
                        // woosh sound?
                    }
                    break;
            }
        
        }

        // Right click
        public void OnInteract(InputValue value)
        {
            switch (state)
            {
                case State.Idle:
                    
                    if (lastInteractable && lastInteractable.CanInteract())
                    {
                        // If you're looking at something that you can hold, pick it up
                        if (lastInteractable.GetComponent<Holdable>() is { } holdable)
                        {
                            holdable.PickUp();
                            AddToHand(holdable);
                            state = State.Holding;
                        }
                    
                        // If you're looking at a door
                        else if (lastInteractable.GetComponentInParent<Door>() is { } door)
                        {
                            door.Use();
                        }
                    
                        // If you're looking at a sink
                        else if (lastInteractable.GetComponent<Sink>() is { } sink)
                        {
                            sink.ToggleTap();
                        }
                        
                        else if (lastInteractable.GetComponent<KnifeSet>() is { } knifeSet)
                        {
                            Knife knife = (Knife) knifeSet.GetKnife();
                            knife.PickUp();
                            AddToHand(knife);
                            state = State.Holding;
                        }
                    }
                    
                    break;
                
                case State.Holding:

                        // If you're not looking at anything
                        if (lastInteractable == null)
                        {
                            DropFromHand();
                            Debug.Log("Dropping item");
                            playerAudio.PutBack();
                            state = State.Idle;
                        }
                        
                        // If you're aiming at the lemon slicer
                        else if (lastInteractable.GetComponent<LemonSlicer>() is { } lemonSlicer)
                        {
                            // If the item you're holding is a lemon
                            if (itemInHand as LemonSlice)
                            {
                                SnapToCuttingBoard(itemInHand, lemonSlicer);
                                lemonSlicer.PlaceLemon();
                                playerAudio.PutBack();
                                state = State.Idle;
                            }
                            
                            else if (itemInHand as Knife)
                            {
                                if (lemonSlicer.HasLemon())
                                {
                                    DropFromHand();
                                    lemonSlicer.EnterSliceMode();
                                    currentLemonSlicer = lemonSlicer;
                                    state = State.Slicing;
                                }
                            }
                        }
            
                        // If you're aiming at the lemonade pitcher
                        else if (lastInteractable.GetComponent<LemonadePitcher>() is { } lemonadePitcher)
                        {
                            // If the item you're holding is a lemon
                            if (itemInHand.GetComponent<LemonSlice>() is { } lemon)
                            {
                                // If the lemon has been sliced before
                                if (lemon.IsSliced() && lemon.CanBeSqueezed())
                                {
                                    // Enters the squeezing mode
                                    SnapToLocation(itemInHand, lemonadePitcher.EnterSqueezingMode());
                                    currentLemon = lemon;
                                    currentLemonadePitcher = lemonadePitcher;
                                    state = State.Squeezing;
                                }
                            }
                        
                            // If the item you're holding is a water pitcher
                            else if (itemInHand.GetComponent<WaterPitcher>() is { } waterPitcher)
                            {
                                // If the water pitcher has water in it
                                if (waterPitcher.HasWater())
                                {
                                    SnapToLocation(itemInHand,lemonadePitcher.EnterPouringMode());
                                    currentWaterPitcher = waterPitcher;
                                    currentLemonadePitcher = lemonadePitcher;
                                    state = State.Pouring;
                                }
                            }
                        
                            // If the item you're holding is a spoon
                            else if (itemInHand.GetComponent<SugarSpoon>() is { } sugarSpoon)
                            {
                                // If the spoon has sugar on it
                                if (sugarSpoon.HasSugar())
                                {
                                    // Adds sugar amount to the lemonade pitcher
                                    currentLemonadePitcher.AddSugar(sugarSpoon.RemoveSugar());
                                    playerAudio.PutSugarBack();
                                }
                            }
                            
                            // else if (itemInHand.GetComponent<Glass>() is { } glass)
                            // {
                            //     // If the glass is empty
                            //     if (glass.IsEmpty())
                            //     {
                            //         // Add liquid
                            //     }
                            //     
                            // }
                        }
                    
                        // If you're aiming at a sugar jar
                        else if (lastInteractable.GetComponent<SugarJar>())
                        {
                            // If the item you're holding is a spoon
                            if (itemInHand.GetComponent<SugarSpoon>() is { } sugarSpoon)
                            {
                                // Put back the sugar if there's sugar on the spoon
                                if (sugarSpoon.HasSugar())
                                {
                                    sugarSpoon.RemoveSugar();
                                    playerAudio.PutSugarBack();
                                }
                                // Otherwise, put sugar on the spoon
                                else
                                {
                                    sugarSpoon.AddSugar();
                                    playerAudio.TakeSugar();
                                }
                            
                            }
                        }
                    
                        // If you're aiming at a trash can
                        else if (lastInteractable.GetComponent<TrashCan>())
                        {
                            // If you're holding a lemon slice, throw it out
                            if (itemInHand as LemonSlice)
                            {
                                itemInHand.transform.SetParent(null);
                                Destroy(itemInHand.gameObject);
                                itemInHand = null;
                                state = State.Idle;
                                playerAudio.Trash();
                                //ResetAll();
                            }
                        }
                    
                        // If you're aiming at the sink
                        else if (lastInteractable.GetComponent<Sink>() is { } sink)
                        {
                            // If you're holding a water pitcher
                            if (itemInHand.GetComponent<WaterPitcher>() is { } waterPitcher)
                            {
                                if (sink.IsTapOn())
                                {
                                    waterPitcher.AddWater();
                                    playerAudio.PourWater();
                                }
                    
                            }
                
                            // // Maybe if you are holding a lemon, you can wash it?
                            // else if (itemInHand.GetComponent<LemonSlice>())
                            // {
                            //     
                            // }
                        }
                    
                        break;
            
                case State.Slicing:
                    currentLemonSlicer.InitiateSlice();
                    state = State.Idle;
                    break;
            
                case State.Squeezing:
                    if (currentLemon.Squeeze())
                    {
                        playerAudio.SqueezeLemon();
                        currentLemonadePitcher.AddLemonJuice();
                    }
                    else
                    {
                        currentLemonadePitcher.ExitSqueezingMode();
                        currentLemon.EnableInteract();
                        currentLemon.gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.5f) ;
                        state = State.Idle;
                    }
                    break;
            
                case State.Stirring:
                    break;
            
                case State.Pouring:
                    // If the player stops pouring and there's no more water left, exit pouring mode
                    if (!currentWaterPitcher.IsPouring() && !currentWaterPitcher.HasWater())
                    {
                        currentLemonadePitcher.Exit();   
                        AddToHand(currentWaterPitcher);
                        state = State.Holding;
                    }
                
                    // Normal pouring situation
                    else
                    {
                        bool pouring = currentWaterPitcher.ToggleWaterPour(currentLemonadePitcher);
                        
                        if(pouring) playerAudio.PourWater();
                        else playerAudio.Stop();
                    }
                
                    break;
            }
        
        }

        private void Update()
        {
            SearchByRaycast();
        
        }
    }
}