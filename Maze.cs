using System;

class Maze
{
    #region Propiedades del Laberinto           ////////////////////////////////////////////////////////////////////////////////////////

    private Random _random = new Random();  //Generador de numeros aleatorios
    private static int _rows = 51, _cols = 51;       //Filas y columnas
    public static int[,] _maze = new int [_rows, _cols];            //Laberinto
    private Players _player1, _player2; //Jugadores

    // Direcciones posibles (arriba, derecha, abajo, izquierda)
    private static readonly int[] DirX = { 0, 1, 0, -1 };
    private static readonly int[] DirY = { -1, 0, 1, 0 };

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Constructor         //////////////////////////////////////////////////////////////////////////////////////////
    
    // Constructor de la clase Maze
    public Maze(Players player1, Players player2)
    {
        // Jugadores
        _player1 = player1;
        _player2 = player2;

        //Cantidad de filas y columnas generadas aleatoriamente
        //int rows = _random.Next(45, 56); 
        //int cols = _random.Next(45, 56);

        // Asegurarse de que el tamaño sea impar para facilitar la generación del laberinto
        //_rows = (rows % 2 == 0) ? rows + 1 : rows;
        //_cols = (cols % 2 == 0) ? cols + 1 : cols;
        //_maze = new int[_rows, _cols];

        _initializeMaze();                  //Crea el laberinto pero vacio(con todo paredes);
        _generateMaze(1, 1);                // Comenzar desde la celda (1,1) a construir el laberinto;
        _setRoad();                         //Genera caminos alternativos
        _setEntryExit();                    //Crear la entrada/salida del laberinto;
        _setPlayer(player1);                //Genera los 1ros jugadores
        _setPlayer(player2);                //Genera los 2dos jugadores
        _setTraps(-2);                      //Genera las trampas
        _setTraps(-3);                      //Genera las trampas
        _setTraps(-4);                      //Genera las trampas
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de Generacion del Laberinto         ////////////////////////////////////////////////////////////////////////////////////////
   
    // Inicializa el laberinto con paredes (1) por defecto
    private void _initializeMaze()
    {
        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _cols; y++)
            {
                _maze[x, y] = -1; // Coloca paredes por defecto
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

    // Establece caminos alternativos
    private void _setRoad()
    {
        int cont = 0;
        while(cont < 81)
        {
            //posiciones en el centro del mapa
            int x = _random.Next(5, _rows - 5); 
            int y = _random.Next(5, _cols - 5);

            //Colocar trampas
            if(_maze[x, y] == -1 && _validRoad(x, y))
            {
                _maze[x, y] = -5;
                cont++;
            }
        }
    }
    
    //  Verifica q se puedan establecer esos caminos
    private bool _validRoad(int x, int y)
    {
        for(int i = 0; i < 2; i++) 
        {
            for(int j = 0; j < 2; j++)
            {
                // Verifica si se puede colocar trampas
                bool trapNearby = 
                    _maze[x + i, y + j] == -5 ||
                    _maze[x - i, y - j] == -5 ||
                    _maze[x + i, y - j] == -5 ||
                    _maze[x - i, y + j] == -5;

                bool surroundedByWalls =
                    (_maze[x + 1, y] == -1 && _maze[x - 1, y] == -1) &&
                    (_maze[x, y + 1] == -1 && _maze[x, y - 1] == -1);

                bool invalidPlacement =
                    !((_maze[x + 1, y] == -1 && _maze[x - 1, y] == -1) || 
                    (_maze[x, y + 1] == -1 && _maze[x, y - 1] == -1));

                if (trapNearby || invalidPlacement || surroundedByWalls)
                {
                    return false;
                }                             
            }    
        }
        return true;
    }

    //  Metodo de verificacion para establecer esos caminos en el laberinto nuevo
    private bool _validRoadForNewMaze(int x, int y)
    {
        bool firstLayer = _validRoad(x, y);

        // Verifica si la posición actual (x, y) no contiene una ficha
        bool positionOccupiedByToken = _maze[x, y] >= 1 && _maze[x, y] <= 8;

        if( firstLayer || positionOccupiedByToken)
            return false;

        return true;
    }

    // Verifica si una celda es válida para colocar un camino
    private bool _isValid(int x, int y)
    {
        return x > 0 && y > 0 && x < _rows - 1 && y < _cols - 1 && _maze[x, y] == -1;
    }
    
    // Genera el laberinto cada cierto tiempo de forma dinamica ******************************************************
    public void GenerateNewMaze()
    {
        
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos para colocar entradas-salidas del laberinto        //////////////////////////////////////////////////////////////////////////////////////////
   
    // Establece la entrada y salida del laberinto**********************************************************
    private void _setEntryExit()
    {
        // Entrada (cerca de la esquina superior izquierda)
        _maze[1, 0] = 0;
        _maze[7, 0] = 0;
        _maze[13, 0] = 0;
        _maze[19, 0] = 0; // Salida (cerca de la esquina inferior derecha)
    }

    //Informacion de si esta en la salida del Laberinto
    public bool IsExit(int x, int y, bool getTarget)
    {
        if(((x ==  1) || (x == 7) || (x == 13) || (x == 19)) && y == 0 && getTarget)
            return true;
        return false;
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodo para estaclecer jugador          //////////////////////////////////////////////////////////////////////////////////////////
    
    // Establece el jugado
    private void _setPlayer(Players player)
    {
        for(int i = 0; i < player._tokens.Length; i++)
        {
            // Jugador (cerca de la esquina superior izquierda)
            _maze[player._tokens[i]._coordX, player._tokens[i]._coordY] = player._tokens[i].InfoId(); 
            
        }
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos para generar las trampas            //////////////////////////////////////////////////////////////////////////////////////////
    
    //Tipos de trampas
    enum KindTrap
    {
        T1 = -2,
        T2 = -3,
        T3 = -4
    }

    //Establece las trampas
    private void _setTraps( int kindTrap)
    {
        int cont = 0;
        while(cont < 6)
        {
            //posiciones en el centro del mapa
            int x = _random.Next(5, _rows - 5); 
            int y = _random.Next(5, _cols - 5);
            //Colocar trampas
            if(_validTrap(x, y))
            {
                _maze[x, y] = kindTrap;
                cont++;
            }
        }
    }

    // Verifica la posicion de la trampa
    private bool _validTrap(int x, int y)
    {
        for(int i = 0; i < 3; i++) 
        {
            for(int j = 0; j < 3; j++)
            {
                //Verifica si se puede colocar trampas
                if(((_maze[x + i, y + j] == -2) || (_maze[x - i, y - j] == -2) || (_maze[x + i, y - j] == -2) || (_maze[x - i, y + j] == -2))
                || ((_maze[x + i, y + j] == -3) || (_maze[x - i, y - j] == -3) || (_maze[x + i, y - j] == -3) || (_maze[x - i, y + j] == -3))
                || ((_maze[x + i, y + j] == -4) || (_maze[x - i, y - j] == -4) || (_maze[x + i, y - j] == -4) || (_maze[x - i, y + j] == -4))
                || (!((_maze[(x+1), y] == 0 && _maze[(x-1), y] == 0) || (_maze[x, (y+1)] == 0 && _maze[x, (y-1)] == 0))))
                    return false;
            }    
        }
        return true;
    }
    
    // Verifica la posicion de la trampa en el nevo laberinto
    private bool _validTrapForNewMaze(int x, int y)
    {
        bool firstLayer = _validTrap(x, y);
        
        // Verifica si la posición actual (x, y) no contiene una ficha
        bool positionOccupiedByToken = _maze[x, y] >= 1 && _maze[x, y] <= 8;

        if( firstLayer || positionOccupiedByToken)
            return false;

        return true;
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodo verificador de pared             //////////////////////////////////////////////////////////////////////////////////////////

    // Metodo para verificar si hay pared, retorna false si hay alguna pared en las direccion introducida y true si no hay pared
    public static bool CheckWall(int direction, int totalCheckBox, Tokens token)
    {
        switch(direction)
        {
            case 1: // Arriba
            
            for (int i = 0; i < totalCheckBox; i++)
            {
                if(Maze._maze[token._coordX + i, token._coordY] == -1)
                    return false;
            }

            break;
        
            case 2: // Abajo
            
            for (int i = 0; i < totalCheckBox; i++)
            {
                if(Maze._maze[token._coordX - i, token._coordY] == -1)
                    return false;
            }

            break;

            case 3: // Izquierda
            
            for (int i = 0; i < totalCheckBox; i++)
            {
                if(Maze._maze[token._coordX, token._coordY - i] == -1)
                    return false;
            }

            break;

            case 4: // Derecha
            
            for (int i = 0; i < totalCheckBox; i++)
            {
                if(Maze._maze[token._coordX, token._coordY + i] == -1)
                    return false;
            }

            break;
        }

        return true;
    }

    #endregion              //////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos para imprimir el laberinto en consola           //////////////////////////////////////////////////////////////////////////////////////////

    // Metodo de imprimir el mapa
    public static void PrintMaze(Players player1, Players player2)
    {
        Console.Clear(); // Limpia la consola

        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _cols; y++)
            {
                // Comprueba si hay alguna ficha del Player1 en esta posición
                bool printedToken = false;
                for (int i = 0; i < player1._tokens.Length; i++)
                {
                    if (_maze[x, y] == player1._tokens[i].InfoId())
                    {
                        Console.Write(player1._tokens[i].InfoCharacter());
                        printedToken = true;
                        break;
                    }
                }

                // Comprobamos si hay alguna ficha de Player2 en esta posición
                if (!printedToken)
                {
                    for (int i = 0; i < player2._tokens.Length; i++)
                    {
                        if (_maze[x, y] == player2._tokens[i].InfoId())
                        {
                            Console.Write(player2._tokens[i].InfoCharacter());
                            printedToken = true;
                            break;
                        }
                    }
                }

                // Si no hay fichas en la posicion x,y del laberinto entonces imprimimos el contenido normal del laberinto
                    Console.Write(
                        _maze[x, y] == -1 ? "██" : // Pared
                        _maze[x, y] == -2 ? "T1" : // Trampa tipo 1
                        _maze[x, y] == -3 ? "T2" : // Trampa tipo 2
                        _maze[x, y] == -4 ? "T3" : // Trampa tipo 3
                        "  "                        // Camino vacío
                    );
            }
            Console.WriteLine();
        }
    }
  
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Metodo de victoria          //////////////////////////////////////////////////////////////////////////////////////////
    
    // Condicion de Victoria ******************
    public bool Win(int x, int y, bool getTarget)
    {
        if(IsExit(x, y, getTarget))
            return true;
        return false;
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Metodos de informacion          //////////////////////////////////////////////////////////////////////////////////////////
    
    // Informacion del  laberinto
    public static int[,] InfoMaze()
    {
        return _maze;
    } 
    
    // Informacion de las filas
    public int InfoRows()
    {
        return _rows;
    }

    // Informacion de las columnas
    public int InfoCols()
    {
        return _cols;
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

}

    /*
    // Muestra el laberinto en la consola
    public void PrintMaze(Tokens[] token1, Tokens[] token2)
    {
        Console.Clear(); //Limpia la consola
        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _cols; y++)
            {
                // Paredes representadas por '██', caminos por espacios, trapas por 'TT' y jugadores por sus respectivos caracteres
                Console.Write(_maze[x, y] == -1 ? "██" : _maze[x, y] == token1[0].InfoId() ? token1[0].InfoCharacter() 
                : _maze[x, y] == token1[1].InfoId() ? token1[1].InfoCharacter() : _maze[x, y] == token1[2].InfoId() ? token1[2].InfoCharacter() 
                : _maze[x, y] == token1[3].InfoId() ? token1[3].InfoCharacter() : _maze[x, y] == -2 ? "TT" : "  "); 
            }
            Console.WriteLine();
        }
    }
    */
