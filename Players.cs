using System;

class Players
{
    #region Propiedades
    private string _name;
    private bool _myTurn;               //Turno de la ficha
    private Tokens[] _token;            //Cantidad de fichas (hasta 4)   
    private int _selectToken;           //Indice de la ficha seleccionado por el jugador
    #endregion

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

    //Metodo para acceder a las fichas sin modificarlas
    public Tokens InfoTokes(int index)
    {
        return _token[index];
    }

    //Metodo para seleccionar ficha
    public Tokens SelectToken(int index) 
    {
        return _token[index];    
    }

    //Informacion del turno del jugador
    public bool InfoTurn()
    {
        return _myTurn;
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