using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

public class Cell {
    public GameObject cell;
    public int empty;
    public Vector2 Position { get; set; }
    public Coordinate Coordinate { get; set; }

    public Cell(Coordinate coord)
    {
        if (CellController.FindCell(coord) != null)
        {
            Cell alreadyCell = CellController.FindCell(coord);
            if (alreadyCell.empty == -1)
            {
                alreadyCell.del();
                empty = -1;
                Coordinate = coord;
                Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize / 2, coord.y * CellController.CellSize + CellController.CellSize / 2);
                cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
                cell.transform.parent = CellController.instance.transform;
                CellController.cells.Add(this);
            }
        }
        else
        {
            empty = -1;
            Coordinate = coord;
            Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize / 2, coord.y * CellController.CellSize + CellController.CellSize / 2);
            cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
            cell.transform.parent = CellController.instance.transform;
            CellController.cells.Add(this);
        }
    }
    public Cell(int X, int Y)
    {
        Coordinate coord = new Coordinate(X, Y);
        if (CellController.FindCell(coord) != null)
        {
            Cell alreadyCell = CellController.FindCell(coord);
            if (alreadyCell.empty == -1)
            {
                alreadyCell.del();
                empty = -1;
                Coordinate = coord;
                Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize / 2, coord.y * CellController.CellSize + CellController.CellSize / 2);
                cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
                cell.transform.parent = CellController.instance.transform;
                CellController.cells.Add(this);
            }
        }
        else
        {
            empty = -1;
            Coordinate = coord;
            Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize / 2, coord.y * CellController.CellSize + CellController.CellSize / 2);
            cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
            cell.transform.parent = CellController.instance.transform;
            CellController.cells.Add(this);
        }
    }
    public Cell(int X, int Y, int _empty)
    {
        Debug.Log(X + "  " + Y);
        if (CellController.FindCell(new Coordinate(X, Y)) != null)
        {
            Cell alreadyCell = CellController.FindCell(new Coordinate(X, Y));
            if (alreadyCell.empty == -1)
            {
                alreadyCell.del();

                if (_empty == 0)
                {
                    empty = 0;
                    Coordinate coord = new Coordinate(X, Y);
                    Coordinate = coord;
                    cell = null;
                    Position = new Vector2();
                    CellController.cells.Add(this);
                }
                else
                {
                    empty = 1;
                    Coordinate coord = new Coordinate(X, Y);
                    Coordinate = coord;
                    Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize / 2, coord.y * CellController.CellSize + CellController.CellSize / 2);
                    cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
                    cell.transform.parent = CellController.instance.transform;
                    CellController.cells.Add(this);
                }
            }
            else
            {
                Debug.Log("alert: interseption between two empty cells  " + X + "  " + Y);
            }
        }
        else
        {
            if (_empty == 0)
            {
                empty = 0;
                Coordinate coord = new Coordinate(X, Y);
                Coordinate = coord;
                cell = null;
                Position = new Vector2();
                CellController.cells.Add(this);
            }
            else
            {
                empty = 1;
                Coordinate coord = new Coordinate(X, Y);
                Coordinate = coord;
                Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize / 2, coord.y * CellController.CellSize + CellController.CellSize / 2);
                cell = PoolScript.instance.GetObjectFromPool("Cell", Position, Quaternion.Euler(0, 0, 0));
                cell.transform.parent = CellController.instance.transform;
                CellController.cells.Add(this);
            }
        }
    }
    public void del()
    {
        CellController.cells.Remove(CellController.FindCell(Coordinate));
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
    public bool Equals(Coordinate _coord)
    {
        if (_coord == null) return false;
        if (_coord.x == x && _coord.y == y) return true;
        return false;
    }
}

