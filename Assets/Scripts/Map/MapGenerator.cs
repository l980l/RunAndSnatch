using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;

    [Range(0, 100)]
    [SerializeField] private int chanceToStartAlive;
    [SerializeField] private int smoothNum;

    [SerializeField] private Tilemap RoadTilemap;
    [SerializeField] private Tilemap WallTilemap;
    [SerializeField] private Tilemap BoarderTilemap;
    [SerializeField] private RuleTile WallTile;
    [SerializeField] private RuleTile RoadTile;
    [SerializeField] private ShadowCasterGenerator shadowCasterGenerator;

    private int[,] map;
    private const int WALL = 0;
    private const int ROAD = 1;
    private List<(int, int)> MaxRoomList;
    private List<bool> MaskRoomList;

    private BitArray arrWallData;
    private Vector2 cellSize;
    private Grid grid;

    public Tilemap GetTilemap() { return RoadTilemap; }

    // 나는 주로 x를 세로축, y를 가로축으로 썼지만, 타일맵 좌표와 혼동하지 않도록 x가 가로축, y가 세로축이다. 
    public bool isWallAtPos(int x, int y)
    {
        x = width / 2 + x;
        y = height / 2 + y;
        int index = x + y * width;

        return arrWallData[index];
    }

    public Vector3 CustomCellToWorld(Vector3Int cellPosition)
    {
        cellPosition.x = width / 2 + cellPosition.x;
        cellPosition.y = height / 2 + cellPosition.y;

        // 타일맵의 중심을 기준으로 셀 좌표를 월드 좌표로 변환
        float offsetX = (width * cellSize.x) / 2.0f;
        float offsetY = (height * cellSize.y) / 2.0f;

        // 셀 좌표를 월드 좌표로 변환
        Vector3 worldPosition = new Vector3(
        cellPosition.x * cellSize.x - offsetX + cellSize.x / 2.0f,
        cellPosition.y * cellSize.y - offsetY + cellSize.y / 2.0f,
        0
        );

        return worldPosition;
    }

    private void Awake()
    {
        GenerateMap();
        grid = GetComponentInParent<Grid>();   
    }

    private void Start()
    {
        GenerateShadowCasters();    // ShadowCaster2D 생성.
    } 

    private void GenerateMap()
    {
        map = new int[width, height];
        MapRandomFill();

        for (int i = 0; i < smoothNum; i++) //반복이 많을수록 동굴의 경계면이 매끄러워진다.
            SmoothMap();

        RemoveSmallSpace(); // 큰 공간만 남기기.

        DrawTile(); // 타일 그리기.
        SetBoarder(); // 경계 그리기.
    }

    private void MapRandomFill() //맵을 비율에 따라 벽 혹은 빈 공간으로 랜덤하게 채우는 메소드
    {
        if (useRandomSeed) 
            seed = Time.time.ToString(); //시드
        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //시드로 부터 의사 난수 생성

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)  
                    // 가장자리는 벽으로 채움
                    map[x, y] = WALL;

                else
                    map[x, y] = (pseudoRandom.Next(0, 100) < chanceToStartAlive) ? ROAD : WALL; // chanceToStartAlive 이하의 값이 나오면 Road로.
            }
        }
    }
    private int CountAliveNeighbours(int gridX, int gridY)
    {
        int count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int neighbour_x = gridX + i;
                int neighbour_y = gridY + j;
                // 가운데면 암것도 안함.
                if (i == 0 && j == 0)
                    continue;
                // 모서리인 경우도.
                else if (neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= width || neighbour_y >= height)
                    continue;
                // Otherwise, a normal check of the neighbour 
                else if (map[neighbour_x, neighbour_y] == 1)
                {
                    count = count + 1;
                }
            }
        }

        return count;
    }
    private void SmoothMap()
    {
        int[,] newMap = new int[width, height];
        Array.Copy(map, newMap, map.Length);
        int DeathLimit = 3;
        int BirthLimit = 4;
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int AliveNeighbours = CountAliveNeighbours(x, y);
                if(map[x, y] == ROAD)
                {
                    if (AliveNeighbours < DeathLimit)
                        newMap[x, y] = WALL; 
                }
                else
                {
                    if (AliveNeighbours > BirthLimit)
                        newMap[x, y] = ROAD; 
                }
            }
        }
        Array.Copy(newMap, map, newMap.Length);
    }

    private void DrawTile()
    {
        arrWallData = new BitArray(width * height);
        cellSize = WallTilemap.cellSize;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                OnDrawTile(x, y, map[x, y]); //타일 생성
            }
        }
    }

    private void OnDrawTile(int x, int y, int isRoad)
    {
        Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0);
        if (isRoad == 1)
        {
            RoadTilemap.SetTile(pos, RoadTile);
            WallTilemap.SetTile(pos, null);
            arrWallData[x + y * width] = false;
        }
        else
        {
            WallTilemap.SetTile(pos, WallTile);
            RoadTilemap.SetTile(pos, null);
            arrWallData[x + y * width] = true;
        }
    }

    private void SetExit()
    {
        // 탈출구 위치 정하기. 맵 중앙에서 상 하 좌 우 4 방향 중 하나에 탈출구를 두자.
        int Dir = UnityEngine.Random.Range(0, 4);
        Vector3Int tempClearTilePos = new Vector3Int(-width / 2, 0, 0);
        Vector3Int MoveUnit = new Vector3Int(1, 0, 0);

        // Width나 Height가 짝수인 경우, 중심이 0이 아니라 -1과 0 사이의 경계가 됨. 그래서, Width가 50이라고 치면, 왼쪽 끝 좌표는 -25지만 오른쪽 끝 좌표는 25가 아니라 0부터 시작해서  24임. 
        // 하지만 홀수인 경우는 양쪽 끝 좌표의 절대값이 같음. 그래서 + width % 2를 해줌.
        switch (Dir)
        {
            case 0:
                tempClearTilePos.Set(-width / 2, 0, 0);
                MoveUnit.Set(1, 0, 0);
                break;
            case 1:
                tempClearTilePos.Set(width / 2 - 1 + width % 2, 0, 0);
                MoveUnit.Set(-1, 0, 0);
                break;
            case 2:
                tempClearTilePos.Set(0, -height / 2, 0);
                MoveUnit.Set(0, 1, 0);
                break;
            case 3:
                tempClearTilePos.Set(0, height / 2 - 1 + height % 2, 0);
                MoveUnit.Set(0, -1, 0);
                break;
        }

        // 벽 지우기.
        while (WallTilemap.GetTile(tempClearTilePos))
        {
            WallTilemap.SetTile(tempClearTilePos, null);
            RoadTilemap.SetTile(tempClearTilePos, RoadTile);

            // 이동.
            tempClearTilePos += MoveUnit;
        }
    }

    private void GenerateShadowCasters()
    {
        SetExit();
        WallTilemap.GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
        shadowCasterGenerator.Create();
    }

    private int SearchMaxRoom()     // MaxRoom의 크기와 좌표들을 구해서 List에 넣어주는 함수.
    {
        int maxRoom = 0;                // 가장 큰 공간의 크기.
        int indexX = 0, indexY = 0;     // 가장 큰 공간의 끝 주소.
        MaxRoomList = new List<(int, int)>();  // 가장 큰 공간의 좌표들

        // BFS 재료
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };
        bool[,] visited = new bool[width, height];
        Queue<(int, int)> queue = new Queue<(int, int)>();

        // 테두리는 어차피 벽이니까 넘기고.
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < height - 1; j++)
            {
                // 방문한적 없는 공간인 경우.
                if (map[i, j] == 1 && !visited[i, j])
                {
                    int nowRoom = 1;    // 이번 방 크기
                    visited[i, j] = true;
                    List<(int, int)> NowRoomList = new List<(int, int)>();  // 이번 방 좌표들
                    NowRoomList.Add((i, j));
                    queue.Enqueue((i, j));
                    while (queue.Count > 0)
                    {
                        var (x, y) = queue.Dequeue();
                        for (int k = 0; k < 4; ++k)
                        {
                            int nx = x + dx[k];
                            int ny = y + dy[k];
                            if (nx < 0 || nx >= width || ny < 0 || ny >= height || visited[nx, ny] || map[nx, ny] == WALL)
                                continue;
                            visited[nx, ny] = true;
                            nowRoom++;
                            NowRoomList.Add((nx, ny));
                            queue.Enqueue((nx, ny));
                        }
                    }
                    // maxRoom 업데이트
                    if (nowRoom > maxRoom)
                    {
                        maxRoom = nowRoom;
                        indexX = i;
                        indexY = j;

                        // MaxRoomList로 깊은 복사.
                        MaxRoomList.Clear();
                        foreach ((int x, int y) in NowRoomList)
                        {
                            MaxRoomList.Add((x, y));
                        }
                    }
                }
            }
        }
        // 최대 방 크기 반환.
        return maxRoom;
    }

    private void RemoveSmallSpace()
    {
        int maxRoom = SearchMaxRoom();

        // 가장 큰 곳만 다시 비우기. 
        map = new int[width, height];
        foreach ((int x, int y) in MaxRoomList)
        {
            map[x, y] = ROAD;
        }
        
        MaskRoomList = new List<bool>(new bool[maxRoom]);
    }

    // Road 위의 랜덤한 위치를 그리드 좌표계로 반환하는 함수.
    // false을 인자로 넣어주면 모든 Road 위에서 랜덤. true을 인자로 넣어주면 벽 근처의 Road 위에서 랜덤.
    private (int x, int y) RandomRoad(bool CheckWall)    
    {
        int ListSize = MaxRoomList.Count;
        int index = 0;

        // 이미 사용한 위치는 사용 안하기 위해 MaskRoomList 이용.
        while (true)
        {
            index = UnityEngine.Random.Range(0, ListSize);
            // 마스킹 안 된 곳인 경우, 이웃한 벽 체크.
            if (!MaskRoomList[index])
            {
                if(CheckWall)
                {
                    int NearWall = 8 - CountAliveNeighbours(MaxRoomList[index].Item1, MaxRoomList[index].Item2);
                    // 벽이 2개 이하면 다른 곳 찾아.
                    if (NearWall < 3)
                        continue;
                }

                MaskRoomList[index] = true;
                break;
            }
        }
        return MaxRoomList[index];
    }

    // Road 위의 랜덤한 위치를 월드 좌표계로 변환하여 반환하는 함수.
    // false을 인자로 넣어주면 모든 Road 위에서 랜덤. true을 인자로 넣어주면 벽 근처의 Road 위에서 랜덤.
    public Vector3 RandomPos(bool CheckWall)
    {
        var (x, y) = RandomRoad(CheckWall);
        Vector3Int temp = new Vector3Int(-width / 2 + x, -height / 2 + y, 0);
        Vector3 result = grid.CellToWorld(temp);
        result += RoadTilemap.GetLayoutCellCenter();

        return result;
    }

    private void SetBoarder()
    {
        // 위 아래 경계 설정
        for (int x = 0; x < width; x++)
        {
            Vector3Int topPos = new Vector3Int(-width / 2 + x, height / 2 + height % 2, 0);
            Vector3Int bottomPos = new Vector3Int(-width / 2 + x, -height / 2 - 1, 0);
            BoarderTilemap.SetTile(topPos, RoadTile);
            BoarderTilemap.SetTile(bottomPos, RoadTile);
        }

        // 좌우 경계 설정
        for (int y = 0; y < height; y++)
        {
            Vector3Int leftPos = new Vector3Int(-width / 2 - 1, -height / 2 + y, 0);
            Vector3Int rightPos = new Vector3Int(width / 2 + width % 2, -height / 2 + y, 0);
            BoarderTilemap.SetTile(leftPos, RoadTile);
            BoarderTilemap.SetTile(rightPos, RoadTile);
        }
    }
}
