using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NCellController : MonoBehaviour {

    public static float CellSize { get; set; }

    private Coordinate playerCoordinate;

    private GameObject player;

    public int direction = 1, predirection = 2;
    public NCell lastCell;

    [SerializeField]
    int pathLength = 10;

    List<NCell> curPath;

    public float screenWidth, screenHeight;
    public int cellWidth, cellHeight;
    public NCell startCell, curCell;

    public void Start()
    {
        GameObject virtualScreenQuad = GameObject.Find("Virtual Screen Quad");
        screenHeight = virtualScreenQuad.transform.localScale.y;
        screenWidth = virtualScreenQuad.transform.localScale.x;

        player = GameObject.FindGameObjectWithTag("Player");
        GameObject cell1 = PoolScript.instance.GetObjectFromPool("Cell", Vector3.zero, Quaternion.Euler(0, 0, 0));
        CellSize = cell1.GetComponent<BoxCollider2D>().size.x;
        cellWidth = Mathf.FloorToInt(screenWidth / CellSize) + 1;
        cellHeight = Mathf.FloorToInt(screenHeight / CellSize) + 1;

        new NCell(0, 0, false);
        new NCell(1, 0, true);
        lastCell = FindCell(new Coordinate(1, 0));
        startCell = 
        List<NCell> list = createPath();
        while (CheckForDistance(startCell, list) || CheckForIntersections(list, list))
        {
            list = createPath();
        }
    }

    public void Update()
    {
        playerCoordinate = new Coordinate(Mathf.FloorToInt(player.transform.position.x / CellSize), Mathf.FloorToInt(player.transform.position.y / CellSize));
        List<NCell> list = new List<NCell>();
        if (startCell.Coordinate.Equals(playerCoordinate))
        {
            list = createPath();
            while (CheckForDistance(startCell, list) || CheckForIntersections(list, curPath))
            {
                list = createPath();
            }
        }
        curPath = list;
    }

    public List<NCell> createPath()
    {
        List<NCell> currentLocalList = new List<NCell>();

        int cellCount = 0;
        while (cellCount < pathLength)
        {
            int localLength = DetermineLineLength();
            cellCount += localLength;
            switch (direction)
            {
                case 1:
                    if (predirection == 4)
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                        if (localLength == 1)
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                        else
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                    }
                    else
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                        if (localLength == 1)
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                        else
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y), currentLocalList);
                        }
                    }
                    break;
                case 2:
                    if (predirection == 1)
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1, true));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                        if (localLength == 1)
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                        else
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y), currentLocalList);
                        }
                    }
                    else
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1, true));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                        if (localLength == 1)
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                        else
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x, lastCell.Coordinate.y + 1), currentLocalList);
                        }
                    }
                    break;
                case 3:
                    if (predirection == 2)
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, true));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1), currentLocalList);
                        }
                        if (localLength == 1)
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1), currentLocalList);
                        }
                        else
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y), currentLocalList);
                        }
                    }
                    else
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1, true));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1), currentLocalList);
                        }
                        if (localLength == 1)
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1), currentLocalList);
                        }
                        else
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y), currentLocalList);
                        }
                    }
                    break;
                case 4:
                    if (predirection == 3)
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1), currentLocalList);
                        }
                        if (localLength == 1)
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1), currentLocalList);
                        }
                        else
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y), currentLocalList);
                        }
                    }
                    else
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, true));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1), currentLocalList);
                        }
                        if (localLength == 1)
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1, true));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1), currentLocalList);
                        }
                        else
                        {
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x, lastCell.Coordinate.y - 1), currentLocalList);
                        }
                    }
                    break;
            }
            int _dir = direction;
            if ((direction == 1 && localLength % 2 == 1 && predirection == 4) || (direction == 1 && localLength % 2 == 0 && predirection == 2) || (direction == 3 && localLength % 2 == 0 && predirection == 2) || (direction == 3 && localLength % 2 == 1 && predirection == 4))
            {
                direction = 2;
            }
            else
            if ((direction == 1 && localLength % 2 == 0 && predirection == 4) || (direction == 1 && localLength % 2 == 1 && predirection == 2) || (direction == 3 && localLength % 2 == 1 && predirection == 2) || (direction == 3 && localLength % 2 == 0 && predirection == 4))
            {
                direction = 4;
            }
            else
            if ((direction == 2 && localLength % 2 == 1 && predirection == 3) || (direction == 2 && localLength % 2 == 0 && predirection == 1) || (direction == 4 && localLength % 2 == 1 && predirection == 3) || (direction == 4 && localLength % 2 == 0 && predirection == 1))
            {
                direction = 1;
            }
            else
            if ((direction == 2 && localLength % 2 == 1 && predirection == 1) || (direction == 2 && localLength % 2 == 0 && predirection == 3) || (direction == 4 && localLength % 2 == 1 && predirection == 1) || (direction == 4 && localLength % 2 == 0 && predirection == 3))
            {
                direction = 3;
            }
            predirection = _dir;
        }
        return currentLocalList;
    }
    public bool CheckForIntersections(List<NCell> currentLocalList, List<NCell> anotherList)
    {
        for (int i = 0; i < currentLocalList.Count - 1; i++)
        {
            for (int j = i + 1; j < currentLocalList.Count; j++)
            {
                if (currentLocalList[i].Coordinate.Equals(currentLocalList[j].Coordinate) && currentLocalList[i].empty != currentLocalList[j].empty)
                {
                    return true;
                }
            }
        }
        for (int i = 0; i < currentLocalList.Count - 1; i++)
        {
            for (int j = 0; j < anotherList.Count - 1; j++)
            {
                if (currentLocalList[i].Coordinate.Equals(anotherList[j].Coordinate) && currentLocalList[i].empty != anotherList[j].empty)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckForDistance(NCell startPoint, List<NCell> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if (Mathf.Abs(list[i].Coordinate.x - startPoint.Coordinate.x) < cellWidth/2 || Mathf.Abs(list[i].Coordinate.y - startPoint.Coordinate.y) < cellHeight / 2)
            {
                return true;
            }
        }
        return false;
    }
    public int DetermineLineLength()
    {
        int localLength = Random.Range(1, 5);
        return localLength;
    }
    public static NCell FindCell(Coordinate coord, List<NCell> cells)
    {
        NCell outCell = null;
        //Debug.Log(cells.Count);
        foreach (NCell cell in cells)
        {
            if (cell.Coordinate.Equals(coord))
            {
                outCell = cell;
            }
        }
        if (outCell != null) return outCell; else return null;
    }
}
