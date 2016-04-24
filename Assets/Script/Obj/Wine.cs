using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Wine : MonoBehaviour {


	[SerializeField] float SelectTime = 1f;
	[SerializeField] WineState state = WineState.Normal;
	[SerializeField] float enterIntense = 0.01f;
	[SerializeField] float pourMoveTime = 5f;
	[SerializeField] float pourTime = 2f;
	[SerializeField] GameObject instrument;
	[SerializeField] Transform target;
	float pointEnterTime;

	[SerializeField] AudioSource open;
	[SerializeField] AudioSource pour;
	[SerializeField] AudioSource raise;
	[SerializeField] AudioSource example;
	Vector3 initRotation;
	Vector3 initPosition;
	void Start()
	{
		initRotation = transform.rotation.eulerAngles;
		initPosition = transform.position;
	}

	void Update()
	{
		switch ( state )
		{
		case WineState.Normal:
			break;
		case WineState.Selected:
			float deltaTime = Time.time - pointEnterTime; 
			if ( deltaTime  > SelectTime )
			{
				Pour();
			}
			break;
		case WineState.Pour:
			break;
		default:
			break;
		}
	}

	public void Pour()
	{
		state = WineState.Pour;

		open.Play();

		Sequence seq = DOTween.Sequence();
		seq.Append( transform.DOMove( target.position, pourMoveTime ) );
		seq.Join(transform.DORotate( target.rotation.eulerAngles , pourMoveTime ));
		seq.AppendCallback( BeginPour );
		seq.AppendInterval( pourTime );
		seq.AppendCallback( SendAddWine );
		seq.Append( transform.DOMove( initPosition , pourMoveTime / 2f ));
		seq.Join( transform.DORotate( initRotation , pourMoveTime / 2f ) );
		seq.AppendCallback( EndPour );

	}

	public void BeginPour()
	{
		pour.Play();
	}

	public void SendAddWine()
	{
		// send message
		Message msg = new Message(this);
		msg.AddMessage("wine" , name );
		Events.FireAddWine( msg );
	}

	public void EndPour()
	{
		 state = WineState.Normal;
	}

	public void PointEnter( )
	{
		if( state == WineState.Normal )
		{
			raise.Play();
			example.Play();
			example.volume = 0;
			example.DOFade( 1f , 0.5f);
			state = WineState.Selected;
			pointEnterTime = Time.time;

			transform.DOMove( Vector3.up * enterIntense , SelectTime ).SetRelative(true); 
			if ( instrument != null )
				instrument.transform.DOScale( 2f , SelectTime );
		}
	}

	public void PointExit( )
	{
		if ( state == WineState.Selected)
		{
			state = WineState.Normal;
			transform.DOMove( initPosition, SelectTime );
			if ( instrument != null )
				instrument.transform.DOScale( 1f , SelectTime );
		}
	}

	public void PointClick()
	{
	}


}
