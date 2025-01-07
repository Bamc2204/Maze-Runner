using System;

class GamePlay
{
    public static void Main(string[] args)
    {
        Console.WriteLine("BIENVENIDO AL JUEGO DEL LABERINTO, ESPERO QUE SE DIVIERTA");

        Console.WriteLine("\n Cual es su nombre jugador 1?");

        string name1 = Console.ReadLine()!;

        Console.WriteLine("\n Ya tengo tu nombre " + name1);

        Console.WriteLine("\n Cual es su nombre jugador 2?");

        string name2 = Console.ReadLine()!;

        Console.WriteLine("\n Ya tengo tu nombre " + name2); 

        Console.WriteLine("\n PRESIONE UNA TECLA PARA CONTINUAR");

        ConsoleKey Next = Console.ReadKey().Key;

        System.Console.WriteLine(" ** Historia **");     // Lore del juego *********************************************************************************

        Console.WriteLine("\n PRESIONE UNA TECLA PARA CONTINUAR");

        Next = Console.ReadKey().Key;

        Console.Clear();

        Console.WriteLine("\n Las fichas disponibles son:");

        Console.WriteLine("\n Los *MAGOS* escogidos por Las 3 Grandes Escuelas de Magia y Los *MONSTRUOS* que divagan por laberinto esperando algo que puedan matar");

        Console.WriteLine("\n El ***///1er JUGADOR///*** sera el que tenga la oportunidad de escoger primero, y el ***///2do JUGADOR///*** se quedará con la faccion restante");

        Console.WriteLine("\n Espero que ambos jugadores se puedan poner de acuerdo entre ambos antes de escoger");

        Console.WriteLine("\n PRESIONE UNA TECLA PARA CONTINUAR");

        Next = Console.ReadKey().Key;

        Console.Clear();
 
        Console.WriteLine("\n Presione la ***///FLECHA IZQUIERDA///*** si quiere ser *MAGO* o la ***///FLECHA DERECHA///*** si quiere ser *MONSTRUO*");    

        int indexPlayer1 = 0;

        int indexPlayer2 = 0;
        
        //Lee el teclado
        while(true)
        {
            ConsoleKey key = Console.ReadKey().Key;

            if(key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
            {
                Players._readBoard(key, ref indexPlayer1);
                break;
            }
            Console.WriteLine("\n No a tocado la tecla correcta, tiene que tocar o bien flecha izquierda o bien flecha derecha, intentelo otra vez");
        }

        Players player1 = new Players(name1, indexPlayer1);

        player1._ChooseFaction(ref indexPlayer1, ref indexPlayer2);
        
        Players player2 = new Players(name2, indexPlayer2);

        player1.CreateTokensFaction(player1, player2);
        
        Console.WriteLine("\n PRECIONE UNA TECLA PARA COMENZAR A JUGAR");

        Next = Console.ReadKey().Key;

        Console.Clear();

        Maze lab = new Maze(player1, player2);

        lab.PrintMaze(player1, player2);
        
        bool run1 = player1.StartTurn();

        bool run2 = player2.EndTurn();

        while(!((player1.InfoTokens(0)._target) && (player2.InfoTokens(0)._target) ))
        {
            if(!(player1.InfoTokens(0)._target) && run1)
            {
                Players.Run(ref run1, lab, player1, player2, ref player1.InfoTokens(0)._target);

                run1 = player1.EndTurn();

                run2 = player2.StartTurn();
            }
            else if(!(player2.InfoTokens(0)._target) && run2)
            {
                Players.Run(ref run1, lab, player1, player2, ref player2.InfoTokens(0)._target);

                run2 = player1.EndTurn();

                run1 = player2.StartTurn();
            }
        }
    }
}   

