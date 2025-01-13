namespace regenhector_prg2_assignment;

public abstract class Flight
{
    //attributes
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; }

    //constructors
    public Flight() {}

    public Flight(string fn, string o, string d, DateTime et, string s)
    {
        FlightNumber = fn;
        Origin = o;
        Destination = d;
        ExpectedTime = et;
        Status = s;
    }

    //methods
    public abstract double CalculateFees();

    public override string ToString()
    {
        return $"{FlightNumber} {Origin} {Destination} {ExpectedTime.ToString("yyyy/MM/dd")} {Status}";
    }
}