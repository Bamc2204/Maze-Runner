using System;

class Maze
{
    private int _rows, _cols;
    private int[,] _maze;
    private Random _random = new Random();

    // Direcciones posibles (arriba, derecha, abajo, izquierda)
    private static readonly int[] DirX = { 0, 1, 0, -1 };
    private static readonly int[] DirY = { -1, 0, 1, 0 };

    // Propiedades para acceder al laberinto
    public int[,] Laberinto => _maze;

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
        _inicializarLaberinto(); //Crea el laberinto pero vacio(con todo paredes);
        _generarLaberinto(1, 1); // Comenzar desde la celda (1,1) a construir el laberinto;
        _establecerEntradaSalida(); //Crear la entrada/salida del laberinto;
        _establecerJugador();
    }

    // Inicializa el laberinto con paredes (1) por defecto
    private void _inicializarLaberinto()
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
    private void _generarLaberinto(int x, int y)
    {
        _maze[y, x] = 0; // Marca la celda actual como un camino vacío
        // Lista de direcciones aleatorias para explorar
        int[] direcciones = {0, 1, 2, 3};
        direcciones = Desordenar(direcciones); // Aleatoriza las direcciones{abajo, derecha, arriba, izquierda}

        foreach (int direccion in direcciones)
        {
            // Saltamos dos celdas en la dirección
            int nx = x + DirX[direccion] * 2;
            int ny = y + DirY[direccion] * 2;

            // Si la nueva celda es válida (dentro de los límites y no ha sido recorrida)
            if (_esValido(nx, ny))
            {
                // Rompemos el muro entre la celda actual y la nueva celda
                _maze[y + DirY[direccion], x + DirX[direccion]] = 0;
                // Llamamos recursivamente para continuar el recorrido
                _generarLaberinto(nx, ny);
            }
        }
    }

    //Desordenar Array
    private int[] Desordenar(int[] a)
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
    private bool _esValido(int x, int y)
    {
        return x > 0 && y > 0 && x < _rows - 1 && y < _cols - 1 && _maze[y, x] == 1;
    }

    // Establece la entrada y salida del laberinto
    private void _establecerEntradaSalida()
    {
        _maze[1, 0] = 0; // Entrada (cerca de la esquina superior izquierda)
        _maze[_cols - 2, _rows - 1] = 0; // Salida (cerca de la esquina inferior derecha)
    }

    // Establece el jugador
    private void _establecerJugador()
    {
        _maze[1,0] = 2; // Jugador (cerca de la esquina superior izquierda)
    }

    // Muestra el laberinto en la consola
    public void ImprimirLaberinto()
    {
        Console.Clear(); //Limpia la consola
        for (int x = 0; x < _cols; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                Console.Write(_maze[x, y] == 1 ? "██" : _maze[x, y] == 2 ? "PP" : "  "); // Paredes representadas por '██' y caminos por espacios
            }
            Console.WriteLine();
        }
    }

    //Desplaza la ficha
    public void Desplazamiento(int pasos)
    {   
        _maze[1,0] = 2;
        int x = 1;
        int y = 0;
        int newX = x;
        int newY = y;
        bool running = true;
        
        while(pasos != 0 && running)
        {
            //Si llegas al final del juego
            if(x == _cols - 2 && y == _rows - 1)//Si llego al final del laberinto o no
            {
                System.Console.WriteLine("Felicidades, Completaste El Laberinto");
                running = false;
            }
            
            else
            {
                //Tecla q toca el jugador en el teclado            
                ConsoleKey key = Console.ReadKey().Key;

                //Casos para cada tecla
                switch (key)
                {
                    case ConsoleKey.UpArrow:    newX = x - 1; break;
                    case ConsoleKey.DownArrow:  newX = x + 1; break;
                    case ConsoleKey.LeftArrow:  newY = y - 1; break;
                    case ConsoleKey.RightArrow: newY = y + 1; break;
                    case ConsoleKey.Escape: Console.WriteLine("Simulación detenida."); running = false; break;
                }

                // Dentro de filas, columnas y si es un camino
                if (newX >= 0 && newX < _maze.GetLength(0) && newY >= 0 && newY < _maze.GetLength(1) && _maze[newX, newY] == 0)                    
                {
                    // Actualiza el tablero
                    _maze[x, y] = 0;        // Vacía la posición actual
                    _maze[newX, newY] = 2;  // Mueve la ficha
                    x = newX;                 // Actualiza las coordenadas actuales
                    y = newY;
                    pasos --;
                }

                else
                {
                    System.Console.WriteLine("los pasos no son validos");
                    newX = x; newY = y;                    
                }

                ImprimirLaberinto();//Imprime el laberinto
            }
        }   
    }
}
