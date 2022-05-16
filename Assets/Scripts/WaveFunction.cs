using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunction : MonoBehaviour
{
    public static WaveFunction Instance = null;

    public List<Prototype> prototypes = null;

    private void Start()
    {
        Instance = this;
    }

    public void CalculateNeighbours()
    {
        List<Prototype> neighbours;

        foreach (Prototype prototype in prototypes)
        {
            prototype.ClearNeighbours();
            neighbours = new List<Prototype>(prototypes);
            neighbours.Remove(prototype);

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
}
