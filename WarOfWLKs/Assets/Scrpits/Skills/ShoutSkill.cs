using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutSkill : AppleSkill
{
    public float delay;
    public ShoutSkill() : base()
	{
        this.KeyCode = KeyCode.W;

        this.SkillName = "Shout";

        this.ProjName = "Shout";

        this.iconPath = "Sprites/Icons/Skills/SpellBookPreface_24";

        this.CoolDownTime = 3;
        //技能ID
        this.Id = 2;

        this.SkillRange = 0.5f;

        this.Damage = 10;

        this.KnockBackDistance = 1;

        this.translationNum = 21;

        this.delay = 2f;
    }
    public override void Update()
    {


        base.Update();
    }
}

