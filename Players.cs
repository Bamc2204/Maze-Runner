using System;

class Players
{
    #region Propiedades del Jugador         ////////////////////////////////////////////////////////////////////////////////////////
    
    private string _name;               // Nombre del jugador
    private bool _myTurn = false;       // Turno de la ficha
    public Tokens[] _tokens;             // Cantidad de fichas (hasta 4)
    private int _index = 0;             // Indice para escoger la faccion
    private string _faction;            // Faccion del jugador
    private string _InfoTarget;         // Objetivo del jugador

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Constructor del Jugador         ////////////////////////////////////////////////////////////////////////////////////////

    // Constructor del Jugador
    public Players(string name, int indexFaction)
    {
        _name = name;                                   //Nombre del jugador
        _myTurn = false;                                //Todos los turnos empiezan falso
        _tokens = new Tokens[4];                        //Cantidad de fichas
        _index = indexFaction;                          //Escoger faccion
        _faction = ((Faction)_index).ToString();        //Escribir el nombre de la faccion
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

    #region Metodos para Crear/Eliminar de Fichas         ////////////////////////////////////////////////////////////////////////////////////////

    // Metodo para crear las fichas buenas*******************************************************************************************************************
    public void CreateTokensGoodPlayer(ref Players player)
    {
        player._tokens[0] = new Tokens("Harry Potter", 1, "⚡", 1, 0, "Velocidad", 4, 10000, 0, 0, 0, 50, 4, 100);

        player._tokens[1] = new Tokens("Cedric Diggory", 2, "🦡", 13, 0, "Velocidad", 4, 4, 0, 0, 0);

        player._tokens[2] = new Tokens("Fleur Delacour", 3, "🌸", 25, 0, "Velocidad", 4, 8, 0, 0, 0);

        player._tokens[3] = new Tokens("Viktor Krum", 4, "💪", 37, 0, "Velocidad", 4, 8, 0, 0, 0);

        string infoTarget = "Obtener la COPA y escapar del laberinto";

        player.SetTarget(ref infoTarget);
    }

    // Metodo para crear las fichas malas********************************************************************************************************************
    private void CreateTokensBadPlayer(ref Players player)
    {
        player._tokens[0] = new Tokens("Acromántula", 5, "🕷️", 7, 43, "Velocidad", 4, 6, 80, 1, 300);

        player._tokens[1] = new Tokens("Esfinge", 6, "🦁", 19, 43, "Velocidad", 4, 4, 90, 1, 300);

        player._tokens[2] = new Tokens("Boggart", 7, "👻", 31, 43, "Velocidad", 4, 4, 50, 3, 300);

        player._tokens[3] = new Tokens("Blast-Ended Skrewts", 8, "🦂", 43, 43, "Velocidad", 4, 8, 60, 5, 300);

        string infoTarget = "Asesinar a los 4 elegidos y evitar que escapen del laberinto";

        player.SetTarget(ref infoTarget);
    }

    // Metodo para eliminar la ficha de la posicion i del array de fichas del jugador
    public void DeleteToken(ref Players player, int index)
    {
        Tokens[] token = new Tokens [player._tokens.Length - 1];
        
        for(int i = 0; i < token.Length; i++)
        {
            if(i < index)
                token[i] = player._tokens[i];
            else
                token[i] = player._tokens[i + 1];
        }

        player._tokens = token;
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
        for(int i = 0; i < player._tokens.Length; i++)
            if(!((player._tokens[i]._target)))
                return true;
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

    // Turno del jugador
    public static void PlayersTurn(Maze maze, Players player1, Players player2, ref bool running)
    {
        int indexPiece = 0;

        int countTurns = 0;

        Console.WriteLine("LA FACCION DE LOS MAGOS COMIENZA 1RO");

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

            ReadBoard(key, ref indexPiece, ref running);


            // Se inicializan los turnos
            if(player1.InfoTurn() == false)
            {
                Console.WriteLine("\n Has escogido la ficha a: " + player1._tokens[indexPiece].InfoName());  
                player1.StartTurn();
                player2.EndTurn();
            } 
            else
            {
                Console.WriteLine("\n Has escogido la ficha a: " + player2._tokens[indexPiece].InfoName());
                player2.StartTurn();
                player1.EndTurn();    
            }

            Console.WriteLine("\n Ya puede desplazarse");

            // Se verifica q el objetivo no se haya cumplido aun para poder deplazarse
            if(!((player1._tokens[indexPiece]._target) && (player2._tokens[indexPiece]._target)))
            {
                // Se verifica de quien es el turno
                if(player1.InfoTurn())
                {
                    _displacement(player1._tokens[indexPiece].InfoSpeed(), maze, ref player1._tokens[indexPiece], player1, player2, ref running, ref player1._tokens[indexPiece]._target);
                }
                else if(player2.InfoTurn())
                {
                    _displacement(player2._tokens[indexPiece].InfoSpeed(), maze, ref player2._tokens[indexPiece], player2, player1, ref running, ref player2._tokens[indexPiece]._target);
                    countTurns++;
                }

                for(int i = 0; i < player1._tokens.Length; i++)
                    if((countTurns % 8 == 0 && player2.InfoTurn()) || Maze.GeneratedDoors(maze.CheckCup(player1._tokens[i])))
                        maze.GenerateNewMaze(maze.CheckCup(player1._tokens[i]));
            }
            else
            {
                running = false;
            }
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodo de Desplazamiento           ////////////////////////////////////////////////////////////////////////////////////////

    // Desplaza la ficha
    private static void _displacement(int steps, Maze maze, ref Tokens piece1, Players player1, Players player2, ref bool running,ref bool getTarget)
    {   
        Maze._maze[piece1.CoordX, piece1.CoordY] = piece1.InfoId();                   // Pone la ficha en el tablero
        int newX = piece1.CoordX;
        int newY = piece1.CoordY;

        // Verifica los pasos y si el juego aun corre
        while(steps != 0 && running)
        {   
            // Tecla q toca el jugador en el teclado            
            ConsoleKey key = Console.ReadKey().Key;

            // Lee el teclado
            ReadBoard(key, ref newX, ref newY, ref running, ref piece1, player1, player2);

            if(key == ConsoleKey.Escape)                                            // Termina la simulacion
            {
                running = false;
                return;
            }

            if(!(newX >= 0 && newX < Maze._maze.GetLength(0) && newY >= 0 && newY < Maze._maze.GetLength(1)))
            {
                Console.Clear();
                GamePlay.Pause("\n NO SE PUEDE AVANZAR POR QUE NO HAY CAMINO \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nPresiona cualquier tecla para comenzar el juego...");
                Maze.PrintMaze(player1,player2);
                continue;
            }

            // Verifica la victoria
            maze.Win(player1, player2, maze, newX, newY, ref running);
            if(!running)
                return;
            
            // Verifica si hay algun jugador en la proxima posicion
            bool isPiece = 
            Maze._maze[newX, newY] == 1 || Maze._maze[newX, newY] == 2 ||
            Maze._maze[newX, newY] == 3 || Maze._maze[newX, newY] == 4 ||
            Maze._maze[newX, newY] == 5 || Maze._maze[newX, newY] == 6 ||
            Maze._maze[newX, newY] == 7 || Maze._maze[newX, newY] == 8;

            // Verifica si la copa esta en la proxima posicion;
            bool isCup = Maze._maze[newX, newY] == -6;

            // Si toca una tecla q no sea para moverse
            if(key == ConsoleKey.I || key == ConsoleKey.E || key == ConsoleKey.Tab || key == ConsoleKey.Q || key == ConsoleKey.G) 
                continue;

            // Dentro de filas, columnas y si es un camino*********************************************
            if (Maze._maze[newX, newY] != -1 && !isPiece && !isCup )                    
            {
                //Verifica si hay trampa y en caso de q si aplica la funcion de la trampa
                _checkTrap(maze, ref newX, ref newY, ref running, ref piece1, ref player1);
                
                // Actualiza el tablero
                Maze._maze[piece1.CoordX, piece1.CoordY] = 0;                 // Vacía la posición actual
                Maze._maze[newX, newY] = piece1.InfoId();                       // Mueve la ficha
                piece1.CoordX = newX;                                          // Actualiza las coordenadas actuales
                piece1.CoordY = newY;
                steps --;                                                       // Pasos q puede recorer cada ficha
            }

            else
            {
                Console.Clear();

                Console.WriteLine("\n LOS PASOS NO SON VALIDOS, INTRODUZCA OTRA DIRECCION\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                newX = piece1.CoordX; newY = piece1.CoordY;   
                    
                GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR");            
            }

            Maze.PrintMaze(player1, player2);                                    // Imprime el laberinto
        } 
        GamePlay.Pause("\n PRESIONE UNA TECLA PARA CAMBIAR TURNO"); 
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Metodos de Lectura del Teclado          ////////////////////////////////////////////////////////////////////////////////////////

    // Lee el teclado
    public static void ReadBoard(ConsoleKey key, ref int newX, ref int newY, ref bool running, ref Tokens piece, Players player1, Players player2) 
    {
        //Casos para cada tecla
        switch (key)
        {
            case ConsoleKey.UpArrow:    newX = piece.CoordX - 1; 
                break;

            case ConsoleKey.DownArrow:  newX = piece.CoordX + 1; 
                break;

            case ConsoleKey.LeftArrow:  newY = piece.CoordY - 1; 
                break;

            case ConsoleKey.RightArrow: newY = piece.CoordY + 1; 
                break;

            case ConsoleKey.Tab: piece._useBoxObject( ref newX, ref newY, ref piece, player1, player2); 
                break;

            case ConsoleKey.I: Console.Clear(); Console.WriteLine("\n " + player1.InfoFaction() + "\n" + " \n ***///OBETIVO///*** \n \n " + player1.InfoTarget()); piece.DisplayStatus(); GamePlay.Pause("\nPRESIONE UNA TECLA PARA VOLVER AL LABERINTO");
                Maze.PrintMaze(player1,player2);
                break;

            case ConsoleKey.E: Console.Clear(); if(player1.InfoIndexFaction() == 1) player1.InfoGoodFaction(); else player1.InfoBadFaction(); Console.WriteLine("\n EL OBJETIVO DE LA FACCION: " + player1.InfoTarget()); GamePlay.Pause("PRESIONE UNA TECLA PARA VOLVER AL LABERINTO");
                Maze.PrintMaze(player1,player2);
                break;

            case ConsoleKey.Q: Console.WriteLine("\nLA FICHA VA A ATACAR"); piece.Attack(piece, ref player2, piece.InfoDamage()); 
                break;

            case ConsoleKey.G: Console.WriteLine("\nLA FICHA VA A INTENTAR COGER UN OBJETO"); piece._collect(piece); GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR"); Console.Clear(); Maze.PrintMaze(player1,player2); 
                break;
        }
    }

    // Sobrecarga para leer el teclado
    public static void ReadBoard(ConsoleKey key, ref int _index, ref bool running)
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
    public static void ReadBoard(ConsoleKey key,ref int _index)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow: Console.WriteLine("\n Has escogido la faccion de los Magos"); _index = 1; break;
            case ConsoleKey.RightArrow:  Console.WriteLine("\n Has escogido la faccion de los Monstruos"); _index = 2; break;
        }
    }

    // 3ra Sobrecarga para leer el teclado
    public static void ReadBoard(ConsoleKey key, Tokens token, ref Players player, int damage)
    {
        if(player.InfoIndexFaction() == 1)
        {  
            switch(key)
            {
                case ConsoleKey.UpArrow: // Arriba
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < player._tokens.Length; j++)
                    {
                        if(Maze._maze[(token.CoordX + i), token.CoordY] == player._tokens[j].InfoId() && Maze.CheckWall(1, 4, token))
                        {
                            player._tokens[j].RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player._tokens[j].InfoName());
                            player.EndTurn();
                            break;
                        }
                        
                    }
                }
                Console.WriteLine("No se infligió daño en ninguna ficha ya q habia una pared de por medio o directamente no habia nada a lo q se pudiera atacar");
                break;

                case ConsoleKey.DownArrow: // Abajo
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < player._tokens.Length; j++)
                    {
                        if(Maze._maze[(token.CoordX - i), token.CoordY] == player._tokens[j].InfoId() && Maze.CheckWall(2, 4, token))
                        {
                            player._tokens[j].RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player._tokens[j].InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                Console.WriteLine("No se infligió daño en ninguna ficha ya q habia una pared de por medio o directamente no habia nada a lo q se pudiera atacar");
                break;

                case ConsoleKey.LeftArrow: // Izquierda
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < player._tokens.Length; j++)
                    {
                        if(Maze._maze[token.CoordX , (token.CoordY - 1)] == player._tokens[j].InfoId() && Maze.CheckWall(3, 4, token))
                        {
                            player._tokens[j].RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player._tokens[j].InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                Console.WriteLine("No se infligió daño en ninguna ficha ya q habia una pared de por medio o directamente no habia nada a lo q se pudiera atacar");
                break;

                case ConsoleKey.RightArrow: // Derecha
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < player._tokens.Length; j++)
                    {
                        if(Maze._maze[token.CoordX , (token.CoordY + 1)] == player._tokens[j].InfoId() && Maze.CheckWall(4, 4, token))
                        {
                            player._tokens[j].RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player._tokens[j].InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                Console.WriteLine("No se infligió daño en ninguna ficha ya q habia una pared de por medio o directamente no habia nada a lo q se pudiera atacar");
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
                    for(int j = 0; j < player._tokens.Length; j++)
                    {
                        if(Maze._maze[(token.CoordX + i), token.CoordY] == player._tokens[j].InfoId() && Maze.CheckWall(1, token.InfoDistAttack(), token))
                        {
                            player._tokens[j].RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player._tokens[j].InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                Console.WriteLine("No se infligió daño en ninguna ficha ya q habia una pared de por medio o directamente no habia nada a lo q se pudiera atacar");
                break;

                case ConsoleKey.DownArrow:
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < player._tokens.Length; j++)
                    {
                        if(Maze._maze[(token.CoordX - i), token.CoordY] == player._tokens[j].InfoId() && Maze.CheckWall(2, token.InfoDistAttack(), token))
                        {
                            player._tokens[j].RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player._tokens[j].InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                Console.WriteLine("No se infligió daño en ninguna ficha ya q habia una pared de por medio o directamente no habia nada a lo q se pudiera atacar");
                break;

                case ConsoleKey.LeftArrow:
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < player._tokens.Length; j++)
                    {
                        if(Maze._maze[token.CoordX , (token.CoordY - 1)] == player._tokens[j].InfoId() && Maze.CheckWall(3, token.InfoDistAttack(), token))
                        {
                            player._tokens[j].RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player._tokens[j].InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                Console.WriteLine("No se infligió daño en ninguna ficha ya q habia una pared de por medio o directamente no habia nada a lo q se pudiera atacar");
                break;

                case ConsoleKey.RightArrow:
                for(int i = 1; i < token.InfoDistAttack(); i++)
                {
                    for(int j = 0; j < player._tokens.Length; j++)
                    {
                        if(Maze._maze[token.CoordX , (token.CoordY + 1)] == player._tokens[j].InfoId() && Maze.CheckWall(4, token.InfoDistAttack(), token))
                        {
                            player._tokens[j].RemoveHealth(damage);
                            Console.WriteLine("Le has quitado " + damage + " puntos de vida a " + " " + player._tokens[j].InfoName());
                            player.EndTurn();
                            break;
                        }
                    }
                }
                Console.WriteLine("No se infligió daño en ninguna ficha ya q habia una pared de por medio o directamente no habia nada a lo q se pudiera atacar");
                break;
            }
        }
    }

    // 4ta Sobrecarga para leer el teclado
    public static void ReadBoard(ConsoleKey key, ref int newX, ref int newY)
    {
        switch(key)
        {
            case ConsoleKey.UpArrow: newX -= 5; break;

            case ConsoleKey.DownArrow: newX += 5; break;

            case ConsoleKey.LeftArrow: newY -= 5; break;

            case ConsoleKey.RightArrow: newY += 5;break;
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
     
    #region Metodo para Verificar las trampas           ////////////////////////////////////////////////////////////////////////////////////////
    
    // Chequea si caiste en una trampa
    public static void _checkTrap(Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens token, ref Players player)
    {
        if(Maze._maze[newX, newY] == -2) //Verificacion de trapas
        {
            token.RemoveHealth(20, token._activeShield);
            Console.WriteLine("\n Has caido en una trampa y has perdido 20 puntos de vidas");
            if(token.InfoHealth() <= 0)
            { 
                Console.WriteLine($"\n {token.InfoName()} HA MUERTO");
                GamePlay.Pause("PRESIONE UAN TECLA PARA CONTINUAR");
                for(int i = 0; i < player._tokens.Length; i++)
                    if(player._tokens[i] == token)
                        player.DeleteToken(ref player, i);
            }
        }
        else if(Maze._maze[newX, newY] == -3)
        {
            if(token.InfoSpeed() == 2)
            {
                return;
            }
            token.SlowDown(2);
            Console.WriteLine("\n Has caido en una trampa de reduccion de velocidad y has perdido 2 puntos de velocidad");
        }
        else if(Maze._maze[newX, newY] == -4)
        {
            if(token.InfoDamage() <= 0)
            {
                Console.WriteLine("\n Has caido muchas veces en trampas de debilitacion de furza, ya tus ataques no hacen daño");
                return;
            }
            token.RemoveDamage(35);
            Console.WriteLine("\n Has caido en una trampa y has perdido 30 puntos de daño, ahora haces menos daño");
        }
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
        Console.WriteLine("\n ********////////INFORMACION DE LA FACCION////////******** \n");
        Console.WriteLine("\n MAGOS \n");

        // Expresa la informacion en forma de tabla
        string nameRowName = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Harry Potter", "Cedric Diggory", "Fleur Delacour", "Viktor Krum");   // Nombres
        string infoRowCharacter = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "HP", "CD", "FD", "VK");                                         // Caracteres
        string infoRowSkill = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Velocidad", "Velocidad", "Velocidad", "Velocidad");                 // Habilidad
        string infoRowColdTime = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                              // Tiempo de Enfriamiento
        string infoRowSpeed = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                                 // Velocidad

        Console.WriteLine("\n ***///NOMBRES///*** \n \n" + nameRowName);
        Console.WriteLine("\n ***///CARACTERES///*** \n \n" + infoRowCharacter);
        Console.WriteLine("\n ***///HABILIDADES///*** \n \n" + infoRowSkill);
        Console.WriteLine("\n ***///TIEMPO DE ENFRIAMIENTO DE LAS HABILIDADES///*** \n \n" + infoRowColdTime);
        Console.WriteLine("\n ***///VELOCIDAD///*** \n \n" + infoRowSpeed);
    }

    // Metodo de Informacionde la faccion buena
    public void InfoBadFaction()
    {
        Console.WriteLine("\n ********////////INFORMACION DE LA FACCION////////******** \n");
        Console.WriteLine("\n MONSTRUOS \n");

        // Expresa la informacion en forma de tabla
        string nameRowName = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Acromántula", "Esfinge", "Boggart", "Blast-Ended Skrewts");  // Nombres
        string infoRowCharacter = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Ac", "Es", "Bo", "Bl");                                 // Caracteres
        string infoRowSkill = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "Velocidad", "Velocidad", "Velocidad", "Velocidad");                                    // Habilidad
        string infoRowColdTime = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                          // Tiempo de Enfriamiento
        string infoRowSpeed = string.Format("{0,-20} {1,-20} {2,-20} {3,-20}", "4", "4", "4", "4");                                             // Velocidad

        Console.WriteLine("\n NOMBRES \n \n" + nameRowName);
        Console.WriteLine("\n CARACTERES \n \n" + infoRowCharacter);
        Console.WriteLine("\n HABILIDADES \n \n" + infoRowSkill);
        Console.WriteLine("\n TIEMPO DE ENFRIAMIENTO DE LAS HABILIDADES \n \n" + infoRowColdTime);
        Console.WriteLine("\n VELOCIDAD \n \n" + infoRowSpeed);
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
