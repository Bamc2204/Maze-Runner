using System;

class Players
{
    #region PropiedadesPepe
    private string _name;               // Nombre del jugador
    private bool _myTurn;               // Turno de la ficha
    private Tokens[] _token;            // Cantidad de fichas (hasta 4)
    #endregion

    // Constructor del Jugador
    public Players(string name)
    {
        _name = name;                   //Nombre del jugador
        _myTurn = false;                //Todos los turnos empiezan falso
        _token = new Tokens[4];         //Cantidad de fichas

        _ChooseTokens(ref _token);                //Escoger Fichas
    }

    // Metodo para escoger las fichas
    public void _ChooseTokens(ref Tokens[] tokens)
    {
        Console.WriteLine("Las fichas disponibles son:\n Los magos escogidos por Las 3 Grandes escuelas de Magia y Las Bestias q abundan en el laberinto para matar");

        Console.WriteLine("\n El 1er jugador sera el q tenga la oportunidad de escoger primero, y el segundo se quedara con la faccion restante");

        Console.WriteLine("\n Espero q ambos jugadores se puedan poner de acuerdo antes  de q el juego comience");  

        Console.WriteLine("\n Presione la flecha a la izquierda si quiere ser mago y la flecha a la derecha si quiere ser Monstruo");      

        ConsoleKey key = Console.ReadKey().Key;

        _readBoard(key, ref tokens);
    }

    // Metodo para crear las fichas buenas*****
    public void CreateTokensGoodPlayer(ref Tokens[] tokens)
    {
        System.Console.WriteLine("Metodo good player");

        System.Console.WriteLine();

        tokens[0] = new Tokens("Harry Potter", 1, "HP", 1, 0, "Velocidad", 4, 3, 3, 4, 6);

        tokens[1] = new Tokens("Cedric Diggory", 2, "CD", 7, 0, "Velocidad", 4, 4, 4, 4, 6);

        tokens[2] = new Tokens("Fleur Delacour", 3, "FD", 13, 0, "Velocidad", 4, 8, 4, 4, 6);

        tokens[3] = new Tokens("Viktor Krum", 4, "VK", 19, 0, "Velocidad", 4, 8, 4, 4, 6);

        System.Console.WriteLine("HOLA");
    }

    // Metodo para crear las fichas malas
    public void CreateTokensBadPlayer(ref Tokens[] tokens)
    {
        tokens[0] = new Tokens("Acromántula", 5, "Ac", "Velocidad", 4, 3, 300);

        tokens[1] = new Tokens("Esfinge", 6, "Es", "Velocidad", 4, 4, 300);

        tokens[2] = new Tokens("Boggart", 7, "Bo", "Velocidad", 4, 8, 300);

        tokens[3] = new Tokens("Blast-Ended Skrewts", 8, "Bl", "Velocidad", 4, 8, 300);
    }

    // Metodo de Informacionde la faccion buena
    public void InfGoodFaction()
    {
        Console.WriteLine("Informacion de la faccion:");
        Console.WriteLine("ELEGIDOS POR LAS 3 GRANDES ESCUELAS DE MAGIA:");

        // Expresa la informacion en forma de tabla
        string nameRowName = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Harry Potter", "Cedric Diggory", "Fleur Delacour", "Viktor Krum");   // Nombres
        string infoRowCharacter = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "HP", "CD", "FD", "VK");                                         // Caracteres
        string infoRowSkill = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Velocidad", "Velocidad", "Velocidad", "Velocidad");                 // Habilidad
        string infoRowColdTime = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                              // Tiempo de Enfriamiento
        string infoRowSpeed = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                                 // Velocidad

