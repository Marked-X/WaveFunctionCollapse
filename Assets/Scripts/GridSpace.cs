using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public List<GameObject> neighbours = new List<GameObject>();
    public List<Tile> tiles = null;
    public GameObject tilePrefab = null;
    public int X;
    public int Y;

    public void Initialize()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject temp = Instantiate(tilePrefab, transform);
            temp.GetComponent<Tile>().Initialize(WaveFunction.Instance.prototypes[i]);
            temp.GetComponent<Button>().onClick.AddListener(delegate { TilePressed(temp); });
        }
    }

    public void TilePressed(GameObject tile)
    {
        gameObject.GetComponent<GridLayoutGroup>().enabled = false;
        tile.transform.localPosition= Vector3.zero;
        tile.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100f);
        tile.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);

        foreach (Transform child in transform)
        {
            if(child.gameObject != tile)
            {
                tiles.Remove(child.gameObject.GetComponent<Tile>());
                Destroy(child.gameObject);
            }
        }

        foreach (GameObject neighbour in neighbours)
        {
            neighbour.GetComponent<GridSpace>().neighbours.Remove(gameObject);
        }

        WaveCollapse();
    }

    private void WaveCollapse()
    {
        Prototype prototype = gameObject.GetComponentInChildren<Tile>().prototype;

        foreach(GameObject neighbour in neighbours)
        {
            GridSpace temp = neighbour.GetComponent<GridSpace>();

            /*
            GridSpace temp = neighbour.GetComponent<GridSpace>();
            if (temp.X != X)
            {
                if (temp.X < X)
                    foreach (GameObject smallSpace in temp.smallSpaces)
                    {
                        if (!prototype.nX.Contains(smallSpace.GetComponentInChildren<Tile>().prototype))
                        {
                            smallSpaces.Remove(smallSpace);
                            //Destroy(smallSpace);
                        }
                    }
                else if (temp.X > X)
                    foreach (GameObject smallSpace in temp.smallSpaces)
                    {
                        if (!prototype.pX.Contains(smallSpace.GetComponentInChildren<Tile>().prototype))
                        {
                            smallSpaces.Remove(smallSpace);
                            //Destroy(smallSpace);
                        }
                    }
            } 
            else
            {
                if (temp.Y > Y)
                    foreach (GameObject smallSpace in temp.smallSpaces)
                    {
                        if (!prototype.pY.Contains(smallSpace.GetComponentInChildren<Tile>().prototype))
                        {
                            smallSpaces.Remove(smallSpace);
                            //Destroy(smallSpace);
                        }
                    }
                else if (temp.Y < Y)
                    foreach (GameObject smallSpace in temp.smallSpaces)
                    {
                        if (!prototype.nY.Contains(smallSpace.GetComponentInChildren<Tile>().prototype))
                        {
                            smallSpaces.Remove(smallSpace);
                            //Destroy(smallSpace);
                        }
                    }
            }*/
        }
    }
}
