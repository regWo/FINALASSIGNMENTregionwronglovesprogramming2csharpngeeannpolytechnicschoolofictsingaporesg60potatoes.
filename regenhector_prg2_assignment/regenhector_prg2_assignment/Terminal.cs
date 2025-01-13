namespace regenhector_prg2_assignment;

public class Terminal
{
    //attributes
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
    public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
    public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
    public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

    //constructors
    public Terminal() {}

    public Terminal(string tn, Dictionary<string, Airline> al, Dictionary<string, Flight> fl,
        Dictionary<string, BoardingGate> bg, Dictionary<string, double> gf)
    {
        TerminalName = tn;
        Airlines = al;
        Flights = fl;
        BoardingGates = bg;
        GateFees = gf;
    }

    //methods
    public bool AddAirline(Airline addA)
    {
        //ok note to self check if
        //nvm i have no clue
        return false;
    }

    public bool AddBoardingGate(BoardingGate addBg)
    {
        //why does it return a bool???????
        return false;
    }

    public Airline GetAirlineFromFlight(Flight flightA)
    {
        //ok so flightA.FlightNumber get the first two letters
        //then return Airlines[first two letters]
    }

    public void PrintAirlineFees()
    {
        //?????????????????????????????????????????
        //gate fees????????????????????????????????
        //wtf is gate fees?????????????????????????
    }

    public override string ToString()
    {
        return $"{TerminalName}";
    }
}