using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Prototype prototype = null;

    public void Initialize(Prototype prot)
    {
        prototype = prot;
        Image image = gameObject.GetComponent<Image>();
        if (image)
        {
            image.sprite = prototype.sprite;
        }
    }
}
