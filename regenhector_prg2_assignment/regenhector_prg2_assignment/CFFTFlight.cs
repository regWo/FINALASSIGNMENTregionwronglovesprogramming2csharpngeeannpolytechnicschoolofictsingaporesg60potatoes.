namespace regenhector_prg2_assignment;

public class CFFTFlight : Flight
{
    //attributes
    public double RequestFee { get; set; }

    //constructors
    public CFFTFlight() {}

    public CFFTFlight(string fn, string o, string d, DateTime et, string s, double rf) : base(fn, o, d, et, s)
    {
        RequestFee = rf;
    }

    //methods
    public override double CalculateFees()
    {
        return base.CalculateFees() + 150;
    }

    public override string ToString()
    {
        return base.ToString() + $" {RequestFee}";
    }
}