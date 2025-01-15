using System;

class Tokens
{
    #region Propiedades de las fichas           ////////////////////////////////////////////////////////////////////////////////////////
    private string _name;                       // Nombre de ficha
    private int _id;                            // Identidad de la ficha
    private string _description;                // Descripcion de la ficha
    public int CoordX;                          // Coordenada X
    public int CoordY;                          // Coordenada Y
    private string _character;                  // Caracter de la ficha
    private int _health;                        // Salud
    private int _damage;                        // Daño
    private int _distAttack;                    // Distacia de ataque
    private string _skill;                      // Habilidad
    private bool _skillActivation = true;       // Verificador de Habilidad
    private int _coldTime = 0;                    // Tiempo de enfriamiento de habilidad
    private int _speed;                         // Velocidad para recorrer casillas
    private int[] _box = new int[3];            // Bolsa con objetos
    public bool ActiveShield = false;           // Escudo activo
    private int _shield = 0;                    // Escudo
    private bool _paralysis = false;            // Paralisis
    private  int _contTurnParalysis = 0;        // Contador de turnos de la paralisis
    private bool _poison = false;               // Veneno
    private int _contTurnPoison = 0;            // Contador de turnos del veneno

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Constructor de fichas           ////////////////////////////////////////////////////////////////////////////////////////

    // Creador de fichas
    public Tokens(string name, int id, string character, string Descripcion, int coordX, int coordY, string skill, int coldTime, int speed, 
    int obj1, int obj2, int obj3, int damage = 50, int distAttack = 4, int health = 100)
    {
        _name = name;
        _id = id;
        _character = character;
        _description = Descripcion;
        CoordX = coordX;
        CoordY = coordY;
        _skill = skill;
        _coldTime = coldTime;
        _speed = speed;
        _box[0] = obj1;
        _box[1] = obj2;
        _box[2] = obj3;
        _damage = damage;
        _distAttack = distAttack;
        _health = health;
    }
    
    //Sobrecarga de constructor
    public Tokens(string name, int id, string character, string Descripcion, int coordX, int coordY, string skill, int coldTime, int speed, int damage, int distAttack, int health = 400)
    {
        _name = name;
        _id = id;
        _character = character;
        _description = Descripcion;
        CoordX = coordX;
        CoordY = coordY;
        _skill = skill;
        _coldTime = coldTime;
        _speed = speed;
        _damage = damage;
        _distAttack = distAttack;
        _health = health;
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de la Bolsa           ////////////////////////////////////////////////////////////////////////////////////////

    // Objetos Bolsa
    public enum Objects
    {
        cup = -6,
        healthPotion = -7,
        speedPotion = -8,
        magicScissors = -9,
        broom = -10,
        shield = -11,
    }

    // Usar objetos bolsa
    public void UseBoxObject( ref int newX, ref int newY, ref Tokens token, Players player1, Players player2)
    {
        //Tecla q toca el jugador en el teclado            
        ConsoleKeyInfo key = Console.ReadKey();
        
        int index = key.KeyChar - '1'; //Posicion de la bolsa

        if(index >= 0 && index < 3 && _box[index] != 0)
        {
            Objects objeto = (Objects)_box[index];//Objeto de la bolsa
            switch (objeto)
            {
                // Posion de Vida
                case Objects.healthPotion: AddHealth(50); Console.WriteLine($"\n {_name} usó una poción de salud. Salud actual: {_health}"); _deleteObject(index); 
                    break;
                
                // Posion de Velocidad
                case Objects.speedPotion: AddSpeed(4); Console.WriteLine($"\n {_name} usó una poción de velocidad. Velocidad actual: {(_speed)}"); _deleteObject(index);    
                    break;
                
                // Escudo 
                case Objects.shield: Console.Write($"\n {_name}, "); Shield(ref token.ActiveShield); if(token.InfoShield() <= -100) {_deleteObject(index); Console.WriteLine("Has perdido el escudo");} 
                    break;
                
                // Tijera Magica
                case Objects.magicScissors: _magicScissors( ref newX, ref newY, ref token, player1, player2); Console.WriteLine($"\n {_name} va a usar una TIJERA MAGICA.");  Console.WriteLine("Se ha roto LA TIJERA MAGICA"); _deleteObject(index); 
                    break;

                // Escoba
                case Objects.broom: __broom(ref token, player1, player2); Console.WriteLine("HAS USADO LA ESCOBA"); _deleteObject(index);
                    break;
            }
             // Elimina el objeto de la bolsa
        }
        else
            System.Console.WriteLine("\n No hay nada en esa espacio de la bolsa");
    }

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de Habilidad           ////////////////////////////////////////////////////////////////////////////////////////
    
    // Recoger recursos
    public void Collect(Tokens token)
    {
        for (int i = -11; i < -5; i++)
        {
            bool canCollect = 
            Maze.GeneralMaze[token.CoordX + 1, token.CoordY] == i ||
            Maze.GeneralMaze[token.CoordX - 1, token.CoordY] == i ||
            Maze.GeneralMaze[token.CoordX , token.CoordY + 1] == i ||
            Maze.GeneralMaze[token.CoordX , token.CoordY - 1] == i;

            if(canCollect)
            {   
                for(int j = 0; j < 3; j++)
                {
                    if(token.InfoBox(j) == 0)
                    {
                        _saveObjects(i);
                        Console.WriteLine($"\nHas obtenido: {((Objects)i).ToString()}");
                        _deleteObject(token);
                        return;
                    }
                }
            }
        }
        Console.WriteLine("\nNO SE PUDO OBTENER NADA, PUEDE QUE NO TIENES ESPACIO EN LA BOLSA");
    }

    // Guardar recursos
    private void _saveObjects(int Object) 
    {
        for(int i = 0; i < _box.Length; i++) 
        {
            if(_box[i] == 0)
            {
                _box[i] = Object;
                break;
            }    
        }
    }

    // Elimina el objeto obtenido de la Bolsa
    private void _deleteObject(int index)
    {
        _box[index] = 0;
    }

    // Elimina el objeto obtenido del mapa
    private void _deleteObject(Tokens token)
    {
         for (int i = -11; i < -5; i++)
        {
            if(Maze.GeneralMaze[token.CoordX + 1, token.CoordY] == i)
            {
                Maze.GeneralMaze[token.CoordX + 1, token.CoordY] = 0;
                return;
            } 
            
            if(Maze.GeneralMaze[token.CoordX - 1, token.CoordY] == i)
            {
                Maze.GeneralMaze[token.CoordX - 1, token.CoordY] = 0;
                return;
            } 

            if(Maze.GeneralMaze[token.CoordX, token.CoordY + 1] == i)
            {
                Maze.GeneralMaze[token.CoordX, token.CoordY + 1] = 0;
                return;
            } 

            if(Maze.GeneralMaze[token.CoordX, token.CoordY - 1] == i)
            {
                Maze.GeneralMaze[token.CoordX, token.CoordY - 1] = 0;
                return;
            } 
        }
    }

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
        if(_speed < 2)
            _speed = 2;
    }
   
