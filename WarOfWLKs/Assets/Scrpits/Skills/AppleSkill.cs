using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSkill : SkillBase
{

    public AppleSkill() : base()
    {
        this.CoolDownTime = 3;
        //技能ID
        this.id = 1;

        this.ProjSpeed = 2;

        this.SkillRange = 2;

        this.KeyCode = KeyCode.Q;

        this.SkillName = "Apple";

        this.ProjName = "Apple";

        this.Damage = 20;

        this.KnockBackDistance = 1f;

        this.iconPath = "Sprites/Icons/Skills/SpellBookPreface_18";

        this.translationNum = 11;
    }
    // Start is called before the first frame update


    // Update is called once per frame
    public override void Update()
    {


        base.Update();
    }
}
