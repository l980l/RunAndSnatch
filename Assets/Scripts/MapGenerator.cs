using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;

    [Range(0, 100)]
    [SerializeField] private int randomFillPercent;
    [SerializeField] private int smoothNum;

    private int[,] map;
    private const int WALL = 0;
    private const int ROAD = 1;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private RuleTile WallTile;
    [SerializeField] private RuleTile RoadTile;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) GenerateMap();
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        MapRandomFill();

        for (int i = 0; i < smoothNum; i++) //반복이 많을수록 동굴의 경계면이 매끄러워진다.
            SmoothMap();
    }

    private void MapRandomFill() //맵을 비율에 따라 벽 혹은 빈 공간으로 랜덤하게 채우는 메소드
    {
        if (useRandomSeed) seed = Time.time.ToString(); //시드

        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //시드로 부터 의사 난수 생성

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = WALL;       //가장자리는 벽으로 채움
                    OnDrawTile(x, y, WALL); //벽 타일 생성
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? WALL : ROAD; //비율에 따라 벽 혹은 빈 공간 생성
                    OnDrawTile(x, y, map[x, y]); //타일 생성
                }
            }
        }
    }

    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > 4) map[x, y] = WALL; //주변 칸 중 벽이 4칸을 초과할 경우 현재 타일을 벽으로 바꿈
                else if (neighbourWallTiles < 4) map[x, y] = ROAD; //주변 칸 중 벽이 4칸 미만일 경우 현재 타일을 빈 공간으로 바꿈
            }
        }
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        { //현재 좌표를 기준으로 주변 8칸 검사
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                { //맵 범위를 초과하지 않게 조건문으로 검사
                    if (neighbourX != gridX || neighbourY != gridY) wallCount += map[neighbourX, neighbourY]; //벽은 1이고 빈 공간은 0이므로 벽일 경우 wallCount 증가
                }
                else wallCount++; //주변 타일이 맵 범위를 벗어날 경우 wallCount 증가
            }
        }
        return wallCount;
    }

    private void OnDrawTile(int x, int y, int isRoad)
    {
        if(isRoad == 1)
        {
            Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0);
            tilemap.SetTile(pos, RoadTile);
        }

        else
        {
            Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0);
            tilemap.SetTile(pos, WallTile);
        }
    }
}
