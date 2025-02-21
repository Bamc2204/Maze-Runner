﻿using System;
using System.Diagnostics; // Para iniciar procesos
using System.IO; // Para manejar rutas de archivos
using System.Threading;  // Para manejar hilos
class GamePlay
{
    // Array de colores disponibles
    public static ConsoleColor[] colors = { ConsoleColor.Red,ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Magenta};

    // Booleano para parar la musica cuando se termine el proyecto
    private static bool musicRunning = true;

    // Variable para almacenar el proceso de la música
    private static Process? musicProcess = null;

    public static void Main(string[] args)
    {
        // Ruta de las canciones (asegúrate de que los archivos existan)
        string[] songs = {Directory.GetCurrentDirectory() + @"\musica\1.M4A"};
        
        // Crear un hilo separado para reproducir música en bucle
        Thread musicThread = new Thread(() => PlayMusicLoop(songs));
        musicThread.IsBackground = true; // El hilo se detendrá automáticamente cuando termine el programa
        musicThread.Start();

        // Limpiar consola y mostrar título del juego
        Console.Clear();
        
        _printGameTitle();

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

        // Asegúrate de que el proceso de la música se cierre
        if (musicProcess != null && !musicProcess.HasExited)
        {
            musicProcess.Kill();
            musicProcess.Dispose();
        }
    }

    //Multijugador
    private static void MultiPlayer()
    {
        Console.Clear();
        
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
        Pause();

        // Historia del juego
        _history();

        // Explicación inicial
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\nLAS FICHAS DISPONIBLES SON:");
        Console.WriteLine("\nLos *MAGOS* escogidos por el Caliz de Fuego y los *MONSTRUOS* que rondan el laberinto esperando algo que puedan devorar.");
        Console.WriteLine("\nEl ***1er JUGADOR*** seleccionará primero su facción, y el ***2do JUGADOR*** tomará la restante.");
        Console.WriteLine("\n¡Pónganse de acuerdo antes de escoger!");
        Console.ResetColor();

        // Pausa antes de continuar
        Pause();

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
                Players.ReadBoard(ref indexPlayer1, key);
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
        Pause();

        Console.Clear();

        // Leyenda
        _legend();

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

    // Reproduce la musica
    private static void PlayMusicLoop(string[] songs)
    {
        int currentSongIndex = 0; // Índice de la canción actual

        while (musicRunning) // El bucle se detiene cuando musicRunning es falso
        {
            if (songs.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay canciones disponibles para reproducir.");
                Console.ResetColor();
                musicRunning = false;
                return;
            }

            try
            {
                musicProcess = new Process();
                musicProcess.StartInfo.FileName = songs[currentSongIndex]; // Ruta de la canción actual
                musicProcess.StartInfo.UseShellExecute = true; // Indica que queremos usar un proceso externo
                musicProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // Oculta la ventana del reproductor
                musicProcess.Start(); // Inicia la reproducción de la canción

                // Esperar a que el proceso termine (cuando la canción termine)
                musicProcess.WaitForExit();

                // Avanzar a la siguiente canción (y volver al inicio si llegamos al final)
                currentSongIndex = (currentSongIndex + 1) % songs.Length;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error al reproducir la canción: {songs[currentSongIndex]}");
                Console.WriteLine($"Detalles: {ex.Message}");
                Console.ResetColor();
                musicRunning = false;
            }
        }
    }

    // Imrpime el texto letra por letra despacio
    public static void PrintTextSlowly(string text, int time)
    {
        foreach (char c in text)
        {
            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // Limpiar la tecla presionada
                Console.Write(text.Substring(text.IndexOf(c))); // Imprimir el resto del texto de una vez
                break;
            }
            Console.Write(c);
            Thread.Sleep(time); // Controla la velocidad de impresión
        }
    }

