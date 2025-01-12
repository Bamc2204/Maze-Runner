using System;

class GamePlay
{
    // Array de colores disponibles
    public static ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Magenta };

    public static void Main(string[] args)
    {
        // Limpiar consola y mostrar título del juego
        Console.Clear();
        PrintGameTitle();

        // Mensaje de bienvenida
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n¡Bienvenido al juego del laberinto mágico! ¿Listo para esta aventura?");
        Console.ResetColor();

        //  Modo de juego multijugador
        MultiPlayer();
    
        // Finalizar el juego
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Espero que hayas disfrutado de esta aventura mágica.");
        Console.WriteLine("\n GRACIAS POR JUGAR \n");
        Console.ResetColor();

    }

    //Multijugador
    private static void MultiPlayer()
    {
        
        // Solicitar nombres de los jugadores
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nJugador 1, por favor ingresa tu nombre:");
        string name1 = Console.ReadLine()!;
        Console.WriteLine($"\n¡Hola, {name1}!");

        Console.WriteLine("\nJugador 2, por favor ingresa tu nombre:");
        string name2 = Console.ReadLine()!;
        Console.WriteLine($"\n¡Hola, {name2}!");
        Console.ResetColor();

        // Pausa antes de continuar
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Pause("\nPresiona cualquier tecla para continuar...");
        Console.ResetColor();

        // Historia del juego
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("********//////// HISTORIA ////////********");
        Console.WriteLine("\nEn el misterioso laberinto del Torneo de los Tres Magos, los competidores deben usar magia, "
            + "estrategia y valentía para superar trampas mortales y monstruos aterradores. "
            + "Pero esta vez, no estás solo. ¡Compite contra otro jugador para alcanzar la gloria!");
        Console.ResetColor();

        // Pausa antes de continuar
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Pause("\nPresiona cualquier tecla para continuar...");
        Console.ResetColor();

        // Explicación inicial
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\nLAS FICHAS DISPONIBLES SON:");
        Console.WriteLine("\nLos *MAGOS* escogidos por el Caliz de Fuego y los *MONSTRUOS* que rondan el laberinto esperando algo que puedan devorar.");
        Console.WriteLine("\nEl ***1er JUGADOR*** seleccionará primero su facción, y el ***2do JUGADOR*** tomará la restante.");
        Console.WriteLine("\n¡Pónganse de acuerdo antes de escoger!");
        Console.ResetColor();

        // Pausa antes de continuar
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Pause("\nPresiona cualquier tecla para continuar...");
        Console.ResetColor();

        // Selección de facciones
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\nJugador 1, presiona la ***FLECHA IZQUIERDA*** para ser un *MAGO* o la ***FLECHA DERECHA*** para ser un *MONSTRUO*.");
        Console.ResetColor();

        int indexPlayer1 = 0, indexPlayer2 = 0;

        while (true)
        {
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
            {
                Players.ReadBoard(key, ref indexPlayer1);
                break;
            }
            Console.WriteLine("\nPor favor, presiona solo FLECHA IZQUIERDA o FLECHA DERECHA. Inténtalo otra vez.");
        }

        Players player1 = new Players(name1, indexPlayer1);
        player1.ChooseFaction(ref indexPlayer1, ref indexPlayer2);

        Players player2 = new Players(name2, indexPlayer2);
        player2.ChooseFaction(ref indexPlayer2, ref indexPlayer1);

        // Crear fichas para los jugadores
        player1.CreateTokensFaction(player1, player2);

        // Pausa antes de continuar
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Pause("\nPresiona cualquier tecla para comenzar el juego...");
        Console.ResetColor();

        Console.Clear();

        // Crear el laberinto y comenzar el juego
        Maze lab = new Maze(player1, player2);
        Maze.PrintMaze(player1, player2);

        bool running = true;
        if (player1.InfoFaction() == "MAGOS")
        {
            Players.PlayersTurn(lab, player1, player2, ref running);
        }
        else
        {
            Players.PlayersTurn(lab, player2, player1, ref running);
        }

    }

    // Crea el titulo de juego
    private static void PrintGameTitle()
    {
        foreach (var color in colors)
        {
            Console.ForegroundColor = color;  // Cambia el color del texto
            Console.Clear(); // Limpiar consola antes de imprimir
            Console.WriteLine(@" 
                ╔═╗╦    ╔╦╗╔═╗╦═╗╔╗╔╔═╗╔═╗  ╔╦╗╔═╗╦    ╦  ╔═╗╔╗ ╔═╗╦═╗╦╔╗╔╔╦╗╔═╗  ╔╦╗╔═╗╔═╗╦╔═╗╔═╗
                ║╣ ║     ║ ║ ║╠╦╝║║║║╣ ║ ║   ║║║╣ ║    ║  ╠═╣╠╩╗║╣ ╠╦╝║║║║ ║ ║ ║  ║║║╠═╣║ ╦║║  ║ ║
                ╚═╝╩═╝   ╩ ╚═╝╩╚═╝╚╝╚═╝╚═╝  ═╩╝╚═╝╩═╝  ╩═╝╩ ╩╚═╝╚═╝╩╚═╩╝╚╝ ╩ ╚═╝  ╩ ╩╩ ╩╚═╝╩╚═╝╚═╝
            ");
            Console.ResetColor();
            System.Threading.Thread.Sleep(500);  // Pequeño retraso para mostrar el cambio de color
        }
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(@" 
                                         EL LABERINTO DE MAGOS Y MONSTRUOS 
          Inspirado en el mítico laberinto del Torneo del Caliz de Fuego de la famosa saga de HARRY POTTER
        ");
        Console.ResetColor();

        // Pausa antes de continuar
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Pause("\nPresiona cualquier tecla para continuar...");
        Console.ResetColor();
    }

    // Hace las pausas necesarias en el juego
    public static void Pause(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.ReadKey();
    }

}

