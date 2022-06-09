using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Prototype prototype = null;
    public Animator animator = null;

    public void Initialize(Prototype prot)
    {
        prototype = prot;
        Image image = gameObject.GetComponent<Image>();
        if (image)
        {
            image.sprite = prototype.sprite;
        }
    }

    public void Appear()
    {
        animator.Play("TileAppear");
    }
    public void TileChoosen()
    {
        animator.Play("TileAppear");
        gameObject.GetComponent<Button>().enabled = false;
    }

    public void Remove()
    {
        animator.Play("TileRemoved");
        gameObject.GetComponent<Button>().enabled = false;
    }

    public void OnTileRemoved()
    {
        gameObject.GetComponent<Image>().enabled = false;
    }
}
