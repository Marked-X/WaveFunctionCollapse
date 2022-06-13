using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Prototype", menuName = "WaveFunctionCollapse/Prototype")]
public class Prototype : ScriptableObject
{
    public Sprite sprite;

    public int posX;
    public int negX;
    public int posY;
    public int negY;

    public List<Prototype> pX = new List<Prototype>();
    public List<Prototype> nX = new List<Prototype>();
    public List<Prototype> pY = new List<Prototype>();
    public List<Prototype> nY = new List<Prototype>();

    public void ClearNeighbours()
    {
        pX.Clear();
        nX.Clear();
        pY.Clear();
        nY.Clear();
    }
}
