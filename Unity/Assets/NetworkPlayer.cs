using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour
{

	[SyncVar(hook = "OnTurnChange")]
	public bool isTurn = false;

	[SyncVar(hook = "UpdateTimeDisplay")]
	public float time = 100;

	public PlayerController controller;

	[SyncVar]
	public bool ready = false;

	// Use this for initialization
	void Start()
	{
		controller.OnPlayerInput += OnPlayerInput;
	}

	// Update is called once per frame
	[Server]
	void Update()
	{
		if (isTurn)
		{
			time -= Time.deltaTime;
			if (time <= 0)
			{
				NetworkManager.Instance.AlterTurns();
			}
		}
	}

	public override void OnStartClient()
	{
		DontDestroyOnLoad(this);

		base.OnStartClient();
		Debug.Log("Client Network Player start");
		StartPlayer();

		NetworkManager.Instance.RegisterNetworkPlayer(this);
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		controller.SetupLocalPlayer();
	}

	[Server]
	public void StartPlayer()
	{
		ready = true;
	}

	public void StartGame()
	{
		TurnStart();
	}

	[Server]
	public void TurnStart()
	{
		isTurn = true;
		time = 90;
		RpcTurnStart();
	}

	[ClientRpc]
	void RpcTurnStart()
	{
		controller.TurnStart();
	}

	[Server]
	public void TurnEnd()
	{
		isTurn = false;
		RpcTurnEnd();
	}

	[ClientRpc]
	void RpcTurnEnd()
	{
		controller.TurnEnd();
	}

	public override void OnNetworkDestroy()
	{
		base.OnNetworkDestroy();
		NetworkManager.Instance.DeregisterNetworkPlayer(this);
	}

	public void OnTurnChange(bool turn)
	{
		if (isLocalPlayer)
		{
			//play turn sound 
		}
	}

	public void UpdateScore(int score)
	{
		Debug.Log ("score:"+score);
	}

	void OnPlayerInput(PlayerAction action, float amount)
	{
		if (action == PlayerAction.SHOOT)
		{
			CmdOnPlayerInput(action, amount);
		}
	}

	[Command]
	void CmdOnPlayerInput(PlayerAction action, float amount)
	{
		//Shoot bullets

		//Update score
		NetworkManager.Instance.UpdateScore((int)amount);
	}

	public void UpdateTimeDisplay(float curtime)
	{
		GameObject timerText = GameObject.FindWithTag("Timer");
		Text timer = timerText.GetComponent<Text> ();
		timer.text = Mathf.Round(curtime).ToString();
	}
}