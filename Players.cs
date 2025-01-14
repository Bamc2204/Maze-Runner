using System;

class Players
{
    #region Propiedades del Jugador         ////////////////////////////////////////////////////////////////////////////////////////
    
    private string _name;               // Nombre del jugador
    private bool _myTurn = false;       // Turno de la ficha
    public Tokens[] Tokens;             // Cantidad de fichas (hasta 4)
    private int _index = 0;             // Indice para escoger la faccion
    private string _faction;            // Faccion del jugador
    private string _InfoTarget;         // Objetivo del jugador
    public static int countTurns = 0;   // Ciclos de turnos

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Constructor del Jugador         ////////////////////////////////////////////////////////////////////////////////////////

    // Constructor del Jugador
    public Players(string name, int indexFaction)
    {
        _name = name;                                   //Nombre del jugador
        _myTurn = false;                                //Todos los turnos empiezan falso
        Tokens = new Tokens[4];                        //Cantidad de fichas
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
    public void ChooseFaction(ref int indexPlayer1, ref int indexPlayer2)
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

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n El jugador " + player1.InfoName() + " ha escogido la faccion " + player1.InfoFaction());
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n Y el jugador " + player2.InfoName() + " ha escogido la faccion " + player2.InfoFaction());
            Console.ResetColor();
        }
        else
        {
            CreateTokensGoodPlayer(ref player2);
            CreateTokensBadPlayer(ref player1);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n El jugador " + player1.InfoName() + " ha escogido la faccion " + player1.InfoFaction());
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n Y el jugador " + player2.InfoName() + " ha escogido la faccion " + player2.InfoFaction()); 
            Console.ResetColor();
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos para Crear/Eliminar de Fichas         ////////////////////////////////////////////////////////////////////////////////////////

    // Metodo para crear las fichas buenas
    public void CreateTokensGoodPlayer(ref Players player)
    {
        player.Tokens[0] = new Tokens("Harry Potter", 1, "⚡", 1, 0, "Expellarmus", 6, 6, -7, -8, -10, 20, 4, 150);

        player.Tokens[1] = new Tokens("Cedric Diggory", 2, "🦡", 13, 0, "Conjuro Elemental", 4, 8, -7, -7, -9, 20, 4, 150);

        player.Tokens[2] = new Tokens("Fleur Delacour", 3, "🌸", 25, 0, "Sanacion Magica", 4, 10, -9, -9, -9, 15, 8);

        player.Tokens[3] = new Tokens("Viktor Krum", 4, "💪", 37, 0, "Draconifors", 4, 6, -8, -8, -9, 30, 1, 250);

        string infoTarget = "Obtener la COPA y escapar del laberinto";

        player.SetTarget(ref infoTarget);
    }

    // Metodo para crear las fichas malas
    private void CreateTokensBadPlayer(ref Players player)
    {
        player.Tokens[0] = new Tokens("Acromántula", 5, "🕷️", 7, 43, "Veneno", 3, 8, 15, 3, 300);

        player.Tokens[1] = new Tokens("Esfinge", 6, "🦁", 19, 43, "Aumento de Fuerza", 6, 6, 25, 2, 320);

        player.Tokens[2] = new Tokens("Boggart", 7, "👻", 31, 43, "Copiar", 4, 12, 0, 0, 40);

        player.Tokens[3] = new Tokens("Blast-Ended Skrewts", 8, "🦂", 43, 43, "Lazar Fuego", 6, 10, 20, 5, 150);

        string infoTarget = "Asesinar a los 4 elegidos y evitar que escapen del laberinto";

        player.SetTarget(ref infoTarget);
    }

    // Metodo para eliminar la ficha de la posicion i del array de fichas del jugador
    public void DeleteToken(ref Players player, int index)
    {
        Tokens[] token = new Tokens [player.Tokens.Length - 1];
        
        for(int i = 0; i < token.Length; i++)
        {
            if(i < index)
                token[i] = player.Tokens[i];
            else
                token[i] = player.Tokens[i + 1];
        }

        Maze.GeneralMaze[player.Tokens[index].CoordX,player.Tokens[index].CoordY] = 0;

        player.Tokens = token;
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Objetivo         ////////////////////////////////////////////////////////////////////////////////////////
    
    // Metodo para asignar el objetivo***********************************************************************************************************************
    public void SetTarget(ref string infoTarget)
    {
        _InfoTarget = infoTarget;
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de Habilidades de las Fichas           ////////////////////////////////////////////////////////////////////////////////////////

    // Metodo para usar la Habilidad de las Fichas ***********************************************************************
    private static void _usedSkill(Tokens token, ref Players player1, ref Players player2)
    {
        if(token.ColdTime != 0)
        {
            GamePlay.Pause("\nAUN NO SE A RESTABLECIDO LA HABILIDAD \n\n\n\n\n\n\n\n\n\n\n\nPRESIONE UNA TECLA PLARA CONTINUAR...");
            return;
        }

        if(token.InfoId() == 1)
        {
            _expellarmus(token, ref player2);
            token.ColdTime = 5;
            return;
        }
        if(token.InfoId() == 2)
        {
            _elementalConjuration(token, ref player2);
            token.ColdTime = 3;
            return;
        }

        if(token.InfoId() == 3)
        {
            _draconifors(token, ref player2);
            token.ColdTime = 3;
            return;
        }

        if(token.InfoId() == 4)
        {
            _magicalHealing(token, ref player1);
            token.ColdTime = 2;
            return;
        }

        if(token.InfoId() == 5)
        {
            _poison(token, ref player2);
            token.ColdTime = 4;
            return;
        }

        if(token.InfoId() == 6)
        {
            _strengthIncrease(token, ref player2);
            token.ColdTime = 5;
            return;
        }

        if(token.InfoId() == 7)
        {
            _copy(token, player1);
            token.ColdTime = 10;
            return;
        }

        if(token.InfoId() == 8)
        {
            _throwFire(token, ref player2);
            token.ColdTime = 5;
            return;
        }

    }
    
    // Habilidades de MAGOS
    
    // Metodo para paralizar un ficha**************************************** no encuentro la manera de paralizar
    private static void _expellarmus(Tokens token, ref Players player2)
    {
        for(int i = 1; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                for (int k = 0; k < player2.Tokens.Length; k++)
                {
                    bool checkArea = 
                    (player2.Tokens[k].CoordX == token.CoordX + i && player2.Tokens[k].CoordY == token.CoordY + j) || 
                    (player2.Tokens[k].CoordX == token.CoordX + i && player2.Tokens[k].CoordY == token.CoordY - j) ||
                    (player2.Tokens[k].CoordX == token.CoordX - i && player2.Tokens[k].CoordY == token.CoordY + j) ||
                    (player2.Tokens[k].CoordX == token.CoordX - i && player2.Tokens[k].CoordY == token.CoordY - j);

                    if(checkArea)
                    {
                        player2.Tokens[k].SlowDown(player2.Tokens[k].InfoSpeed());
                        Console.WriteLine($"Se ha paralizado a {player2.Tokens[k].InfoName()}");
                        GamePlay.Pause("\n\n\n\n\n\nPresione una tecla para continuar...");
                        return;
                    }
                }
            }
        }
    }

    // Lazar un ataque Elemental en Area distancia de 4 casillas y radio de 2
    private static void _elementalConjuration(Tokens tokens, ref Players player2)
    {
        Console.WriteLine("En que direccion va a aplicar el efeccto");
        ConsoleKey key = Console.ReadKey().Key;
        ReadBoard(key, tokens, ref player2, 20, 4, 2);
    }

    // Aumenta Daño Cuerpo a cuerpo
    private static void _draconifors(Tokens tokens, ref Players player2)
    {
        Console.WriteLine("En que direccion va a aplicar el efeccto");
        ConsoleKey key = Console.ReadKey().Key;
        ReadBoard(key, tokens, ref player2, 40, 1);
    }

    // Sana a la persona con menos vida en un radio de 2 casillas alrededor de la ficha
    private static void _magicalHealing(Tokens token, ref Players player1)
    {
        int menor = int.MaxValue;
        
        int newX = token.CoordX;
        int newY = token.CoordY;

        int upX = 3;             //X---
        int downX = 3;           //X+++
        int leftY = 3;           //Y---
        int rightY = 3;          //Y+++

        while(true)
        {
            if(newX - upX <= 0)
                upX--;
            else if( newX + downX >= Maze.GeneralMaze.GetLength(0))
                downX--;
            else if(newY - leftY <= 0)
                leftY--;
            else if(newY + rightY >= Maze.GeneralMaze.GetLength(1))
                rightY--;
            else
                break;
        }

        // Verifica si hay un enemigo en la nueva posición
        for (int i = 0; i <= upX; i++)
        {
            for (int j = 0; j <= downX; j++)
            {
                for (int k = 0; k < leftY; k++)
                {
                    for (int l = 0; l < rightY; l++)
                    {
                        for (int m = 0; m < player1.Tokens.Length; m++)
                        {    
                            // Verifica si hay algun enemigo en esa area
                            bool checkArea = 
                            (player1.Tokens[m].CoordX == newX + downX && player1.Tokens[m].CoordY == newY + rightY) || 
                            (player1.Tokens[m].CoordX == newX + downX && player1.Tokens[m].CoordY == newY - leftY) ||
                            (player1.Tokens[m].CoordX == newX - upX && player1.Tokens[m].CoordY == newY + rightY) ||
                            (player1.Tokens[m].CoordX == newX - upX && player1.Tokens[m].CoordY == newY - leftY);

                            if(checkArea && player1.Tokens[m].InfoHealth() < menor)
                            {
                                menor = player1.Tokens[m].InfoHealth();
                            }
                        }
                    }
                }
            }
        }
       
        for (int i = 0; i < player1.Tokens.Length; i++)
        {
            if(menor == player1.Tokens[i].InfoHealth())
            {
                player1.Tokens[i].AddHealth(20);
                Console.WriteLine($"Se ha curado a {player1.Tokens[i].InfoName()}");
                GamePlay.Pause("\n\n\n\n\n\nPresione una tecla para continuar...");
                return;
            }
        }

    }
    
    // Habilidades de MONSTRUOS

    // Envenamiento ***************************************************************
    private static void _poison(Tokens tokens, ref Players player2)
    {
        
    }

    // Aumenta Daño cuerpo a cuerpo, a una distancia de 1 casillas    
    private static void _strengthIncrease(Tokens tokens, ref Players player2)
    {
        Console.WriteLine("En que direccion va a aplicar el efeccto");
        ConsoleKey key = Console.ReadKey().Key;
        ReadBoard(key, tokens, ref player2, 50, 1);
    }

    // Copiar otros monstruos
    private static void _copy(Tokens token, Players player1)
    {
        int index = -1;
        Console.WriteLine("En que Monstruo te quieres transformar(1-4)");
        ConsoleKey key = Console.ReadKey().Key;
        
        if(key != ConsoleKey.NumPad1 && key != ConsoleKey.NumPad2 && key != ConsoleKey.NumPad3 && key != ConsoleKey.NumPad4)
        {
            Console.WriteLine("\nNo escogio convertirse en algo"); 
            GamePlay.Pause("\n\n\n\n\n\nPresione una tecla para continuar...");
            return; 
        }
           
        ReadBoard(key, ref index);  

        if(index > player1.Tokens.Length)
        {
            Console.WriteLine("Se han eliminado fichas por lo q esa posicion ya no existe");
            GamePlay.Pause("Presione una tecla para continuar...");
            return;
        }

        int id = player1.Tokens[index].InfoId();

        switch(id)
        {
            case 5: 
                token.ModifyCharacter("🕷️");
                token.ModifiHealth(300 );
                token.ModifySpeed(8);
                token.ModifiDamage(15);
                token.ModifiDistAttack(3);
                break;

            case 6: 
                token.ModifyCharacter("🦁");
                token.ModifiHealth(320);
                token.ModifySpeed(6);
                token.ModifiDamage(25);
                token.ModifiDistAttack(2);
                break;

            case 7: 
                token.ModifyCharacter("👻");
                token.ModifiHealth(40);
                token.ModifySpeed(12);
                token.ModifiDamage(0);
                token.ModifiDistAttack(0);
                break;

            case 8: 
                token.ModifyCharacter("🦂");
                token.ModifiHealth(150);
                token.ModifySpeed(10);
                token.ModifiDamage(20);
                token.ModifiDistAttack(5);
                break;
        }
    }
    
    // Lazar fuego en Area distancia de 5 casillas y radio de 1
    public static void _throwFire(Tokens tokens, ref Players player2)
    {
        Console.WriteLine("En que direccion va a aplicar el efeccto");
        ConsoleKey key = Console.ReadKey().Key;
        ReadBoard(key, tokens, ref player2, 35, 5, 1);
    }

    #endregion              ////////////////////////////////////////////////////////////////////////////////////////

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

        bool CanRunning = true;

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

            ReadBoard(key, ref indexPiece);

            // Se inicializan los turnos
            if(player1.InfoTurn() == false && CanRunning == true)
            {
                Console.WriteLine("\n Has escogido la ficha a: " + player1.Tokens[indexPiece].InfoName());  
                player1.StartTurn();
                player2.EndTurn();
            } 
            else if(CanRunning == true)
            {
                Console.WriteLine("\n Has escogido la ficha a: " + player2.Tokens[indexPiece].InfoName());
                player2.StartTurn();
                player1.EndTurn();    
            }

                        

            // Se verifica de quien es el turno
            if(player1.InfoTurn())
            {
                if(indexPiece > player1.Tokens.Length)
                {
                    Console.WriteLine("Hay una fichas eliminadas por lo que esa posicion ya no existe");
                    CanRunning = false;
                    continue;
                }
                _displacement(player1.Tokens[indexPiece].InfoSpeed(), maze, ref player1.Tokens[indexPiece], player1, player2, ref running);
                CanRunning = true;
            }
            else if(player2.InfoTurn())
            {
                if(indexPiece > player2.Tokens.Length)
                {
                    Console.WriteLine("Hay una fichas eliminadas por lo que esa posicion ya no existe");
                    CanRunning = false;
                    continue;
                }
                _displacement(player2.Tokens[indexPiece].InfoSpeed(), maze, ref player2.Tokens[indexPiece], player2, player1, ref running);
                CanRunning = true;
                countTurns++;
            }

            if(countTurns % 8 == 0 && player2.InfoTurn())
            {
                maze.GenerateNewMaze(player1, player2);
                Maze.PrintMaze(player1, player2);
            }    
            
            _minusColdTime(player1);
            _minusColdTime(player2);
        }
    }

