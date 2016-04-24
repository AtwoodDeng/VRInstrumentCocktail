using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Order : MonoBehaviour {

	public AudioSource m_audio;
	public OrderState state = OrderState.Normal;
	[SerializeField] float SelectTime;
	float pointEnterTime = 99999f;
	[SerializeField] Transform target;
	[SerializeField] float moveTime;
	public string orderName;

	[SerializeField] AudioSource paper;

	public void Init(string _orderName)
	{
		orderName = _orderName;
		if ( m_audio == null )
			m_audio = GetComponent<AudioSource>();
		
		m_audio.clip = Resources.Load( "Music/" + orderName ) as AudioClip ;
		m_audio.volume  = 0.01f;

		Renderer myRender = GetComponent<Renderer>();
		Material newMaterial = new Material( myRender.material.shader );
		newMaterial.SetTexture( "_MainTex" , myRender.material.GetTexture("_MainTex"));
		newMaterial.SetColor ( "_Color" , new Color( Random.Range( 0.5f , 0.8f ) , Random.Range( 0.5f , 0.8f ) , Random.Range( 0.5f , 0.8f ) , 1f));
		myRender.material = newMaterial;
	}

	void Update()
	{
		switch ( state )
		{
		case OrderState.Normal:
			break;
		case OrderState.Selected:
			float deltaTime = Time.time - pointEnterTime; 
			if ( deltaTime  > SelectTime && LogicManager.Instance.temOrder == "" )
			{
				Stick();
			}
			break;

		default:
			break;
		}
	}

	public void Stick()
	{
		state = OrderState.Stick;

		transform.SetParent( LogicManager.OrderTarget.transform);
		transform.localScale = Vector3.one;
		Sequence seq = DOTween.Sequence();
		seq.AppendCallback( SendAddStick );
		seq.Append( transform.DOLocalMove( Vector3.zero, moveTime ) );

	}

	void SendAddStick()
	{
		// send message
		Message msg = new Message(this);
		msg.AddMessage("order" , this.orderName.ToString() );
		Events.FireAddOrder( msg );
	}

	public void PointEnter( )
	{
		Debug.Log("Enter Order");
		paper.Play();

		if ( m_audio != null )
		{
			m_audio.DOFade( 1f , 0.5f );
			if ( ! m_audio.isPlaying )
				m_audio.Play();
		}
		if ( state == OrderState.Normal )
		{
			pointEnterTime = Time.time;
			state = OrderState.Selected;
		}

	}

	public void PointExit( )
	{
		if ( m_audio != null )	
			m_audio.DOFade( 0.01f , 0.5f );
		if ( state == OrderState.Selected )
		{
			state = OrderState.Normal;
			pointEnterTime = 99999f;
			
		}
	}

}
