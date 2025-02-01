namespace regenhector_prg2_assignment
{
    public class Terminal
    {
        //attributes
        public string TerminalName { get; set; }
        //VERY IMPORTANT KEY FOR AIRLINES IS THE AIRLINE CODE NOT AIRLINE NAME
        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();

        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

        //constructors
        public Terminal() { }

        public Terminal(string tn)
        {
            TerminalName = tn;
        }

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
            if (Airlines.ContainsKey(addA.Code))
            {
                return false;
            }
            Airlines[addA.Code] = addA;
            return true;
        }

        public bool AddBoardingGate(BoardingGate addBg)
        {
            if (BoardingGates.ContainsKey(addBg.GateName))
            {
                return false;
            }
            BoardingGates[addBg.GateName] = addBg;
            return true;
        }

        public Airline GetAirlineFromFlight(Flight flightA)
        {
            if (!Flights.ContainsKey(flightA.FlightNumber))
            {
                return null;
            }

            Airline airlineCompany = Airlines[flightA.FlightNumber.Substring(0, 2)];
            return airlineCompany;
        }

        public void PrintAirlineFees()
        {
            foreach (Airline airline in Airlines.Values)
            {
                Console.WriteLine($"{airline.CalculateFees()}");
            }
        }

        public override string ToString()
        {
            return $"{TerminalName}";
        }
    }
}