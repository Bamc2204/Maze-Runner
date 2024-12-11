using System;

class GamePlay
{
    public static void Main(string[] args)
    {
        System.Console.WriteLine("Holaaaaa");

        Maze laberinto = new Maze();

        Tokens pieza1 = new Tokens("Joseito", 1, 0, "PP", "Velocidad", 4, 3, 4, 5, 6);

        laberinto.ImprimirLaberinto();

        Desplazamiento(10000, laberinto, pieza1);
    }

    //Desplaza la ficha
    public static void Desplazamiento(int pasos, Maze lab, Tokens pieza)
    {   
        lab._maze[pieza._coordX, pieza._coordY] = 2;
        int newX = pieza._coordX;
        int newY = pieza._coordY;
        bool running = true;

        while(pasos != 0 && running)
        {
            //Si llegas al final del juego
            if(pieza._coordX == lab._maze.GetLength(0) - 2 && pieza._coordY == lab._maze.GetLength(1) - 1)//Si llego al final del laberinto o no
            {
                System.Console.WriteLine("Felicidades, Completaste El Laberinto");
                running = false;
            }
                
            else
            {
                //Tecla q toca el jugador en el teclado            
                ConsoleKey key = Console.ReadKey().Key;

                _readBoard(key, lab, ref pieza._coordX, ref pieza._coordY, ref newX, ref newY, ref running, ref pieza);

                // Dentro de filas, columnas y si es un camino
                if (newX >= 0 && newX < lab._maze.GetLength(0) && newY >= 0 && newY < lab._maze.GetLength(1) && lab._maze[newX, newY] != 1 )                    
                {
                    // Actualiza el tablero
                    lab._maze[pieza._coordX, pieza._coordY] = 0;        // Vacía la posición actual
                    lab._maze[newX, newY] = 2;            // Mueve la ficha
                    pieza._coordX = newX;                 // Actualiza las coordenadas actuales
                    pieza._coordY = newY;
                    pasos --;                  //Pasos q puede recorer cada ficha
                }

                else
                {
                    System.Console.WriteLine("los pasos no son validos");
                    newX = pieza._coordX; newY = pieza._coordY;                    
                }

                lab.ImprimirLaberinto();//Imprime el laberinto
            }
        }   
    }

    public static void _readBoard(ConsoleKey key, Maze lab, ref int x, ref int y, ref int newX, ref int newY, ref bool running, ref Tokens pieza) 
    {
        //Casos para cada tecla
        switch (key)
        {
            case ConsoleKey.UpArrow:    newX = x - 1; break;
            case ConsoleKey.DownArrow:  newX = x + 1; break;
            case ConsoleKey.LeftArrow:  newY = y - 1; break;
            case ConsoleKey.RightArrow: newY = y + 1; break;
            case ConsoleKey.Escape: Console.WriteLine("Simulación detenida."); running = false; break;
            case ConsoleKey.Tab: pieza._useBoxObject(lab, ref x, ref y, ref newX, ref newY, ref pieza); break;
        }
    }
}   


