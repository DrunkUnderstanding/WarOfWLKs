using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTile : TileScript
{
	private float damage = 2f;
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player1" || collision.tag == "Player2")
			collision.GetComponent<Actor>().HandleBurned(damage);
	}
	private void OnCollisionStay2D(Collision2D collision)
	{

	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
