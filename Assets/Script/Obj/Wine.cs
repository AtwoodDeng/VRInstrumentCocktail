using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Wine : MonoBehaviour {


	[SerializeField] float SelectTime = 1f;
	[SerializeField] WineState state = WineState.Normal;
	[SerializeField] float enterIntense = 0.01f;
	float pointEnterTime;


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
				state = WineState.Pour;
			break;
		case WineState.Pour:
			state = WineState.Normal;
			break;
		default:
			break;
		}
	}


	public void PointEnter( )
	{
		if( state == WineState.Normal )
		{
			
			state = WineState.Selected;
			pointEnterTime = Time.time;

			transform.DOMove( Vector3.up * enterIntense , 0.5f ).SetRelative(true); 
		}
	}

	public void PointExit( )
	{
		if ( state == WineState.Selected)
		{
			state = WineState.Normal;
			transform.DOMove( initPosition, 0.5f );
		}
	}

	public void PointClick()
	{
		GetComponent<Renderer>().material.color = Color.blue;
	}


}
