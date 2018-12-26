using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

public class Cell {
    public GameObject cell;
    public int empty;
    public bool potential;
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
    public Cell(int X, int Y, bool potential, int _empty)
    {
        Debug.Log("cell: " + X + "  " + Y + " was insert");
        empty = _empty;
        potential = true;
        Coordinate coord = new Coordinate(X, Y);
        //empty = -1;
        Coordinate = coord;
        Position = new Vector2(coord.x * CellController.CellSize + CellController.CellSize / 2, coord.y * CellController.CellSize + CellController.CellSize / 2);
        CellController.currentLocalList.Add(this);
        Debug.Log("potent after insertion  " + CellController.FindCell(coord).potential);
    }
    public Cell Manifest()
    {
        if (potential)
        {
            Debug.Log("potentional cell was manifested: " + Coordinate.x + "  " + Coordinate.y);
            potential = false;
            return new Cell(Coordinate.x, Coordinate.y, empty);
        }
        else
        {
            Debug.Log("trying to manifest unpotentional cell:  " + Coordinate.x + "  " + Coordinate.y);
            return null;
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
                if (alreadyCell.empty != _empty)
                {
                    Debug.Log("ohh, shit. This is realy bad  " + X + "  " + Y);
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
                    Debug.Log("nahh, not so bad  " + X + "  " + Y);
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
        if (cell != null)
        {
        PoolScript.instance.ReturnObjectToPool(cell);
        }
    }
}



