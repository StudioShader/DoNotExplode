using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NCell {
    public GameObject cell;
    public bool empty;
    public Vector2 Position { get; set; }
    public Coordinate Coordinate { get; set; }
    public NCell(int X, int Y, bool _empty)
    {
        empty = _empty;
        Coordinate coord = new Coordinate(X, Y);
        Coordinate = coord;
        Position = new Vector2(coord.x * NCellController.CellSize + NCellController.CellSize / 2, coord.y * NCellController.CellSize + NCellController.CellSize / 2);
        cell = null;
    }
    public NCell(int X, int Y)
    {
        empty = false;
        Coordinate coord = new Coordinate(X, Y);
        Coordinate = coord;
        Position = new Vector2(coord.x * NCellController.CellSize + NCellController.CellSize / 2, coord.y * NCellController.CellSize + NCellController.CellSize / 2);
        cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
        cell.transform.parent = NCellController.instance.transform;
    }
    public NCell Manifest()
    {
        if (!empty && cell == null)
        {
            return new NCell(Coordinate.x, Coordinate.y);
        }
        else
        {
            return this;
            Debug.Log("trying to manifest unpotentional cell:  " + Coordinate.x + "  " + Coordinate.y);
            return null;
        }
    }
}
[System.Serializable]
public class Coordinate
{
    public int x;
    public int y;
    public Coordinate(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public static Coordinate RandomCoordinateToStartPolymino(int n)
    {
        bool output = false;
        int _y = 0;
        int _x = 0;
        while (!output)
        {
            output = true;
            _x = Random.Range(0, n);
            _y = Random.Range(0, n);
            foreach (Polymino poly in CellController.polyminos)
            {
                foreach (Cell cl in poly.cells)
                {
                    if (cl.Coordinate.x == _x || cl.Coordinate.y == _y)
                    {
                        output = false;
                    }
                }
            }
        }
        return new Coordinate(_x, _y);
    }
    public bool Equals(Coordinate _coord)
    {
        if (_coord == null) return false;
        if (_coord.x == x && _coord.y == y) return true;
        return false;
    }
}
