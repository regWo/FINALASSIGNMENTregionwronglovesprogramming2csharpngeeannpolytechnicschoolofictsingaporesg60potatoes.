namespace regenhector_prg2_assignment
{
    class Program
    {
        //helper functions
        static Flight CreateFlightType(string[] flightInfo)
        {
            Flight newFlight;

            newFlight = flightInfo[4] switch
            {
                "" => new NORMFlight(flightInfo[0], flightInfo[1], flightInfo[2],
                    DateTime.Parse(flightInfo[3])),
                "CFFT" => new CFFTFlight(flightInfo[0], flightInfo[1], flightInfo[2],
                    DateTime.Parse(flightInfo[3])),
                "DDJB" => new DDJBFlight(flightInfo[0], flightInfo[1], flightInfo[2],
                    DateTime.Parse(flightInfo[3])),
                "LWTT" => new LWTTFlight(flightInfo[0], flightInfo[1], flightInfo[2],
                    DateTime.Parse(flightInfo[3])),

                _ => throw new Exception("Invalid flight")
            };

            return newFlight;
        }

        static string GetBoardingGate(Terminal terminal, string flightNumber)
        {
            string flightBoardingGate = "Unassigned";
            foreach (KeyValuePair<string, BoardingGate> kvp in terminal.BoardingGates)
            {
                if (kvp.Value.Flight != null && kvp.Value.Flight.FlightNumber == flightNumber)
                {
                    flightBoardingGate = kvp.Key;
                    break;
                }
            }
            return flightBoardingGate;
        }




        //Feature 1
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

        //Feature 1
        static void InitBoardingGates(Terminal terminal)
        {
            using (StreamReader sr = new StreamReader("boardinggates.csv"))
            {
                // DONT TOUCH THIS CORRECT
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

        //Feature 2
        static void InitFlights(Terminal terminal)
        {
            DateTime today = DateTime.Today;

            using (StreamReader sr = new StreamReader("flights.csv"))
            {
                string? s = sr.ReadLine();
                while ((s = sr.ReadLine()) != null)
                {
                    try
                    {
                        string[] flightInfo = s.Trim().Split(",");

                        Flight newFlight = CreateFlightType(flightInfo);

                        foreach (Airline airline in terminal.Airlines.Values)
                        {
                            bool canAddFlight = airline.AddFlight(newFlight);
                            if (canAddFlight) break;
                        }

                        terminal.Flights[newFlight.FlightNumber] = newFlight;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error has occured");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        static string DisplayMainMenu()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.Write("Please select your option: ");
            string userOption = Console.ReadLine();

            return userOption;
        }

        static void Main()
        {
            //DO NOT TOUCH THESE ARE THE INITIALISERS
            Terminal terminalFive = new Terminal("Terminal Name");
            InitBoardingGates(terminalFive); 
            //DO NOT TOUCH THESE ARE THE INITIALISERS

            while (true)
            {
                string userOption = DisplayMainMenu();
                if (userOption == "0") break;

                switch (userOption)
                {
                    case "1":
                        
                        break;
                    case "2":

                        break;
                }
            }
            
        }
    }
}
