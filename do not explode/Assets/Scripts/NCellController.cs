using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NCellController : MonoBehaviour {

    public static float CellSize { get; set; }

    public static GameObject instance;

    private Coordinate playerCoordinate;

    private GameObject player;

    public int direction = 1, predirection = 2;

    public NCell lastCell;

    [SerializeField]
    int pathLength;

    [SerializeField]
    List<NCell> curPath = new List<NCell>();

    public float screenWidth, screenHeight;
    public int cellWidth, cellHeight;

    [SerializeField]
    private List<NCell> auxiliaryList = new List<NCell>();

    [SerializeField]
    public NCell startCell, curCell, preStartCell;

    public void Start()
    {
        instance = gameObject;
        GameObject virtualScreenQuad = GameObject.Find("Virtual Screen Quad");
        screenHeight = virtualScreenQuad.transform.localScale.y;
        screenWidth = virtualScreenQuad.transform.localScale.x;

        player = GameObject.FindGameObjectWithTag("Player");
        GameObject cell1 = PoolScript.instance.GetObjectFromPool("Cell", Vector3.zero, Quaternion.Euler(0, 0, 0));
        CellSize = cell1.GetComponent<BoxCollider2D>().size.x;
        PoolScript.instance.ReturnObjectToPool(cell1);
        cellWidth = Mathf.FloorToInt(screenWidth / CellSize) + 1;
        cellHeight = Mathf.FloorToInt(screenHeight / CellSize) + 1;
        preStartCell = new NCell(1, 0, true);
        lastCell = new NCell(0, 0, false);
        lastCell.Manifest();
        preStartCell.Manifest();

        int pDir = direction;
        int pPreDir = predirection;
        NCell pLastCell = lastCell;
        curPath = createPath();

        while (CheckForSelfIntersections(curPath) || CheckForDistanceBetweenCells(preStartCell, startCell))
        {
            CreateClearData(pLastCell, pDir, pPreDir);
            curPath = createPath();
        }
        ManifestListOfNCells(curPath);
        curPath.Add(preStartCell);
        Debug.Log(Mathf.Abs(5 - 10));
    }

    public void Update()
    {
        playerCoordinate = new Coordinate(Mathf.FloorToInt(player.transform.position.x / CellSize), Mathf.FloorToInt(player.transform.position.y / CellSize));
        List<NCell> list = new List<NCell>();
        //Debug.Log(playerCoordinate.x + " " + playerCoordinate.y);
        if (preStartCell.Coordinate.Equals(playerCoordinate))
        {
            //Debug.Log("A");
            NCell _startCell = startCell;
            int pDir = direction;
            int pPreDir = predirection;
            NCell pLastCell = lastCell;
            list = createPath();
            //CheckForDistance(startCell, list)   || CheckForDistanceBetweenCells(preStartCell, startCell)     || CheckForDistance(preStartCell, list)
            while (CheckForIntersections(list, curPath) || CheckForSelfIntersections(list) || CheckForDistanceBetweenCells(preStartCell, startCell) || CheckForDistance(preStartCell, list))
            {
                //Debug.Log("b");
                CreateClearData(pLastCell, pDir, pPreDir);
                list = createPath();
            }
            //startCell = lastCell;
            preStartCell = _startCell;
            auxiliaryList = curPath;
            curPath = list;
            ManifestListOfNCells(curPath);
        }
    }

    public List<NCell> createPath()
    {
        //Debug.Log(lastCell.Coordinate.x + " " + lastCell.Coordinate.y);
        List<NCell> currentLocalList = new List<NCell>();

        int cellCount = 0;
        while (cellCount < pathLength)
        {
            int localLength = DetermineLineLength();
            cellCount += localLength;
            //Debug.Log(direction);
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
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, false));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x, lastCell.Coordinate.y - 1), currentLocalList);
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
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1, false));
                            currentLocalList.Add(new NCell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, true));
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

            if (predirection == 3 && direction == 4 || predirection == 2 && direction == 1)
            {
                startCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y), currentLocalList);
            }
            if (predirection == 3 && direction == 2 || predirection == 4 && direction == 1)
            {
                startCell = FindCell(new Coordinate(lastCell.Coordinate.x, lastCell.Coordinate.y + 1), currentLocalList);
            }
            if (predirection == 4 && direction == 3 || predirection == 1 && direction == 2)
            {
                startCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y), currentLocalList);
            }
            if (predirection == 1 && direction == 4 || predirection == 2 && direction == 3)
            {
                startCell = FindCell(new Coordinate(lastCell.Coordinate.x, lastCell.Coordinate.y - 1), currentLocalList);
            }
        }
        return currentLocalList;
    }
    public bool CheckForIntersections(List<NCell> currentLocalList, List<NCell> anotherList)
    {
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
    public bool CheckForSelfIntersections(List<NCell> list)
    {
        for (int i = 0; i < list.Count-1; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[i].Coordinate.Equals(list[j].Coordinate) && list[i].empty != list[j].empty)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckForDistance(NCell startPoint, List<NCell> list)
    {
        //Debug.Log("OOOOOOOOONNNNNNNNNNNNNNEEEEEEEEEEEEEEE");
        for(int i = 0; i < list.Count; i++)
        {
            //Debug.Log("strartpoint: " + startPoint.Coordinate.x + " " + startPoint.Coordinate.y);
            //Debug.Log("list i : " + list[i].Coordinate.x + " " + list[i].Coordinate.y);
            if (Mathf.Abs(list[i].Coordinate.x - startPoint.Coordinate.x) < cellWidth/2 && Mathf.Abs(list[i].Coordinate.y - startPoint.Coordinate.y) < cellHeight / 2)
            {
                return true;
            }
        }
        return false;
    }
    public int DetermineLineLength()
    {
        int localLength = Random.Range(1, 7);
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
    public void DebugList(List<NCell> list)
    {
        foreach (NCell cell in list)
        {
            Debug.Log(cell.Coordinate.x + "   " + cell.Coordinate.y + " " + cell.empty);
        }
    }
    void ManifestListOfNCells(List<NCell> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = list[i].Manifest();
        }
    }
    public void CreateClearData(NCell pLastNCell, int pDir,int pPreDir)
    {
        lastCell = pLastNCell;
        predirection = pPreDir;
        direction = pDir;
    }
    public bool CheckForDistanceBetweenCells(NCell cell1, NCell cell2)
    {
        if (Mathf.Abs(cell1.Coordinate.x - cell2.Coordinate.x) < 4 && Mathf.Abs(cell1.Coordinate.y - cell2.Coordinate.y) < 4)
        {
            return true;
        }
        return false;
    }
}
