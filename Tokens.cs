using System;

class Tokens
{
    #region Propiedades de las fichas
    private string _name;                       // Nombre de ficha
    private int _id;                            // Identidad de la ficha
    public int _coordX;                         // Coordenada X
    public int _coordY;                         // Coordenada Y
    private string _character;                  // Caracter de la ficha
    private int _health;                        // Salud
    private string _skill;                      // Habilidad
    private bool _skillActivation = true;       // Verificador de Habilidad
    private int _coldTime;                      // Tiempo de enfriamiento de habilidad
    private int _speed;                         // Velocidad para recorrer casillas
    private int[] _box = new int[3];            // Bolsa con objetos
    public bool _target = false;                // Objetivo
    #endregion

    // Creador de fichas
    public Tokens(string name, int id, string character, int coordX, int coordY, string skill, int coldTime, int speed, 
    int obj1, int obj2, int obj3, int health = 100)
    {
        Random random = new Random();
        _name = name;
        _id = id;
        _coordX = coordX;
        _coordY = coordY;
        _character = character;
        _health = health;
        _skill = skill;
        _coldTime = coldTime;
        _speed = speed;
        _box[0] = obj1;
        _box[1] = obj2;
        _box[2] = obj3;
    }
    
    //Sobrecarga de constructor
    public Tokens(string name, int id, string character, string skill, int coldTime, int speed, int health = 400)
    {
        Random random = new Random();
        _name = name;
        _id = id;
        _coordX = random.Next(10, 30);
        _coordY = random.Next(10, 30);
        _character = character;
        _health = health;
        _skill = skill;
        _coldTime = coldTime;
        _speed = speed;
    }

    // Objetos Bolsa
    enum Objets
    {
        healthPotion = 3,
        speedPotion = 4,
        shield = 5,
        pick = 6
    }

    // Recoger recursos
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

    // Usar objetos bolsa*****
    public void _useBoxObject(Maze lab, ref int newX, ref int newY, ref Tokens piece, Players player1, Players player2)
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
                    _health += 20;
                    Console.WriteLine($"{_name} usó una poción de salud. Salud actual: {_health}");
                    break;
                case Objets.speedPotion:
                    _speed += 4;
                    Console.WriteLine($"{_name} usó una poción de velocidad. Velocidad actual: {(_speed - 1)}");
                    break;
                case Objets.shield:
                    Console.WriteLine($"{_name} usó un escudo.");
                    break;
                case Objets.pick:
                    _beak(lab, ref newX, ref newY, ref piece, player1, player2);
                    Console.WriteLine($"{_name} va a usar un pico.");
                    break;
            }
            _box[index] = 0; // Elimina el objeto de la bolsa
        }
        else
            System.Console.WriteLine("No hay nada en esa espacio de la bolsa");
    }

    // Metodo de la herramenta pico*****
    private void _beak(Maze lab, ref int newX, ref int newY, ref Tokens piece, Players player1, Players player2)  
    {
        newX = piece._coordX;
        newY = piece._coordY;
        bool running = true;
        do                                                          //Minimetodo de Desplazamiento
        {
            //Tecla q toca el jugador en el teclado            
            ConsoleKey key = Console.ReadKey().Key;
            Players._readBoard(key, lab, ref newX, ref newY, ref running, ref piece, player1, player2);

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

            lab.PrintMaze(player1,  player2);//Imprime el laberinto
        }while(false);
    }
    
    // Informacion de la Identidad
    public int InfoId()
    {
        return _id;
    }

    // Informacion del caracter de la ficha
    public string InfoCharacter()
    {
        return _character;
    }

    // informacion de la vida
    public int InfoHealth()
    {
        return _health;
    }

    // informacion de la velocidad
    public int InfoSpeed()
    {
        return _speed;
    }

    // Metodo para agregar salud
    public void AddHealth(int add)
    {
        _health += add;
    }

    // Metodo para quitar a la salud
    public void RemoveHealth(int remove) 
    {
        _health -= remove;    
    }

    // Metodo para mostrar todo soble la ficha
    public void DisplayStatus()
    {
        //Informacion del Nombre, Salud, Velocidad, Habilidad, Tiempo de Enfrimiento
        Console.WriteLine($"Ficha: {_name} | Salud: {_health} | Velocidad: {_speed} | Habilidad: {_skill} | Tiempo de enfriamiento: {_coldTime}"); 
        for(int i = 0; i < _box.Length; i++) 
        {
            Console.WriteLine($"Objeto {(i+1)}: {(Objets)_box[i]}" );   //Objetos de la bolsa
        }
    }
}
