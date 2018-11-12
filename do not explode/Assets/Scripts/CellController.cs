using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour {

    public static float CellSize { get; set; }
    public static GameObject instance;
    private Coordinate playerCoordinate;
    public int maxPolyminoLength = 4;
    public static List<Polymino> polyminos = new List<Polymino>();
    public static Cell[,] cells = new Cell[100,100];

    public static List<Cell> Cells = new List<Cell>();

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

        Cells.Add(new Cell(1, 1));
        Cells.Add(new Cell(1, 2));
        Cells.Add(new Cell(2, 1));
        Cells.Add(new Cell(2, 2));
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
    //public void InitializeCellMass(int n)    this is a bad cause this is a list but not a mass
    //{
    //    for (int j = -n; j < n; j++)
    //    {
    //        cells.Add(new List<Cell>());
    //        for (int k = -n; k < n; k++)
    //        {
    //            cells[j].Add(new Cell(j, k));
    //        }
    //    }
    //}
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