        Console.WriteLine("NOMBRES \n" + nameRowName);
        Console.WriteLine("CARACTERES \n" + infoRowCharacter);
        Console.WriteLine("HABILIDADES \n" + infoRowSkill);
        Console.WriteLine("TIEMPO DE ENFRIAMIENTO DE LAS HABILIDADES \n" + infoRowColdTime);
        Console.WriteLine("VELOCIDAD \n" + infoRowSpeed);
    }

    // Metodo de Informacionde la faccion buena
    public void InfBadFaction()
    {
        Console.WriteLine("Informacion de la faccion:");
        Console.WriteLine("MONSTRUOS QUE VAGAN POR EL LABERINTO:");

        // Expresa la informacion en forma de tabla
        string nameRowName = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Acromántula", "Esfinge", "Boggart", "Blast-Ended Skrewts");  // Nombres
        string infoRowCharacter = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Ac", "Es", "Bo", "Bl");                                 // Caracteres
        string infoRowSkill = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Velocidad", "Velocidad", "Velocidad", "Velocidad");                                    // Habilidad
        string infoRowColdTime = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                          // Tiempo de Enfriamiento
        string infoRowSpeed = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                             // Velocidad

        Console.WriteLine("NOMBRES \n" + nameRowName);
        Console.WriteLine("CARACTERES \n" + infoRowCharacter);
        Console.WriteLine("HABILIDADES \n" + infoRowSkill);
        Console.WriteLine("TIEMPO DE ENFRIAMIENTO DE LAS HABILIDADES \n" + infoRowColdTime);
        Console.WriteLine("VELOCIDAD \n" + infoRowSpeed);
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
    
    public Tokens InfoPiece(int index)
    {
        return _token[index];
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

    // Turno del jugador*****
    public void PlayersTurn(Maze maze, Players player1, Players player2, ref bool running, bool getTarget)
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

            _displacement(player1.SelectToken(indexPiece).InfoSpeed(), maze, player1.SelectToken(indexPiece), player1, player2, ref running, getTarget);
        }
    }

    // Desplaza la ficha*****
    private static void _displacement(int steps, Maze lab, Tokens piece1,Players player1, Players player2, ref bool running, bool getTarget)
    {   
        lab._maze[piece1._coordX, piece1._coordY] = 2;
        int newX = piece1._coordX;
        int newY = piece1._coordY;


        while(steps != 0 && running)
        {
            // Si llegas al final del juego
            if(lab.Win(piece1._coordX, piece1._coordY, getTarget))                  // Si llego al final del laberinto o no
            {
                Console.WriteLine("Felicidades, Completaste El Laberinto");                
                running = false;
            }
                
            else
            {
                // Tecla q toca el jugador en el teclado            
                ConsoleKey key = Console.ReadKey().Key;

                _readBoard(key, lab, ref newX, ref newY, ref running, ref piece1, player1, player2);

                if(key == ConsoleKey.I) 
                    continue;

                _checkTrap(lab, ref newX, ref newY, ref running, ref piece1);

                // Dentro de filas, columnas y si es un camino
                if (newX >= 0 && newX < lab._maze.GetLength(0) && newY >= 0 && newY < lab._maze.GetLength(1) && 
                lab._maze[newX, newY] != -1 && lab._maze[newX, newY] != 2)                    
                {
                    // Actualiza el tablero
                    lab._maze[piece1._coordX, piece1._coordY] = 0;                 // Vacía la posición actual
                    lab._maze[newX, newY] = 2;                                     // Mueve la ficha
                    piece1._coordX = newX;                                          // Actualiza las coordenadas actuales
                    piece1._coordY = newY;
                    steps --;                                                       // Pasos q puede recorer cada ficha
                }

                else
                {
                    System.Console.WriteLine("los pasos no son validos");
                    newX = piece1._coordX; newY = piece1._coordY;                    
                }

                lab.PrintMaze(player1, player2);                                    // Imprime el laberinto
            }
        }  
    }
    
    // Lee el teclado*****
    public static void _readBoard(ConsoleKey key, Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens piece, Players player1, Players player2) 
    {
        //Casos para cada tecla
        switch (key)
        {
            case ConsoleKey.UpArrow:    newX = piece._coordX - 1; break;
            case ConsoleKey.DownArrow:  newX = piece._coordX + 1; break;
            case ConsoleKey.LeftArrow:  newY = piece._coordY - 1; break;
            case ConsoleKey.RightArrow: newY = piece._coordY + 1; break;
            case ConsoleKey.Escape: Console.WriteLine("Simulación detenida."); running = false; break;
            case ConsoleKey.Tab: piece._useBoxObject(lab, ref newX, ref newY, ref piece, player1, player2); break;
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
    
    // 2da Sobrecarga para leer el teclado
     public void _readBoard(ConsoleKey key, ref Tokens[] tokens)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow: Console.WriteLine("Has escogido la faccion de los Magos"); CreateTokensGoodPlayer(ref tokens); break;
            case ConsoleKey.RightArrow:  Console.WriteLine("Has escogido la faccion de los Monstruos"); CreateTokensBadPlayer(ref tokens); break;
            default: System.Console.WriteLine("No a tocado la tecla correcta, tiene q tocar o bien flecha izquierda o bien flecha derecha, intentelo otra vez"); break;
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

    // Metodo de Caminar en el turno*****
    public static void Run(bool run, Maze _maze, Players player1, Players player2, bool getTarget)
    {
        while(run)
        {
            player1.PlayersTurn(_maze, player1, player2, ref run, getTarget);
            
            player2.PlayersTurn(_maze, player2, player1, ref run, getTarget);

            if(_maze.Win(player1.SelectToken(0)._coordX, player1.SelectToken(0)._coordY, getTarget))
                run = false;
        }
    }	
}