    // Crea el titulo de juego
    private static void _printGameTitle()
    {
        int contSecond = 1;
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
            _timeInit(contSecond);
            contSecond++;
        }
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(@" 
                                         EL LABERINTO DE MAGOS Y MONSTRUOS 
          Inspirado en el mítico laberinto del Torneo del Caliz de Fuego de la famosa saga de HARRY POTTER
        ");
        Console.ResetColor();

        // Pausa antes de continuar
        Pause();
    }

    // Tiempo de demora de la presentacion
    private static void _timeInit(int cont)
    {
        if(cont == 1)
                Thread.Sleep(6000);  // Pequeño retraso para mostrar el cambio de color
        if(cont == 2)
                Thread.Sleep(4000);  // Pequeño retraso para mostrar el cambio de color
        if(cont == 3)
                Thread.Sleep(5000);  // Pequeño retraso para mostrar el cambio de color
        if(cont == 4)
                Thread.Sleep(4000);  // Pequeño retraso para mostrar el cambio de color
        if(cont == 5)
                Thread.Sleep(0);  // Pequeño retraso para mostrar el cambio de color

    }

    // Historia
    private static void _history()
    {
        // Historia del juego
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("          ********//////// HISTORIA ////////********");
        string History1 = "\nAlbus Dumbledore anuncia, durante el banquete de bienvenida en "
            + "\nHogwarts, que la escuela será la sede del Torneo de los Tres "
            + "\nMagos, una competencia interescolar entre Hogwarts, la Academia "
            + "\nBeauxbatons y el Instituto Durmstrang. Un estudiante de cada una "
            + "\nde tres escuelas de magia sería elegido por el Cáliz de fuego para "
            + "\ncompetir en el torneo. Los campeones que elige el Cáliz son Fleur "
            + "\nDelacour de Beauxbatons, Viktor Krum de Durmstrang y Cedrics "
            + "\nDiggory de Hogwarts. Misteriosamente, Harry Potter también es "
            + "\nelegido, pese a que no había ingresado su nombre en el Cáliz del "
            + "\nfuego. Ahora son 4 los magos que deben enfrentarse a los desafíos "
            + "\ndel Torneo. ";
        PrintTextSlowly(History1, 5);
        Console.ResetColor();

        // Pausa antes de continuar
        Pause("\n\n\n\n\n\n\n\n\n\n\n\n\n\nPRESIONE UNA TECLA PARA CONTINUAR LA HISTORIA...");
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("          ********//////// HISTORIA ////////********");
        string History2 = "\nLa tercera y última prueba, en la que está basada este juego, "
            + "\nconsiste en un laberinto lleno de obstáculos mágicos diseñados "
            + "\npara confundir incluso a los magos más astutos, y como si no fuera  "
            + "\nsuficiente lo habitan monstruos como acromántulas sedientas de "
            + "\ncarne humana; boggarts que se alimentan del miedo; esfinges que "
            + "\ndesafían la mente con enigmas mortales y otras criaturas que "
            + "\nobstaculizan el camino a la legendaria Copa que se haya en el "
            + "\ncorazón del laberinto. La Copa otorga, a quien la alcance, 1000 "
            + "\ngaleones en concepto de premio. ";
        PrintTextSlowly(History2, 10);
        Console.ResetColor();

        // Pausa antes de continuar
        Pause("\n\n\n\n\n\n\n\n\n\n\n\n\n\nPRESIONE UNA TECLA PARA CONTINUAR LA HISTORIA...");
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("          ********//////// HISTORIA ////////********");
        string History3 = "\nEl jugador puede elegir entre ser bestia, y evitar de forma "
            + "\nsanguinaria que cualquiera de los magos se convierta en el "
            + "\nganador, o mago, y hacer uso de sus conocimientos magícos para "
            + "\nalcanzar la Copa y escapar triunfante. "; 
        PrintTextSlowly(History3, 10);
        Console.ResetColor();

        // Pausa antes de continuar
        Pause();
        Console.Clear();
    }

