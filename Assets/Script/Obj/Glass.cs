using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Glass : MonoBehaviour {

	[SerializeField] AudioSource correctMale;
	[SerializeField] AudioSource correctFemale;
	[SerializeField] AudioSource wrongMale;
	[SerializeField] AudioSource wrongFemale;
	[SerializeField] AudioSource drink;


	void OnEnable()
	{
		Events.CorrectWine += OnCorreckDrink;
		Events.WrongWine += OnWrongDrink;
	}

	void OnDisable()
	{
		Events.CorrectWine -= OnCorreckDrink;
		Events.WrongWine -= OnWrongDrink;
	}

	Vector3 initPos;

	void Awake()
	{
		initPos = transform.position;
	}

	void OnCorreckDrink( Message msg )
	{	
		drink.Play();
		transform.DOLocalMoveY( 3f , drink.clip.length ).SetRelative(true).OnComplete(AfterDrink);

		if ( Random.Range(0,2) ==0 )
			correctMale.PlayDelayed(drink.clip.length);
		else
			correctFemale.PlayDelayed(drink.clip.length);
		
		transform.DOMove( initPos , 2f ).SetDelay(wrongMale.clip.length+drink.clip.length);
		
	}

	void AfterDrink()
	{
		Order order = GetComponentInChildren<Order>();
		order.transform.SetParent( null );
		order.gameObject.SetActive(false);
		Events.FireAfterDrink(new Message(this));

	}

	void OnWrongDrink( Message msg )
	{
		if ( Random.Range(0,2) ==0 )
			wrongMale.Play();
		else
			wrongFemale.Play();
		
		Order order = GetComponentInChildren<Order>();
		order.gameObject.SetActive(false);
		
	}
}