    // Metodo Escudo
    public void Shield(ref bool activeShield)
    {
        if(!activeShield)
        {
            Console.WriteLine(" El escudo se ha activado");
            _shield += 100;
            _speed -= 2;
            if(_shield < 0)
                Console.WriteLine("\n El escudo ya no funciona");
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

    // Metodo Tijera Magica
    private void _magicScissors( ref int newX, ref int newY, ref Tokens token, Players player1, Players player2)  
    {
        newX = token.CoordX;
        newY = token.CoordY;

        // Creado para poder usar el mismo metodo y no crear otro
        int step = 0;
        
        do                                                          //Minimetodo de Desplazamiento
        {
            //Tecla q toca el jugador en el teclado            
            ConsoleKey key = Console.ReadKey().Key;
            Players.ReadBoard(key, ref newX, ref newY, ref step, ref token, player1, player2);

            // Dentro de filas, columnas y si es un camino
            if (newX >= 0 && newX < Maze.GeneralMaze.GetLength(0) && newY >= 0 && newY < Maze.GeneralMaze.GetLength(1))                    
            {
                // Actualiza el tablero
                Maze.GeneralMaze[token.CoordX, token.CoordY] = 0;        // Vacía la posición actual
                Maze.GeneralMaze[newX, newY] = token.InfoId();                          // Mueve la ficha
                token.CoordX = newX;                               // Actualiza las coordenadas actuales
                token.CoordY = newY;
            }

            else
            {
                System.Console.WriteLine("\n los pasos no son validos");
                newX = token.CoordX; newY = token.CoordY;                    
            }

            Maze.PrintMaze(player1,  player2);//Imprime el laberinto
        }while(false);
    }
    
    // Metodo Escoba
    private void __broom(ref Tokens token, Players player1, Players player2)
    {
        int newX = token.CoordX;
        int newY = token.CoordY;

        ConsoleKey key = Console.ReadKey().Key;

        // Verifica q el jugador haya introducido una direccion
        if(key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow)
        {
            GamePlay.Pause("LA DIRECCION NO ES CORRECTA \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n PRESIONE UNA TECLA PARA CONTINUAR");
            return;
        }

        Players.ReadBoard(key, ref newX, ref newY);

        // Verifica los limites del mapa
        if(newX <= 0 || newX >= Maze.GeneralMaze.GetLength(0) - 1 || newY <= 0 || newY > Maze.GeneralMaze.GetLength(1) - 1)
        {
            GamePlay.Pause("VAS EN DIRECCION AL VACIO Q TE CAUSARA LA MUERTE (VAS FUERA DEL MAPA) \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n PRESIONE UNA TECLA PARA CONNTINUAR");
            return;
        }

        if(_checkRoad(newX, newY))
        {
            Maze.GeneralMaze[token.CoordX, token.CoordY] = 0;

            token.CoordX = newX;
            token.CoordY = newY;

            Maze.GeneralMaze[token.CoordX, token.CoordY] = token.InfoId();
            Maze.PrintMaze(player1, player2);
        }
    }

    // Metodo Verificar Camino
    private bool _checkRoad(int newX, int newY)
    {
        if(Maze.GeneralMaze[newX,  newY] == 0)
            return true;
        return false;
    }

    // Metodo para quitar fuerza
    public void RemoveDamage(int remove)
    {
        _damage -= remove;
        if(_damage < 0)
            _damage = 0;
    }

    //Metodo Atacar
    public void Attack(Tokens token, ref int step, ref Players player2, Players player1, int damage)
    {
        Console.WriteLine("\n En q direccion piensa atacar, presione una flecha para ver la direccion \n");
        
        ConsoleKey key = Console.ReadKey().Key;

        if(key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow)
        {
            Console.WriteLine("No ha atacado");
            return;
        }

        Players.ReadBoard(key, token, ref step, ref player2, damage, token.InfoDistAttack());

        Maze.PrintMaze(player1,player2);
    } 

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

    #region Metodos de Modificacion             ////////////////////////////////////////////////////////////////////////////////////////

    // Metodo para modificar el caracter
    public void ModifyCharacter(string newCharacter)
    {
        _character = newCharacter;
    }

    // Metodo para modificar la velocidad
    public void ModifySpeed(int newSpeed)
    {
        _speed = newSpeed;
    }

    //Metodo para modificar la vida
    public void ModifiHealth(int newHealth)
    {
        _health = newHealth;
    }

    // Metodo para modificar el daño
    public void ModifiDamage(int newDamage)
    {
        _damage = newDamage;
    }

    // Metodo para modificar la distacia de ataque
    public void ModifiDistAttack(int newDistAttack)
    {
        _distAttack = newDistAttack;
    }

    //  Metodo para modificar el Tiempo de enfriamiento
    public void ModifiColdTime(int newColdTime)
    {
        _coldTime = newColdTime;
    }

    // Disminuye el tiempo de enfriamiento
    public void MinusColdTime(int minus)
    {
        _coldTime -= minus;
    }

    // Metodo para modificar la propiedad paralisis
    public void ModifiParalysis(bool newParalysis)
    {
        _paralysis = newParalysis;
    }

    // Metodo para modificar el contador de turnos de la paralisis
    public void ModifiContTurnParalysis(int cont)
    {
        _contTurnParalysis +=  cont;
    }

    // Metodo para modificar la propiedad Veneno
    public void ModifiPoison(bool newPoison)
    {
        _poison = newPoison;
    }

    // Metodo para modificar el contador de turnos del Veneno
    public void ModifiContTurnPoison(int cont)
    {
        _contTurnPoison += cont;
    }

    #endregion              ////////////////////////////////////////////////////////////////////////////////////////

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
    
    // Informacion del Caracter de la Ficha
    public string InfoCharacter()
    {
        return _character;
    }

    // Informacion de la Vida
    public int InfoHealth()
    {
        return _health;
    }

    // Informacion del Daño
    public int InfoDamage()
    {
        return _damage;
    }

    //Informacion de la Distancia de Ataque
    public int InfoDistAttack()
    {
        return _distAttack;
    }

    // Informacion de la Velocidad
    public int InfoSpeed()
    {
        return _speed;
    }

    // Informacion de la Habilidad
    public string infoSkill()
    {
        return _skill;
    }

    // Informacion del Tiempo de Enfriamiento de la Habilidad
    public int InfoColdTime()
    {
        return _coldTime;
    }

    // Informacion del Escudo
    public int InfoShield()
    {
        return _shield;
    }
    
    // Informacion de la Bolsa
    public int InfoBox(int index)
    {
        return _box[index];
    }

    // Informacion de Paralisis
    public bool InfoParalysis()
    {
        return _paralysis;
    }
    
    // Informacion del contador de turnos de la Paralisis
    public int InfoContTurnParalysis()
    {
        return _contTurnParalysis;
    }
    
    // Informacion del Veneno
    public bool InfoPoison()
    {
        return _poison;
    }

    // Informacion del contador de turnos del veneno
    public int InfoContTurnPoison()
    {
        return _contTurnPoison;
    }

    // Metodo para mostrar todo soble la ficha
    public void DisplayStatus()
    {
        //Informacion del Nombre, Salud, Velocidad, Habilidad, Tiempo de Enfrimiento
        Console.WriteLine("\n ***///PROPIEDADES DE LA FICHA///***");
        Console.WriteLine($"\n FICHA: {_name} | DAÑO: {_damage} | SALUD: {_health} | VELOCIDAD: {_speed} | HABILIDAD: {_skill} | TIEMPO DE ENFRIAMIENTO: {_coldTime}"); 
        Console.WriteLine($"\n DESCRIPCION: \n{_description}\n");
        Console.WriteLine($"\n ***///OBJETOS///***"); 
        for(int i = 0; i < _box.Length; i++) 
        {
            Console.WriteLine($"\n Objeto #{(i + 1)}: {(Objects)_box[i]}");  //Objetos de la bolsa
        }
    }    

    #endregion          ////////////////////////////////////////////////////////////////////////////////////////

}