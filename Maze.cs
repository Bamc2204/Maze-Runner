using System;

class Maze
{
    private int _rows, _cols;
    public int[,] _maze;
    private Random _random = new Random();

    // Direcciones posibles (arriba, derecha, abajo, izquierda)
    private static readonly int[] DirX = { 0, 1, 0, -1 };
    private static readonly int[] DirY = { -1, 0, 1, 0 };

    // Propiedades para acceder al laberinto
    public int[,] CopyMaze => _maze;

    // Constructor
    public Maze()
    {
        //Cantidad de filas y columnas generadas aleatoriamente
        int rows = _random.Next(20, 31); 
        int cols = _random.Next(20, 31);
        // Asegurarse de que el tamaño sea impar para facilitar la generación del laberinto
        _rows = (rows % 2 == 0) ? rows + 1 : rows;
        _cols = (cols % 2 == 0) ? cols + 1 : cols;

        _maze = new int[_cols, _rows];
        _initializeMaze(); //Crea el laberinto pero vacio(con todo paredes);
        _generateMaze(1, 1); // Comenzar desde la celda (1,1) a construir el laberinto;
        _setEntryExit(); //Crear la entrada/salida del laberinto;
        _setPlayer();
        _setTraps();
    }

    // Inicializa el laberinto con paredes (1) por defecto
    private void _initializeMaze()
    {
        for (int x = 0; x < _cols; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                _maze[x, y] = 1; // Coloca paredes por defecto
            }
        }
    }

    // Genera el laberinto utilizando el algoritmo de backtracking
    private void _generateMaze(int x, int y)
    {
        _maze[y, x] = 0; // Marca la celda actual como un camino vacío
        // Lista de direcciones aleatorias para explorar
        int[] addresses = {0, 1, 2, 3};
        addresses = _mess(addresses); // Aleatoriza las direcciones{abajo, derecha, arriba, izquierda}

        foreach (int address in addresses)
        {
            // Saltamos dos celdas en la dirección
            int nx = x + DirX[address] * 2;
            int ny = y + DirY[address] * 2;

            // Si la nueva celda es válida (dentro de los límites y no ha sido recorrida)
            if (_isValid(nx, ny))
            {
                // Rompemos el muro entre la celda actual y la nueva celda
                _maze[y + DirY[address], x + DirX[address]] = 0;
                // Llamamos recursivamente para continuar el recorrido
                _generateMaze(nx, ny);
            }
        }
    }

    //Desordenar Array
    private int[] _mess(int[] a)
    {
        int k;
        for(int i = 0; i < a.Length; i++) 
        {
            k = _random.Next(i, a.Length);
            (a[k], a[i]) = (a[i], a[k]);
        }
        return a;
    }

    // Verifica si una celda es válida para colocar un camino*******
    private bool _isValid(int x, int y)
    {
        return x > 0 && y > 0 && x < _rows - 1 && y < _cols - 1 && _maze[y, x] == 1;
    }

    // Establece la entrada y salida del laberinto
    private void _setEntryExit()
    {
        _maze[1, 0] = 0; // Entrada (cerca de la esquina superior izquierda)
        _maze[_cols - 2, _rows - 1] = 0; // Salida (cerca de la esquina inferior derecha)
    }

    // Establece el jugador
    private void _setPlayer()
    {
        _maze[1,0] = 2; // Jugador (cerca de la esquina superior izquierda)
    }

    // Establece Trampas
    private void _setTraps()
    {
        for(int i = 0; i < 20; i++) 
        {
            int x = _random.Next(5, _rows - 3);
            int y = _random.Next(5, _cols - 3);
            if(!((_maze[y + 1, x] == 1 && _maze[y - 1, x] == 0) || (_maze[y + 1, x] == 0 && _maze[y - 1, x] == 1) || 
            (_maze[y, x + 1] == 1 && _maze[y, x - 1] == 0) || (_maze[y, x + 1] == 0 && _maze[y, x - 1] == 1) || 
            ((y == _maze.GetLength(0) - 2) && (x == _maze.GetLength(1) - 1))))
                _maze [y, x] = 3;
        }
    }

    // Muestra el laberinto en la consola
    public void PrintMaze()
    {
        Console.Clear(); //Limpia la consola
        for (int x = 0; x < _cols; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                Console.Write(_maze[x, y] == 1 ? "██" : _maze[x, y] == 2 ? "PP" : _maze[x, y] == 3 ? "TT" : "  "); // Paredes representadas por '██' y caminos por espacios
            }
            Console.WriteLine();
        }
    }
}
