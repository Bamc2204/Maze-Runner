using System;

class Tokens
{
    //Propiedades de las fichas
    private string _name; //Nombre de ficha
    public int _coordX; //Coordenada X
    public int _coordY; //Coordenada Y
    private string _character;
    public int health; //Salud
    private string _skill; //Habilidad
    private bool _skillActivation = true; //Verificador de Habilidad
    private int _coldTime; //Tiempo de enfriamiento de habilidad
    public int _speed; //Velocidad para recorrer casillas
    private int[] _box = new int[3]; //Bolsa con objetos
    private bool _goals = false; //Objetivo

    //Creador de fichas
    public Tokens(string name, int coordX, int coordY, string character, string skill, int coldTime, int speed, 
    int obj1 = 0, int obj2 = 0, int obj3 = 0, int health = 100)
    {
        _name = name;
        _coordX = coordX;
        _coordY = coordY;
        _character = character;
        this.health = health;
        _skill = skill;
        _coldTime = coldTime;
        _speed = speed;
        _box[0] = obj1;
        _box[1] = obj2;
        _box[2] = obj3;
    }

    //Objetos Bolsa
    enum Objets
    {
        healthPotion = 3,
        speedPotion = 4,
        shield = 5,
        pick = 6
    }

    //Recoger recursos
    private void _collect(Objets objet) 
    {
        for(int i = 0; i < _box.Length; i++) 
        {
            if(_box[i] == 0)
            {
                _box[i] = (int)objet;
                break;
            }    
        }
    }

    //Usar objetos bolsa
    public void _useBoxObject(Maze lab, ref int newX, ref int newY, ref Tokens piece)
    {
        //Tecla q toca el jugador en el teclado            
        ConsoleKeyInfo key = Console.ReadKey();
        int index = key.KeyChar - '1'; //Posicion de la bolsa
        if(index >= 0 && index < 3 && _box[index] != 0)
        {
            Objets objeto = (Objets)_box[index];//Objeto de la bolsa
            switch (objeto)
            {
                case Objets.healthPotion:
                    health += 20;
                    Console.WriteLine($"{_name} usó una poción de salud. Salud actual: {health}");
                    break;
                case Objets.speedPotion:
                    _speed += 4;
                    Console.WriteLine($"{_name} usó una poción de velocidad. Velocidad actual: {(_speed - 1)}");
                    break;
                case Objets.shield:
                    Console.WriteLine($"{_name} usó un escudo.");
                    break;
                case Objets.pick:
                    _beak(lab, ref newX, ref newY, ref piece);
                    Console.WriteLine($"{_name} va a usar un pico.");
                    break;
            }
            _box[index] = 0; // Elimina el objeto de la bolsa
        }
        else
            System.Console.WriteLine("No hay nada en esa espacio de la bolsa");
    }

    //Metodo de la herramenta pico
    private void _beak(Maze lab, ref int newX, ref int newY, ref Tokens piece)  //Minimetodo de Desplazamiento
    {
        newX = piece._coordX;
        newY = piece._coordY;
        bool running = true;
        
        do
        {
            //Tecla q toca el jugador en el teclado            
            ConsoleKey key = Console.ReadKey().Key;
            GamePlay._readBoard(key, lab, ref newX, ref newY, ref running, ref piece);

            // Dentro de filas, columnas y si es un camino
            if (newX >= 0 && newX < lab._maze.GetLength(0) && newY >= 0 && newY < lab._maze.GetLength(1))                    
            {
                // Actualiza el tablero
                lab._maze[piece._coordX, piece._coordY] = 0;        // Vacía la posición actual
                lab._maze[newX, newY] = 2;                          // Mueve la ficha
                piece._coordX = newX;                               // Actualiza las coordenadas actuales
                piece._coordY = newY;
            }

            else
            {
                System.Console.WriteLine("los pasos no son validos");
                newX = piece._coordX; newY = piece._coordY;                    
            }

            lab.PrintMaze();//Imprime el laberinto
        }while(false);
    }

}
