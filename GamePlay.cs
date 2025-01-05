using System;

class GamePlay
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Holaaaaa");

        Console.WriteLine("Yo soy el tanke");

        Tokens piece1 = new Tokens("Joseito", 1, "B1", "Velocidad", 4, 3, 3, 4, 6);

        Tokens piece2 = new Tokens("Pedrito", 2, "B2", "Velocidad", 4, 4, 4, 4, 6);

        Tokens piece3 = new Tokens("Pedrito", 3, "B3", "Velocidad", 4, 8, 4, 4, 6);

        Tokens piece4 = new Tokens("Pedrito", 4, "B4", "Velocidad", 4, 8, 4, 4, 6);

        Tokens piece21 = new Tokens("Joseito", 5, "D1", "Velocidad", 4, 3, 3, 4, 6);

        Tokens piece22 = new Tokens("Pedrito", 6, "D2", "Velocidad", 4, 4, 4, 4, 6);

        Tokens piece23 = new Tokens("Pedrito", 7, "D3", "Velocidad", 4, 8, 4, 4, 6);

        Tokens piece24 = new Tokens("Pedrito", 8, "D4", "Velocidad", 4, 8, 4, 4, 6);

        Players Bryan = new Players("Bryan");

        Bryan.AddTokens(piece1, piece2, piece3, piece4);
        
        Players Daniel = new Players("Daniel");

        Daniel.AddTokens(piece21, piece22, piece23, piece24);

        Maze laberinto = new Maze(Bryan, Daniel);

        laberinto.PrintMaze(Bryan.InfoTokens(), Daniel.InfoTokens());

        bool run = Bryan.StartTurn();

        Players.Run(run, laberinto, Bryan, Daniel);
    }
}   


