using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicManager : MonoBehaviour {
	
	public LogicManager() { s_Instance = this; }
	public static LogicManager Instance { get { return s_Instance; } }
	private static LogicManager s_Instance;

	static GameObject m_orderTarget = null;
	static public GameObject OrderTarget
	{
		get  {
			if ( m_orderTarget == null )
				m_orderTarget = GameObject.Find("OrderTarget");
			return m_orderTarget;
		}
	}

	static public string[,] OrderLib = {
		{"moonlight" , "Piano" , "" },
		{"paganini" , "Violin" , ""}
	};

	Dictionary<string , List<string>> m_orderPool = new Dictionary<string, List<string>>();

	void CreateOrder()
	{
		Debug.Log("Create Ordder");
		GameObject orderObj = Instantiate( orderPrefab ) as GameObject;
		orderObj.transform.position = new Vector3( 2.5f + Random.Range(-0.1f,0.1f) , Random.Range( 0.5f , 2f ) , Random.Range( -2f , 2f ));

		Order order = orderObj.GetComponent<Order>();
		int orderID = Random.Range( 0, OrderLib.GetLength(0));
		Debug.Log("Order  ID " + orderID );
		order.Init( OrderLib[orderID,0]);

	}

	public string temOrder = "";
	List<string> tempWine = new List<string>();

	[SerializeField] GameObject orderPrefab;

	void OnEnable()
	{
		Events.AddWine += OnAddWine;
		Events.AddOrder += OnAddOrder;
		Events.AfterDrink += OnAfterDrink;
	}

	void OnDisable()
	{
		Events.AddWine -= OnAddWine;
		Events.AddOrder -= OnAddOrder;
		Events.AfterDrink -= OnAfterDrink;
	}

	void OnAddWine(Message msg )
	{
		Debug.Log("On add Wine" + msg.GetMessage("wine"));
		tempWine.Add((string) msg.GetMessage("wine"));
		Check();
	}

	void OnAddOrder(Message msg )
	{
		Debug.Log("On add ordr" + msg.GetMessage("order"));
		string order = (string) msg.GetMessage("order");
		temOrder = order;
		Check();
	}

	void OnAfterDrink(Message msg )
	{
		temOrder = "";

	}

	void Check()
	{
		if ( ! m_orderPool.ContainsKey( temOrder ) )
			return;
		List<string> res = m_orderPool[temOrder];
		foreach( string wine in tempWine )
		{
			if ( !res.Contains( wine ))
			{
				OnWrongWine();
				return;
			}
		}
		if ( res.Count == tempWine.Count )
			OnCorrectWine();
		//other wise not enough wine
	}

	void OnWrongWine()
	{
		Debug.Log("OnWrongWine");
		temOrder = "";
		tempWine.Clear();
		Events.FireWrongWine(new Message(this));
	}

	void OnCorrectWine()
	{
		Debug.Log("OnCorrectWine");
		temOrder = "*****";
		tempWine.Clear();
		Events.FireCorrectWine(new Message(this));
	}

	void Awake()
	{
		
		for( int i = 0 ; i < OrderLib.GetLength(0) ; ++ i )
		{
			List<string> wineList = new List<string>();
			for( int j = 1 ; j < OrderLib.GetLength(1) ; ++ j )
			{
				if ( OrderLib[i,j] != "")
					wineList.Add(OrderLib[i,j]);
			}
			m_orderPool.Add( OrderLib[i,0] , wineList );
		}

		StartCoroutine( CreateOrderCor() );
	}

	IEnumerator CreateOrderCor()
	{
		while (true)
		{
			CreateOrder();
			yield return new WaitForSeconds( Random.Range( 7f , 12f ));
		}
	}

	void LateUpdate() {
		Cardboard.SDK.UpdateState();
		if (Cardboard.SDK.BackButtonPressed) {
			Application.Quit();
		}
	}

//======== VR Manager ==========

	public void ToggleVRMode() {
		Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
	}

	public void ToggleDistortionCorrection() {
		switch(Cardboard.SDK.DistortionCorrection) {
		case Cardboard.DistortionCorrectionMethod.Unity:
			Cardboard.SDK.DistortionCorrection = Cardboard.DistortionCorrectionMethod.Native;
			break;
		case Cardboard.DistortionCorrectionMethod.Native:
			Cardboard.SDK.DistortionCorrection = Cardboard.DistortionCorrectionMethod.None;
			break;
		case Cardboard.DistortionCorrectionMethod.None:
		default:
			Cardboard.SDK.DistortionCorrection = Cardboard.DistortionCorrectionMethod.Unity;
			break;
		}
	}

	public void ToggleDirectRender() {
		Cardboard.Controller.directRender = !Cardboard.Controller.directRender;
	}

	public void TeleportRandomly() {
		Vector3 direction = Random.onUnitSphere;
		direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
		float distance = 2 * Random.value + 1.5f;
		transform.localPosition = direction * distance;
	}

}
