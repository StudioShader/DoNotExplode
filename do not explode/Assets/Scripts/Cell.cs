using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

public class Cell {
    public GameObject cell;
    public bool empty;
    public Vector2 Position { get; set; }
    public Coordinate Coordinate { get; set; }

    public Cell(Coordinate coord)
    {
        empty = false;
        Coordinate = coord;
        Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize/2, coord.y * CellController.CellSize + CellController.CellSize / 2);
        cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
        cell.transform.parent = CellController.instance.transform;
        CellController.cells[coord.x,coord.y] = this;
    }
    public Cell(int X, int Y)
    {
        empty = false;
        Coordinate coord = new Coordinate(X, Y);
        Coordinate = coord;
        Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize / 2, coord.y * CellController.CellSize + CellController.CellSize / 2);
        cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
        cell.transform.parent = CellController.instance.transform;
        CellController.cells[X, Y] = this;
    }
    public Cell(int X, int Y, bool empty)
    {
        if (empty)
        {
            empty = true;
            Coordinate coord = new Coordinate(X, Y);
            Coordinate = coord;
            cell = null;
            Position = new Vector2();
        }
        else
        {
            empty = false;
            Coordinate coord = new Coordinate(X, Y);
            Coordinate = coord;
            Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize / 2, coord.y * CellController.CellSize + CellController.CellSize / 2);
            cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
            cell.transform.parent = CellController.instance.transform;
            CellController.cells[X, Y] = this;
        }

    }
    public void del()
    {
        CellController.cells[Coordinate.x, Coordinate.y] = null;
        PoolScript.instance.ReturnObjectToPool(cell);
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
        while (!output) {
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
}

