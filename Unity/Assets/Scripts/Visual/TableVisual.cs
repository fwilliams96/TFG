﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TableVisual : MonoBehaviour 
{
    // PUBLIC FIELDS

    // an enum that mark to whish caracter this table belongs. The alues are - Top or Low
    public AreaPosition owner;

    // a referense to a game object that marks positions where we should put new Creatures
    public SameDistanceChildren slots;

    // PRIVATE FIELDS

    // list of all the creature cards on the table as GameObjects
    private List<GameObject> CreaturesOnTable = new List<GameObject>();

    // are we hovering over this table`s collider with a mouse
    private bool cursorOverThisTable = false;

    // A 3D collider attached to this game object
    private BoxCollider col;

    // PROPERTIES

    // returns true if we are hovering over any player`s table collider
    public static bool CursorOverSomeTable
    {
        get
        {
            TableVisual[] bothTables = GameObject.FindObjectsOfType<TableVisual>();
            return (bothTables[0].CursorOverThisTable || bothTables[1].CursorOverThisTable);
        }
    }

    // returns true only if we are hovering over this table`s collider
    public bool CursorOverThisTable
    {
        get{ return cursorOverThisTable; }
    }

    // MONOBEHAVIOUR SCRIPTS (mouse over collider detection)
    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    // CURSOR/MOUSE DETECTION
    void Update()
    {
        // we need to Raycast because OnMouseEnter, etc reacts to colliders on cards and cards "cover" the table
        RaycastHit[] hits;
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);
        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                passedThroughTableCollider = true;
        }
        cursorOverThisTable = passedThroughTableCollider;
    }
   

    public void AddCreatureAtIndex(CardAsset ca, int UniqueID ,int index, bool posicionAtaque)
    {
        Debug.Log("AddCreatureAtIndex ataque " + posicionAtaque);
        bool magica = ca.TipoDeCarta.Equals(TipoCarta.Magica);
        GameObject creature;
        if (!magica)
        {
            // create a new creature from prefab
            creature = GameObject.Instantiate(GlobalSettings.Instance.CreaturePrefab, slots.Children[index].transform.position, Quaternion.identity) as GameObject;

        }
        else
        {
            creature = GameObject.Instantiate(GlobalSettings.Instance.CriaturaPrefab, slots.Children[index].transform.position, Quaternion.identity) as GameObject;
        }
        
        // apply the look from CardAsset
        OneCreatureManager manager = creature.GetComponent<OneCreatureManager>();
        
        if (magica)
        {
            float x = creature.transform.eulerAngles.x;
            float y = creature.transform.eulerAngles.y+180;
            float z = creature.transform.eulerAngles.z;
            creature.transform.DORotate(new Vector3(x, y, z), 1);
        }
        else
        {
            if (!posicionAtaque)
            {
                /*float x = creature.transform.localEulerAngles.x;
                float y = creature.transform.localEulerAngles.y;
                float z = creature.transform.localEulerAngles.z + 90;
                creature.transform.DOLocalRotate(new Vector3(x, y, z), 1);*/
                float x = creature.transform.eulerAngles.x;
                float y = creature.transform.eulerAngles.y;
                float z = creature.transform.eulerAngles.z + 90;
                creature.transform.DORotate(new Vector3(x, y, z), 1);
            }
        }
        manager.cardAsset = ca;
        manager.ReadCreatureFromAsset();
        // add tag according to owner
        foreach (Transform t in creature.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString()+"Creature";
        // parent a new creature gameObject to table slots
        creature.transform.SetParent(slots.transform);
        // add a new creature to the list
        // Debug.Log ("insert index: " + index.ToString());
        CreaturesOnTable.Insert(index, creature);
        // let this creature know about its position
        WhereIsTheCardOrCreature w = creature.GetComponent<WhereIsTheCardOrCreature>();
        w.Slot = index;
        w.VisualState = VisualStates.LowTable;
        // add our unique ID to this creature
        IDHolder id = creature.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();
        // TODO: remove this
        Comandas.Instance.CompletarEjecucionComanda();
    }

    public int TablePosForNewCreature(float MouseX)
    {
        // if there are no creatures or if we are pointing to the right of all creatures with a mouse.
        // right - because the table slots are flipped and 0 is on the right side.
        if (CreaturesOnTable.Count == 0 || MouseX > slots.Children[0].transform.position.x)
            return 0;
        else if (MouseX < slots.Children[CreaturesOnTable.Count - 1].transform.position.x) // cursor on the left relative to all creatures on the table
            return CreaturesOnTable.Count;
        for (int i = 0; i < CreaturesOnTable.Count; i++)
        {
            if (MouseX < slots.Children[i].transform.position.x && MouseX > slots.Children[i + 1].transform.position.x)
                return i + 1;
        }
        Debug.Log("Suspicious behavior. Reached end of TablePosForNewCreature method. Returning 0");
        return 0;
    }

    public void RemoveCreatureWithID(int IDToRemove)
    {
        // TODO: This has to last for some time
        // Adding delay here did not work because it shows one creature die, then another creature die. 
        // 
        //Sequence s = DOTween.Sequence();
        //s.AppendInterval(1f);
        //s.OnComplete(() =>
        //   {
                
        //    });
        GameObject creatureToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        CreaturesOnTable.Remove(creatureToRemove);
        Destroy(creatureToRemove);

        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();
        Comandas.Instance.CompletarEjecucionComanda();
    }

    /// <summary>
    /// Shifts the slots game object according to number of creatures.
    /// </summary>
    void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
    {
        float posX;
        if (CreaturesOnTable.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CreaturesOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    /// <summary>
    /// After a new creature is added or an old creature dies, this method
    /// shifts all the creatures and places the creatures on new slots.
    /// </summary>
    void PlaceCreaturesOnNewSlots()
    {
        foreach (GameObject g in CreaturesOnTable)
        {
            g.transform.DOLocalMoveX(slots.Children[CreaturesOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
            // apply correct sorting order and HandSlot value for later 
            // TODO: figure out if I need to do something here:
            // g.GetComponent<WhereIsTheCardOrCreature>().SetTableSortingOrder() = CreaturesOnTable.IndexOf(g);
        }
    }

}
