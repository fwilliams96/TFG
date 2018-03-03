using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TableVisual : MonoBehaviour
{
    #region Atributos

    // an enum that mark to whish caracter this table belongs. The alues are - Top or Low
    public AreaPosition owner;

    // a referense to a game object that marks positions where we should put new Creatures
    public SameDistanceChildren slots;

    // PRIVATE FIELDS

    // list of all the creature cards on the table as GameObjects
    private List<GameObject> CreaturesOnTable = new List<GameObject>();

    // are we hovering over this table`s collider with a mouse
    private bool cursorSobreEstaMesa = false;

    // A 3D collider attached to this game object
    private BoxCollider col;

    #endregion

    // returns true if we are hovering over any player`s table collider
    public static bool CursorSobreAlgunaMesa
    {
        get
        {
            TableVisual[] bothTables = GameObject.FindObjectsOfType<TableVisual>();
            return (bothTables[0].CursorSobreEstaMesa || bothTables[1].CursorSobreEstaMesa);
        }
    }

    // returns true only if we are hovering over this table`s collider
    public bool CursorSobreEstaMesa
    {
        get { return cursorSobreEstaMesa; }
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
        bool haPasadoPorColider = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                haPasadoPorColider = true;
        }
        cursorSobreEstaMesa = haPasadoPorColider;
    }
    public void AñadirMagica(CardAsset ca, int idUnico, int indiceSlot)
    {
        Debug.Log("Añadir ente magica");
        GameObject creature = GameObject.Instantiate(DatosGenerales.Instance.MagicaPrefab, slots.Children[indiceSlot].transform.position, Quaternion.identity) as GameObject;
        float x = creature.transform.eulerAngles.x;
        float y = creature.transform.eulerAngles.y + 180;
        float z = creature.transform.eulerAngles.z;
        creature.transform.DORotate(new Vector3(x, y, z), 1);
        ConfigurarEnte(creature, ca, idUnico, indiceSlot);
    }
    //TODO mejorar codigo
    public void AñadirCriaturaDefensa(CardAsset ca, int idUnico, int indiceSlot)
    {
        Debug.Log("Añadir ente criatura como defensa");
        //TODO cuando sea una carta magica no entrara en esta funcion
        // create a new creature from prefab
        GameObject creature = GameObject.Instantiate(DatosGenerales.Instance.CriaturaPrefab, slots.Children[indiceSlot].transform.position, Quaternion.identity) as GameObject;
        // apply the look from CardAsset

        /*float x = creature.transform.localEulerAngles.x;
        float y = creature.transform.localEulerAngles.y;
        float z = creature.transform.localEulerAngles.z + 90;
        creature.transform.DOLocalRotate(new Vector3(x, y, z), 1);*/
        float x = creature.transform.eulerAngles.x;
        float y = creature.transform.eulerAngles.y;
        float z = creature.transform.eulerAngles.z + 90;
        creature.transform.DORotate(new Vector3(x, y, z), 0.1f);
        ConfigurarEnte(creature, ca, idUnico, indiceSlot);
    }

    public void AñadirCriaturaAtaque(CardAsset ca, int idUnico, int indiceSlot)
    {
        Debug.Log("Añadir ente criatura como ataque");
        // create a new creature from prefab
        GameObject creature = GameObject.Instantiate(DatosGenerales.Instance.CriaturaPrefab, slots.Children[indiceSlot].transform.position, Quaternion.identity) as GameObject;
        // apply the look from CardAsset
        ConfigurarEnte(creature, ca, idUnico, indiceSlot);
    }

    private void ConfigurarEnte(GameObject criaturaOMagica, CardAsset ca, int idUnico, int indiceSlot)
    {
        OneCreatureManager manager = criaturaOMagica.GetComponent<OneCreatureManager>();
        manager.cardAsset = ca;
        manager.LeerDatosAsset();
        // add tag according to owner
        foreach (Transform t in criaturaOMagica.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString() + "Ente";
        // parent a new creature gameObject to table slots
        criaturaOMagica.transform.SetParent(slots.transform);
        // add a new creature to the list
        // Debug.Log ("insert index: " + index.ToString());
        CreaturesOnTable.Insert(indiceSlot, criaturaOMagica);
        // let this creature know about its position
        WhereIsTheCardOrEntity w = criaturaOMagica.GetComponent<WhereIsTheCardOrEntity>();
        w.Slot = indiceSlot;
        if (criaturaOMagica.tag.Contains("Low"))
            w.EstadoVisual = VisualStates.MesaJugadorAbajo;
        else
            w.EstadoVisual = VisualStates.MesaJugadorArriba;
        // add our unique ID to this creature
        IDHolder id = criaturaOMagica.AddComponent<IDHolder>();
        id.UniqueID = idUnico;

        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();
        // TODO: remove this
        Comandas.Instance.CompletarEjecucionComanda();
    }

    public int PosicionSlotNuevaCriatura(float MouseX)
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

    public void EliminarCriaturaID(int IDToRemove)
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
