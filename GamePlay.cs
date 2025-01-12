using System;
using System.Diagnostics; // Para iniciar procesos
using System.Threading;  // Para manejar hilos
class GamePlay
{
    // Array de colores disponibles
    public static ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Magenta };

    // Booleano para parar la musica cuando se termine el proyecto
    private static bool musicRunning = true;

    public static void Main(string[] args)
    {
        // Ruta de las canciones (asegúrate de que los archivos existan)
        string[] songs = {
            @"D:\Programacion\Maze-Runner\Backend\musica\1.mp3",
            @"D:\Programacion\Maze-Runner\Backend\musica\2.mp3",
            @"D:\Programacion\Maze-Runner\Backend\musica\3.mp3"
        };

        // Crear un hilo separado para reproducir música en bucle
        Thread musicThread = new Thread(() => PlayMusicLoop(songs));
        musicThread.IsBackground = true; // El hilo se detendrá automáticamente cuando termine el programa
        musicThread.Start();

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

        // Finaliza el juego y detén la música
        musicRunning = false;
        Thread.Sleep(500); // Breve pausa para que el hilo de música termine correctamente

    }

    // Reproduce la musica
    private static void PlayMusicLoop(string[] songs)
    {
        int currentSongIndex = 0; // Índice de la canción actual

        while (musicRunning) // El bucle se detiene cuando musicRunning es falso
        {
            try
            {
                using (Process player = new Process())
                {
                    player.StartInfo.FileName = songs[currentSongIndex]; // Ruta de la canción actual
                    player.StartInfo.UseShellExecute = true; // Indica que queremos usar un proceso externo
                    player.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // Oculta la ventana del reproductor
                    player.Start(); // Inicia la reproducción de la canción

                    // Esperar a que el proceso termine (cuando la canción termine)
                    player.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error al reproducir la canción: {songs[currentSongIndex]}");
                Console.WriteLine($"Detalles: {ex.Message}");
                Console.ResetColor();
            }

            // Avanzar a la siguiente canción (y volver al inicio si llegamos al final)
            currentSongIndex = (currentSongIndex + 1) % songs.Length;
        }
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
        Console.WriteLine("          ********//////// HISTORIA ////////********");
        string History = "\nAlbus Dumbledore anuncia, durante el banquete de bienvenida en "
            + "\nHogwarts, que la escuela será la sede del Torneo de los Tres "
            + "\nMagos, una competencia interescolar entre Hogwarts, la Academia "
            + "\nBeauxbatons y el Instituto Durmstrang. Un estudiante de cada una "
            + "\nde tres escuelas de magia sería elegido por el Cáliz de fuego para "
            + "\ncompetir en el torneo. Los campeones que elige el Cáliz son Fleur "
            + "\nDelacour de Beauxbatons, Viktor Krum de Durmstrang y Cedrics "
            + "\nDiggory de Hogwarts. Misteriosamente, Harry Potter también es "
            + "\nelegido, pese a que no había ingresado su nombre en el Cáliz del "
            + "\nfuego. Ahora son 4 los magos que deben enfrentarse a los desafíos "
            + "\ndel Torneo. "
            + "\nLa tercera y última prueba, en la que está basada este juego, "
            + "\nconsiste en un laberinto lleno de obstáculos mágicos diseñados "
            + "\npara confundir incluso a los magos más astutos, y como si no fuera  "
            + "\nsuficiente lo habitan monstruos como acromántulas sedientas de "
            + "\ncarne humana; boggarts que se alimentan del miedo; esfinges que "
            + "\ndesafían la mente con enigmas mortales y otras criaturas que "
            + "\nobstaculizan el camino a la legendaria Copa que se haya en el "
            + "\ncorazón del laberinto. La Copa otorga, a quien la alcance, 1000 "
            + "\ngaleones en concepto de premio. "
            + "\nEl jugador puede elegir entre ser bestia, y evitar de forma "
            + "\nsanguinaria que cualquiera de los magos se convierta en el "
            + "\nganador, o mago, y hacer uso de sus conocimientos magícos para "
            + "\nalcanzar la Copa y escapar triunfante. "; 
        PrintTextSlowly(History);
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

    // Imrpime el texto letra por letra despacio
    public static void PrintTextSlowly(string text)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(50); // Controla la velocidad de impresión
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
            Thread.Sleep(3500);  // Pequeño retraso para mostrar el cambio de color
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

