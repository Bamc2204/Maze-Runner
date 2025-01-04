using System;

class GamePlay
{
    public static void Main(string[] args)
    {
        System.Console.WriteLine("Holaaaaa");

        bool running = true;

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

        int indexPiece;

        while(running)
        {    
            Console.WriteLine("Inroduzca cual de sus fichas va a coger (del 1 al 4)");

            indexPiece = int.Parse(Console.ReadLine()!);            
            
            Console.WriteLine("Ya puede desplazarse");

            _displacement(Bryan.SelectToken(indexPiece - 1).InfoSpeed(), laberinto, Bryan.SelectToken(indexPiece - 1), ref running);
        }
    }

    //Desplaza la ficha
    private static void _displacement(int steps, Maze lab, Tokens piece, ref bool running)
    {   
        lab._maze[piece._coordX, piece._coordY] = 2;
        int newX = piece._coordX;
        int newY = piece._coordY;

        while(steps != 0 && running)
        {
            //Si llegas al final del juego
            if(piece._coordX == lab._maze.GetLength(0) - 2 && piece._coordY == lab._maze.GetLength(1) - 1)//Si llego al final del laberinto o no
            {
                System.Console.WriteLine("Felicidades, Completaste El Laberinto");                
                running = false;
            }
                
            else
            {
                //Tecla q toca el jugador en el teclado            
                ConsoleKey key = Console.ReadKey().Key;

                _readBoard(key, lab, ref newX, ref newY, ref running, ref piece);

                _checkTrap(lab, ref newX, ref newY, ref running, ref piece);

                // Dentro de filas, columnas y si es un camino
                if (newX >= 0 && newX < lab._maze.GetLength(0) && newY >= 0 && newY < lab._maze.GetLength(1) && 
                lab._maze[newX, newY] != 1 && lab._maze[newX, newY] != 2)                    
                {
                    // Actualiza el tablero
                    lab._maze[piece._coordX, piece._coordY] = 0;        // Vacía la posición actual
                    lab._maze[newX, newY] = 2;                          // Mueve la ficha
                    piece._coordX = newX;                               // Actualiza las coordenadas actuales
                    piece._coordY = newY;
                    steps --;                                           //Pasos q puede recorer cada ficha
                }

                else
                {
                    System.Console.WriteLine("los pasos no son validos");
                    newX = piece._coordX; newY = piece._coordY;                    
                }

                lab.PrintMaze();//Imprime el laberinto
            }
        }  
    }
    //Lee el teclado
    public static void _readBoard(ConsoleKey key, Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens piece) 
    {
        //Casos para cada tecla
        switch (key)
        {
            case ConsoleKey.UpArrow:    newX = piece._coordX - 1; break;
            case ConsoleKey.DownArrow:  newX = piece._coordX + 1; break;
            case ConsoleKey.LeftArrow:  newY = piece._coordY - 1; break;
            case ConsoleKey.RightArrow: newY = piece._coordY + 1; break;
            case ConsoleKey.Escape: Console.WriteLine("Simulación detenida."); running = false; break;
            case ConsoleKey.Tab: piece._useBoxObject(lab, ref newX, ref newY, ref piece); break;
            case ConsoleKey.I: piece.DisplayStatus(); break;
        }
    }

    //Chequea si caiste en una trampa
    public static void _checkTrap(Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens piece)
    {
        if(lab._maze[newX, newY] == 3) //Verificacion de trapas
        {
            piece.RemoveHealth(20);
            System.Console.WriteLine("Has caido en una trampa y has perdido 20 puntos de vidas");
            if(piece.InfoHealth() == 0)
            {   
                running = false;
                System.Console.WriteLine("Has muerto");
            }
        }
    }
}   


