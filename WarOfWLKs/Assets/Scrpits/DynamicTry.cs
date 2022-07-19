using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTry : MonoBehaviour
{
    [SerializeField]
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        var sprite = ResourceManager.Instance.LoadRes<GameObject>("Prefabs/New Sprite");
        GameObject go = Instantiate(sprite);
        Sprite m_sprite = ResourceManager.Instance.LoadRes<Sprite>("Sprites/Brawler1");
        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = m_sprite;

        Sprite sprite1 = ResourceManager.Instance.LoadRes<Sprite>("Sprites/Brawler1");

        Debug.Log(sprite1.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
