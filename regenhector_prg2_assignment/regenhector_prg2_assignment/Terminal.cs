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

        //i have no clue what this is used for
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
            //no clue lol
            try
            {
                Airlines[addA.Code] = addA;
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("uh oh");
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate addBg)
        {
            //no clue lol
            try
            {
                BoardingGates[addBg.GateName] = addBg;
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("uh oh");
            }
            return false;
        }

        public Airline GetAirlineFromFlight(Flight flightA)
        {
            Airline airlineCompany = Airlines[flightA.FlightNumber.Substring(0, 2)];
            return airlineCompany;
        }

        public void PrintAirlineFees()
        {
            //?????????????????????????????????????????
            //gate fees????????????????????????????????
            //wtf is gate fees?????????????????????????
        }

        public override string ToString()
        {
            return $"{TerminalName}";
        }
    }
}