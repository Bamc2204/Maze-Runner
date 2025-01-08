using System;

class Players
{
    #region Propiedades del Jugador         ////////////////////////////////////////////////////////////////////////////////////////
    
    private string _name;               // Nombre del jugador
    private bool _myTurn = true;               // Turno de la ficha
    public Tokens[] _token;             // Cantidad de fichas (hasta 4)
    private int _index = 0;             // Indice para escoger la faccion
    private string _faction;            // Faccion del jugador

    private string _InfoTarget;         // Objetivo del jugador

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Constructor del Jugador         ////////////////////////////////////////////////////////////////////////////////////////

    // Constructor del Jugador
    public Players(string name, int indexFaction)
    {
        _name = name;                   //Nombre del jugador
        _myTurn = false;                //Todos los turnos empiezan falso
        _token = new Tokens[4];         //Cantidad de fichas
        _index = indexFaction;
        _faction = ((Faction)_index).ToString();
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Faccion         ////////////////////////////////////////////////////////////////////////////////////////
    
    // Enumeracion de las facciones
    enum Faction
    {
        MAGOS = 1,
        MONSTRUOS = 2
    }

    // Metodo para escoger las fichas
    public void _ChooseFaction(ref int indexPlayer1, ref int indexPlayer2)
    {
        if(indexPlayer1 == 1)
        {
            indexPlayer2 = 2;
            _faction = ((Faction)indexPlayer1).ToString();
        }
        else
        {
            indexPlayer2 = 1;
            _faction = ((Faction)indexPlayer1).ToString();
        }  
    }

    // metodo para crear las fichas de la facion
    public void CreateTokensFaction(Players player1, Players player2)
    {   
        if(player1.InfoIndexFaction() == 1)
        {
            CreateTokensGoodPlayer(ref player1);
            CreateTokensBadPlayer(ref player2);
            Console.WriteLine("\n El jugador " + player1.InfoName() + " ha escogido la faccion " + player1.InfoFaction());
            Console.WriteLine("\n Y el jugador " + player2.InfoName() + " ha escogido la faccion " + player2.InfoFaction());
        }
        else
        {
            CreateTokensGoodPlayer(ref player2);
            CreateTokensBadPlayer(ref player1);
            Console.WriteLine("\n El jugador " + player1.InfoName() + " ha escogido la faccion " + player1.InfoFaction());
            Console.WriteLine("\n Y el jugador " + player2.InfoName() + " ha escogido la faccion " + player2.InfoFaction()); 
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Creacion de Fichas         ////////////////////////////////////////////////////////////////////////////////////////

    // Metodo para crear las fichas buenas*******************************************************************************************************************
    public void CreateTokensGoodPlayer(ref Players player)
    {
        player._token[0] = new Tokens("Harry Potter", 1, "HP", 1, 0, "Velocidad", 4, 3, 3, 4, 6);

        player._token[1] = new Tokens("Cedric Diggory", 2, "CD", 13, 0, "Velocidad", 4, 4, 4, 4, 6);

        player._token[2] = new Tokens("Fleur Delacour", 3, "FD", 25, 0, "Velocidad", 4, 8, 4, 4, 6);

        player._token[3] = new Tokens("Viktor Krum", 4, "VK", 37, 0, "Velocidad", 4, 8, 4, 4, 6);

        string infoTarget = "Obtener el Caliz de fuego y escapar del laberinto";

        player.SetTarget(ref infoTarget);
    }

    // Metodo para crear las fichas malas********************************************************************************************************************
    private void CreateTokensBadPlayer(ref Players player)
    {
        player._token[0] = new Tokens("Acromántula", 5, "Ac", 7, 43, "Velocidad", 4, 3, 80, 1, 300);

        player._token[1] = new Tokens("Esfinge", 6, "Es", 19, 43, "Velocidad", 4, 4, 90, 1, 300);

        player._token[2] = new Tokens("Boggart", 7, "Bo", 31, 43, "Velocidad", 4, 8, 50, 3, 300);

        player._token[3] = new Tokens("Blast-Ended Skrewts", 8, "Bl", 43, 43, "Velocidad", 4, 8, 60, 5, 300);

        string infoTarget = "Asesinar a los 4 campeones y evitar que escapen del laberinto";

        player.SetTarget(ref infoTarget);
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Objetivo         ////////////////////////////////////////////////////////////////////////////////////////
    
    // Metodo para asignar el objetivo***********************************************************************************************************************
    public void SetTarget(ref string infoTarget)
    {
        _InfoTarget = infoTarget;
    }

    //Verifica se logro el objetivo 
    public bool _checkTargetPlayers(Players player)
    {
        if(!((player.InfoTokens(0)._target) && (player.InfoTokens(1)._target) && (player.InfoTokens(2)._target) && (player.InfoTokens(3)._target)))
        {
            return true;
        }
        return false;
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de Turnos           ////////////////////////////////////////////////////////////////////////////////////////

    // Metodo para iniciar el turno del jugador
    public void StartTurn()
    {
        _myTurn = true;
    }

    // Terminar el turno
    public void EndTurn() 
    {
        _myTurn = false;    
    }

    // Turno del jugador*****
    public static void PlayersTurn(Maze maze, Players player1, Players player2, ref bool running)
    {
        int indexPiece = 0;

        while(running)
        {    
            Console.WriteLine("\n Inroduzca cual de sus fichas va a coger (del 1 al 4)");

            ConsoleKey key = Console.ReadKey().Key;

            //Verifica q no escoja otro numero q no sea el de las fichas
            if (!(key == ConsoleKey.NumPad1 || key == ConsoleKey.NumPad2 || key == ConsoleKey.NumPad3 || key == ConsoleKey.NumPad4 ||
            key == ConsoleKey.D1 || key == ConsoleKey.D2 || key == ConsoleKey.D3 || key == ConsoleKey.D4))
            {
                Console.WriteLine("\n Esa ficha no exite, tiene q escoger una ficha del 1-4, inténtelo de nuevo " + key);

                continue;
            }

            _readBoard(key, ref indexPiece, ref running);

            Console.WriteLine("\n Ya puede desplazarse");

            if(!((player1.InfoTokens(indexPiece)._target) && (player2.InfoTokens(indexPiece)._target)))
            {
                if(!(player1.InfoTokens(indexPiece)._target) && player1.InfoTurn())
                {
                    
                    _displacement(player1.SelectToken(indexPiece).InfoSpeed(), maze, player1.SelectToken(indexPiece), player1, player2, ref running, ref player1.InfoTokens(indexPiece)._target);

                    player1.EndTurn();

                    player2.StartTurn();
                }
                else if(!(player2.InfoTokens(indexPiece)._target) && player2.InfoTurn())
                {

                    _displacement(player2.SelectToken(indexPiece).InfoSpeed(), maze, player2.SelectToken(indexPiece), player2, player1, ref running, ref player2.InfoTokens(indexPiece)._target);

                    player1.EndTurn();

                    player2.StartTurn();
                    
                }
            }
            //Aqui se verifica si se logro el objetivo
            else
            {
                running = false;
            }
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodo de Desplazamiento           ////////////////////////////////////////////////////////////////////////////////////////

    // Desplaza la ficha
    private static void _displacement(int steps, Maze maze, Tokens piece1,Players player1, Players player2, ref bool running,ref bool getTarget)
    {   
        Maze._maze[piece1._coordX, piece1._coordY] = piece1.InfoId();                 // Pone la ficha en el tablero
        int newX = piece1._coordX;
        int newY = piece1._coordY;


        while(steps != 0 && running)
        {
            // Si llegas al final del juego
            if(maze.Win(piece1._coordX, piece1._coordY, getTarget))                  // Si llego al final del laberinto o no
            {
                Console.WriteLine("\n Felicidades, Completaste El Laberinto");                
                running = false;
            }
                
            else
            {
                // Tecla q toca el jugador en el teclado            
                ConsoleKey key = Console.ReadKey().Key;

                _readBoard(key, maze, ref newX, ref newY, ref running, ref piece1, player1, player2);

                if(key == ConsoleKey.I || key == ConsoleKey.E || key == ConsoleKey.Tab || key == ConsoleKey.Escape) 
                    continue;

                _checkTrap(maze, ref newX, ref newY, ref running, ref piece1);

                // Dentro de filas, columnas y si es un camino
                if (newX >= 0 && newX < Maze._maze.GetLength(0) && newY >= 0 && newY < Maze._maze.GetLength(1) && 
                Maze._maze[newX, newY] != -1 && Maze._maze[newX, newY] != 2)                    
                {
                    // Actualiza el tablero
                    Maze._maze[piece1._coordX, piece1._coordY] = 0;                 // Vacía la posición actual
                    Maze._maze[newX, newY] = piece1.InfoId();                                     // Mueve la ficha
                    piece1._coordX = newX;                                          // Actualiza las coordenadas actuales
                    piece1._coordY = newY;
                    steps --;                                                       // Pasos q puede recorer cada ficha
                }

                else
                {
                    System.Console.WriteLine("\n Los pasos no son validos");
                    newX = piece1._coordX; newY = piece1._coordY;   
                    
                    Console.WriteLine("\n PRESIONE UNA TECLA PARA CONTINUAR"); 
                    key = Console.ReadKey().Key;                 
                }

                Maze.PrintMaze(player1, player2);                                    // Imprime el laberinto
            }
        }  
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Metodos de Lectura del Teclado          ////////////////////////////////////////////////////////////////////////////////////////

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

            case ConsoleKey.Escape: Console.WriteLine("\n Simulación detenida."); running = false ; break;

            case ConsoleKey.Tab: piece._useBoxObject(lab, ref newX, ref newY, ref piece, player1, player2); break;

            case ConsoleKey.I: Console.WriteLine("\n " + player1.InfoFaction() + "\n" + " \n OBETIVO: " + player1.InfoTarget()); piece.DisplayStatus(); break;

            case ConsoleKey.E: if(player1.InfoIndexFaction() == 1) player1.InfoGoodFaction(); else player1.InfoBadFaction(); Console.WriteLine("\n EL OBJETIVO DE LA FACCION: " + player1.InfoTarget()); break;

            case ConsoleKey.Q: piece.Attack(piece, ref player2, piece.InfoDamage()); break;
        }
    }

    // Sobrecarga para leer el teclado
    public static void _readBoard(ConsoleKey key, ref int _index, ref bool running)
    {
        switch (key)
        {
            case ConsoleKey.NumPad1: _index = 0; break;
            case ConsoleKey.NumPad2: _index = 1; break;
            case ConsoleKey.NumPad3: _index = 2; break;
            case ConsoleKey.NumPad4: _index = 3; break;
            case ConsoleKey.D1: _index = 0; break;
            case ConsoleKey.D2: _index = 1; break;
            case ConsoleKey.D3: _index = 2; break;
            case ConsoleKey.D4: _index = 3; break;
        }
    }
    
    // 2da Sobrecarga para leer el teclado
    public static void _readBoard(ConsoleKey key,ref int _index)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow: Console.WriteLine("\n Has escogido la faccion de los Magos"); _index = 1; break;
            case ConsoleKey.RightArrow:  Console.WriteLine("\n Has escogido la faccion de los Monstruos"); _index = 2; break;
        }
    }

    // 3ra Sobrecarga para leer el teclado
    public static void _readBoard(ConsoleKey key, Tokens token, ref Players player, int damage)
    {
        if(player.InfoIndexFaction() == 1)
        {  
            switch(key)
            {
                case ConsoleKey.UpArrow: 
                for(int i = 1; i < 4; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        if(Maze._maze[(token._coordX + i), token._coordY] == player.InfoTokens(j).InfoId())
                        {
                            player.InfoTokens(j).RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player.InfoTokens(j).InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                break;

                case ConsoleKey.DownArrow:
                for(int i = 1; i < 4; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        if(Maze._maze[(token._coordX - i), token._coordY] == player.InfoTokens(j).InfoId())
                        {
                            player.InfoTokens(j).RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player.InfoTokens(j).InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                break;

                case ConsoleKey.LeftArrow:
                for(int i = 1; i < 4; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        if(Maze._maze[token._coordX , (token._coordY - 1)] == player.InfoTokens(j).InfoId())
                        {
                            player.InfoTokens(j).RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player.InfoTokens(j).InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                break;

                case ConsoleKey.RightArrow:
                for(int i = 1; i < 4; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        if(Maze._maze[token._coordX , (token._coordY + 1)] == player.InfoTokens(j).InfoId())
                        {
                            player.InfoTokens(j).RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player.InfoTokens(j).InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                break;
            }
        }
        
        else
        {
            switch(key)
            {
                case ConsoleKey.UpArrow: 
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        if(Maze._maze[(token._coordX + i), token._coordY] == player.InfoTokens(j).InfoId())
                        {
                            player.InfoTokens(j).RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player.InfoTokens(j).InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                break;

                case ConsoleKey.DownArrow:
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        if(Maze._maze[(token._coordX - i), token._coordY] == player.InfoTokens(j).InfoId())
                        {
                            player.InfoTokens(j).RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player.InfoTokens(j).InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                break;

                case ConsoleKey.LeftArrow:
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        if(Maze._maze[token._coordX , (token._coordY - 1)] == player.InfoTokens(j).InfoId())
                        {
                            player.InfoTokens(j).RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player.InfoTokens(j).InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                break;

                case ConsoleKey.RightArrow:
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        if(Maze._maze[token._coordX , (token._coordY + 1)] == player.InfoTokens(j).InfoId())
                        {
                            player.InfoTokens(j).RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player.InfoTokens(j).InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                break;
            }
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodo para Verificar las trampas           ////////////////////////////////////////////////////////////////////////////////////////
    
    // Chequea si caiste en una trampa
    public static void _checkTrap(Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens piece)
    {
        if(Maze._maze[newX, newY] == -2) //Verificacion de trapas
        {
            piece.RemoveHealth(20, piece._activeShield);
            Console.WriteLine("\n Has caido en una trampa y has perdido 20 puntos de vidas");
            if(piece.InfoHealth() == 0)
            {   
                running = false;
                System.Console.WriteLine("\n Has muerto");
            }
        }
        else if(Maze._maze[newX, newY] == -3)
        {
            if(piece.InfoSpeed() == 1)
            {
                Console.WriteLine("\n Has caido muchas veces en trampas de velocidad, ya no puedes moverte");
                return;
            }
            piece.SlowDown(-1);
            Console.WriteLine("\n Has caido en una trampa y has perdido 2 puntos de velocidad");
        }
        else if(Maze._maze[newX, newY] == -4)
        {
            if(piece.InfoDamage() <= 0)
            {
                Console.WriteLine("\n Has caido muchas veces en trampas de daño, estas muy debil para inflingir daño");
                return;
            }
            piece.RemoveDamage(-30);
            Console.WriteLine("\n Has caido en una trampa y has perdido 30 puntos de daño, ahora haces menos daño");
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Metodo para Selecionar Fichas         ////////////////////////////////////////////////////////////////////////////////////////
    
    // Metodo para seleccionar ficha
    public Tokens SelectToken(int _index) 
    {
        return _token[_index];    
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de Informacion           ////////////////////////////////////////////////////////////////////////////////////////

    public int InfoIndexFaction()
    {
        return _index;
    }

        // Metodo de Informacionde la faccion buena
    public void InfoGoodFaction()
    {
        Console.WriteLine("\n Informacion de la faccion:");
        Console.WriteLine("\n ELEGIDOS POR LAS 3 GRANDES ESCUELAS DE MAGIA:");

        // Expresa la informacion en forma de tabla
        string nameRowName = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Harry Potter", "Cedric Diggory", "Fleur Delacour", "Viktor Krum");   // Nombres
        string infoRowCharacter = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "HP", "CD", "FD", "VK");                                         // Caracteres
        string infoRowSkill = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Velocidad", "Velocidad", "Velocidad", "Velocidad");                 // Habilidad
        string infoRowColdTime = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                              // Tiempo de Enfriamiento
        string infoRowSpeed = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                                 // Velocidad

        Console.WriteLine("\n ***///NOMBRES///*** \n" + nameRowName);
        Console.WriteLine("\n ***///CARACTERES///*** \n" + infoRowCharacter);
        Console.WriteLine("\n ***///HABILIDADES///*** \n" + infoRowSkill);
        Console.WriteLine("\n ***///TIEMPO DE ENFRIAMIENTO DE LAS HABILIDADES///*** \n" + infoRowColdTime);
        Console.WriteLine("\n ***///VELOCIDAD///*** \n" + infoRowSpeed);
    }

    // Metodo de Informacionde la faccion buena
    public void InfoBadFaction()
    {
        Console.WriteLine("\n Informacion de la faccion:");
        Console.WriteLine("\n MONSTRUOS QUE VAGAN POR EL LABERINTO:");

        // Expresa la informacion en forma de tabla
        string nameRowName = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Acromántula", "Esfinge", "Boggart", "Blast-Ended Skrewts");  // Nombres
        string infoRowCharacter = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Ac", "Es", "Bo", "Bl");                                 // Caracteres
        string infoRowSkill = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Velocidad", "Velocidad", "Velocidad", "Velocidad");                                    // Habilidad
        string infoRowColdTime = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                          // Tiempo de Enfriamiento
        string infoRowSpeed = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                             // Velocidad

        Console.WriteLine("\n NOMBRES \n" + nameRowName);
        Console.WriteLine("\n CARACTERES \n" + infoRowCharacter);
        Console.WriteLine("\n HABILIDADES \n" + infoRowSkill);
        Console.WriteLine("\n TIEMPO DE ENFRIAMIENTO DE LAS HABILIDADES \n" + infoRowColdTime);
        Console.WriteLine("\n VELOCIDAD \n" + infoRowSpeed);
    }

    // Metodo para acceder a las fichas sin modificarlas
    public Tokens InfoTokens(int _index)
    {
        return _token[_index];
    }

    // Metodo de sobrecarga para acceder a las fichas sin modificarlas
    public Tokens[] InfoTokens()
    {
        return _token;
    }
    
    public Tokens InfoPiece(int _index)
    {
        return _token[_index];
    }

    // Informacion del turno del jugador
    public bool InfoTurn()
    {
        return _myTurn;
    }

    // Informacion del nombre del jugador
    public string InfoName()
    {
        return _name;
    }

    public string InfoFaction()
    {
        return _faction;
    }

    public string InfoTarget()
    {
        return _InfoTarget;
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

}

/*
    // Metodo de Caminar en el turno*****
    public static void Run(ref bool run, Maze _maze, Players player1, Players player2,ref bool getTarget)
    {
        while(run)
        {
            player1.PlayersTurn(_maze, player1, player2, ref run, ref getTarget);
            
            player2.PlayersTurn(_maze, player2, player1, ref run, ref getTarget);

            if(_maze.Win(player1.SelectToken(0)._coordX, player1.SelectToken(0)._coordY, getTarget))
                run = false;
        }
    }	
    */