    // Leyenda del Juego (botones y objetivos)
    private static void _legend()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("////////////////////// TECLAS DEL JUEGO //////////////////////\n\n\n");
        Console.WriteLine("\n DESPLAZAMIENTO: FLECHA ARRIBA, FLECHA ABAJO, FLECHA IZQUIERDA, FLECHA DERECHA \n");
        Console.WriteLine("\n INFORMACION: \n I: INFORMACION DE LA FICHA           J: INFORMACION DE LA FACCION \n");
        Console.WriteLine("\n BOLSA: \n TAB: ACCEDER A LA BOLSA DEL JUGADOR (SOLO MAGOS)   \n 1, 2, 3: SELECCIONAR LOS OBJETOS EN ESAS POSICIONES RESPECTIVAMENTE \n");
        Console.WriteLine("\n ATACAR: E (CADA FICHA TIENE SU PROPIA DISTANCIA DE ATAQUE Y SI ATACA TERMINA EL TURNO DEL JUGADOR) \n");
        Console.WriteLine("\n RECOLECTAR OBJETOS: Q (SOLO MAGOS, DISTANCIA DE RECOLECCION: 1 CASILLA) \n");
        Console.WriteLine("\n USAR HABILIDAD DE LA FICHA: F (CADA FICHA TIENE UN TIEMPO DETERMINADO DE ENFRIAMIENTO) \n");
        Console.ResetColor();

        Pause();

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("////////////////////// LEYENDA //////////////////////\n\n\n");
        Console.WriteLine("Pared: ██ \nCamino:   \n\nMAGOS:\n \nFicha1: HP \nFicha2: CD \nFicha3: FD \nFicha4: VK \n\nMONSTRUOS:\n \nFicha5: Ac \nFicha6: Es \nFicha7: Bo \nFicha8: Bl ");
        Console.WriteLine("\nTrampa de vida(quita vida): ☠️(hace 20 pts de daño) \nTrampa de velocidad(quita velocidad): ❄️ \nTrampa de daño(quita daño): 💥");
        Console.WriteLine("\nCopa: Cu \nPosion de vida: He \nPosion de velocidad: Sp \nTijeras Magicas(sirve para abrir caminos): Sc \nEscoba: Br \nEscudo: Sh");
        Console.WriteLine("\nPortal/Salida: Do \nPiedra Filosofal(revive a los magos) PS \nTumba de Mago: Gr");
        Console.ResetColor();

        Pause();
        
        string text = ("\n EL LABERINTO CADA 8 CICLOS DE TURNOS DE AMBOS JUGADORES, SE MODIFICA;" 
        + "\n Si pasa por ensima de algun objeto excepto la copa, este sera eliminado si no lo coge"
        + "\n EL OBJETIVO DE LOS MAGOS ES OBTENER LA COPA Y ESCAPAR POR UNO DE LOS PORTALES;"
        + "\n EL OBJETIVO DE LOS MONSTRUOS ES EVITAR QUE ALGUN MAGO LOGRE ESCAPAR CON VIDA DEL LABERINTO," 
        + "\n EN CASO DE QUE PASE HABRAN FRACASADO SU MISION;" 
        + "\n LE ACONSEJAMOS A CADA JUGADOR QUE LEAN LA INFORMACION DE SU FACCION Y DE CADA FICHA,"
        + "\n PARA TENER MEJOR NOCION DE LAS CAPACIDADES DE LAS MISMAS");

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("\n\n\n ////////////////////// INFORMACION DEL LABERINTO //////////////////////\n\n\n");
        PrintTextSlowly(text, 5);
        Console.WriteLine("\n\n\nESPERO QUE SE DIVIERTAN MUCHO");
        Console.ResetColor();

        Pause("\n\n\n\n\n\n\n\n PRESIONE UNA TECLA PARA COMENZAR A JUGAR......");

    } 

    // Hace las pausas necesarias en el juego
    public static void Pause(string message = "\n\n\n\n\n\nPRESIONE UNA TECLA PARA CONTINUAR...")
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.ReadKey();
    }

}