    // Metodo para El Tiempo de Enfriamiento
    private static void _minusColdTime(Players player1)
    {
        for (int i = 0; i < player1.Tokens.Length; i++)
        {
            if(player1.Tokens[i].ColdTime == 0)
                continue;
            player1.Tokens[i].ColdTime--;
        }
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodo de Desplazamiento           ////////////////////////////////////////////////////////////////////////////////////////

    // Desplaza la ficha
    private static void _displacement(int steps, Maze maze, ref Tokens token, Players player1, Players player2, ref bool running)
    {   
        Maze.GeneralMaze[token.CoordX, token.CoordY] = token.InfoId();                   // Pone la ficha en el tablero
        int newX = token.CoordX;
        int newY = token.CoordY;

        // Verifica los pasos y si el juego aun corre
        while(steps != 0 && running)
        {   
            Console.WriteLine("\n Ya puede desplazarse");

            // Tecla q toca el jugador en el teclado            
            ConsoleKey key = Console.ReadKey().Key;

            // Lee el teclado
            ReadBoard(key, ref newX, ref newY, ref steps, ref token, player1, player2);

            if(key == ConsoleKey.Escape)                                            // Termina la simulacion
            {
                running = false;
                return;
            }

            if(!(newX >= 0 && newX < Maze.GeneralMaze.GetLength(0) && newY >= 0 && newY < Maze.GeneralMaze.GetLength(1)))
            {
                Console.Clear();
                GamePlay.Pause("\n NO SE PUEDE AVANZAR POR QUE NO HAY CAMINO \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nPresiona cualquier tecla para comenzar el juego...");
                Maze.PrintMaze(player1,player2);
                continue;
            }

            // Verifica la victoria
            maze.Win(token, player1, player2, maze, newX, newY, ref running);
            if(!running)
                return;
            
            // Verifica si hay algun jugador en la proxima posicion
            bool isPiece = 
            Maze.GeneralMaze[newX, newY] == 1 || Maze.GeneralMaze[newX, newY] == 2 ||
            Maze.GeneralMaze[newX, newY] == 3 || Maze.GeneralMaze[newX, newY] == 4 ||
            Maze.GeneralMaze[newX, newY] == 5 || Maze.GeneralMaze[newX, newY] == 6 ||
            Maze.GeneralMaze[newX, newY] == 7 || Maze.GeneralMaze[newX, newY] == 8;

            // Verifica si la copa esta en la proxima posicion;
            bool isCup = Maze.GeneralMaze[newX, newY] == -6;

            // Si toca una tecla q no sea para moverse
            if(key == ConsoleKey.I || key == ConsoleKey.E || key == ConsoleKey.Tab || key == ConsoleKey.Q || key == ConsoleKey.G) 
                continue;

            // Dentro de filas, columnas y si es un camino*********************************************
            if (Maze.GeneralMaze[newX, newY] != -1 && !isPiece && !isCup )                    
            {
                //Verifica si hay trampa y en caso de q si aplica la funcion de la trampa
                CheckTrap(maze, ref newX, ref newY, ref running, ref token, ref player1);
                
                // Actualiza el tablero
                Maze.GeneralMaze[token.CoordX, token.CoordY] = 0;                 // Vacía la posición actual
                Maze.GeneralMaze[newX, newY] = token.InfoId();                       // Mueve la ficha
                token.CoordX = newX;                                          // Actualiza las coordenadas actuales
                token.CoordY = newY;
                steps --;                                                       // Pasos q puede recorer cada ficha
            }

            else
            {
                Console.Clear();

                Console.WriteLine("\n LOS PASOS NO SON VALIDOS, INTRODUZCA OTRA DIRECCION\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                newX = token.CoordX; newY = token.CoordY;   
                    
                GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR");            
            }

            Maze.PrintMaze(player1, player2);                                    // Imprime el laberinto
        } 
        GamePlay.Pause("\n PRESIONE UNA TECLA PARA CAMBIAR TURNO"); 
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
    
    #region Metodos de Lectura del Teclado          ////////////////////////////////////////////////////////////////////////////////////////

    // Lee el teclado para las funciones principales del juego
    public static void ReadBoard(ConsoleKey key, ref int newX, ref int newY,ref int steps, ref Tokens token, Players player1, Players player2) 
    {
        //Casos para cada tecla
        switch (key)
        {
            // Arriba
            case ConsoleKey.UpArrow:    newX = token.CoordX - 1; 
                break;

            // Abajo
            case ConsoleKey.DownArrow:  newX = token.CoordX + 1; 
                break;

            // Izquierda
            case ConsoleKey.LeftArrow:  newY = token.CoordY - 1; 
                break;

            // Derecha
            case ConsoleKey.RightArrow: newY = token.CoordY + 1; 
                break;

            // Bolsa
            case ConsoleKey.Tab: token.UseBoxObject( ref newX, ref newY, ref token, player1, player2); 
                break;

            // Informacion de la Ficha
            case ConsoleKey.I: Console.Clear(); Console.WriteLine("\n " + player1.InfoFaction() + "\n" + " \n ***///OBETIVO///*** \n \n " + player1.InfoTarget()); token.DisplayStatus(); GamePlay.Pause("\nPRESIONE UNA TECLA PARA VOLVER AL LABERINTO");
                Maze.PrintMaze(player1,player2);
                break;

            // Informacion de la Faccion
            case ConsoleKey.J: Console.Clear(); if(player1.InfoIndexFaction() == 1) player1.InfoGoodFaction(); else player1.InfoBadFaction(); Console.WriteLine("\n EL OBJETIVO DE LA FACCION: " + player1.InfoTarget()); GamePlay.Pause("PRESIONE UNA TECLA PARA VOLVER AL LABERINTO");
                Maze.PrintMaze(player1,player2);
                break;

            // Atacar
            case ConsoleKey.E: Console.WriteLine("\nLA FICHA VA A ATACAR"); token.Attack(token, ref steps, ref player2, player1, token.InfoDamage()); 
                break;

            // Obtener Objetos
            case ConsoleKey.Q: Console.WriteLine("\nLA FICHA VA A INTENTAR COGER UN OBJETO"); token.Collect(token); GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR"); Console.Clear(); Maze.PrintMaze(player1,player2); 
                break;

            // Usar Habilidad
            case ConsoleKey.F: Console.WriteLine("\nLA FICHA VA A USAR SU HABILIDAD"); _usedSkill(token, ref player1, ref player2); 
                break;
        }
    }

    // Sobrecarga para leer el teclado para escoger una de las fichas o el Id de las fichas
    public static void ReadBoard(ConsoleKey key, ref int _index)
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
    
    // 2da Sobrecarga para leer el teclado para escoger faccion
    public static void ReadBoard(ref int index, ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow: index = 1; break;
            case ConsoleKey.RightArrow: index = 2; break;
        }
    }

    // 3ra Sobrecarga para leer el teclado para atacar
    public static void ReadBoard(ConsoleKey key, Tokens token, ref int step, ref Players player, int damage, int attackRange)
    {
        int newX = token.CoordX;
        int newY = token.CoordY;

        for (int i = 1; i <= attackRange; i++)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    newX = token.CoordX - i;
                    newY = token.CoordY;
                    break;
                case ConsoleKey.DownArrow:
                    newX = token.CoordX + i;
                    newY = token.CoordY;
                    break;
                case ConsoleKey.LeftArrow:
                    newX = token.CoordX;
                    newY = token.CoordY - i;
                    break;
                case ConsoleKey.RightArrow:
                    newX = token.CoordX;
                    newY = token.CoordY + i;
                    break;
            }

            // Verifica si hay una pared en la nueva posición
            if (Maze.GeneralMaze[newX, newY] == -1)
            {
                Console.WriteLine("El ataque fue bloqueado por una pared.");
                GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR...");
                return;
            }

            // Verifica si hay un enemigo en la nueva posición
            for (int j = 0; j < player.Tokens.Length; j++)
            {
                if (player.Tokens[j].CoordX == newX && player.Tokens[j].CoordY == newY)
                {
                    player.Tokens[j].RemoveHealth(damage);
                    Console.WriteLine($"Le has quitado {damage} puntos de vida a {player.Tokens[j].InfoName()}, Le queda {player.Tokens[j].InfoHealth()}");

                    step = 0;

                    // Verifica si la ficha aún tiene vida
                    if (player.Tokens[j].InfoHealth() <= 0)
                    {
                        Console.WriteLine($"{player.Tokens[j].InfoName()} ha sido eliminado.");
                        player.DeleteToken(ref player, j);
                    }
                    GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR...");
                    Console.Clear();
                    return;
                }
            }
        }

        Console.WriteLine("\nNo se infligió daño en ninguna ficha\n");
        GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR...");
    }
    
