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

        static void InitFlights(Terminal terminal)
        {
            DateTime today = DateTime.Today;

            using (StreamReader sr = new StreamReader("flights.csv"))
            {
                string? s = sr.ReadLine();
                while ((s = sr.ReadLine()) != null)
                {
                    string[] flightInfo = s.Trim().Split(",");

                    //this is a very bad idea
                    //newFlight should ALWAYS be reassigned if the data is correct
                    //but c# won't let me add newFlight if it's unassigned
                    //which is obviously a fair point
                    //but this is even worse
                    Flight newFlight = new NORMFlight();

                    //yikes
                    switch (flightInfo[4])
                    {
                        case "":
                            newFlight = new NORMFlight(flightInfo[0], flightInfo[1], flightInfo[2], DateTime.Parse(flightInfo[3]));
                            //terminal.Flights[flightInfo[0]] = new NORMFlight(flightInfo[0], flightInfo[1], flightInfo[2], DateTime.Parse(flightInfo[3]));
                            break;
                        case "DDJB":
                            newFlight = new DDJBFlight(flightInfo[0], flightInfo[1], flightInfo[2], DateTime.Parse(flightInfo[3]));
                            //terminal.Flights[flightInfo[0]] = new DDJBFlight(flightInfo[0], flightInfo[1], flightInfo[2], DateTime.Parse(flightInfo[3]));
                            break;
                        case "CFFT":
                            newFlight = new CFFTFlight(flightInfo[0], flightInfo[1], flightInfo[2], DateTime.Parse(flightInfo[3]));
                            //terminal.Flights[flightInfo[0]] = new CFFTFlight(flightInfo[0], flightInfo[1], flightInfo[2], DateTime.Parse(flightInfo[3]));
                            break;
                        case "LWTT":
                            newFlight = new LWTTFlight(flightInfo[0], flightInfo[1], flightInfo[2], DateTime.Parse(flightInfo[3]));
                            //terminal.Flights[flightInfo[0]] = new LWTTFlight(flightInfo[0], flightInfo[1], flightInfo[2], DateTime.Parse(flightInfo[3]));
                            break;
                    }

                    foreach (Airline airline in terminal.Airlines.Values)
                    {
                        bool canAddFlight = airline.AddFlight(newFlight);
                        if (canAddFlight) break;
                    }

                    terminal.Flights[newFlight.FlightNumber] = newFlight;
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
