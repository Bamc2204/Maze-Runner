using System;

class Tokens
{
    #region Propiedades de las fichas           ////////////////////////////////////////////////////////////////////////////////////////
    private string _name;                       // Nombre de ficha
    private int _id;                            // Identidad de la ficha
    public int _coordX;                         // Coordenada X
    public int _coordY;                         // Coordenada Y
    private string _character;                  // Caracter de la ficha
    public int _health;                         // Salud
    private int _damage;                        // Daño
    private int _distAttack;                    // Distacia de ataque
    private string _skill;                      // Habilidad
    private bool _skillActivation = true;       // Verificador de Habilidad
    private int _coldTime;                      // Tiempo de enfriamiento de habilidad
    private int _speed;                         // Velocidad para recorrer casillas
    private int[] _box = new int[3];            // Bolsa con objetos
    public bool _activeShield = false;          // Escudo activo
    private int _shield = 0;                    // Escudo
    public bool _target = false;                // Objetivo

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Constructor de fichas           ////////////////////////////////////////////////////////////////////////////////////////

    // Creador de fichas
    public Tokens(string name, int id, string character, int coordX, int coordY, string skill, int coldTime, int speed, 
    int obj1, int obj2, int obj3, int damage = 50, int distAttack = 4, int health = 100)
    {
        _name = name;
        _id = id;
        _coordX = coordX;
        _coordY = coordY;
        _character = character;
        _health = health;
        _damage = damage;
        _distAttack = distAttack;
        _skill = skill;
        _coldTime = coldTime;
        _speed = speed;
        _box[0] = obj1;
        _box[1] = obj2;
        _box[2] = obj3;
    }
    
    //Sobrecarga de constructor
    public Tokens(string name, int id, string character, int coordX, int coordY, string skill, int coldTime, int speed, int damage, int distAttack, int health = 400)
    {
        Random random = new Random();
        _name = name;
        _id = id;
        _coordX = coordX;
        _coordY = coordY;
        _character = character;
        _health = health;
        _damage = damage;
        _distAttack = distAttack;
        _skill = skill;
        _coldTime = coldTime;
        _speed = speed;
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de la Bolsa           ////////////////////////////////////////////////////////////////////////////////////////

    // Objetos Bolsa
    enum Objets
    {
        healthPotion = 3,
        speedPotion = 4,
        shield = 5,
        pick = 6
    }

    // Usar objetos bolsa*****************************************************************
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
                case Objets.healthPotion: AddHealth(50); Console.WriteLine($"\n {_name} usó una poción de salud. Salud actual: {_health}"); _box[index] = 0; 
                    break;
                
                case Objets.speedPotion: AddSpeed(4); Console.WriteLine($"\n {_name} usó una poción de velocidad. Velocidad actual: {(_speed - 1)}"); 
                    _box[index] = 0;    break;
                
                case Objets.shield: Console.Write($"\n {_name}, "); Shield(ref piece._activeShield); 
                    break;
                
                case Objets.pick: _beak(lab, ref newX, ref newY, ref piece, player1, player2); Console.WriteLine($"\n {_name} va a usar un pico.");
                    Console.WriteLine("Se ha roto el pico"); _box[index] = 0;break;
            }
             // Elimina el objeto de la bolsa
        }
        else
            System.Console.WriteLine("\n No hay nada en esa espacio de la bolsa");
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

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de Habilidad           ////////////////////////////////////////////////////////////////////////////////////////
    
    // Metodo para agregar salud
    public void AddHealth(int add)
    {
        _health += add;
    }

    // Metodo para quitar a la salud
    public void RemoveHealth(int remove, bool activeShield) 
    {
        if(activeShield && _shield > 0)
            _shield -= remove;
        else
            _health -= remove;
    }

    // Sobre carga del Metodo para quitar salud
    public void RemoveHealth(int remove)
    {
        _health -= remove;
    }

    // Metodo para agregar velocidad
    public void AddSpeed(int add)
    {
        _speed += add;
    }

    // Metodo para quitar velocidad
    public void SlowDown(int remove)
    {
        _speed -= remove;
    }
   
    // Metodo Escudo
    public void Shield(ref bool activeShield)
    {
        if(!activeShield)
        {
            Console.WriteLine(" El escudo se ha activado");
            _shield += 100;
            _speed -= 3;
            if(_shield < 0)
                System.Console.WriteLine("\n El escudo ya no funciona");
            activeShield = true;
        }
        else
        {
            Console.WriteLine("\n El escudo se ha desactivado");
            _shield -= 100;
            _speed += 3;
            activeShield = false;
        }
    }

    // Metodo Pico
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
            if (newX >= 0 && newX < Maze._maze.GetLength(0) && newY >= 0 && newY < Maze._maze.GetLength(1))                    
            {
                // Actualiza el tablero
                Maze._maze[piece._coordX, piece._coordY] = 0;        // Vacía la posición actual
                Maze._maze[newX, newY] = 2;                          // Mueve la ficha
                piece._coordX = newX;                               // Actualiza las coordenadas actuales
                piece._coordY = newY;
            }

            else
            {
                System.Console.WriteLine("\n los pasos no son validos");
                newX = piece._coordX; newY = piece._coordY;                    
            }

            Maze.PrintMaze(player1,  player2);//Imprime el laberinto
        }while(false);
    }
    
    // Metodo para quitar fuerza
    public void RemoveDamage(int remove)
    {
        _damage -= remove;
    }

    //Metodo Atacar
    public void Attack(Tokens token, ref Players player, int damage)
    {
        Console.WriteLine("\n En q direccion piensa atacar, presione una flecha para ver la direccion \n");
        
        ConsoleKey key = Console.ReadKey().Key;
        
        for(int i = 0; i < 4; i++)
            Console.WriteLine("\n " + player._tokens[i].InfoHealth());

        if(key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow)
        {
            Console.WriteLine("No ha atacado");
            return;
        }

        Players._readBoard(key, token, ref player, damage);

        for(int i = 0; i < 4; i++)
            Console.WriteLine("\n " + player._tokens[i].InfoHealth());
    } 

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de Informacion        ////////////////////////////////////////////////////////////////////////////////////////
    
    // Informacion de la Identidad
    public int InfoId()
    {
        return _id;
    }

    // Informacion del Nombre
    public string InfoName()
    
    {
        return _name;
    }
    
    // Informacion del caracter de la ficha
    public string InfoCharacter()
    {
        return _character;
    }

    // Informacion de la vida
    public int InfoHealth()
    {
        return _health;
    }

    // Informacion del daño
    public int InfoDamage()
    {
        return _damage;
    }

    //Informacion de la Distancia de Ataque
    public int InfoDistAttack()
    {
        return _distAttack;
    }

    // Informacion de la velocidad
    public int InfoSpeed()
    {
        return _speed;
    }

    // Metodo para mostrar todo soble la ficha
    public void DisplayStatus()
    {
        //Informacion del Nombre, Salud, Velocidad, Habilidad, Tiempo de Enfrimiento
        Console.WriteLine($"\n Ficha: {_name} | Daño: {_damage} | Salud: {_health} | Velocidad: {_speed} | Habilidad: {_skill} | Tiempo de enfriamiento: {_coldTime}"); 
        for(int i = 0; i < _box.Length; i++) 
        {
            Console.WriteLine($"\n Objeto {(i + 1)}: {(Objets)_box[i]}" );   //Objetos de la bolsa
        }
    }
    
    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

}