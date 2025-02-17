using System;
using System.Collections;
using Code.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public enum State
{
    Idle,
    Slicing,
    Squeezing,
    Sugaring,
    Stirring,
    Pouring
}
public class Player : MonoBehaviour
{
    
    [Header("Debug")] 
    public TextMeshProUGUI t1;
    public TextMeshProUGUI t2;
    public TextMeshProUGUI t3;
    
    private Interactable lastInteractable;
    bool currentlySlicing = false;
    
    private Interactable itemInHand = null;
    private LemonSlice currentLemon = null;
    private LemonSlicer currentLemonSlicer = null;
    private LemonadePitcher currentLemonadePitcher = null;
    private WaterPitcher currentWaterPitcher = null;
    private SugarSpoon currentSpoon = null;
    
    private ReticleController reticleController;
    private Throw throwController;
    private PlayerAudio playerAudio;
    
    private State state = State.Idle;

    [SerializeField] private Transform hand;

    private void Start()
    {
        reticleController = GetComponentInChildren<ReticleController>();
        playerAudio = GetComponentInChildren<PlayerAudio>();
        throwController = gameObject.AddComponent<Throw>();
        
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
        
        // // Make certain objects uninteractable
        // LemonSlicer slicer = FindFirstObjectByType<LemonSlicer>();
        // slicer.DisableInteract();
        
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
        itemInHand = null;
        
        //StartCoroutine(nameof(EnableGeneralInteraction));
        
    }

    private void SnapToCuttingBoard(Interactable interactable, LemonSlicer slicer)
    {
        SnapToLocation(interactable, slicer.GetObjectSpawn());

        slicer.SetObjectToSlice(interactable);
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

    }

    private IEnumerator EnableGeneralInteraction()
    {
        yield return null;
        
        // Make certain objects interactable again
        LemonSlicer slicer = FindFirstObjectByType<LemonSlicer>();
        slicer.EnableInteract();
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
                if(lastInteractable) lastInteractable.HideOutline();
                interactable.ShowOutline();
                lastInteractable = interactable;
            }
            else
            {
                if (lastInteractable) lastInteractable.HideOutline();
            }
        }
    }
    
    // Left click
    public void OnThrow(InputValue value)
    {
        switch (state)
        {
            case State.Idle:
                // If you have an item in your hand
                if (itemInHand)
                {
                    // If you're aiming at the lemon slicer
                    if (lastInteractable.GetComponent<LemonSlicer>() is { } lemonSlicer)
                    {
                        // If the item you're holding is a lemon
                        if (itemInHand as LemonSlice)
                        {
                            SnapToCuttingBoard(itemInHand, lemonSlicer);
                            playerAudio.PutBack();
                                    
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
                            }
                        }
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
                            }
                            // Otherwise, put sugar on the spoon
                            else
                            {
                                sugarSpoon.AddSugar();
                            }
                            
                        }
                    }
                    
                    // If you're aiming at a trash can
                    else if (lastInteractable.GetComponent<TrashCan>())
                    {
                        itemInHand.transform.SetParent(null);
                        Destroy(itemInHand.gameObject);
                        itemInHand = null;
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
                            }
                    
                        }
                
                        // // Maybe if you are holding a lemon, you can wash it?
                        // else if (itemInHand.GetComponent<LemonSlice>())
                        // {
                        //     
                        // }
                    }
    
                    // You're not aiming at lemon slicer
                    else
                    {
                        // Throw whatever object is in your hand
                        throwController.ThrowObject(itemInHand);
                        itemInHand = null;
                        // woosh sound?
                    }
                }
                break;
        
        case State.Slicing:
            currentLemonSlicer.InitiateSlice();
            state = State.Idle;
            break;
        }
        
    }

    // Right click
    public void OnInteract(InputValue value)
    {
        switch (state)
        {
            case State.Idle:
                
                // You have nothing in your hand
                if (!itemInHand)
                {
                    // If you're looking at a lemon, pick it up
                    if (lastInteractable.GetComponent<LemonSlice>() is { } lemonSlice)
                    {
                        AddToHand(lemonSlice);
                        playerAudio.PickUp();
                    }
            
                    // If you're looking at a cutting board, enter slicing mode
                    else if (lastInteractable.GetComponent<LemonSlicer>() is { } lemonSlicer)
                    {
                        lemonSlicer.EnterSliceMode();
                        currentLemonSlicer = lemonSlicer;
                        state = State.Slicing;

                    }
                    
                    // If you're looking at a water pitcher, pick it up
                    else if (lastInteractable.GetComponent<WaterPitcher>() is { } waterPitcher)
                    {
                        AddToHand(waterPitcher);
                        playerAudio.PickUp();
                        currentWaterPitcher = waterPitcher;
                        
                    }
                    
                    // If you're looking at a sugar spoon, pick it up
                    else if (lastInteractable.GetComponent<SugarSpoon>() is { } sugarSpoon)
                    {
                        AddToHand(sugarSpoon);
                        playerAudio.PickUp();
                        currentSpoon = sugarSpoon; // idk if this will be used
                    }
                    
                    // If you're looking at a door
                    else if (lastInteractable.GetComponentInParent<Door>() is { } door)
                    {
                        door.Use();
                    }
                    
                    else if (lastInteractable.GetComponent<Sink>() is { } sink)
                    {
                        sink.ToggleTap();
                    }
                }
        
                // You have something in your hand
                else
                {
                    DropFromHand();
                    playerAudio.PutBack();
                }

                break;
            
            case State.Slicing:
                break;
            
            case State.Squeezing:
                if (currentLemon.Squeeze())
                {
                    playerAudio.SqueezeLemon();
                    currentLemonadePitcher.AddLemonJuice(5);
                }
                else
                {
                    currentLemonadePitcher.ExitSqueezingMode();
                    currentLemon.EnableInteract();
                    currentLemon.transform.localScale = new Vector3(1, 1, 1);
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
                    state = State.Idle;
                }
                
                // Normal pouring situation
                else
                {
                    currentWaterPitcher.ToggleWaterPour(currentLemonadePitcher);
                }
                
                break;
        }
        
    }

    private void Update()
    {
        SearchByRaycast();
        
    }
}
