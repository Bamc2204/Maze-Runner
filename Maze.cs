using System;

class Maze
{
    #region Propiedades del Laberinto           ////////////////////////////////////////////////////////////////////////////////////////

    private Random _random = new Random();                          // Generador de numeros aleatorios
    private static int _rows = 51, _cols = 51;                      // Filas y columnas
    public static int[,] GeneralMaze = new int [_rows, _cols];            // Laberinto
    private Players _player1, _player2;                             // Jugadores
    private int _cupX, _cupY;                                       // Coordenadas de la copa

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
                GeneralMaze[x, y] = -1; // Coloca paredes por defecto
            }
        }
    }

    // Genera el laberinto utilizando el algoritmo de backtracking
    private void _generateMaze(int x, int y)
    {
        GeneralMaze[x, y] = 0; // Marca la celda actual como un camino vacío
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
                GeneralMaze[x + DirX[address], y + DirY[address]] = 0;
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

    // Genera el laberinto cada cierto tiempo de forma dinamica
    public void GenerateNewMaze(Players player1, Players player2)
    {
        int [,] copyMaze = GeneralMaze;
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
                GeneralMaze[x, y] = typeTrap;
                cont++;
            }
        }
    }

    // Establece caminos alternativos
    private void _setRoad()
    {
        int cont = 0;
        while(cont < 80)
        {
            //posiciones en el centro del mapa
            int x = _random.Next(3, _rows - 3); 
            int y = _random.Next(3, _cols - 3);

            //Colocar trampas
            if(_validRoad(x, y))
            {
                GeneralMaze[x, y] = -5;
                cont++;
            }
        }
    }

    // Establece el jugado
    private void _setPlayer(Players player)
    {
        for(int i = 0; i < player.Tokens.Length; i++)
        {
            // Jugador (cerca de la esquina superior izquierda)
            GeneralMaze[player.Tokens[i].CoordX, player.Tokens[i].CoordY] = player.Tokens[i].InfoId(); 
            
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
                if(GeneralMaze[x, y] == 0)
                {
                    GeneralMaze[x, y] = i;
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
            GeneralMaze[_cupX,_cupY] = -6;
    }
   
    // Establece la entrada y salida del laberinto
    private void _setExit()
    {

        int cont = 0;
        int x;
        int y;
        while(cont < 4)
        {
            x = _random.Next(5, 40);
            y = _random.Next(5, 20);
            if(GeneralMaze[x, y] == 0 && _validExit(x, y))
            {
                GeneralMaze[x, y] = -12;
                cont++;
            }
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodo de Verificacion          //////////////////////////////////////////////////////////////////////////////////////////

    // Verifica si es la salida del Laberinto
    public bool IsExit(int x, int y)
    {
        if(Maze.GeneralMaze[x, y] == -12)
            return true;
        return false;
    }
     
    // Verifica si es valida la salida
    private bool _validExit(int x, int y)
    {
        for(int i = 0; i < 5; i++) 
        {
            for(int j = 0; j < 5; j++)
            {
                // Verifica q las salidas no esten cerca
                bool exitAway =
                (GeneralMaze[x + i, y + j] == -12) || 
                (GeneralMaze[x - i, y - j] == -12) || 
                (GeneralMaze[x + i, y - j] == -12) || 
                (GeneralMaze[x - i, y + j] == -12);

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
                    (GeneralMaze[x + 1, y] == -1 && GeneralMaze[x - 1, y] == -1) &&
                    (GeneralMaze[x, y + 1] == -1 && GeneralMaze[x, y - 1] == -1);

                bool invalidPlacement =
                    ((GeneralMaze[x + 1, y] == 0 && GeneralMaze[x - 1, y] == 0) || 
                    (GeneralMaze[x, y + 1] == 0 && GeneralMaze[x, y - 1] == 0));

                bool isWall = GeneralMaze[x, y] == -1;


                if (!invalidPlacement || surroundedByWalls || !isWall)
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
        return x > 0 && y > 0 && x < _rows - 1 && y < _cols - 1 && GeneralMaze[x, y] == -1;
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
                (GeneralMaze[x + i, y + j] == -2) || (GeneralMaze[x - i, y - j] == -2) || (GeneralMaze[x + i, y - j] == -2) || (GeneralMaze[x - i, y + j] == -2) || 
                (GeneralMaze[x + i, y + j] == -3) || (GeneralMaze[x - i, y - j] == -3) || (GeneralMaze[x + i, y - j] == -3) || (GeneralMaze[x - i, y + j] == -3) || 
                (GeneralMaze[x + i, y + j] == -4) || (GeneralMaze[x - i, y - j] == -4) || (GeneralMaze[x + i, y - j] == -4) || (GeneralMaze[x - i, y + j] == -4);

                // Verifica de q hayan paredes adyacense para colocar las trampas
                bool adjacentRoads = ((GeneralMaze[(x+1), y] == 0 && GeneralMaze[(x-1), y] == 0) || (GeneralMaze[x, (y+1)] == 0 && GeneralMaze[x, (y-1)] == 0));

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
            if(GeneralMaze[x, y] == i)
                return true;
        }
        return false;
    }

    // Verifica si alguna de las pieazas del jugador logro el objetivo
    public bool CheckCupPlayer(Players player)
    {
        for(int i = 0; i < player.Tokens.Length; i++)
            if(CheckCup(player.Tokens[i]))
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
        for (int i = 0; i < player.Tokens.Length; i++)
            for(int j = 0; j < 3; j++)
                if(player.Tokens[i].InfoBox(j) == -6)
                    return true;
        return false;
    }

    // Verifica q en el mapa ya no haya una copa
    private bool _checkCupInMaze()
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                if(Maze.GeneralMaze[i, j] == -6)
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

                    for (int i = 0; i < player1.Tokens.Length; i++)
                    {
                        if (GeneralMaze[x, y] == player1.Tokens[i].InfoId())
                        {
                            Console.Write(player1.Tokens[i].InfoCharacter());
                            printedToken = true;
                            break;
                        }
                    }

                // Comprobamos si hay alguna ficha de Player2 en esta posición
                if (!printedToken)
                {
                    for (int i = 0; i < player2.Tokens.Length; i++)
                    {
                        if (GeneralMaze[x, y] == player2.Tokens[i].InfoId())
                        {
                            Console.Write(player2.Tokens[i].InfoCharacter());
                            printedToken = true;
                            break;
                        }
                    }
                }

                if (!printedToken)
                {
                    // Si no hay fichas en la posicion x,y del laberinto entonces imprimimos el contenido normal del laberinto
                    Console.Write(
                        GeneralMaze[x, y] == -1 ? "🌿" : // Pared
                        GeneralMaze[x, y] == -2 ? "☠️" : // Trampa tipo 1
                        GeneralMaze[x, y] == -3 ? "❄️" : // Trampa tipo 2
                        GeneralMaze[x, y] == -4 ? "💥" : // Trampa tipo 3
                        GeneralMaze[x, y] == -6 ? "🏆" : // COPA
                        GeneralMaze[x, y] == -7 ? "🧬" : // Posion de vida
                        GeneralMaze[x, y] == -8 ? "🏃" : // Posion de velocidad
                        GeneralMaze[x, y] == -9 ? "✂️" : // Tijera Magica
                        GeneralMaze[x, y] == -10 ? "🧹" : // Escoba
                        GeneralMaze[x, y] == -11 ? "🛡️" : // Escudo
                        GeneralMaze[x, y] == -12 ? "🚪" : // Salida
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
    public void Win(Tokens token, Players player1, Players player2, Maze maze, int newX, int newY, ref bool running)
    {
        
        if(player1.InfoFaction() == "MAGOS")
        {            
            _checkGoodWin(token, player1, player2, maze, newX, newY, ref running);
            _checkBadWin(player2, player1, maze, newX, newY, ref running);
        }
        else
        { 
            _checkGoodWin(token, player2, player1, maze, newX, newY, ref running);
            _checkBadWin(player1, player2, maze, newX, newY, ref running);
        }
    }

    // Verifica q los Magos ganaron
    private void _checkGoodWin(Tokens token, Players player1, Players player2, Maze maze, int newX, int newY, ref bool running)
    {   
        // Verificaa si los MAGOS llegaron a la salida con la COPA
        if(IsExit(newX, newY) && maze.CheckCup(token))
        {
            running = false;
            foreach (var color in GamePlay.colors)
            {
                Console.Clear();
                Console.ForegroundColor = color;
                Console.WriteLine($"FELICIDADES {player1.InfoName()}, HAS GANADO EL JUEGO DEL LABERINTO");
                Console.ResetColor();
                System.Threading.Thread.Sleep(500);  // Pequeño retraso para mostrar el cambio de color
            }
            GamePlay.Pause("PRESIONE UNA TECLA PARA FINALIZAR");
        }
    }

    // Verifica q los Monstruos ganaron
    private void _checkBadWin(Players player1, Players player2, Maze maze, int newX, int newY, ref bool running)
    {
        //  Verifica si los MONSTRUOS mataron a todos los MAGOS
        if(player2.Tokens.Length < 1 || _anotherWay(player1))
        {
            running = false;
            foreach (var color in GamePlay.colors)
            {
                Console.Clear();
                Console.ForegroundColor = color;
                Console.WriteLine($"FELICIDADES {player1.InfoName()}, HAS GANADO EL JUEGO DEL LABERINTO");
                Console.ResetColor();
                System.Threading.Thread.Sleep(500);  // Pequeño retraso para mostrar el cambio de color
            }
            GamePlay.Pause("PRESIONE UNA TECLA PARA FINALIZAR");
            return;
        }
    }

    // Otra forma de ganar
    private bool _anotherWay(Players player)
    {
        if(!CheckCup(player) && !_checkCupInMaze())
            return true;

        return false;
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Metodos de informacion          //////////////////////////////////////////////////////////////////////////////////////////
    
    // Informacion del  laberinto
    public static int[,] InfoMaze()
    {
        return GeneralMaze;
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
