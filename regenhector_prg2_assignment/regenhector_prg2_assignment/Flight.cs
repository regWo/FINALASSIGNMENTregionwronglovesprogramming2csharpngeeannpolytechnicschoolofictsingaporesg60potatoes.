namespace regenhector_prg2_assignment
{
    public abstract class Flight
    {
        //attributes
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        //constructors
        public Flight() { }

        public Flight(string fn, string o, string d, DateTime et)
        {
            FlightNumber = fn;
            Origin = o;
            Destination = d;
            ExpectedTime = et;
        }

        public Flight(string fn, string o, string d, DateTime et, string s)
        {
            FlightNumber = fn;
            Origin = o;
            Destination = d;
            ExpectedTime = et;
            Status = s;
        }

        //methods
        public virtual double CalculateFees()
        {
            //if both values > 0 something has gone very wrong; either value should be > 0, not both
            double singaporeOriginFee = (Origin == "Singapore (SIN)") ? 800 : 0;
            double singaporeArrivingFee = (Destination == "Singapore (SIN)") ? 500 : 0;

            return singaporeOriginFee + singaporeArrivingFee;
        }

        public override string ToString()
        {
            return $"{FlightNumber} {Origin} {Destination} {ExpectedTime.ToString("dd/MM/yyyy h:MM:ss tt")} {Status}";
        }
    }
}