    // 4ta Sobrecarga para leer el teclado para desplazarse con la escoba
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

    // 5ta Sobrecarga para leer el teclado para habilidad de ataque cuerpo a cuerpo 
    public static void ReadBoard(ConsoleKey key, Tokens token, ref Players player, int damage, int attackRange)
    {
        int newX = token.CoordX;
        int newY = token.CoordY;

        for (int i = 1; i <= attackRange; i++)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    newX = token.CoordX - i;
                    newY = token.CoordY;
                    break;
                case ConsoleKey.DownArrow:
                    newX = token.CoordX + i;
                    newY = token.CoordY;
                    break;
                case ConsoleKey.LeftArrow:
                    newX = token.CoordX;
                    newY = token.CoordY - i;
                    break;
                case ConsoleKey.RightArrow:
                    newX = token.CoordX;
                    newY = token.CoordY + i;
                    break;
            }

            // Verifica si hay una pared en la nueva posición
            if (Maze.GeneralMaze[newX, newY] == -1)
            {
                Console.WriteLine("El ataque fue bloqueado por una pared.");
                GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR...");
                return;
            }

            // Verifica si hay un enemigo en la nueva posición
            for (int j = 0; j < player.Tokens.Length; j++)
            {
                if (player.Tokens[j].CoordX == newX && player.Tokens[j].CoordY == newY)
                {
                    player.Tokens[j].RemoveHealth(damage);
                    Console.WriteLine($"Le has quitado {damage} puntos de vida a {player.Tokens[j].InfoName()}, Le queda {player.Tokens[j].InfoHealth()}");

                    // Verifica si la ficha aún tiene vida
                    if (player.Tokens[j].InfoHealth() <= 0)
                    {
                        Console.WriteLine($"{player.Tokens[j].InfoName()} ha sido eliminado.");
                        player.DeleteToken(ref player, j);
                    }
                    GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR...");
                    Console.Clear();
                    return;
                }
            }
        }

        Console.WriteLine("\nNo se infligió daño en ninguna ficha\n");
        GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR...");
    }
    
    // 6ta Sobrecarga para leer el teclado para habilidad de ataque de Area
    public static void ReadBoard(ConsoleKey key, Tokens token, ref Players player, int damage, int attackRange, int area)
    {
        int newX = token.CoordX;
        int newY = token.CoordY;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                newX = token.CoordX - attackRange;
                newY = token.CoordY;
                break;
            case ConsoleKey.DownArrow:
                newX = token.CoordX + attackRange;
                newY = token.CoordY;
                break;
            case ConsoleKey.LeftArrow:
                newX = token.CoordX;
                newY = token.CoordY - attackRange;
                break;
            case ConsoleKey.RightArrow:
                newX = token.CoordX;
                newY = token.CoordY + attackRange;
                break;
        }

        if(newX <= 0 || newX >= Maze.GeneralMaze.GetLength(0) || newY <= 0 || newY >= Maze.GeneralMaze.GetLength(1))
        {
            Console.WriteLine("El ataque fue dirigido hacia afuera del Laberinto");
            GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR...");
            return;
        }

        int upX = area;             //X---
        int downX = area;           //X+++
        int leftY = area;           //Y---
        int rightY = area;          //Y+++

        while(true)
        {
            if(newX - upX <= 0)
                upX--;
            else if( newX + downX >= Maze.GeneralMaze.GetLength(0))
                downX--;
            else if(newY - leftY <= 0)
                leftY--;
            else if(newY + rightY >= Maze.GeneralMaze.GetLength(1))
                rightY--;
            else
                break;
        }


        // Verifica si hay un enemigo en la nueva posición
        for (int i = 0; i <= upX; i++)
        {
            for (int j = 0; j <= downX; j++)
            {
                for (int k = 0; k < leftY; k++)
                {
                    for (int l = 0; l < rightY; l++)
                    {
                        for (int m = 0; m < player.Tokens.Length; m++)
                        {    
                            // Verifica si hay algun enemigo en esa area
                            bool checkArea = 
                            (player.Tokens[m].CoordX == newX + downX && player.Tokens[m].CoordY == newY + rightY) || 
                            (player.Tokens[m].CoordX == newX + downX && player.Tokens[m].CoordY == newY - leftY) ||
                            (player.Tokens[m].CoordX == newX - upX && player.Tokens[m].CoordY == newY + rightY) ||
                            (player.Tokens[m].CoordX == newX - upX && player.Tokens[m].CoordY == newY - leftY);

                            if (checkArea)
                            {
                                player.Tokens[j].RemoveHealth(damage);
                                Console.WriteLine($"Le has quitado {damage} puntos de vida a {player.Tokens[j].InfoName()}, Le queda {player.Tokens[j].InfoHealth()}");

                                // Verifica si la ficha aún tiene vida
                                if (player.Tokens[j].InfoHealth() <= 0)
                                {
                                    Console.WriteLine($"{player.Tokens[j].InfoName()} ha sido eliminado.");
                                    player.DeleteToken(ref player, j);
                                }
                                GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR...");
                                Console.Clear();
                                return;
                            }
                        }            
                    }
                }
            }
        }

        Console.WriteLine("\nNo se infligió daño en ninguna ficha\n");
        GamePlay.Pause("\n PRESIONE UNA TECLA PARA CONTINUAR...");
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
     
    #region Metodo para Verificar las trampas           ////////////////////////////////////////////////////////////////////////////////////////
    
    // Chequea si caiste en una trampa
    public static void CheckTrap(Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens token, ref Players player)
    {
        if(Maze.GeneralMaze[newX, newY] == -2) //Verificacion de trapas
        {
            token.RemoveHealth(20, token.ActiveShield);
            Console.WriteLine("\n Has caido en una trampa y has perdido 20 puntos de vidas");
            if(token.InfoHealth() <= 0)
            { 
                Console.WriteLine($"\n {token.InfoName()} HA MUERTO");
                GamePlay.Pause("\n\n\n\n\nPRESIONE UAN TECLA PARA CONTINUAR...");
                for(int i = 0; i < player.Tokens.Length; i++)
                    if(player.Tokens[i] == token)
                        player.DeleteToken(ref player, i);
            }
        }
        else if(Maze.GeneralMaze[newX, newY] == -3)
        {
            if(token.InfoSpeed() == 2)
            {
                Console.WriteLine("ESTAS EN TU LIMITE DE VELOCIDAD");
                GamePlay.Pause("\n\n\n\n\nPRESIONE UAN TECLA PARA CONTINUAR...");
                return;
            }
            token.SlowDown(2);
            Console.WriteLine("\n Has caido en una trampa de reduccion de velocidad y has perdido 2 puntos de velocidad");
            GamePlay.Pause("\n\n\n\n\nPRESIONE UAN TECLA PARA CONTINUAR...");
        }
        else if(Maze.GeneralMaze[newX, newY] == -4)
        {
            if(token.InfoDamage() == 0)
            {
                Console.WriteLine("\n Has caido muchas veces en trampas de debilitacion de furza, ya tus ataques no hacen daño");
                GamePlay.Pause("\n\n\n\n\nPRESIONE UAN TECLA PARA CONTINUAR...");
                return;
            }
            token.RemoveDamage(35);
            Console.WriteLine("\n Has caido en una trampa y has perdido 30 puntos de daño, ahora haces menos daño");
            GamePlay.Pause("\n\n\n\n\nPRESIONE UAN TECLA PARA CONTINUAR...");
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
