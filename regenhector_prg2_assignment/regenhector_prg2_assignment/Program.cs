namespace regenhector_prg2_assignment
{
    class Program
    {
        static void InitAirlines(Terminal terminal)
        {
            using (StreamReader sr = new StreamReader("airlines.csv"))
            {
                string? s = sr.ReadLine();
                while ((s = sr.ReadLine()) != null)
                {
                    string[] airlineInfo = s.Split(",");
                    terminal.AddAirline(new Airline(airlineInfo[0], airlineInfo[1]));
                }
            }
        }

        static void InitBoardingGates(Terminal terminal)
        {
            using (StreamReader sr = new StreamReader("boardinggates.csv"))
            {
                string? s = sr.ReadLine();
                while ((s = sr.ReadLine()) != null)
                {
                    string[] boardingGateInfo = s.Split(",");
                    terminal.AddBoardingGate(new BoardingGate(boardingGateInfo[0],
                        Convert.ToBoolean(boardingGateInfo[1]), Convert.ToBoolean(boardingGateInfo[2]),
                        Convert.ToBoolean(boardingGateInfo[3])));
                }
            }
        }
        static void Main()
        {
            //DO NOT TOUCH THESE ARE THE INITIALISERS
            Terminal terminalFive = new Terminal("Terminal Name");
            InitAirlines(terminalFive);
            InitBoardingGates(terminalFive);
        }
    }
}
