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

    enum Sides
    {
        Up, Down, Left, Right
    }

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

        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            if (tiles[i] != choosenPrototype)
            {
                Destroy(tiles[i]);
                tiles.RemoveAt(i);
            }
        }

        prototypeCount = -1;

        foreach (GameObject neighbour in neighbours)
        {
            neighbour.GetComponent<GridSpace>().neighbours.Remove(gameObject);
        }

        Propagate(gameObject);
    }

    private void Propagate(GameObject mainObj)
    {
        Queue<GridSpace> queue = new Queue<GridSpace>();
        queue.Enqueue(mainObj.GetComponent<GridSpace>());

        while (queue.Count > 0)
        {
            GridSpace currentGridSpace = queue.Dequeue();

            foreach (GameObject neighbour in currentGridSpace.neighbours)
            {
                GridSpace neighbourGridSpace = neighbour.GetComponent<GridSpace>();
                if (neighbourGridSpace.isChecked == false)
                {
                    Sides side = CheckSide(currentGridSpace, neighbourGridSpace);
                    Constrain(currentGridSpace, neighbourGridSpace, side);

                    if (!queue.Contains(neighbourGridSpace))
                        queue.Enqueue(neighbourGridSpace);
                }
            }

            currentGridSpace.isChecked = true;
        }

        WaveFunction.Instance.ResetGridCheck();
    }

    private Sides CheckSide(GridSpace main, GridSpace neighbour)
    {
        if (neighbour.X != main.X)
        {
            if (neighbour.X < main.X)
                return Sides.Left;
            else
                return Sides.Right;
        }
        else
        {
            if (neighbour.Y < main.Y)
                return Sides.Down;
            else
                return Sides.Up;
        }
    }

    private void Constrain(GridSpace first, GridSpace second, Sides side)
    {
        List<Prototype> possibleList = new List<Prototype>();

        foreach (GameObject tile in first.tiles)
        {
            List<Prototype> listToCheck = null;
            switch (side)
            {
                case Sides.Up:
                    listToCheck = tile.GetComponent<Tile>().prototype.pY;
                    break;
                case Sides.Down:
                    listToCheck = tile.GetComponent<Tile>().prototype.nY;
                    break;
                case Sides.Left:
                    listToCheck = tile.GetComponent<Tile>().prototype.nX;
                    break;
                case Sides.Right:
                    listToCheck = tile.GetComponent<Tile>().prototype.pX;
                    break;
            }
            foreach (Prototype prot in listToCheck)
            {
                if (!possibleList.Contains(prot))
                    possibleList.Add(prot);
            }
        }
        
        for (int i = second.tiles.Count - 1; i >= 0; i--)
        { 
            if (!possibleList.Contains(second.tiles[i].GetComponent<Tile>().prototype))
            {
                Destroy(second.tiles[i]);
                second.tiles.RemoveAt(i);
                second.prototypeCount--;
            }
        }
    }
}
