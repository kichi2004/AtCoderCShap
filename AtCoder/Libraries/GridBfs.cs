using System;
using System.Collections.Generic;
using System.Linq;

namespace Solve.Libraries.GridBfs
{
    public static class GridBfs
    {
        public static int Run(char[,] grid, char start = 'S', char goal = 'G', char wall = '#') {
            int sy = -1, sx = -1, gy = -1, gx = -1;
            int H = grid.GetLength(0), W = grid.GetLength(1);
            var newGrid = new bool[H, W];
            for (int i = 0; i < H; i++) {
                for (int j = 0; j < W; j++) {
                    if (grid[i, j] == start) {
                        if (sy != -1)
                            throw new ArgumentException("There are multiple start points in the grid.", nameof(grid));
                        sy = i;
                        sx = j;
                        newGrid[i, j] = true;
                    } else if (grid[i, j] == goal) {
                        if (gy != -1)
                            throw new ArgumentException("There are multiple goal points in the grid.", nameof(grid));
                        gy = i;
                        gx = j;
                        newGrid[i, j] = true;
                    } else {
                        newGrid[i, j] = grid[i, j] != wall;
                    }
                }
            }

            if (sy == -1) throw new ArgumentException("There is no start point in the grid.", nameof(grid));
            if (gy == -1) throw new ArgumentException("There is no goal point in the grid.", nameof(grid));
            return Run(newGrid, sy, sx, gy, gx);
        }
        public static int Run(char[,] grid, int sy, int sx, int gy, int gx, char wall = '#') =>
            Run(grid, sy, sx, gy, gx, new[] {wall});
        public static int Run(char[,] grid, int sy, int sx, int gy, int gx, IEnumerable<char> walls) {
            if (walls == null) throw new ArgumentNullException(nameof(walls));
            int H = grid.GetLength(0);
            int W = grid.GetLength(1);
            if (sy < 0 || sy >= H)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter sy out of range.");
            if (gy < 0 || gy >= H)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter gy out of range.");
            if (sx < 0 || sx >= W)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter sx out of range.");
            if (gx < 0 || gx >= W)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter gx out of range.");
            return Run(grid, sy, sx, walls)[gy, gx];
        }
        public static int[,] Run(char[,] grid, int sy, int sx, IEnumerable<char> walls) {
            if (grid == null) throw new ArgumentNullException(nameof(grid));
            if (walls == null) throw new ArgumentNullException(nameof(walls));
            int H = grid.GetLength(0);
            int W = grid.GetLength(1);
            if (sy < 0 || sy >= H)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter sy out of range.");
            if (sx < 0 || sx >= W)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter sx out of range.");

            var wallSet = new HashSet<char>(walls);
            if (!wallSet.Any()) throw new ArgumentException("Parameter walls is empty.", nameof(walls));

            var boolGrid = new bool[H, W];
            for (int i = 0; i < H; i++) for (int j = 0; j < W; j++) boolGrid[i, j] = !wallSet.Contains(grid[i, j]);

            return Run(boolGrid, sy, sx);
        }
        public static int Run(string[] grid, char start = 'S', char goal = 'G', char wall = '#') =>
            Run(grid.To2DArray(), start, goal, wall);
        public static int Run(string[] grid, int sy, int sx, int gy, int gx, char wall = '#') =>
            Run(grid.To2DArray(), sy, sx, gy, gx, new[] {wall});
        public static int Run(string[] grid, int sy, int sx, int gy, int gx, IEnumerable<char> walls) =>
            Run(grid.To2DArray(), sy, sx, gy, gx, walls);
        public static int[,] Run(string[] grid, int sy, int sx, IEnumerable<char> walls) =>
            Run(grid.To2DArray(), sy, sx, walls);
        public static int Run(bool[,] grid, int sy, int sx, int gy, int gx) {
            int H = grid.GetLength(0);
            int W = grid.GetLength(1);
            if (sy < 0 || sy >= H)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter sy out of range.");
            if (gy < 0 || gy >= H)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter gy out of range.");
            if (sx < 0 || sx >= W)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter sx out of range.");
            if (gx < 0 || gx >= W)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter gx out of range.");
            return Run(grid, sy, sx)[gy, gx];
        }
        public static int[,] Run(bool[,] grid, int sy, int sx) {
            if (grid == null) throw new ArgumentNullException(nameof(grid));
            int H = grid.GetLength(0);
            int W = grid.GetLength(1);
            if (sy < 0 || sy >= H)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter sy out of range.");
            if (sx < 0 || sx >= W)
                throw new ArgumentOutOfRangeException(nameof(sy), "Parameter sx out of range.");

            var queue = new Queue<(int y, int x)>();
            queue.Enqueue((sy, sx));
            var ret = Lib.Array2D(H, W, -1);
            ret[sy, sx] = 0;
            while (queue.Any()) {
                var (y, x) = queue.Dequeue();
                for (int __d = 0; __d < 4; __d++) {
                    int ny = y + Solver.dy[__d], nx = x + Solver.dx[__d];
                    if ((uint) ny >= H || (uint) nx >= W || ret[ny, nx] != -1 || !grid[ny, nx]) continue;

                    ret[ny, nx] = ret[y, x] + 1;
                    queue.Enqueue((ny, nx));
                }
            }

            return ret;
        }
        static char[,] To2DArray(this IEnumerable<string> array) =>
            array.Select(x => x.ToCharArray()).ToArray().To2DArray();
    }
}