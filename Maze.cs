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

        _maze = new int[_rows, _cols];
        _initializeMaze(); //Crea el laberinto pero vacio(con todo paredes);
        _generateMaze(1, 1); // Comenzar desde la celda (1,1) a construir el laberinto;
        _setEntryExit(); //Crear la entrada/salida del laberinto;
        _setPlayer();
        _setTraps();
    }

    // Inicializa el laberinto con paredes (1) por defecto
    private void _initializeMaze()
    {
        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _cols; y++)
            {
                _maze[x, y] = 1; // Coloca paredes por defecto
            }
        }
    }

    // Genera el laberinto utilizando el algoritmo de backtracking
    private void _generateMaze(int x, int y)
    {
        _maze[x, y] = 0; // Marca la celda actual como un camino vacío
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
                _maze[x + DirX[address], y + DirY[address]] = 0;
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
        return x > 0 && y > 0 && x < _rows - 1 && y < _cols - 1 && _maze[x, y] == 1;
    }

    // Establece la entrada y salida del laberinto
    private void _setEntryExit()
    {
        _maze[1, 0] = 0; // Entrada (cerca de la esquina superior izquierda)
        _maze[_rows - 2, _cols - 1] = 0; // Salida (cerca de la esquina inferior derecha)
    }

    // Establece el jugador
    private void _setPlayer()
    {
        _maze[1,0] = 2; // Jugador (cerca de la esquina superior izquierda)
    }

    // Establece Trampas
    private void _setTraps()
    {
        int cont = 0;
        while(cont < 16)
        {
            //posiciones en el centro del mapa
            int x = _random.Next(5, _rows - 5); 
            int y = _random.Next(5, _cols - 5);
            //Colocar trampas
            if(_validTrap(x, y))
            {
                _maze[x, y] = 3;
                cont++;
            }
            System.Console.WriteLine(_rows + " " + _maze.GetLength(0) + " " + _cols + " " + _maze.GetLength(1));
        }
    }

    // Verifica la posicion de la trampa
    private bool _validTrap(int x, int y)
    {
        for(int i = 0; i < 3; i++) 
        {
            for(int j = 0; j < 3; j++)
            {
                //Da una Exception de rango *******************************
                //Verifica si se puede colocar trampas
                if((_maze[x + i, y + j] == 3) || (_maze[x - i, y - j] == 3) || (_maze[x + i, y - j] == 3) || (_maze[x - i, y + j] == 3) 
                || (!((_maze[(x+1), y] == 0 && _maze[(x-1), y] == 0) || (_maze[x, (y+1)] == 0 && _maze[x, (y-1)] == 0))))
                    return false;
            }    
        }
        return true;
    }

    // Muestra el laberinto en la consola
    public void PrintMaze()
    {
        Console.Clear(); //Limpia la consola
        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _cols; y++)
            {
                Console.Write(_maze[x, y] == 1 ? "██" : _maze[x, y] == 2 ? "PP" : _maze[x, y] == 3 ? "TT" : "  "); // Paredes representadas por '██' y caminos por espacios
            }
            Console.WriteLine();
        }
    }
}
