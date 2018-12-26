using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour {

    public static float CellSize { get; set; }
    public static GameObject instance;
    private Coordinate playerCoordinate;
    public int maxPolyminoLength = 4;
    public static List<Polymino> polyminos = new List<Polymino>();
    public int direction = 1, predirection = 2;
    public static List<Cell> cells = new List<Cell>(), currentLocalList;

    [SerializeField]
    private float density;

    public Cell lastCell;

    public Coordinate plate, plate1, plate2, plate3, plate4;

    private GameObject player;

    private int preFourth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        instance = gameObject;
        GameObject cell1 = PoolScript.instance.GetObjectFromPool("Cell", Vector3.zero, Quaternion.Euler(0, 0, 0));
        CellSize = cell1.GetComponent<BoxCollider2D>().size.x;
        PoolScript.instance.ReturnObjectToPool(cell1);
        GeneratePlate(plate1 = new Coordinate(0, 0));
        GeneratePlate(plate2 = new Coordinate(-plate.x, 0));
        GeneratePlate(plate3 = new Coordinate(-plate.x, -plate.x));
        GeneratePlate(plate4 = new Coordinate(0, -plate.x));
        SummWhitePathGen();
    }

    public void Update()
    {
        playerCoordinate = new Coordinate(Mathf.FloorToInt(player.transform.position.x/CellSize), Mathf.FloorToInt(player.transform.position.y / CellSize));

        preFourth = DetermineFourth();
    }
    public void SummWhitePathGen()
    {
        // for now just a long white path
        cells.Add(new Cell(0, 0));
        cells.Add(new Cell(1, 0, 1));
        lastCell = FindCell(new Coordinate(1, 0));
        WhitePathFirstGen(10);
        while (CheckPathCollisions())
        {
            direction = 1;
            predirection = 2;
            lastCell = FindCell(new Coordinate(1, 0));
            WhitePathFirstGen(10);
        }
        Debug.Log("pppottt  " + currentLocalList[1].potential);
        ManifestListOfCells(currentLocalList);
    }
    bool CheckPathCollisions()
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
        return false;
    }
    void ManifestListOfCells(List<Cell> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = list[i].Manifest();
        }
    }
    public void WhitePathFirstGen(int n)
    {
        currentLocalList = new List<Cell>();
        int cellCount = 0;
        while(cellCount < n)
        {
            int localLength = DetermineLineLength();
            //Debug.Log("localLength = " + localLength + " direction  = " + direction + "   predirection =    " + predirection);
            cellCount += localLength;
            switch (direction)
            {
                case 1:
                    if (predirection == 4) {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true, 0));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1));
                        }
                        if (localLength == 1)
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1));
                        }
                        else
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x, lastCell.Coordinate.y + 1));
                        }
                    }
                    else
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true, 0));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1));
                        }
                        if (localLength == 1)
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y + 1));
                        }
                        else
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y));
                        }
                    }
                    break;
                case 2:
                    if (predirection == 1)
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1, true, 0));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1));
                        }
                        if (localLength == 1)
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1));
                        }
                        else
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y));
                        }
                    }
                    else
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1, true, 0));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1));
                        }
                        if (localLength == 1)
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y + 1));
                        }
                        else
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y + 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x, lastCell.Coordinate.y + 1));
                        }
                    }
                    break;
                case 3:
                    if (predirection == 2)
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, true, 0));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1));
                        }
                        if (localLength == 1)
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1));
                        }
                        else
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y));
                        }
                    }
                    else
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1, true, 0));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1));
                        }
                        if (localLength == 1)
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y - 1));
                        }
                        else
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x - 1, lastCell.Coordinate.y, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x - 1, lastCell.Coordinate.y));
                        }
                    }
                    break;
                case 4:
                    if (predirection == 3)
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true, 0));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1));
                        }
                        if (localLength == 1)
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1));
                        }
                        else
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y));
                        }
                    }
                    else
                    {
                        while (localLength > 1)
                        {
                            localLength -= 2;
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, true, 0));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1));
                        }
                        if (localLength == 1)
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1, true, 0));
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x + 1, lastCell.Coordinate.y - 1));
                        }
                        else
                        {
                            cells.Add(new Cell(lastCell.Coordinate.x, lastCell.Coordinate.y - 1, true, 1));
                            lastCell = FindCell(new Coordinate(lastCell.Coordinate.x, lastCell.Coordinate.y - 1));
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
    }
    public int DetermineLineLength()
    {
        int localLength = Random.Range(1, 5);
        return localLength;
    }
    public void PlateGeneration()
    {
        if (DetermineFourth() != preFourth)
        {
            switch (preFourth)
            {
                case 1:
                    switch (DetermineFourth())
                    {
                        case 2:

                            break;
                        case 3:

                            break;
                        case 4:

                            break;
                    }
                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
            }
        }
    }
    public static Cell FindCell(Coordinate coord)
    {
        Cell outCell = null;
        //Debug.Log(cells.Count);
        foreach(Cell cell in cells)
        {
            if (cell.Coordinate.Equals(coord))
            {
                outCell = cell;
            }
        }
        if (outCell != null) return outCell; else return null;
    }
    public void DebugList(List<Cell> list)
    {
        foreach(Cell cell in list)
        {
            Debug.Log(cell.Coordinate.x + "   "  + cell.Coordinate.y);
        }
    }
    public void GeneratePlate(Coordinate _plate)
    {
        float densityOfPlate = 0;
        int summPolyminoLength = 0;
        while (densityOfPlate < density)
        {
            Coordinate rCoord = Coordinate.RandomCoordinateToStartPolymino(plate.x);
            int polyminoLength = Random.Range(1, maxPolyminoLength);
            summPolyminoLength += polyminoLength;
            CreatePolymino(polyminoLength,new Coordinate(_plate.x + rCoord.x, _plate.y + rCoord.y));
            densityOfPlate = (float)summPolyminoLength / plate.x / plate.x;
        }
    }
    public Polymino CreatePolymino(int n, Coordinate coord)
    {
        bool output = false;
        Polymino outPoly = new Polymino();
        while (!output)
        {
            output = true;
            outPoly = new Polymino(n, coord);
            foreach (Polymino poly2 in polyminos)
            {
                if (outPoly.CheckForCollisions(poly2))
                {
                    output = false;
                    outPoly.del();
                }
            }
        }
        return outPoly;
    }
    public int DetermineFourth()
    {
        Coordinate coord = DetermineCurrentPlateCoordinate();
        if (playerCoordinate.x < coord.x + plate.x/2)
        {
            if (playerCoordinate.y < coord.y + plate.y / 2)
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
        else
        {
            if (playerCoordinate.y < coord.y + plate.y / 2)
            {
                return 4;
            }
            else
            {
                return 1;
            }
        }
    }
    public Coordinate DetermineCurrentPlateCoordinate()
    {
        int currentx = playerCoordinate.x;
        int currenty = playerCoordinate.y;
        while (currentx % plate.x != 0)
        {
            currentx--;
        }
        while (currenty % plate.y != 0)
        {
            currenty--;
        }
        return new Coordinate(currentx, currenty);
    }
}
