using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
	public float skillRange = 0.02f;

	public void ChangeSkillRange(float range)
	{
		skillRange = range;
		Debug.Log("Change skillRange to：" + skillRange.ToString());
	}
	public List<Actor> GetActorsInsideRange()
	{
		List<Actor> actors = new List<Actor>();
		foreach (Actor actor in BattleManager.actors.Values)
		{
			if (JudgeInside(actor.gameObject) && actor.id != GameManager.Instance.ctrllerId) actors.Add(actor);
		}
		//Debug.Log(actors);
		return actors;
	}

	public bool JudgeInside(GameObject go)
	{
		float subX = go.transform.position.x - this.transform.position.x;
		float subY = go.transform.position.y - this.transform.position.y;
		float squre = subX * subX + subY * subY;
		float distance = Mathf.Sqrt(squre);
		if (distance <= skillRange) return true;
		return false;
	}
	private void OnTriggerStay2D(Collider2D collision)
	{

	}
}
