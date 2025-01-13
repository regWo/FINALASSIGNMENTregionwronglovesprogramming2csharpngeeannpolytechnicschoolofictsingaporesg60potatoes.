namespace regenhector_prg2_assignment;

public class LWTTFlight : Flight
{
    //attributes
    public double RequestFee { get; set; }

    //constructors
    public LWTTFlight() {}

    public LWTTFlight(string fn, string o, string d, DateTime et, string s, double rf) : base(fn, o, d, et, s)
    {
        RequestFee = rf;
    }

    //methods
    public override double CalculateFees()
    {
        return base.CalculateFees() + 500;
    }

    public override string ToString()
    {
        return base.ToString() + $" {RequestFee}";
    }
}