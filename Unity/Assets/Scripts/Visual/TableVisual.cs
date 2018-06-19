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
	private List<GameObject> EntesOnTable = new List<GameObject>();

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
		if (Input.touchCount <= 0)
			return;
        // we need to Raycast because OnMouseEnter, etc reacts to colliders on cards and cards "cover" the table
        RaycastHit[] hits;
		hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), 30f);
        bool haPasadoPorColider = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                haPasadoPorColider = true;
        }
        cursorSobreEstaMesa = haPasadoPorColider;
    }
    public void AñadirMagica(CartaBase ca, int idUnico, int indiceSlot)
    {
        //Debug.Log("Añadir ente magica");
        GameObject creature = GameObject.Instantiate(ObjetosGenerales.Instance.MagicaPrefab, slots.Children[indiceSlot].transform.position, Quaternion.identity) as GameObject;
		creature.GetComponent<MagicEffectVisual>().ColocarMagicaBocaAbajo(0f);
        ConfigurarEnte(creature, ca, idUnico, indiceSlot);
    }
		
    public void AñadirCriaturaDefensa(CartaBase ca, int idUnico, int indiceSlot)
    {
        //Debug.Log("Añadir ente criatura como defensa");
        GameObject creature = GameObject.Instantiate(ObjetosGenerales.Instance.CriaturaPrefab, slots.Children[indiceSlot].transform.position, Quaternion.identity) as GameObject;
		creature.GetComponent<CreatureAttackVisual>().ColocarCriaturaEnDefensa(ConfiguracionUsuario.Instance.CardTransitionTimeFast);
        ConfigurarEnte(creature, ca, idUnico, indiceSlot);
    }

    public void AñadirCriaturaAtaque(CartaBase ca, int idUnico, int indiceSlot)
    {
        //Debug.Log("Añadir ente criatura como ataque");
        // create a new creature from prefab
        GameObject creature = GameObject.Instantiate(ObjetosGenerales.Instance.CriaturaPrefab, slots.Children[indiceSlot].transform.position, Quaternion.identity) as GameObject;
        // apply the look from CardAsset
        ConfigurarEnte(creature, ca, idUnico, indiceSlot);
    }

	private void ConfigurarEnte(GameObject ente, CartaBase ca, int idUnico, int indiceSlot)
    {
		string tagEnte ="";
		OneEnteManager manager = null;
		if (ente.name.Contains ("Magica")) {
			tagEnte = "Magica";
			manager = ente.GetComponent<OneMagicaManager> ();
		}else{
			tagEnte = "Criatura";
			manager = ente.GetComponent<OneCreatureManager>();
		}
        manager.CartaAsset = ca;
        manager.LeerDatosAsset();
        foreach (Transform t in ente.GetComponentsInChildren<Transform>())
			t.tag = owner.ToString() + tagEnte;
        // parent a new creature gameObject to table slots
        ente.transform.SetParent(slots.transform);
        // add a new creature to the list
        // Debug.Log ("insert index: " + index.ToString());
        EntesOnTable.Insert(indiceSlot, ente);
        // let this creature know about its position
        WhereIsTheCardOrEntity w = ente.GetComponent<WhereIsTheCardOrEntity>();
        w.Slot = indiceSlot;
        if (ente.tag.Contains("Low"))
            //PETA
            w.EstadoVisual = VisualStates.MesaJugadorAbajo;
        else
            w.EstadoVisual = VisualStates.MesaJugadorArriba;
        // add our unique ID to this creature
        IDHolder id = ente.AddComponent<IDHolder>();
        id.UniqueID = idUnico;

        ActualizarSlots();
        MoverSlotCartas();
        // TODO: remove this
        Comandas.Instance.CompletarEjecucionComanda();
    }

    public int PosicionSlotNuevaCriatura(float MouseX)
    {
        // if there are no creatures or if we are pointing to the right of all creatures with a mouse.
        // right - because the table slots are flipped and 0 is on the right side.
        if (EntesOnTable.Count == 0 || MouseX > slots.Children[0].transform.position.x)
            return 0;
        else if (MouseX < slots.Children[EntesOnTable.Count - 1].transform.position.x) // cursor on the left relative to all creatures on the table
            return EntesOnTable.Count;
        for (int i = 0; i < EntesOnTable.Count; i++)
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
		EntesOnTable.Remove(creatureToRemove);
		IDHolder.EliminarElemento (creatureToRemove.GetComponent<IDHolder>());
        Destroy(creatureToRemove);

        ActualizarSlots();
        MoverSlotCartas();
        Comandas.Instance.CompletarEjecucionComanda();
    }

    /// <summary>
    /// Shifts the slots game object according to number of creatures.
    /// </summary>
    void ActualizarSlots()
    {
        float posX;
        if (EntesOnTable.Count > 0)
			//posX = (slots.Children[CreaturesOnTable.Count - 1].transform.localPosition.x - slots.Children[0].transform.localPosition.x) / 2f;
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[EntesOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    /// <summary>
    /// After a new creature is added or an old creature dies, this method
    /// shifts all the creatures and places the creatures on new slots.
    /// </summary>
    void MoverSlotCartas()
    {
        foreach (GameObject g in EntesOnTable)
		//for (int i = CreaturesOnTable.Count; i > 0;i--)
        {
			//GameObject g = CreaturesOnTable [i];
            g.transform.DOLocalMoveX(slots.Children[EntesOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
            // apply correct sorting order and HandSlot value for later 
            // TODO: figure out if I need to do something here:
            // g.GetComponent<WhereIsTheCardOrCreature>().SetTableSortingOrder() = CreaturesOnTable.IndexOf(g);
        }
    }
    

}
