namespace regenhector_prg2_assignment
{
    public class NORMFlight : Flight
    {
        //constructors
        public NORMFlight() { }

        //i have no clue if we need this since RequestFee should be a constant value and always use the default value when class is instantiated
        public NORMFlight(string fn, string o, string d, DateTime et, string s) : base(fn, o, d, et, s) { }

        //methods
        public override double CalculateFees()
        {
            //if both values > 0 something has gone very wrong; either value should be > 0, not both
            return base.CalculateFees();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}