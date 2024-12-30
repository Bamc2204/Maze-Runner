using System;

class GamePlay
{
    public static void Main(string[] args)
    {
        System.Console.WriteLine("Holaaaaa");

        Maze laberinto = new Maze();

        Tokens piece1 = new Tokens("Joseito", 1, 0, "PP", "Velocidad", 4, 3, 3, 4, 6);

        laberinto.PrintMaze();

        Displacement(10000, laberinto, piece1);
    }

    //Desplaza la ficha
    public static void Displacement(int pasos, Maze lab, Tokens piece)
    {   
        lab._maze[piece._coordX, piece._coordY] = 2;
        int newX = piece._coordX;
        int newY = piece._coordY;
        bool running = true;

        while(pasos != 0 && running)
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

                _checkTricks(lab, ref newX, ref newY, ref running, ref piece);

                // Dentro de filas, columnas y si es un camino
                if (newX >= 0 && newX < lab._maze.GetLength(0) && newY >= 0 && newY < lab._maze.GetLength(1) && lab._maze[newX, newY] != 1 )                    
                {
                    // Actualiza el tablero
                    lab._maze[piece._coordX, piece._coordY] = 0;        // Vacía la posición actual
                    lab._maze[newX, newY] = 2;                          // Mueve la ficha
                    piece._coordX = newX;                               // Actualiza las coordenadas actuales
                    piece._coordY = newY;
                    pasos --;                                           //Pasos q puede recorer cada ficha
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
        }
    }

    public static void _checkTricks(Maze lab, ref int newX, ref int newY, ref bool running, ref Tokens piece)
    {
        if(lab._maze[newX, newY] == 3) //Verificacion de trapas
        {
            piece.health -= 20;
            System.Console.WriteLine("Has caido en una trampa y has perdido 20 puntos de vidas");
            if(piece.health == 0)
            {   
                running = false;
                System.Console.WriteLine("Has muerto");
            }
        }
    }
}   


