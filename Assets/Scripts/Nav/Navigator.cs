using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PriorityQueue<T>
{
    private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();
    public int Count => elements.Count;
    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Value < elements[bestIndex].Value)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }

    public bool Contains(T item)
    {
        foreach (var element in elements)
        {
            if (EqualityComparer<T>.Default.Equals(element.Key, item))
            {
                return true;
            }
        }
        return false;
    }

    public void UpdatePriority(T item, float priority)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(elements[i].Key, item))
            {
                elements[i] = new KeyValuePair<T, float>(item, priority);
                return;
            }
        }
    }

    public void Clear()
    {
        elements.Clear();
    }
}

public class Navigator : MonoBehaviour
{
    private Vector3Int nowTilePos;
    private Vector3Int desTilePos;
    private Grid grid;
    [HideInInspector] public List<Vector3> totalWorldPath;
    public int curPathIndex;

    // A* 알고리즘을 위한 우선순위 큐
    PriorityQueue<Vector3Int> openSet = new PriorityQueue<Vector3Int>();
    HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
    Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
    Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
    Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();

    private void SetNowTilePos()
    {
        nowTilePos = grid.WorldToCell(transform.position);
    }

    public void SetDesTilePos(Vector3 _WorldVec3)
    {
        _WorldVec3 += grid.GetLayoutCellCenter();
        desTilePos = grid.WorldToCell(_WorldVec3);
    }

    public void SetDesTilePos(Vector3Int _TilePosVec3)
    {
        desTilePos = _TilePosVec3;
    }

    private void Awake()
    {
        totalWorldPath = new List<Vector3>();
        grid = GameManager.Instance.GetMapGenerator().GetGrid();
    }

    public async void FindPath()
    {
        SetNowTilePos();

        bool pathFound = await Task.Run(() => AStarAlgorithm());

        if (pathFound)
        {
            Logging.Log("경로를 찾았습니다!");
        }
        else
        {
            Logging.Log("경로를 찾을 수 없습니다.");
        }
    }
    private bool AStarAlgorithm()
    {
        openSet.Clear();
        closedSet.Clear();
        cameFrom.Clear();
        gScore.Clear();
        fScore.Clear();

        openSet.Enqueue(nowTilePos, 0);
        gScore[nowTilePos] = 0;
        fScore[nowTilePos] = HeuristicCostEstimate(nowTilePos, desTilePos);

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet.Dequeue();

            if (current == desTilePos)
            {
                ReconstructPath(cameFrom, current);
                return true;
            }

            closedSet.Add(current);

            foreach (Vector3Int direction in GetDirections())
            {
                Vector3Int neighbor = current + direction;

                if (closedSet.Contains(neighbor) || !CanMoveToTile(neighbor))
                {
                    continue;
                }

                // 대각선 이동 시 인접한 타일 체크
                if (Mathf.Abs(direction.x) == 1 && Mathf.Abs(direction.y) == 1 && !CanMoveDiagonally(current, direction))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + Vector3Int.Distance(current, neighbor);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Enqueue(neighbor, float.PositiveInfinity);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, desTilePos);
                openSet.UpdatePriority(neighbor, fScore[neighbor]);
            }
        }

        return false;
    }
    private bool CanMoveDiagonally(Vector3Int current, Vector3Int direction)
    {
        Vector3Int check1 = new Vector3Int(current.x + direction.x, current.y, 0);
        Vector3Int check2 = new Vector3Int(current.x, current.y + direction.y, 0);

        return CanMoveToTile(check1) && CanMoveToTile(check2);
    }

    private bool CanMoveToTile(Vector3Int position)
    {
        return !GameManager.Instance.GetMapGenerator().isWallAtPos(position.x, position.y);
    }

    private static readonly List<Vector3Int> directions = new List<Vector3Int>
    {
        new Vector3Int(1, 0, 0),    // 오른쪽
        new Vector3Int(-1, 0, 0),   // 왼쪽
        new Vector3Int(0, 1, 0),    // 위
        new Vector3Int(0, -1, 0),   // 아래
        new Vector3Int(1, 1, 0),    // 오른쪽 위 대각선
        new Vector3Int(-1, 1, 0),   // 왼쪽 위 대각선
        new Vector3Int(1, -1, 0),   // 오른쪽 아래 대각선
        new Vector3Int(-1, -1, 0)   // 왼쪽 아래 대각선
    };

    private List<Vector3Int> GetDirections()
    {
        return directions;
    }

    private float HeuristicCostEstimate(Vector3Int a, Vector3Int b)
    {
        // 유클리드 거리 사용
        return Vector3Int.Distance(a, b);
    }

    private void ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> Path = new List<Vector3Int> { current };
        List<Vector3> WorldPath = new List<Vector3>();
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];

            Vector3 temp = GameManager.Instance.GetMapGenerator().CustomCellToWorld(current);

            WorldPath.Add(temp);
        }
        WorldPath.Reverse();
        totalWorldPath = WorldPath;
        curPathIndex = 2;
    }
    public void DrawPath()
    {
        for (int i = 1; i < totalWorldPath.Count; i++)
        {
            Debug.DrawLine(totalWorldPath[i - 1], totalWorldPath[i], Color.red, 1f);
        }
    }
}
