namespace regenhector_prg2_assignment
{
    public class DDJBFlight : Flight
    {
        //attributes
        public double RequestFee { get; set; } = 300;

        //constructors
        public DDJBFlight() { }

        public DDJBFlight(string fn, string o, string d, DateTime et) : base(fn, o, d, et) { }

        //i have no clue if we need this since RequestFee should be a constant value and always use the default value when class is instantiated
        public DDJBFlight(string fn, string o, string d, DateTime et, string s, double rf) : base(fn, o, d, et, s)
        {
            RequestFee = rf;
        }

        //methods
        public override double CalculateFees()
        {
            return base.CalculateFees() + 300;
        }

        public override string ToString()
        {
            return base.ToString() + $" {RequestFee}";
        }
    }
}