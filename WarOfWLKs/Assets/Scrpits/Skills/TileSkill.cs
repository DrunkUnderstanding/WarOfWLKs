using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSkill : AppleSkill
{
    public TileSkill(Actor target) : base()
    {
        //this.CoolDown = 3;
        //技能ID
        this.Id = 2;

        //this.ProjSpeed = 2;

        //this.CastDistance = 2;

        //this.KeyCode = KeyCode.Q;

        //this.SkillName = "Fire";

        //this.ProjName = "Apple";

        this.Damage = 10;
    }
    // Start is called before the first frame update


    // Update is called once per frame
    public override void Update()
    {


        base.Update();
    }

}
