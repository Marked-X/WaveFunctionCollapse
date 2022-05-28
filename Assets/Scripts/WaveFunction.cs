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

    public GameObject tilePrefab = null;
    public GameObject gridSpacePrefab = null;

    private GameObject[,] spaces = null;

    private void Start()
    {
        Instance = this;

        CalculateNeighbours();

        spaces = new GameObject[8, 5];
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
        for (int i = 7; i >= 0; i--)
        {
            for (int j = 4; j >= 0; j--)
            {
                Destroy(spaces[i, j]);
            }
        }

        spaces = new GameObject[8, 5];
        CreateGridSpaces();
    }

    private void CreateGridSpaces()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                spaces[i, j] = Instantiate(gridSpacePrefab, gridSpace.transform);
                spaces[i, j].transform.localPosition = new Vector3(i * 100, j * 100);
                GridSpace space = spaces[i, j].GetComponent<GridSpace>();
                space.Initialize();
                space.X = i;
                space.Y = j;
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (i - 1 >= 0)
                    spaces[i, j].GetComponent<GridSpace>().neighbours.Add(spaces[i - 1, j]);
                if (i + 1 < 8)
                    spaces[i, j].GetComponent<GridSpace>().neighbours.Add(spaces[i + 1, j]);
                if (j - 1 >= 0)
                    spaces[i, j].GetComponent<GridSpace>().neighbours.Add(spaces[i, j - 1]);
                if (j + 1 < 5)
                    spaces[i, j].GetComponent<GridSpace>().neighbours.Add(spaces[i, j + 1]);
            }
        }
    }

    private bool Collapse()
    {
        GridSpace lowestEntropy = null;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 5; j++)
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
}
