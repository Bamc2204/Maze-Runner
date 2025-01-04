using System;

class GamePlay
{
    public static void Main(string[] args)
    {
        System.Console.WriteLine("Holaaaaa");

        bool run;

        Tokens piece1 = new Tokens("Joseito", "P1", "Velocidad", 4, 3, 3, 4, 6);

        Tokens piece2 = new Tokens("Pedrito", "P2", "Velocidad", 4, 4, 4, 4, 6);

        Tokens piece3 = new Tokens("Pedrito", "P2", "Velocidad", 4, 8, 4, 4, 6);

        Tokens piece4 = new Tokens("Pedrito", "P4", "Velocidad", 4, 8, 4, 4, 6);

        Tokens piece21 = new Tokens("Joseito", "P1", "Velocidad", 4, 3, 3, 4, 6);

        Tokens piece22 = new Tokens("Pedrito", "P2", "Velocidad", 4, 4, 4, 4, 6);

        Tokens piece23 = new Tokens("Pedrito", "P2", "Velocidad", 4, 8, 4, 4, 6);

        Tokens piece24 = new Tokens("Pedrito", "P4", "Velocidad", 4, 8, 4, 4, 6);

        Players Bryan = new Players("Bryan");

        Bryan.AddTokens(piece1, piece2, piece3, piece4);
        
        Players Daniel = new Players("Daniel");

        Daniel.AddTokens(piece21, piece22, piece23, piece24);

        Maze laberinto = new Maze(Bryan, Daniel);

        laberinto.PrintMaze();

        run = Bryan.StartTurn();

        Players.Run(run, laberinto, Bryan, Daniel);
    }
}   


