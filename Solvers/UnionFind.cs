// csharp
namespace AoC2025.Solvers;

public class UnionFind
{
    private int[] parent;
    private int[] size;

    public UnionFind(int n)
    {
        parent = new int[n];
        size = new int[n];
        for (int i = 0; i < n; i++)
        {
            parent[i] = i;
            size[i] = 1;
        }
    }

    public int Find(int x)
    {
        if (parent[x] == x) return x;
        return parent[x] = Find(parent[x]);
    }

    public bool Union(int a, int b)
    {
        int ra = Find(a);
        int rb = Find(b);
        if (ra == rb) return false;
        if (size[ra] < size[rb])
        {
            parent[ra] = rb;
            size[rb] += size[ra];
        }
        else
        {
            parent[rb] = ra;
            size[ra] += size[rb];
        }
        return true;
    }
}