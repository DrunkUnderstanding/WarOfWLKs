using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutSkill : AppleSkill
{
    public float chantTime;
    public ShoutSkill() : base()
	{
        this.KeyCode = KeyCode.W;

        this.SkillName = "Shout";

        this.ProjName = "Shout";

        this.iconPath = "Sprites/Icons/Skills/SpellBookPreface_24";

        this.CoolDownTime = 5;
        //技能ID
        this.id = 2;

        this.SkillRange = 0.5f;

        this.Damage = 10;

        this.KnockBackDistance = 2f;

        this.translationNum = 21;

        this.chantTime = 2f;
    }
    public override void Update()
    {


        base.Update();
    }
}

