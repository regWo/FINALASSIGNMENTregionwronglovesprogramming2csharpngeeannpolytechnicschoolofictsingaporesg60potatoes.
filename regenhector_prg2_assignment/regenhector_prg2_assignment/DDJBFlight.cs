namespace regenhector_prg2_assignment;

public class DDJBFlight : Flight
{
    //attributes
    public double RequestFee { get; set; }

    //constructors
    public DDJBFlight() {}

    public DDJBFlight(string fn, string o, string d, DateTime et, string s, double rf) : base(fn, o, d, et, s)
    {
        RequestFee = rf;
    }

    //methods
    public override double CalculateFees()
    {
        //pls fix
        return 0;
    }

    public override string ToString()
    {
        return base.ToString() + $" {RequestFee}";
    }
}