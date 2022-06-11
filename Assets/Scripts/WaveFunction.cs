using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveFunction : MonoBehaviour
{
    public static WaveFunction Instance = null;

    public List<Prototype> prototypes = null;

    public GameObject gridSpace = null;
    public int horizontalGridSpaces = 8;
    public int verticalGridSpaces = 5;

    public GameObject tilePrefab = null;
    public GameObject gridSpacePrefab = null;

    private GameObject[,] spaces = null;

    private enum Sides
    {
        Up, Down, Left, Right
    }

    private void Start()
    {
        Instance = this;

        CalculateNeighbours();

        spaces = new GameObject[horizontalGridSpaces, verticalGridSpaces];
        CreateGridSpaces();
    }

    public void Solve()
    {
        while (Collapse()) ;
    }

    public void SolveOnce()
    {
        Collapse();
    }

    public void ResetGridCheck()
    {
        foreach (GameObject obj in spaces)
        {
            obj.GetComponent<GridSpace>().isChecked = false;
        }
    }

    public void CalculateNeighbours()
    {
        List<Prototype> neighbours;

        foreach (Prototype prototype in prototypes)
        {
            prototype.ClearNeighbours();
            neighbours = new List<Prototype>(prototypes);

            foreach (Prototype neighbour in neighbours)
            {
                if (prototype.negX == neighbour.posX)
                    prototype.nX.Add(neighbour);
                if (prototype.posX == neighbour.negX)
                    prototype.pX.Add(neighbour);
                if (prototype.negY == neighbour.posY)
                    prototype.nY.Add(neighbour);
                if (prototype.posY == neighbour.negY)
                    prototype.pY.Add(neighbour);
            }
        }
    }

    public void Restart()
    {
        for (int i = horizontalGridSpaces - 1; i >= 0; i--)
        {
            for (int j = verticalGridSpaces - 1; j >= 0; j--)
            {
                Destroy(spaces[i, j]);
            }
        }

        spaces = new GameObject[horizontalGridSpaces, verticalGridSpaces];
        CreateGridSpaces();
    }

    private void CreateGridSpaces()
    {
        for (int i = 0; i < horizontalGridSpaces; i++)
        {
            for (int j = 0; j < verticalGridSpaces; j++)
            {
                spaces[i, j] = Instantiate(gridSpacePrefab, gridSpace.transform);
                spaces[i, j].transform.localPosition = new Vector3(i * 100, j * 100);
                GridSpace space = spaces[i, j].GetComponent<GridSpace>();
                space.Initialize();
                space.X = i;
                space.Y = j;
            }
        }

        for (int i = 0; i < horizontalGridSpaces; i++)
        {
            for (int j = 0; j < verticalGridSpaces; j++)
            {
                if (i - 1 >= 0)
                    spaces[i, j].GetComponent<GridSpace>().neighbours.Add(spaces[i - 1, j]);
                if (i + 1 < horizontalGridSpaces)
                    spaces[i, j].GetComponent<GridSpace>().neighbours.Add(spaces[i + 1, j]);
                if (j - 1 >= 0)
                    spaces[i, j].GetComponent<GridSpace>().neighbours.Add(spaces[i, j - 1]);
                if (j + 1 < verticalGridSpaces)
                    spaces[i, j].GetComponent<GridSpace>().neighbours.Add(spaces[i, j + 1]);
            }
        }
    }

    private bool Collapse()
    {
        GridSpace lowestEntropy = null;

        for (int i = 0; i < horizontalGridSpaces; i++)
        {
            for (int j = 0; j < verticalGridSpaces; j++)
            {
                GridSpace space = spaces[i, j].GetComponent<GridSpace>();
                if (space.prototypeCount > 0)
                {
                    if (lowestEntropy == null || lowestEntropy.prototypeCount > space.prototypeCount)
                        lowestEntropy = space;
                }
            }
        }

        if (lowestEntropy == null)
        {
            return false;
        }
        else if (lowestEntropy.prototypeCount == 1)
        {
            lowestEntropy.PrototypePicked(lowestEntropy.tiles[0]);
        }
        else if (lowestEntropy.prototypeCount > 1)
        {
            int rand = UnityEngine.Random.Range(0, lowestEntropy.prototypeCount);
            lowestEntropy.PrototypePicked(lowestEntropy.tiles[rand]);
        }
        return true;
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

    public void Propagate(GameObject mainObj)
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

        ResetGridCheck();
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

        second.ConstrainByList(possibleList);
    }
}
