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
    public void _useBoxObject(ref bool pico)
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
                    pico = true;
                    Console.WriteLine($"{_name} va a usar un pico.");
                    break;
            }
            _box[index] = 0; // Elimina el objeto de la bolsa
        }
        else
            System.Console.WriteLine("No hay nada en esa espacio de la bolsa");

    }
}
