using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour {

    public static float CellSize { get; set; }
    public static GameObject instance;
    private Coordinate playerCoordinate;
    public int maxPolyminoLength = 4;
    public static List<Polymino> polyminos = new List<Polymino>();

    public static List<Cell> cells = new List<Cell>();

    [SerializeField]
    private float density;

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
        new Cell(1, 1);
        new Cell(1, 2);
        new Cell(2, 1);
        new Cell(2, 2);
        Debug.Log(Find(new Coordinate(2, 1)));
        //GeneratePlate(plate1 = new Coordinate(0, 0));
        //GeneratePlate(plate2 = new Coordinate(-plate.x, 0));
        //GeneratePlate(plate3 = new Coordinate(-plate.x, -plate.x));
        //GeneratePlate(plate4 = new Coordinate(0, -plate.x));

    }

    public void Update()
    {
        playerCoordinate = new Coordinate(Mathf.FloorToInt(player.transform.position.x/CellSize), Mathf.FloorToInt(player.transform.position.y / CellSize));

        preFourth = DetermineFourth();
    }
    public void WhitePathFirstGen(int n)
    {
        //for (int i )
        //{

        //}
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
    public static Cell Find(Coordinate coord)
    {
        Cell outCell = null;
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
