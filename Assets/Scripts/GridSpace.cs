using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public List<GameObject> neighbours = new List<GameObject>();
    public List<GameObject> tiles = new List<GameObject>();
    public int prototypeCount = 0;
    public GameObject tilePrefab = null;
    public int X;
    public int Y;
    public bool isChecked = false;

    public void Initialize()
    {
        prototypeCount = WaveFunction.Instance.prototypes.Count;
        for (int i = 0; i < prototypeCount; i++)
        {
            GameObject temp = Instantiate(tilePrefab, transform);
            temp.GetComponent<Tile>().Initialize(WaveFunction.Instance.prototypes[i]);
            temp.GetComponent<Button>().onClick.AddListener(delegate { PrototypePicked(temp); });
            tiles.Add(temp);
        }
    }

    public void PrototypePicked(GameObject choosenPrototype)
    {
        gameObject.GetComponent<GridLayoutGroup>().enabled = false;
        choosenPrototype.transform.localPosition = Vector3.zero;
        choosenPrototype.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100f);
        choosenPrototype.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);
        choosenPrototype.GetComponent<Tile>().TileChoosen();

        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            if (tiles[i] != choosenPrototype)
            {
                tiles[i].GetComponent<Tile>().Remove();
                tiles.RemoveAt(i);
            }
        }

        prototypeCount = -1;

        foreach (GameObject neighbour in neighbours)
        {
            neighbour.GetComponent<GridSpace>().neighbours.Remove(gameObject);
        }

        WaveFunction.Instance.Propagate(gameObject);
    }

    public void ConstrainByList(List<Prototype> possibleList)
    {
        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            if (!possibleList.Contains(tiles[i].GetComponent<Tile>().prototype))
            {
                prototypeCount--;
                tiles[i].GetComponent<Tile>().Remove();
                tiles.RemoveAt(i);
            }
        }
    }
}
