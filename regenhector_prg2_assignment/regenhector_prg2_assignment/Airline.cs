namespace regenhector_prg2_assignment;

public class Airline
{
    //attributes
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; set; }

    //constructors
    public Airline() {}

    public Airline(string n, string c, Dictionary<string, Flight> f)
    {
        Name = n;
        Code = c;
        Flights = f;
    }

    //methods
    public bool AddFlight(Flight addF)
    {
        //ok note to self string splice first 2 of flight from flights.csv then
        //check if first 2 letters belong to airline
        //if yes add to dict then return true
        //if no return false
        return false;
    }

    public double CalculateFees()
    {
        //pls fix
        return 0;
    }

    public bool RemoveFlight(Flight removeF)
    {
        //checks if flight is in airline's flight list
        if (Flights.ContainsValue(removeF))
        {
            Flights.Remove(removeF.FlightNumber);
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return $"{Name} {Code}";
    }
}