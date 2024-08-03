using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public static PositionManager instance;

    public CombatantController[] combatants = new CombatantController[12];
    public CombatSlot[] combatSlots;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }


    private void Start()
    {
        combatSlots = FindObjectsOfType<CombatSlot>();
        UpdateCombatantPositions();
        UpdatePositions();
    }


    private void UpdateCombatantPositions()
    {
        // clear combatants
        for (int i = 0; i < combatants.Length; i++)
        {
            combatants[i] = null;
        }


        CombatantController[] allControllers = FindObjectsOfType<CombatantController>();
        foreach (CombatantController controller in allControllers)
        {
            // find the nearest slot
            CombatSlot slot = NearestSlot(controller);
            // move combatant to that slot
            controller.transform.position = slot.transform.position;
            // put combatant in the corresponding index position
            combatants[slot.transform.GetSiblingIndex()] = controller;
        }
    }

    private CombatSlot NearestSlot(CombatantController controller)
    {
        CombatSlot nearestSlot = null;
        float smallestDistance = 100f;

        foreach (CombatSlot slot in combatSlots)
        {
            // measure the distance
            float distBetween = Vector3.Distance(controller.transform.position, slot.transform.position);

            // if it's smaller than distance between nearest, replace nearest
            if (distBetween < smallestDistance)
            {
                smallestDistance = distBetween;
                nearestSlot = slot;
            }
        }

        return nearestSlot;
    }

    private void UpdatePositions()
    {
        foreach (CombatSlot slot in combatSlots)
        {
            slot.UpdateSlot();
        }
    }




    // FOR MOVING ALLIES //

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PickUpPlaceRat();
        }
    }

    private CombatantController _heldRat;
    private SpriteRenderer _heldRatSprite;
    private void PickUpPlaceRat()
    {
        // raycast for a slot
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // define raycast layer
        int layerMask = LayerMask.GetMask("CombatSlot");

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            // if you hit a combat slot, check to see if it has a rat in it
            CombatSlot hitSlot = hit.transform.GetComponent<CombatSlot>();
            CombatantController hitController = combatants[hitSlot.transform.GetSiblingIndex()];

            // if the slot doesn't belong to your team, return
            if (hitSlot.team != Team.Player) return;

            if (hitController != null && _heldRat == null)
            {
                // if it has a rat in it and there's no rat selected, select the rat
                _heldRat = hitController;
                _heldRatSprite = hitController.GetComponent<SpriteRenderer>();
                _heldRatSprite.color = Color.green;
            }
            else if (hitController != null && _heldRat != null)
            {
                // if it has a rat, and I'm holding a rat, deselect my rat and select the new rat
                _heldRatSprite.color = Color.white;
                _heldRat = hitController;
                _heldRatSprite = hitController.GetComponent<SpriteRenderer>();
                _heldRatSprite.color = Color.green;
            }
            else if (hitController == null && _heldRat == null)
            {
                // if there isn't a rat, and I'm not holding a rat, just return
                return;
            }
            else if (hitController == null && _heldRat != null)
            {
                // if there isn't a rat, and I'm  holding a rat, place the rat down and deselect it
                TryAdd(hitSlot, _heldRat);
                _heldRatSprite.color = Color.white;
                _heldRat = null;
                _heldRatSprite = null;
            }

            // update the combatants list
            UpdateCombatantPositions();
        }

        // call this to see if you can put a rat in a particular slot
        void TryAdd(CombatSlot slot, CombatantController rat)
        {
            // check if the slot is empty
            if (combatants[slot.transform.GetSiblingIndex()] == null)
            {
                // check if the slot belongs to your team
                if (slot.team == Team.Player)
                {
                    // put the guy in
                    combatants[slot.transform.GetSiblingIndex()] = rat;
                    slot.UpdateSlot();
                }
            }
        }
    }
}
