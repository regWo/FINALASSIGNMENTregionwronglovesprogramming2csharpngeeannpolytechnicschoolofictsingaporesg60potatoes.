namespace regenhector_prg2_assignment;

public class NORMFlight : Flight
{
    //constructors
    public NORMFlight() {}
    public NORMFlight(string fn, string o, string d, DateTime et, string s) : base(fn, o, d, et, s) {}

    //methods
    public override double CalculateFees()
    {
        //pls fix
        return 0;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}