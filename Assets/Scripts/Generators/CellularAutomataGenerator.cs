using UnityEngine;

public class CellularAutomataGenerator : Generator
{
    private int width;
    private int height;

    [Header("Cellular Automata Settings")]
    [Tooltip("Percentage chance each cell starts filled as a wall. Higher values make the map more solid.")]
    [Range(0f, 1f)] public float fillPercent = 0.45f;
    public bool clampBorders = false;
    public bool randomSeed = false;
    [Tooltip("Ignored if random seed is true.")]
    public int seed = 0;

    [Min(0)] public int iterations = 5;
    [Tooltip("Cells become/remain wall if neighbours >= birthLimit")]
    [Range(0, 8)] public int birthLimit = 5;
    [Tooltip("Empty cells become wall if neighbors > deathLimit")]
    [Range(0, 8)] public int deathLimit = 3;

    public override void Initialize(WorldSettings settings)
    {
        width = settings.width;
        height = settings.height;

        map = new float[width, height];

        if (!randomSeed) Random.InitState(seed);
        else Random.InitState(System.Environment.TickCount);
    }

    public override float[,] Generate()
    {
        RandomFill();

        for (int i = 0; i < iterations; i++)
            StepSimulation();

        return map;
    }

    // 5 MARKS
    private void RandomFill()
    {
        // PSEUDOCODE:
        // for each x in grid width
        for (int x = 0; x < width; x++)
        {
        //   for each y in grid height
            for (int y = 0; y < height; y++)
            {
        //     if border clamping is enabled AND this cell is on an outer edge
                if (clampBorders && (x == 0 || x == width -1 || y == 0 || y == height - 1))
                {
        //         set this cell to "wall" (e.g., 1)
                    map[x,y] = 1;

                }
        //     else
                else
                {
                    //         designate as wall or empty cell randomly using the fill percentage
                    //         set the cell accordingly (e.g., 1 for wall, 0 for empty)
                    float roll = Random.value;
                    map[x, y] = (roll < fillPercent) ? 1f : 0f;
                }
            }

        }
    }
        

    // 5 MARKS
    private void StepSimulation()
    {
        // PSEUDOCODE:
        // create a new grid the same size as the current map
        float[,] newMap = new float[width,height];
        //
        // for each x in grid width
        for (int x = 0; x < width; x++)
        {
        //   for each y in grid height
            for (int y = 0; y < height; y++)
            {
        //     use CountWallNeighbours to figure out how many neighboring cells around (x,y) are walls
                int wallCount = CountWallNeighbours(x,y);
        //     if the current cell is a wall
                if (map[x, y] == 1)
                {
        //         if neighbour count meets or exceeds the "birth" threshold
        //             mark this cell as wall in the new grid
        //         else
        //             mark this cell as empty in the new grid
                    newMap[x,y] = (wallCount >= birthLimit) ? 1f : 0f;
                }
        //     else (current cell is empty)
                else
                {
        //         if neighbour count is greater than the "death" threshold
        //             mark this cell as wall in the new grid
        //         else
        //             mark this cell as empty in the new grid
                    newMap[x,y] = (wallCount > deathLimit) ? 1f : 0f;
                }
            }
        //  
        }
        // after processing all cells, replace the current map with the new grid
        map = newMap;
    }

    private int CountWallNeighbours(int cx, int cy)
    {
        int count = 0;
        for (int nx = cx - 1; nx <= cx + 1; nx++)
        {
            for (int ny = cy - 1; ny <= cy + 1; ny++)
            {
                if (nx == cx && ny == cy) continue;

                if (nx < 0 || ny < 0 || nx >= width || ny >= height
                    || map[nx, ny] == 1f)
                    count += 1;
            }
        }

        return count;
    }
}
