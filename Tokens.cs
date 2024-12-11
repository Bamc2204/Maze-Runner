using System;

class Tokens
{
    //Propiedades de las fichas
    private string _name; //Nombre de ficha
    public int _coordX; //Coordenada X
    public int _coordY; //Coordenada Y
    private string _caracter;
    public int health; //Salud
    private string _skill; //Habilidad
    private bool _skillActivation = true; //Verificador de Habilidad
    private int _coldTime; //Tiempo de enfriamiento de habilidad
    private int _speed; //Velocidad para recorrer casillas
    private int[] _box = new int[3]; //Bolsa con objetos
    private bool _goals = false; //Objetivo

    //Creador de fichas
    public Tokens(string name, int coordX, int coordY, string caracter, string skill, int coldTime, int speed, 
    int obj1 = 0, int obj2 = 0, int obj3 = 0, int health = 100)
    {
        _name = name;
        _coordX = coordX;
        _coordY = coordY;
        _caracter = caracter;
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
    private void Recoger(Objets objet) 
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
    public void _useBoxObject(Maze lab, ref int x, ref int y, ref int newX, ref int newY, ref Tokens pieza)
    {
        //Tecla q toca el jugador en el teclado            
        ConsoleKeyInfo key = Console.ReadKey();
        int index = key.KeyChar - '1';
        if(index >= 0 && index < 3 && _box[index] != 0)
        {
            Objets objeto = (Objets)_box[index];
            switch (objeto)
            {
                case Objets.healthPotion:
                    health += 20;
                    Console.WriteLine($"{_name} usó una poción de salud. Salud actual: {health}");
                    break;
                case Objets.speedPotion:
                    _speed += 2;
                    Console.WriteLine($"{_name} usó una poción de velocidad. Velocidad actual: {_speed}");
                    break;
                case Objets.shield:
                    Console.WriteLine($"{_name} usó un escudo.");
                    break;
                case Objets.pick:
                    _pico(lab, ref x, ref y, ref newX, ref newY, ref pieza);
                    Console.WriteLine($"{_name} va a usar un pico.");
                    break;
            }
            _box[index] = 0; // Elimina el objeto de la bolsa
        }
        else
            System.Console.WriteLine("No hay nada en esa espacio de la bolsa");
    }

    //Metodo de la herramenta pico
        private void _pico(Maze lab, ref int x, ref int y, ref int newX, ref int newY, ref Tokens pieza)  //Mini metodo de Desplazamiento
    {
        newX = x;
        newY = y;
        bool running = true;
        
        for(int i = 0; i < 3; i++)
        {
            //Tecla q toca el jugador en el teclado            
            ConsoleKey key = Console.ReadKey().Key;
            GamePlay._readBoard(key, lab, ref x, ref y, ref newX, ref newY, ref running, ref pieza);

            // Dentro de filas, columnas y si es un camino
            if (newX >= 0 && newX < lab._maze.GetLength(0) && newY >= 0 && newY < lab._maze.GetLength(1))                    
            {
                // Actualiza el tablero
                lab._maze[x, y] = 0;        // Vacía la posición actual
                lab._maze[newX, newY] = 2;  // Mueve la ficha
                x = newX;                 // Actualiza las coordenadas actuales
                y = newY;
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
