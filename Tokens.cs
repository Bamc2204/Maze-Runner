using System;

class Tokens
{
    //Propiedades de las fichas
    private string _name; //Nombre de ficha
    private int _coordX; //Coordenada X
    private int _coordY; //Coordenada Y
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
}
