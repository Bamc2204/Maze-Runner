using System;

class Maze
{
    #region Propiedades del Laberinto           ////////////////////////////////////////////////////////////////////////////////////////

    private Random _random = new Random();                          // Generador de numeros aleatorios
    private static int _rows = 51, _cols = 51;                      // Filas y columnas
    public static int[,] _maze = new int [_rows, _cols];            // Laberinto
    private Players _player1, _player2;                             // Jugadores
    private int _cupX, _cupY;                                       // Coordenadas de la copa
    public static bool _generatedDoors = false;

    // Direcciones posibles (arriba, derecha, abajo, izquierda)
    private static readonly int[] DirX = { 0, 1, 0, -1 };
    private static readonly int[] DirY = { -1, 0, 1, 0 };

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Constructor         //////////////////////////////////////////////////////////////////////////////////////////
    
    // Constructor de la clase Maze ***** eliminar comentarios
    public Maze(Players player1, Players player2)
    {
        // Jugadores
        _player1 = player1;
        _player2 = player2;

        _initializeMaze();                  // Crea el laberinto pero vacio(con todo paredes);
        _generateMaze(1, 1);                // Comenzar desde la celda (1,1) a construir el laberinto;
        _setRoad();                         // Genera caminos alternativos
        _setPlayer(player1);                // Genera los 1ros jugadores
        _setPlayer(player2);                // Genera los 2dos jugadores
        _setTraps(-2);                      // Genera las trampas
        _setTraps(-3);                      // Genera las trampas
        _setTraps(-4);                      // Genera las trampas
        _setCup();                          // Genera la COPA
        _setExit();                         // Genera las Salidas
        _setObject();                       // Genera los Objetos para ayudar al Jugador
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de Generacion del Laberinto         ////////////////////////////////////////////////////////////////////////////////////////
   
    // Inicializa el laberinto con paredes (-1) por defecto
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

    // Sobrecarga del Metodo Inicializar el laberinto para la generacion dinamica del mismo
    // Inicializa el laberinto con paredes (-1) por defecto, excepto donde hay piezas
    private void _initializeMaze(bool[,] occupiedPositions)
    {
        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _cols; y++)
            {
                // Si la posición está ocupada por una pieza, no la modificamos
                _maze[x, y] = occupiedPositions[x, y] ? _maze[x, y] : -1;
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
 
    // Sobrecarga del metodo de generacion del laberinto pero de forma dinamica
    // Genera el laberinto utilizando el algoritmo de backtracking
    private void _generateMaze(int x, int y, bool[,] occupiedPositions)
    {
        // Si la celda actual está ocupada por una pieza, no la modificamos
        if (occupiedPositions[x, y]) return;

        _maze[x, y] = 0; // Marca la celda actual como un camino vacío

        // Lista de direcciones aleatorias para explorar
        int[] addresses = { 0, 1, 2, 3 };
        addresses = _mess(addresses); // Aleatoriza las direcciones

        foreach (int address in addresses)
        {
            // Saltamos dos celdas en la dirección
            int nx = x + DirX[address] * 2;
            int ny = y + DirY[address] * 2;

            // Si la nueva celda es válida (dentro de los límites y no ha sido recorrida)
            if (_isValid(nx, ny) && !occupiedPositions[nx, ny])
            {
                // Rompemos el muro entre la celda actual y la nueva celda
                _maze[x + DirX[address], y + DirY[address]] = 0;

                // Llamamos recursivamente para continuar el recorrido
                _generateMaze(nx, ny, occupiedPositions);
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

    //Crea una Mascara Booleana para guardar las ocurrencias del laberinto
    private bool[,] BooleanMask()
    {
        bool[,] booleanMask = new bool[_rows, _cols];
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                if(_maze[i, j] != 0 && _maze[i, j] != -1 && _maze[i, j] != -6)
                    booleanMask[i, j] = true;
                else
                    booleanMask[i, j] = false;
            }
        }
        return booleanMask;
    }

    // Genera el laberinto cada cierto tiempo de forma dinamica ******************************************************
    public void GenerateNewMaze(bool cup)
    {
        _initializeMaze(BooleanMask());     // Inicializa el laberinto nuevamente
        _generateMaze(1, 1, BooleanMask()); // Genera el Laberinto de forma dinamica
        _setRoad();                         // Generar Caminos Alternativos
        _setCup();                          // Genera la COPA    
        _setExit();                    // Genera las Salidas
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodo de Colocacion        //////////////////////////////////////////////////////////////////////////////////////////
    ///     
    //Tipos de trampas
    enum typeTrap
    {
        T1 = -2,
        T2 = -3,
        T3 = -4
    }

    //Establece las trampas
    private void _setTraps( int typeTrap)
    {
        int cont = 0;
        while(cont < 9)
        {
            //posiciones en el centro del mapa
            int x = _random.Next(5, _rows - 5); 
            int y = _random.Next(5, _cols - 5);
            //Colocar trampas
            if(_validTrap(x, y))
            {
                _maze[x, y] = typeTrap;
                cont++;
            }
        }
    }

    // Establece caminos alternativos
    private void _setRoad()
    {
        int cont = 0;
        while(cont < 50)
        {
            //posiciones en el centro del mapa
            int x = _random.Next(5, _rows - 5); 
            int y = _random.Next(5, _cols - 5);

            //Colocar trampas
            if(_validRoad(x, y))
            {
                _maze[x, y] = -5;
                cont++;
            }
        }
    }

    // Establece el jugado
    private void _setPlayer(Players player)
    {
        for(int i = 0; i < player._tokens.Length; i++)
        {
            // Jugador (cerca de la esquina superior izquierda)
            _maze[player._tokens[i].CoordX, player._tokens[i].CoordY] = player._tokens[i].InfoId(); 
            
        }
    }
    
    // Establece los objetos en el mapa   
    // NOTA: SI ALGUNA FICHA PASA POR LA POSICION DE UNO DE ESTOS OBJETOS Y NO LO TOMA, ESTE OBJETO DESAPARECERA DEL MAPA AUTOMATICAMENTE
    private void _setObject()
    {
        int cont = 0;
        for (int i = -11; i < -6; i++)
        {
            while (cont < 2)
            {
                int x = _random.Next(1,40);
                int y = _random.Next(20,45);
                if(_maze[x, y] == 0)
                {
                    _maze[x, y] = i;
                    cont++;
                }
            }
            cont = 0;
        }
    }

    // Coloca la Copa en el Mapa
    private void _setCup()
    {
        _cupX = _random.Next(8, 40);
        _cupY = _random.Next(34, 40);
        if(!_checkTokenInThisPosition(_cupX, _cupY))
            _maze[_cupX,_cupY] = -6;
    }
   
    // Establece la entrada y salida del laberinto**********************************************************
    private void _setExit()
    {

        int cont = 0;
        int x;
        int y;
        while(cont < 4)
        {
            x = _random.Next(5, 40);
            y = _random.Next(5, 20);
            if(_maze[x, y] == 0 && _validExit(x, y))
            {
                _maze[x, y] = -12;
                cont++;
            }
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodo de Verificacion          //////////////////////////////////////////////////////////////////////////////////////////

    // Verifica si es la salida del Laberinto
    public bool IsExit(int x, int y)
    {
        for(int i = 0; i < 4; i++)
        {
            if(Maze._maze[x, y] == -12)
                return true;
        }
        return false;
    }
     
    private bool _validExit(int x, int y)
    {
        for(int i = 0; i < 5; i++) 
        {
            for(int j = 0; j < 5; j++)
            {
                // Verifica q las salidas no esten cerca
                bool exitAway =
                (_maze[x + i, y + j] == -12) || 
                (_maze[x - i, y - j] == -12) || 
                (_maze[x + i, y - j] == -12) || 
                (_maze[x - i, y + j] == -12);

                if(exitAway)
                    return false;
            }
        }
        return true;
    }

    //  Verifica q se puedan establecer esos caminos
    private bool _validRoad(int x, int y)
    {
        for(int i = 0; i < 2; i++) 
        {
            for(int j = 0; j < 2; j++)
            {
                bool surroundedByWalls =
                    (_maze[x + 1, y] == -1 && _maze[x - 1, y] == -1) &&
                    (_maze[x, y + 1] == -1 && _maze[x, y - 1] == -1);

                bool invalidPlacement =
                    !((_maze[x + 1, y] == -1 && _maze[x - 1, y] == -1) || 
                    (_maze[x, y + 1] == -1 && _maze[x, y - 1] == -1));

                bool isWall = _maze[x, y] == -1;


                if (invalidPlacement || surroundedByWalls || !isWall)
                {
                    return false;
                }                             
            }    
        }
        return true;
    }

    // Verifica si una celda es válida para colocar un camino
    private bool _isValid(int x, int y)
    {
        return x > 0 && y > 0 && x < _rows - 1 && y < _cols - 1 && _maze[x, y] == -1;
    }

    // Metodo para verificar si hay pared, retorna false si hay alguna pared en las direccion introducida y true si no hay pared
    public static bool CheckWall(int direction, int totalCheckBox, Tokens token)
    {
        switch(direction)
        {
            case 1: // Arriba
            
            for (int i = 0; i < totalCheckBox; i++)
            {
                if(Maze._maze[token.CoordX + i, token.CoordY] == -1)
                    return false;
            }

            break;
        
            case 2: // Abajo
            
            for (int i = 0; i < totalCheckBox; i++)
            {
                if(Maze._maze[token.CoordX - i, token.CoordY] == -1)
                    return false;
            }

            break;

            case 3: // Izquierda
            
            for (int i = 0; i < totalCheckBox; i++)
            {
                if(Maze._maze[token.CoordX, token.CoordY - i] == -1)
                    return false;
            }

            break;

            case 4: // Derecha
            
            for (int i = 0; i < totalCheckBox; i++)
            {
                if(Maze._maze[token.CoordX, token.CoordY + i] == -1)
                    return false;
            }

            break;
        }

        return true;
    }

    // Verifica la posicion de la trampa
    private bool _validTrap(int x, int y)
    {
        for(int i = 0; i < 3; i++) 
        {
            for(int j = 0; j < 3; j++)
            {
                // Verifica q las trampas no esten cerca
                bool trapsAway =
                (_maze[x + i, y + j] == -2) || (_maze[x - i, y - j] == -2) || (_maze[x + i, y - j] == -2) || (_maze[x - i, y + j] == -2) || 
                (_maze[x + i, y + j] == -3) || (_maze[x - i, y - j] == -3) || (_maze[x + i, y - j] == -3) || (_maze[x - i, y + j] == -3) || 
                (_maze[x + i, y + j] == -4) || (_maze[x - i, y - j] == -4) || (_maze[x + i, y - j] == -4) || (_maze[x - i, y + j] == -4);

                // Verifica de q hayan paredes adyacense para colocar las trampas
                bool adjacentRoads = !((_maze[(x+1), y] == 0 && _maze[(x-1), y] == 0) || (_maze[x, (y+1)] == 0 && _maze[x, (y-1)] == 0));

                //Verifica si se puede colocar trampas
                if(trapsAway || !adjacentRoads || _checkTokenInThisPosition(x, y))
                    return false;
            }    
        }
        return true;
    }

    // Verifica q en esa posicion haya una ficha o no
    private bool _checkTokenInThisPosition(int x, int y)
    {
        for (int i = 1; i < 9; i++)
        {
            if(_maze[x, y] == i)
                return true;
        }
        return false;
    }

    // Verifica si alguna de las pieazas del jugador logro el objetivo
    public bool CheckCupPlayer(Players player)
    {
        for(int i = 0; i < player._tokens.Length; i++)
            if(CheckCup(player._tokens[i]))
                return true;
        return false;
    }

    // Verifica si tiene la copa
    public bool CheckCup(Tokens token)
    {
        for(int i = 0; i < 3; i++)
            if(token.InfoBox(i) == -6)
                return true;
        return false;
    }

    // Sobrecarga del Metodo q verifica si tienes la copa
    public bool CheckCup(Players player)
    {
        for (int i = 0; i < player._tokens.Length; i++)
            for(int j = 0; j < 3; j++)
                if(player._tokens[i].InfoBox(j) == -6)
                    return true;
        return false;
    }

    // Verifica q las puertas ya hayn sido generadas
    public static bool GeneratedDoors(bool cup)
    {
        if(cup && !_generatedDoors)
        {
            _generatedDoors = true;
            return true;
        }
        return false;
    }

    // Verifica q en el mapa ya no haya una copa
    private bool _checkCupInMaze()
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                if(Maze._maze[i, j] == -6)
                    return true;
            }
        }
        return false;
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

                if (!printedToken)
                {
                    // Si no hay fichas en la posicion x,y del laberinto entonces imprimimos el contenido normal del laberinto
                    Console.Write(
                        _maze[x, y] == -1 ? "🌿" : // Pared
                        _maze[x, y] == -2 ? "☠️" : // Trampa tipo 1
                        _maze[x, y] == -3 ? "❄️" : // Trampa tipo 2
                        _maze[x, y] == -4 ? "💥" : // Trampa tipo 3
                        _maze[x, y] == -6 ? "🏆" : // COPA
                        _maze[x, y] == -7 ? "🧬" : // Posion de vida
                        _maze[x, y] == -8 ? "🏃" : // Posion de velocidad
                        _maze[x, y] == -9 ? "✂️" : // Tijera Magica
                        _maze[x, y] == -10 ? "🧹" : // Escoba
                        _maze[x, y] == -11 ? "🛡️" : // Escudo
                        _maze[x, y] == -12 ? "🚪" : // Salida
                        "  "                        // Camino vacío
                    );
                }
            }
            Console.WriteLine();
        }
    }
     
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Metodo de victoria          //////////////////////////////////////////////////////////////////////////////////////////
    
    // Condicion de Victoria 
    public void Win(Players player1, Players player2, Maze maze, int newX, int newY, ref bool running)
    {
        
        if(player1.InfoFaction() == "MAGOS")
        {            
            for (int i = 0; i < player1._tokens.Length; i++)
            {
                // Verificaa si los MAGOS llegaron a la salida con la COPA
                if(IsExit(newX, newY) && Maze.GeneratedDoors(maze.CheckCup(player1)))
                {
                    running = false;
                    foreach (var color in GamePlay.colors)
                    {
                        Console.ForegroundColor = color;
                        Console.WriteLine($"FELICIDADES {player1.InfoName()}, HAS GANADO EL JUEGO DEL LABERINTO");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(500);  // Pequeño retraso para mostrar el cambio de color
                    }
                    GamePlay.Pause("PRESIONE UNA TECLA PARA FINALIZAR");
                }
                //  Verifica si los MONSTRUOS mataron a todos los MAGOS
                else if(player1._tokens.Length == 0 || _anotherWay(player1))
                {
                    running = false;
                    foreach (var color in GamePlay.colors)
                    {
                        Console.ForegroundColor = color;
                        Console.WriteLine($"FELICIDADES {player2.InfoName()}, HAS GANADO EL JUEGO DEL LABERINTO");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(500);  // Pequeño retraso para mostrar el cambio de color
                    }
                    GamePlay.Pause("PRESIONE UNA TECLA PARA FINALIZAR");
                }
            }
        }
        else
        { 
            for (int i = 0; i < player2._tokens.Length; i++)
            {
                // Verificaa si los MAGOS llegaron a la salida con la COPA
                if(IsExit(newX, newY) && Maze.GeneratedDoors(maze.CheckCup(player1)))
                {
                    running = false;
                    foreach (var color in GamePlay.colors)
                    {
                        Console.ForegroundColor = color;
                        Console.WriteLine($"FELICIDADES {player2.InfoName()}, HAS GANADO EL JUEGO DEL LABERINTO");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(500);  // Pequeño retraso para mostrar el cambio de color
                    }
                    GamePlay.Pause("PRESIONE UNA TECLA PARA FINALIZAR");
                }
                //  Verifica si los MONSTRUOS mataron a todos los MAGOS
                else if(player2._tokens.Length == 0 || _anotherWay(player2))
                {
                    running = false;
                    foreach (var color in GamePlay.colors)
                    {
                        Console.ForegroundColor = color;
                        Console.WriteLine($"FELICIDADES {player1.InfoName()}, HAS GANADO EL JUEGO DEL LABERINTO");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(500);  // Pequeño retraso para mostrar el cambio de color
                    }
                    GamePlay.Pause("PRESIONE UNA TECLA PARA FINALIZAR");
                }
            }
        }
    }

    private bool _anotherWay(Players player)
    {
        if(CheckCup(player) && !_checkCupInMaze())
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

    /*      LEYENDA DE LABERINTO            //////////////////////////////////////////////////////////////////////////////////////////
        -1 = PARED
        -2 = TRAMPA 1
        -3 = TRAMPA 2
        -4 = TRAMPA 3
        -5 = CAMINOS OCULTOS
        -6 = COPA
        -7 = Posion de vida 
        -8 = Posion de velocidad
        -9 = Tijera Magica
        -10 = Escoba
        -11 = Escudo
        -12 = Salida 
        0 = CAMINO
        1 - 4 = PERSONAJES DEL JUGADOR 1
        5 - 8 = PERSONAJES DEL JUGADOR 2
*/

}
