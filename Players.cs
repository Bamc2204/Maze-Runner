using System;

class Players
{
    #region Propiedades
    private string _name;
    private bool _myTurn;               //Turno de la ficha
    private Tokens[] _token;            //Cantidad de fichas (hasta 4)   
    private int _selectToken;           //Indice de la ficha seleccionado por el jugador
    #endregion

    // Constructor del Jugador
    public Players(string name)
    {
        _name = name;                   //Nombre del jugador
        _myTurn = false;                //Todos los turnos empiezan falso
        _token = new Tokens[4];         //Cantidad de fichas
        _selectToken = -1;               //No se ha seleccionado fichas
    }

    // Metodo para coger las fichas del jugador
    public void AddTokens(Tokens token1, Tokens token2, Tokens token3, Tokens token4)
    {
        _token[0] = token1;
        _token[1] = token2;
        _token[2] = token3;
        _token[3] = token4;
    }

    // Metodo para acceder a las fichas sin modificarlas
    public Tokens InfoTokens(int index)
    {
        return _token[index];
    }

    // Metodo de sobrecarga para acceder a las fichas sin modificarlas
    public Tokens[] InfoTokens()
    {
        return _token;
    }

    // Metodo para seleccionar ficha
    public Tokens SelectToken(int index) 
    {
        return _token[index];    
    }

    // Informacion del turno del jugador
    public bool InfoTurn()
    {
        return _myTurn;
    }

    // Metodo para iniciar el turno del jugador
    public bool StartTurn()
    {
        return true;
    }

    // Terminar el turno
    public bool EndTurn() 
    {
        return false;    
    }

    // Turno del jugador
    public void PlayersTurn(Maze maze, Players player, ref bool running)
    {
        int indexPiece = 0;

        while(running)
        {    
            Console.WriteLine("Inroduzca cual de sus fichas va a coger (del 1 al 4)");

            ConsoleKey key = Console.ReadKey().Key;

            //Verifica q no escoja otro numero q no sea el de las fichas
            if (!(key == ConsoleKey.NumPad1 || key == ConsoleKey.NumPad2 || key == ConsoleKey.NumPad3 || key == ConsoleKey.NumPad4 ||
            key == ConsoleKey.D1 || key == ConsoleKey.D2 || key == ConsoleKey.D3 || key == ConsoleKey.D4))
            {
                Console.WriteLine("Esa ficha no exite, tiene q escoger una ficha del 1-4, inténtelo de nuevo " + key);

                continue;
            }

            _readBoard(key, ref indexPiece, ref running);

            Console.WriteLine("Ya puede desplazarse");

            _displacement(player.SelectToken(indexPiece).InfoSpeed(), maze, player.SelectToken(indexPiece), ref running);
        }
    }

    // Desplaza la ficha
    private static void _displacement(int steps, Maze lab, Tokens piece, ref bool running)
    {   
        lab._maze[piece._coordX, piece._coordY] = 2;
        int newX = piece._coordX;
        int newY = piece._coordY;

        while(steps != 0 && running)
        {
            // Si llegas al final del juego
            if(lab.Win(piece._coordX, piece._coordY))               // Si llego al final del laberinto o no
            {
                Console.WriteLine("Felicidades, Completaste El Laberinto");                
                running = false;
            }
                
            else
            {
                // Tecla q toca el jugador en el teclado            
                ConsoleKey key = Console.ReadKey().Key;

                _readBoard(key, lab, ref newX, ref newY, ref running, ref piece);

                if(key == ConsoleKey.I) 
                    continue;

                _checkTrap(lab, ref newX, ref newY, ref running, ref piece);

                // Dentro de filas, columnas y si es un camino
                if (newX >= 0 && newX < lab._maze.GetLength(0) && newY >= 0 && newY < lab._maze.GetLength(1) && 
                lab._maze[newX, newY] != -1 && lab._maze[newX, newY] != 2)                    
                {
                    // Actualiza el tablero
                    lab._maze[piece._coordX, piece._coordY] = 0;        // Vacía la posición actual
                    lab._maze[newX, newY] = 2;                          // Mueve la ficha
                    piece._coordX = newX;                               // Actualiza las coordenadas actuales
                    piece._coordY = newY;
                    steps --;                                           // Pasos q puede recorer cada ficha
                }

                else
                {
                    System.Console.WriteLine("los pasos no son validos");
                    newX = piece._coordX; newY = piece._coordY;                    
                }

                lab.PrintMaze(piece);                   // Imprime el laberinto
            }
        }  
    }
    
    // Lee el teclado
    public static void _readBoard(ConsoleKey key, Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens piece) 
    {
        //Casos para cada tecla
        switch (key)
        {
            case ConsoleKey.UpArrow:    newX = piece._coordX - 1; break;
            case ConsoleKey.DownArrow:  newX = piece._coordX + 1; break;
            case ConsoleKey.LeftArrow:  newY = piece._coordY - 1; break;
            case ConsoleKey.RightArrow: newY = piece._coordY + 1; break;
            case ConsoleKey.Escape: Console.WriteLine("Simulación detenida."); running = false; break;
            case ConsoleKey.Tab: piece._useBoxObject(lab, ref newX, ref newY, ref piece); break;
            case ConsoleKey.I: piece.DisplayStatus(); break;
        }
    }

    // Sobrecarga para leer el teclado
    public static void _readBoard(ConsoleKey key, ref int index, ref bool running)
    {
        switch (key)
        {
            case ConsoleKey.NumPad1: index = 0; break;
            case ConsoleKey.NumPad2: index = 1; break;
            case ConsoleKey.NumPad3: index = 2; break;
            case ConsoleKey.NumPad4: index = 3; break;
            case ConsoleKey.D1: index = 0; break;
            case ConsoleKey.D2: index = 1; break;
            case ConsoleKey.D3: index = 2; break;
            case ConsoleKey.D4: index = 3; break;
        }
    }
    // Chequea si caiste en una trampa
    public static void _checkTrap(Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens piece)
    {
        if(lab._maze[newX, newY] == -2) //Verificacion de trapas
        {
            piece.RemoveHealth(20);
            System.Console.WriteLine("Has caido en una trampa y has perdido 20 puntos de vidas");
            if(piece.InfoHealth() == 0)
            {   
                running = false;
                System.Console.WriteLine("Has muerto");
            }
        }
    }

    // Metodo de Caminar en el turno
    public static void Run(bool run, Maze _maze, Players player1, Players player2)
    {
        while(run)
        {
            player1.PlayersTurn(_maze, player1, ref run);
            
            player2.PlayersTurn(_maze, player2, ref run);

            if(_maze.Win(player1.SelectToken(0)._coordX, player1.SelectToken(0)._coordY))
                run = false;
        }
    }	
}