using System;

namespace GamePlay
{
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
            int x = 1;
            int y = 0;
            int newX = x;
            int newY = y;
            bool running = true;
            bool pico = false;

            while(pasos != 0 && running)
            {
                //Si llegas al final del juego
                if(x == lab._maze.GetLength(0) - 2 && y == lab._maze.GetLength(1) - 1)//Si llego al final del laberinto o no
                {
                    System.Console.WriteLine("Felicidades, Completaste El Laberinto");
                    running = false;
                }
                
                else
                {
                    //Tecla q toca el jugador en el teclado            
                    ConsoleKey key = Console.ReadKey().Key;

                    //Casos para cada tecla
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:    newX = x - 1; break;
                        case ConsoleKey.DownArrow:  newX = x + 1; break;
                        case ConsoleKey.LeftArrow:  newY = y - 1; break;
                        case ConsoleKey.RightArrow: newY = y + 1; break;
                        case ConsoleKey.Escape: Console.WriteLine("Simulación detenida."); running = false; break;
                        case ConsoleKey.Tab: pieza._useBoxObject(ref pico); break;
                    }

                    // Dentro de filas, columnas y si es un camino
                    if (newX >= 0 && newX < lab._maze.GetLength(0) && newY >= 0 && newY < lab._maze.GetLength(1) && (lab._maze[newX, newY] != 1 ||(lab._maze[newX, newY] == 1 && pico == true && pasos > 0)))                    
                    {
                        // Actualiza el tablero
                        lab._maze[x, y] = 0;        // Vacía la posición actual
                        lab._maze[newX, newY] = 2;  // Mueve la ficha
                        x = newX;                 // Actualiza las coordenadas actuales
                        y = newY;
                        pasos --;                  //Pasos q puede recorer cada ficha

                        //Cambiar cuando esten terminadas las fichas
                        if(pasos == 1)
                            pico = false;  
                        else if(pico)
                            pasos = 2;
                        else
                            pasos = 1000;
                    }

                    else
                    {
                        System.Console.WriteLine("los pasos no son validos");
                        newX = x; newY = y;                    
                    }

                    lab.ImprimirLaberinto();//Imprime el laberinto
                }
            }   
        }
    }   
}

