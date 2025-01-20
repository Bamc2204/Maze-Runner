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
        player.Tokens[0] = new Tokens("Harry Potter", 1, "⚡", 
        "Harry Potter, el niño que vivió, es conocido por su valentía y habilidades mágicas excepcionales. En el Torneo de los Tres Magos, Harry demuestra su capacidad para enfrentarse a desafíos peligrosos y salir victorioso. "
        + "Harry Potter participa en el Torneo de los Tres Magos, donde debe superar varias pruebas peligrosas. Su habilidad *Expellarmus* de un rango de 2 casillas permite paralizar a sus oponentes, dándole una ventaja crucial en el laberinto. "
        + "Historia: Harry Potter, junto a otros campeones, compite en el Torneo de los Tres Magos, enfrentándose a dragones, criaturas mágicas y laberintos encantados para ganar el Cáliz de Fuego.", 
        1, 0, "Expellarmus", 6, 6, -7, -8, -10, 20, 4, 150);

        player.Tokens[1] = new Tokens("Cedric Diggory", 2, "🦡", 
        "Cedric Diggory, un estudiante de Hufflepuff, es conocido por su lealtad y habilidades mágicas. Como uno de los campeones del Torneo de los Tres Magos, Cedric demuestra su destreza en la magia elemental. "
        + "Cedric Diggory compite en el Torneo de los Tres Magos, utilizando su habilidad *Conjuro Elemental*, con una distancia de 4 casillas y un área de 2 casillas que le permite atacar a múltiples enemigos a la vez, lo que le permite despejar su camino hacia la COPA. "
        + "Historia: Cedric Diggory, junto a Harry Potter, enfrenta los desafíos del Torneo de los Tres Magos, demostrando su valentía y habilidades mágicas.", 
        13, 0, "Conjuro Elemental", 4, 8, -7, -7, -9, 20, 4, 150);

        player.Tokens[2] = new Tokens("Fleur Delacour", 3, "🌸", 
        "Fleur Delacour, una talentosa bruja de Beauxbatons, es conocida por su gracia y habilidades curativas. En el Torneo de los Tres Magos, Fleur demuestra su capacidad para sanar a sus compañeros. "
        + "Fleur Delacour utiliza su habilidad *Sanación Mágica* para curar a la persona con menos vida en un radio de 3 casillas alrededor de la ficha. "
        + "Historia: Fleur Delacour, una de las campeonas del Torneo de los Tres Magos, enfrenta desafíos mágicos mientras utiliza sus habilidades curativas para ayudar a sus compañeros.", 
        25, 0, "Sanacion Magica", 4, 10, -9, -9, -9, 15, 8);

        player.Tokens[3] = new Tokens("Viktor Krum", 4, "💪", 
        "Viktor Krum, un famoso buscador de Quidditch de Durmstrang, es conocido por su fuerza y habilidades físicas excepcionales. En el Torneo de los Tres Magos, Viktor demuestra su capacidad para enfrentarse a desafíos físicos y mágicos. "
        + "Viktor Krum utiliza su habilidad *Draconifors* para aumentar su daño cuerpo a cuerpo, permitiéndole atacar con mayor fuerza a una distancia de 1 casilla. "
        + "Historia: Viktor Krum, uno de los campeones del Torneo de los Tres Magos, enfrenta desafíos mágicos y físicos mientras compite por el Cáliz de Fuego, demostrando su valentía y habilidades en cada prueba.", 
        37, 0, "Draconifors", 4, 6, -8, -8, -9, 30, 1, 250);

        string infoTarget = "Obtener la COPA y escapar del laberinto";

        player.SetTarget(ref infoTarget);
    }

    // Metodo para crear las fichas malas
    private void CreateTokensBadPlayer(ref Players player)
    {
        player.Tokens[0] = new Tokens("Acromántula", 5, "🕷️", 
        "Acromántula, una gigantesca araña mágica, es conocida por su veneno mortal y su capacidad para moverse rápidamente a través del laberinto. En el Torneo de los Tres Magos, las acromántulas representan uno de los muchos peligros que los campeones deben enfrentar. "
        + "La habilidad *Veneno* de la Acromántula permite envenenar a sus oponentes a una distancia de 3 casillas, causando daño continuo durante varios turnos. "    
        + "Historia: En el Torneo de los Tres Magos, las acromántulas son una de las criaturas mágicas que los campeones deben superar para obtener el Cáliz de Fuego. Su veneno y agilidad las convierten en adversarios formidables.", 
        7, 43, "Veneno", 5, 8, 40, 3, 300);

        player.Tokens[1] = new Tokens("Esfinge", 6, "🦁", 
        "Esfinge, una criatura mágica con cuerpo de león y cabeza humana, es conocida por su inteligencia y fuerza. En el Torneo de los Tres Magos, la esfinge representa uno de los muchos desafíos que los campeones deben superar. "
        + "La habilidad *Aumento de Fuerza* de la Esfinge permite atacar con mayor daño a una distancia de 1 casilla, haciendo que sus ataques sean devastadores. "
        + "Historia: En el Torneo de los Tres Magos, la esfinge es uno de los obstáculos que los campeones deben enfrentar para obtener el Cáliz de Fuego. Su fuerza y astucia la convierten en un adversario formidable.", 
        19, 43, "Aumento de Fuerza", 6, 6, 55, 2, 320);

        player.Tokens[2] = new Tokens("Boggart", 7, "👻", 
        "Boggart, una criatura mágica que toma la forma del peor miedo de quien lo ve, es conocido por su habilidad para transformarse. En el Torneo de los Tres Magos, los boggarts representan un desafío psicológico para los campeones. "
        + "La habilidad *Copiar* del Boggart le permite transformarse en cualquier ficha de su misma facción, adoptando sus habilidades y características. "
        + "Historia: En el Torneo de los Tres Magos, los boggarts son una de las criaturas mágicas que los campeones deben superar para obtener el Cáliz de Fuego. Su capacidad para transformarse en los peores miedos de los campeones los convierte en adversarios formidables.", 
        31, 43, "Copiar", 10, 12, 0, 0, 40);

        player.Tokens[3] = new Tokens("Blast-Ended Skrewts", 8, "🦂", 
        "Blast-Ended Skrewts, criaturas mágicas híbridas creadas por Hagrid, son conocidas por su capacidad para lanzar fuego y causar explosiones. En el Torneo de los Tres Magos, los Blast-Ended Skrewts representan un peligro significativo para los campeones. "
        + "La habilidad *Lanzar Fuego* de los Blast-Ended Skrewts permite atacar en un área de 1 casilla a una distancia de 5 casillas, causando daño a múltiples oponentes. "
        + "Historia: En el Torneo de los Tres Magos, los Blast-Ended Skrewts son una de las criaturas mágicas que los campeones deben superar para obtener el Cáliz de Fuego. Su capacidad para lanzar fuego y causar explosiones los convierte en adversarios formidables.", 
        43, 43, "Lazar Fuego", 6, 10, 40, 5, 150);

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

    // Metodo para usar la Habilidad de las Fichas
    private static void _usedSkill(Tokens token, ref Players player1, ref Players player2)
    {
        if(token.InfoColdTime() != 0)
        {
            GamePlay.Pause("\nAUN NO SE HA ENFRIADO LA HABILIDAD \n\n\n\n\n\n\n\n\n\n\n\nPRESIONE UNA TECLA PLARA CONTINUAR...");
            return;
        }

        // Habilidad de Harry Potter
        if(token.InfoId() == 1)
        {
            _expellarmus(token, ref player2);
            token.ModifyColdTime(6);
            return;
        }

        // Habilidad de Cedric Diggory
        if(token.InfoId() == 2)
        {
            _elementalConjuration(token, ref player2);
            token.ModifyColdTime(4);
            return;
        }

        // Habilidad de Fleur Delacour
        if(token.InfoId() == 3)
        {
            _magicalHealing(token, ref player1);
            token.ModifyColdTime(4);
            return;
            
        }

        // Habilidad de Viktor Krum
        if(token.InfoId() == 4)
        {
            _draconifors(token, ref player2);
            token.ModifyColdTime(4);
            return;
        }

        // Habilidad de Acromantula
        if(token.InfoId() == 5)
        {
            _poison(token, ref player2);
            token.ModifyColdTime(5);
            return;
        }

        // Habilidad de Esfinge
        if(token.InfoId() == 6)
        {
            _strengthIncrease(token, ref player2);
            token.ModifyColdTime(6);
            return;
        }

        // Habilidad de Boggart
        if(token.InfoId() == 7)
        {
            _copy(token, player1);
            token.ModifyColdTime(10);
            return;
        }

        // Habilidad de Blast-Ended Skrewts
        if(token.InfoId() == 8)
        {
            _throwFire(token, ref player2);
            token.ModifyColdTime(6);
            return;
        }

    }
    
    // Habilidades de MAGOS
    
    // Metodo para paralizar un ficha en un rango de 3 casillas
    private static void _expellarmus(Tokens token, ref Players player2)
    {
        int newX = token.CoordX;
        int newY = token.CoordY;

        int upX = 2;             //X---
        int downX = 2;           //X+++
        int leftY = 2;           //Y---
        int rightY = 2;          //Y+++

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
                for (int k = 0; k <= leftY; k++)
                {
                    for (int l = 0; l <= rightY; l++)
                    {
                        for (int m = 0; m < player2.Tokens.Length; m++)
                        {    
                            // Verifica si hay algun enemigo en esa area
                            bool checkArea = 
                            (player2.Tokens[m].CoordX == newX + j && player2.Tokens[m].CoordY == newY + l) || 
                            (player2.Tokens[m].CoordX == newX + j && player2.Tokens[m].CoordY == newY - k) ||
                            (player2.Tokens[m].CoordX == newX - i && player2.Tokens[m].CoordY == newY + l) ||
                            (player2.Tokens[m].CoordX == newX - i && player2.Tokens[m].CoordY == newY - k);

                            if(checkArea)
                            {
                                player2.Tokens[m].ModifyParalysis(true);
                                player2.Tokens[m].ModifyContTurnParalysis(2);

                                Console.WriteLine($"Se ha paralizado a {player2.Tokens[m].InfoName()} por 3 turnos");

                                GamePlay.Pause();

                                return;
                            }
                        }
                    }
                }
            }
        } 
        Console.WriteLine("NO HAY NADA QUE PARALIZAR");
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
        int index = 3;
        int newX = token.CoordX;
        int newY = token.CoordY;

        int upX = 3;             //X---
        int downX = 3;           //X+++
        int leftY = 3;           //Y---
        int rightY = 3;          //Y+++

        while(true)
        {
            if(newX - upX < 0)
                upX--;
            else if( newX + downX >= Maze.GeneralMaze.GetLength(0))
                downX--;
            else if(newY - leftY < 0)
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
                for (int k = 0; k <= leftY; k++)
                {
                    for (int l = 0; l <= rightY; l++)
                    {
                        for (int m = 0; m < player1.Tokens.Length; m++)
                        {    
                            // Verifica si hay algun enemigo en esa area
                            bool checkArea = 
                            (player1.Tokens[m].CoordX == newX - i && player1.Tokens[m].CoordY == newY + l) ||
                            (player1.Tokens[m].CoordX == newX - i && player1.Tokens[m].CoordY == newY - k)  ||
                            (player1.Tokens[m].CoordX == newX + j && player1.Tokens[m].CoordY == newY + l) || 
                            (player1.Tokens[m].CoordX == newX + j && player1.Tokens[m].CoordY == newY - k); 

                            if(checkArea && player1.Tokens[m].InfoHealth() < menor)
                            {
                                menor = player1.Tokens[m].InfoHealth();
                                index = m;
                            }
                        }
                    }
                }
            }
        }

        player1.Tokens[index].AddHealth(20);
        Console.WriteLine($"Se ha curado a {player1.Tokens[index].InfoName()}");
        GamePlay.Pause();
    
    }
    
    // Habilidades de MONSTRUOS

    // Envenamiento
    private static void _poison(Tokens tokens, ref Players player2)
    {
        int newX = tokens.CoordX;
        int newY = tokens.CoordY;

        Console.WriteLine("\nEn que direccion va a envenenar");
        ConsoleKey key = Console.ReadKey().Key;

        if(key != ConsoleKey.UpArrow &&  key != ConsoleKey.DownArrow && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow)
        {
            Console.WriteLine("No ha tocado una direccion");
            GamePlay.Pause();
            return;
        }

        ReadBoard(key, ref newX, ref newY, ref player2,  3);

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
        
        bool CheckKey = key != ConsoleKey.NumPad1 && key != ConsoleKey.NumPad2 && key != ConsoleKey.NumPad3 && key != ConsoleKey.NumPad4
        && key == ConsoleKey.D1 && key == ConsoleKey.D2 && key == ConsoleKey.D4 && key == ConsoleKey.D4;

        if(CheckKey)
        {
            Console.WriteLine("\nNo escogio convertirse en algo"); 
            GamePlay.Pause();
            return; 
        }
           
        ReadBoard(key, ref index);  

        if(index > player1.Tokens.Length)
        {
            Console.WriteLine("Se han eliminado fichas por lo q esa posicion ya no existe");
            GamePlay.Pause();
            return;
        }

        int id = player1.Tokens[index].InfoId();

        switch(id)
        {
            case 5: 
                token.ModifyCharacter("🕷️");
                token.ModifyHealth(300 );
                token.ModifySpeed(8);
                token.ModifyDamage(40);
                token.ModifyDistAttack(3);
                break;

            case 6: 
                token.ModifyCharacter("🦁");
                token.ModifyHealth(320);
                token.ModifySpeed(6);
                token.ModifyDamage(55);
                token.ModifyDistAttack(2);
                break;

            case 7: 
                token.ModifyCharacter("👻");
                token.ModifyHealth(40);
                token.ModifySpeed(12);
                token.ModifyDamage(0);
                token.ModifyDistAttack(0);
                break;

            case 8: 
                token.ModifyCharacter("🦂");
                token.ModifyHealth(150);
                token.ModifySpeed(10);
                token.ModifyDamage(40);
                token.ModifyDistAttack(5);
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
            Console.WriteLine("\n Introduzca cual de sus fichas va a coger (del 1 al 4)");

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
            if(!player1.InfoTurn() && CanRunning == true )
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
                    Console.WriteLine("Hay una fichas eliminadas por lo que esa posicion ya no existe. \nVuelva a escoger ficha");
                    GamePlay.Pause();
                    CanRunning = false;
                    continue;
                }
                _displacement(player1.Tokens[indexPiece].InfoSpeed(), maze, ref player1.Tokens[indexPiece], player1, player2, ref running);
                CanRunning = true;
            }
            else if(player2.InfoTurn())
            {
                if(indexPiece > player2.Tokens.Length || player2.Tokens[indexPiece].InfoParalysis())
                {
                    Console.WriteLine("Hay una fichas eliminadas por lo que esa posicion ya no existe o  esta paralizada. \nVuelva a escoger ficha");
                    GamePlay.Pause();
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
            
            CheckParalysis(ref player2);

            CheckPoison(ref player1);

            _minusColdTime(player1);
            _minusColdTime(player2);
        }
    }

    // Metodo para El Tiempo de Enfriamiento
    private static void _minusColdTime(Players player1)
    {
        for (int i = 0; i < player1.Tokens.Length; i++)
        {
            if(player1.Tokens[i].InfoColdTime() == 0)
                continue;
            player1.Tokens[i].MinusColdTime(1);
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
            if(key == ConsoleKey.I || key == ConsoleKey.E || key == ConsoleKey.Tab || key == ConsoleKey.Q || key == ConsoleKey.G || key == ConsoleKey.F) 
                continue;

            // Dentro de filas, columnas y si es un camino
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
                GamePlay.Pause();
                return;
            }

            // Verifica si hay un enemigo en la nueva posición
            for (int j = 0; j < player.Tokens.Length; j++)
            {
                if (player.Tokens[j].CoordX == newX && player.Tokens[j].CoordY == newY)
                {
                    player.Tokens[j].RemoveHealth(damage, player.Tokens[j].ActiveShield, player.Tokens[j]);

                    if(player.Tokens[j].ActiveShield == false || player.Tokens[j].InfoShield() < 0)
                        Console.WriteLine($"Le has quitado {damage} puntos de vida a {player.Tokens[j].InfoName()}. Le queda {player.Tokens[j].InfoHealth()}");
                    else
                        Console.WriteLine($"Le has quitado {damage} puntos al escudo de {player.Tokens[j].InfoName()}. Le queda {player.Tokens[j].InfoShield()}");

                    step = 0;

                    // Verifica si la ficha aún tiene vida
                    if (player.Tokens[j].InfoHealth() <= 0)
                    {
                        Console.WriteLine($"{player.Tokens[j].InfoName()} ha sido eliminado.");
                        player.DeleteToken(ref player, j);
                    }
                    GamePlay.Pause();
                    Console.Clear();
                    return;
                }
            }
        }

        Console.WriteLine("\nNo se infligió daño en ninguna ficha\n");
        GamePlay.Pause();
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
                GamePlay.Pause();
                return;
            }

            // Verifica si hay un enemigo en la nueva posición
            for (int j = 0; j < player.Tokens.Length; j++)
            {
                if (player.Tokens[j].CoordX == newX && player.Tokens[j].CoordY == newY)
                {
                    player.Tokens[j].RemoveHealth(damage, player.Tokens[j].ActiveShield, player.Tokens[j]);
                    if(player.Tokens[j].ActiveShield == false || player.Tokens[j].InfoShield() < 0)
                        Console.WriteLine($"Le has quitado {damage} puntos de vida a {player.Tokens[j].InfoName()}. Le queda {player.Tokens[j].InfoHealth()}");
                    else
                        Console.WriteLine($"Le has quitado {damage} puntos al escudo de {player.Tokens[j].InfoName()}. Le queda {player.Tokens[j].InfoShield()}");
                    // Verifica si la ficha aún tiene vida
                    if (player.Tokens[j].InfoHealth() <= 0)
                    {
                        Console.WriteLine($"{player.Tokens[j].InfoName()} ha sido eliminado.");
                        player.DeleteToken(ref player, j);
                    }
                    GamePlay.Pause();
                    Console.Clear();
                    return;
                }
            }
        }

        Console.WriteLine("\nNo se infligió daño en ninguna ficha\n");
        GamePlay.Pause();
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
            GamePlay.Pause();
            return;
        }

        int upX = area;             //X---
        int downX = area;           //X+++
        int leftY = area;           //Y---
        int rightY = area;          //Y+++

        while(true)
        {
            if(newX - upX < 0)
                upX--;
            else if( newX + downX >= Maze.GeneralMaze.GetLength(0))
                downX--;
            else if(newY - leftY < 0)
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
                for (int k = 0; k <= leftY; k++)
                {
                    for (int l = 0; l <= rightY; l++)
                    {
                        for (int m = 0; m < player.Tokens.Length; m++)
                        {    
                            // Verifica si hay algun enemigo en esa area
                            bool checkArea = 
                            (player.Tokens[m].CoordX == newX + j && player.Tokens[m].CoordY == newY + l) || 
                            (player.Tokens[m].CoordX == newX + j && player.Tokens[m].CoordY == newY - k) ||
                            (player.Tokens[m].CoordX == newX - i && player.Tokens[m].CoordY == newY + l) ||
                            (player.Tokens[m].CoordX == newX - i && player.Tokens[m].CoordY == newY - k);

                            if (checkArea)
                            {
                                player.Tokens[m].RemoveHealth(damage, player.Tokens[m].ActiveShield, player.Tokens[m]);
                                
                                if(player.Tokens[m].ActiveShield == false || player.Tokens[j].InfoShield() < 0)
                                    Console.WriteLine($"Le has quitado {damage} puntos de vida a {player.Tokens[m].InfoName()}. Le queda {player.Tokens[m].InfoHealth()}");
                                else
                                    Console.WriteLine($"Le has quitado {damage} puntos al escudo de {player.Tokens[m].InfoName()}. Le queda {player.Tokens[m].InfoShield()}");

                                // Verifica si la ficha aún tiene vida
                                if (player.Tokens[m].InfoHealth() <= 0)
                                {
                                    Console.WriteLine($"{player.Tokens[m].InfoName()} ha sido eliminado.");
                                    player.DeleteToken(ref player, m);
                                }
                                GamePlay.Pause();
                                Console.Clear();
                                return;
                            }
                        }            
                    }
                }
            }
        }
        Console.WriteLine("\nNo se infligió daño en ninguna ficha\n");
        GamePlay.Pause();
    }
    
    // 7ma Sobrecarga para leer el teclado para la habilidad de Veneno
    public static void ReadBoard(ConsoleKey key, ref int newX, ref int newY, ref Players player2,  int distPoison)
    {
       for (int i = 1; i <= distPoison; i++)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    newX -= i;
                    break;

                case ConsoleKey.DownArrow:
                    newX += i;
                    break;
                    
                case ConsoleKey.LeftArrow:
                    newY -= i;
                    break;
                    
                case ConsoleKey.RightArrow:
                    newY += i;
                    break;
            }

            // Verifica si hay una pared en la nueva posición
            if (Maze.GeneralMaze[newX, newY] == -1)
            {
                Console.WriteLine("No se puede envenenar en esa direcion puesto q hay una pared de por medio.");
                GamePlay.Pause();
                return;
            }

            for (int j = 0; j < player2.Tokens.Length; j++)
            {
                if(player2.Tokens[i].CoordX == newX && player2.Tokens[i].CoordY == newY)
                {
                    player2.Tokens[i].ModifyPoison(true);
                    player2.Tokens[i].ModifyContTurnPoison(3); 

                    Console.WriteLine($"Has envenenado a {player2.Tokens[j].InfoName()} por 3 turnos, se le restara 15 de vida en cada turno");
                    GamePlay.Pause();
                    return;
                }
            }
        }
        Console.WriteLine("No hay nada a lo que pudieras envenenar en esa direccion");
        GamePlay.Pause("\n\n\n\n\n\nPresione una tecla para continuar");
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////
     
    #region Metodos de Verificacion           ////////////////////////////////////////////////////////////////////////////////////////
    
    // Chequea si caiste en una trampa
    public static void CheckTrap(Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens token, ref Players player)
    {
        if(Maze.GeneralMaze[newX, newY] == -2) //Verificacion de trapas
        {
            token.RemoveHealth(20, token.ActiveShield, token);
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
            token.RemoveDamage(10);
            Console.WriteLine("\n Has caido en una trampa y has perdido 30 puntos de daño, ahora haces menos daño");
            GamePlay.Pause("\n\n\n\n\nPRESIONE UAN TECLA PARA CONTINUAR...");
        }
    }

    // Chequea si hay alguna ficha paralisada para comenzar de desparalizar
    public static void CheckParalysis(ref Players player2)
    {
        for (int i = 0; i < player2.Tokens.Length; i++)
        { 
            if(player2.Tokens[i].InfoParalysis())
            {
                player2.Tokens[i].ModifyContTurnParalysis(-1);
                if(player2.Tokens[i].InfoContTurnParalysis() == 0)
                    player2.Tokens[i].ModifyParalysis(false);
            }   
        }
    }

    // Chequea si hay alguna ficha envenenada para ejecutar el  efecto del veneno
    public static void CheckPoison(ref Players player1)
    {
        for (int i = 0; i < player1.Tokens.Length; i++)
        { 
            if(player1.Tokens[i].InfoPoison())
            {
                player1.Tokens[i].RemoveHealth(15);
                player1.Tokens[i].ModifyContTurnPoison(-1);
                if(player1.Tokens[i].InfoContTurnPoison() == 0)
                    player1.Tokens[i].ModifyPoison(false);
            }   
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
