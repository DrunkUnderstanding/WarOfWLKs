﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTile : TileScript
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Actor>().HandleDamage(5f);
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
