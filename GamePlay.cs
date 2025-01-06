using System;

class GamePlay
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Bienvenido al juego del Laberinto, espero que se diviertan");

        Console.WriteLine("Cual es su nombre jugador 1?");

        string name1 = Console.ReadLine()!;

        Console.WriteLine("Ya tengo tu nombre " + name1);

        Console.WriteLine("Cual es su nombre jugador 2?");

        string name2 = Console.ReadLine()!;

        Console.WriteLine("Ya tengo tu nombre " + name2);

        Console.WriteLine("Comiencen el juego");

        Players player1 = new Players(name1);
        
        Players player2 = new Players(name2);

        Maze laberinto = new Maze(player1, player2);

        laberinto.PrintMaze(player1, player2);

        bool run = player1.StartTurn();

        Players.Run(run, laberinto, player1, player2);
    }
}   

