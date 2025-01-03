using System;

class Players
{
    private string _name;
    private bool _myTurn;               //Turno de la ficha
    private Tokens[] _token;            //Cantidad de fichas (hasta 4)   
    private int _selectToken;           //Indice de la ficha seleccionado por el jugador

    //Constructor del Jugador
    public Players(string name)
    {
        _name = name;                   //Nombre del jugador
        _myTurn = false;                //Todos los turnos empiezan falso
        _token = new Tokens[4];         //Cantidad de fichas
        _selectToken = -1;               //No se ha seleccionado fichas
    }

    //Metodo para coger las fichas del jugador
    public void AddTokens(Tokens token1, Tokens token2, Tokens token3, Tokens token4)
    {
        _token[0] = token1;
        _token[1] = token2;
        _token[2] = token3;
        _token[3] = token4;
    }

    //Metodo para seleccionar ficha
    public void SelectToken(int index) 
    {
        if(index > -1 && index < 4)
            _selectToken = index;
        else
            System.Console.WriteLine("No existe esa ficha");    
    }

    //Metodo para iniciar el turno del jugador
    public void StartTurn()
    {
        _myTurn = true;
    }

    //Terminar el turno
    public void EndTurn() 
    {
        _myTurn = false;    
    }
}