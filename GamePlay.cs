using System;

namespace GamePlay
{
    class GamePlay
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Holaaaaa");

            Maze laberinto = new Maze();

            laberinto.ImprimirLaberinto();

            laberinto.Desplazamiento(10000);
        }
    }   
}

