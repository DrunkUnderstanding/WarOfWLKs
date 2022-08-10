using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundScroll : MonoBehaviour
{
    [SerializeField]
    Vector2 ScorllSpeed;

    private Material BackgroundMaterial;
    void Start()
    {
        BackgroundMaterial = GetComponent<Image>().material;
    }

    void Update()
    {
        BackgroundMaterial.mainTextureOffset += ScorllSpeed * Time.deltaTime;
    }
}
