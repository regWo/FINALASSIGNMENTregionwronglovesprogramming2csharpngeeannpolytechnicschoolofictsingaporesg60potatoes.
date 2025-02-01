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

        //Feature 3
        static void DisplayFlightInfo(Terminal terminal)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("List of Flights for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine($"{"Flight Number",-13} {"Airline Name",-18} {"Origin",-18} {"Destination",-18} {"Expected Departure/Arrival Time",-22}");
            foreach (Flight flight in terminal.Flights.Values)
            {
                //get airline company
                //substring just gets the first two letters of the flight number i.e. "SQ"
                //returns Airline class and .Name to get airline name
                string airlineCompany = terminal.GetAirlineFromFlight(flight).Name;

                Console.WriteLine($"{flight.FlightNumber,-13} {airlineCompany,-18} {flight.Origin,-18} {flight.Destination,-18} {flight.ExpectedTime: dd/MM/yyyy h:mm:ss tt}");
            }
        }

        //Feature 4
        static void DisplayBoardingGates(Terminal terminal)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine($"{"Gate Name",-9} {"DDJB",-5} {"CFFT",-5} {"LWTT",-5}");
            foreach (BoardingGate gate in terminal.BoardingGates.Values)
            {
                Console.WriteLine($"{gate.GateName,-9} {gate.SupportsDDJB,-5} {gate.SupportsCFFT,-5} {gate.SupportsLWTT,-5}");
            }
        }

        //Feature 5
        static void AssignGateToFlight(Terminal terminal)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("Assign a Boarding Gate to a Flight");
            Console.WriteLine("=============================================");
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine();
            var flight = terminal.Flights[flightNumber];

            //note to self this is bad
            //it should always return 4 letters for the flight type designation
            //but its probably not a good idea
            string specialRequestCode = terminal.Flights[flightNumber].GetType().Name.Substring(0, 4);
            specialRequestCode = (specialRequestCode == "NORM") ? "None" : specialRequestCode;

            Console.WriteLine($"Flight Number: {flightNumber}\nOrigin: {flight.Origin}\nDestination: {flight.Destination}\n" +
                              $"Expected Time: {flight.ExpectedTime: dd/MM/yyyy h:mm:ss tt}\nSpecial Request Code: {specialRequestCode}");

            string boardingGateName;
            while (true)
            {
                Console.Write("Enter Boarding Gate Name: ");
                boardingGateName = Console.ReadLine();
                BoardingGate boardingGate = terminal.BoardingGates[boardingGateName];

                Console.WriteLine($"Supports DDJB: {boardingGate.SupportsDDJB}\nSupports CFFT: {boardingGate.SupportsCFFT}\nSuuports LWTT: {boardingGate.SupportsLWTT}");
                //NOTE NO NEED TO CHECK FOR SPECIAL REQUEST CODE MATCH FOR BASIC FEATURES, MIGHT WANT TO IMPLEMENT LATER FOR ADVANCED
                if (boardingGate.Flight != null)
                {
                    Console.WriteLine("A flight already exists for this boarding gate. Please pick another gate.");
                    continue;
                }
                //this will not execute if a flight exists for a gate as continue in the if block will skip this line back to the start
                terminal.BoardingGates[boardingGateName].Flight = flight;
                break;
            }

            Console.WriteLine($"Flight Number: {flightNumber}\nOrigin: {flight.Origin}\nDestination: {flight.Destination}\n" +
                              $"Expected Time: {flight.ExpectedTime: dd/MM/yyyy h:mm:ss tt}\nSpecial Request Code: {specialRequestCode}\n" +
                              $"Boarding Gate: {boardingGateName}");

            Console.Write("Would you like to update the status of the flight? (Y/N) ");
            string willUpdateStatus = Console.ReadLine();

            //default to on time, if the user enters y this will be overwritten so it doesn't matter
            terminal.Flights[flightNumber].Status = "On Time";

            if (willUpdateStatus == "Y")
            {
                Console.WriteLine("1. Delayed\n2. Boarding\n3. On Time");
                Console.Write("Please select the new status of the flight: ");
                string newFlightStatusOption = Console.ReadLine();

                if (newFlightStatusOption == "1") terminal.Flights[flightNumber].Status = "Delayed";
                else if (newFlightStatusOption == "2") terminal.Flights[flightNumber].Status = "Boarding";
                //no need for "3" (refer to the comment above)
            }

            Console.WriteLine($"Flight {flightNumber} has been assigned to Boarding Gate {boardingGateName}");
        }

        //Feature 6
        static void CreateNewFlight(Terminal terminal)
        {
            while (true)
            {
                Console.Write("Enter Flight Number: ");
                string flightNumber = Console.ReadLine();

                Console.Write("Enter Origin: ");
                string flightOrigin = Console.ReadLine();

                Console.Write("Enter Destination: ");
                string flightDestination = Console.ReadLine();

                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                string flightExpectedTime = Console.ReadLine();

                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                string flightSpecialRequestCode = Console.ReadLine();
                flightSpecialRequestCode = (flightSpecialRequestCode == "None") ? "" : flightSpecialRequestCode;

                try
                {
                    Flight newFlight = CreateFlightType(new string[] { flightNumber, flightOrigin, flightDestination, flightExpectedTime, flightSpecialRequestCode });

                    foreach (Airline airline in terminal.Airlines.Values)
                    {
                        bool canAddFlight = airline.AddFlight(newFlight);
                        if (canAddFlight) break;
                    }
                    terminal.Flights[newFlight.FlightNumber] = newFlight;



                    using (StreamWriter sw = new StreamWriter("flights.csv", true))
                    {
                        sw.WriteLine($"{flightNumber},{flightOrigin},{flightDestination},{flightExpectedTime: h:mm tt},{flightSpecialRequestCode}");
                    }

                    Console.WriteLine($"Flight {flightNumber} has been added!");

                    Console.Write("Would you like to add another flight? (Y/N): ");
                    string addAnotherFlight = Console.ReadLine();

                    if (addAnotherFlight == "N") break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error has occured");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Feature 7
        
        static void DisplayAirlineFlights(Terminal terminal)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");

            Console.WriteLine($"{"Airline Code", -12} {"Airline Name", -18}");

            foreach (Airline airline in terminal.Airlines.Values)
            {
                Console.WriteLine($"{airline.Code, -12} {airline.Name, -18}");
            }

            Console.Write("Enter Airline Code: ");
            string userAirlineCode = Console.ReadLine();

            Console.WriteLine("=============================================");
            Console.WriteLine($"List of Flights for {terminal.Airlines[userAirlineCode].Name}");
            Console.WriteLine("=============================================");

            Console.WriteLine($"{"Flight Number",-13} {"Airline Name",-18} {"Origin",-18} {"Destination",-18} {"Expected Departure/Arrival Time",-22}");
            foreach (Flight flight in terminal.Airlines[userAirlineCode].Flights.Values)
            {
                string airlineCompany = terminal.GetAirlineFromFlight(flight).Name;
                Console.WriteLine($"{flight.FlightNumber, -13} {airlineCompany, -18} {flight.Origin, -18} {flight.Destination, -17} {flight.ExpectedTime: d/MM/yyyy h:mm:ss tt}");
            }
        }
        //Feature 8
        static void DisplayFlightInfo(Terminal terminal, string flightNumber, string whatUpdated /bad name lol/)
        {
            string airlineCompany = terminal.GetAirlineFromFlight(terminal.Flights[flightNumber] /Flight object/).Name;

            Console.WriteLine($"{whatUpdated}");
            Console.WriteLine($"Flight Number: {flightNumber}");
            Console.WriteLine($"Airline Name: {airlineCompany}");
            Console.WriteLine($"Origin: {terminal.Flights[flightNumber].Origin}");
            Console.WriteLine($"Destination: {terminal.Flights[flightNumber].Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {terminal.Flights[flightNumber].ExpectedTime: d/M/yyyy h:mm:ss tt}");
            Console.WriteLine($"Status: {terminal.Flights[flightNumber].Status}");

            //note to self this is bad
            //it should always return 4 letters for the flight type designation
            //but its probably not a good idea
            string flightSpecialRequestCode = terminal.Flights[flightNumber].GetType().Name.Substring(0, 4);
            flightSpecialRequestCode = (flightSpecialRequestCode == "NORM") ? "None" : flightSpecialRequestCode;

            Console.WriteLine($"Special Request Code: {flightSpecialRequestCode}");

            string flightBoardingGate = GetBoardingGate(terminal, flightNumber);
            Console.WriteLine($"Boarding Gate: {flightBoardingGate}");
        }

        static void ModifyBasicInfo(Terminal terminal, string flightNumber)
        {
            Console.Write("Enter new Origin: ");
            string newOrigin = Console.ReadLine();
            terminal.Flights[flightNumber].Origin = newOrigin;

            Console.Write("Enter new Destination: ");
            string newDestination = Console.ReadLine();
            terminal.Flights[flightNumber].Destination = newDestination;

            Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            DateTime newExpectedTime = DateTime.Parse(Console.ReadLine());
            terminal.Flights[flightNumber].ExpectedTime = newExpectedTime;
            DisplayFlightInfo(terminal, flightNumber, "Flight updated!");
        }

        static void ModifyStatus(Terminal terminal, string flightNumber)
        {
            Console.Write("Enter new flight status: ");
            string newFlightStatus = Console.ReadLine();
            terminal.Flights[flightNumber].Status = newFlightStatus;
            DisplayFlightInfo(terminal, flightNumber, "Status updated!");
        }

        static void ModifySpecialRequestCode(Terminal terminal, string flightNumber)
        {
            Console.Write("Enter new special request code (CFFT/DDJB/LWTT/None): ");
            string newSpecialRequestCode = Console.ReadLine();
            newSpecialRequestCode = (newSpecialRequestCode == "None") ? "" : newSpecialRequestCode;

            terminal.Flights[flightNumber] = CreateFlightType(new string[]
            {
                flightNumber, terminal.Flights[flightNumber].Origin, terminal.Flights[flightNumber].Destination,
                        terminal.Flights[flightNumber].ExpectedTime.ToString() /VERY BAD/, newSpecialRequestCode
            });

            DisplayFlightInfo(terminal, flightNumber, "Special Request Code updated!");
        }

        static void ModifyBoardingGate(Terminal terminal, string flightNumber)
        {
            string specialRequestCode = terminal.Flights[flightNumber].GetType().Name.Substring(0, 4);

            while (true)
            {
                Console.Write("Enter new boarding gate: ");
                string newBoardingGate = Console.ReadLine();

                if (!terminal.BoardingGates.ContainsKey(newBoardingGate))
                {
                    Console.WriteLine("No boarding gate found.");
                    continue;
                }

                //this checks the current flight's special request code
                //for example, if it is SRC is CFFT it checks whether the current boarding gate supports CFFT
                //this works because flights can only 1 have SRC
                //NORM is always true because normal flights have no SRC thus we want to avoid the supports SRC check
                bool supportsSpecialRequest = specialRequestCode switch
                {
                    "NORM" => true,
                    "CFFT" => terminal.BoardingGates[newBoardingGate].SupportsCFFT,
                    "DDJB" => terminal.BoardingGates[newBoardingGate].SupportsDDJB,
                    "LWTT" => terminal.BoardingGates[newBoardingGate].SupportsLWTT,
                    _ => false
                };

                if (!supportsSpecialRequest)
                {
                    Console.WriteLine($"Boarding gate does not support {specialRequestCode}");
                    continue;
                }

                terminal.BoardingGates[newBoardingGate].Flight = terminal.Flights[flightNumber];

                //this should only execute if boarding gate exists and supports SRC
                break;
            }
        }

        static void DeleteFlight(Terminal terminal, string flightNumber)
        {
            Console.Write("Are you sure you want to delete this flight? (Y/N) ");
            string deleteConfirmation = Console.ReadLine();

            //this is just for safety's sake
            //i think all other times flight is used in other classes it's by reference
            //but i could be wrong and it's probably better to just ensure it's totally deleted
            terminal.Flights.Remove(flightNumber);
            terminal.Airlines[terminal.GetAirlineFromFlight(terminal.Flights[flightNumber]).Code].Flights
                .Remove(flightNumber);

            string flightBoardingGate = GetBoardingGate(terminal, flightNumber);
            if (flightBoardingGate != "Unassigned") terminal.BoardingGates[flightBoardingGate].Flight = null!;
        }

        static void ModifyFlightDetails(Terminal terminal)
        {
            DisplayAirlineFlights(terminal);

            Console.Write("Choose an existing Flight to modify or delete: ");
            string userFlightNumber = Console.ReadLine();

            if (userOption == "1")
            {
                Console.WriteLine("1. Modify Basic Information");
                Console.WriteLine("2. Modify Status");
                Console.WriteLine("3. Modify Special Request Code");
                Console.WriteLine("4. Modify Boarding Gate");
                string optionOneChoice = Console.ReadLine();

                switch (optionOneChoice)
                {
                    case "1":
                        ModifyBasicInfo(terminal, userFlightNumber);
                        break;
                    case "2":
                        ModifyStatus(terminal, userFlightNumber);
                        break;
                    case "3":
                        ModifySpecialRequestCode(terminal, userFlightNumber);
                        break;
                    case "4":
                        ModifyBoardingGate(terminal, userFlightNumber);
                        break;
                }
            }

            else if (userOption == "2")
            {
                DeleteFlight(terminal, userFlightNumber);
            }
        }

        //Feature 9
        static void DisplayScheduledFlights(Terminal terminal)
        {
            List<Flight> sortedFlightsList = new List<Flight>();

            foreach (Flight flight in terminal.Flights.Values)
            {
                sortedFlightsList.Add(flight);
            }

            sortedFlightsList.Sort();

            Console.WriteLine("=============================================");
            Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");

            Console.WriteLine($"{"Flight Number",-13} {"Airline Name",-18} {"Origin",-18} {"Destination",-18} " +
                              $"{"Expected Departure/Arrival Time",-31}");
            Console.WriteLine($"{"Status",-13} {"Boarding Gate",-13}\n");

            foreach (Flight flight in sortedFlightsList)
            {
                Console.WriteLine($"{flight.FlightNumber,-13} {terminal.GetAirlineFromFlight(flight).Name,-18} {flight.Origin,-18} " +
                                  $"{flight.Destination,-17} {flight.ExpectedTime,-31: d/M/yyyy h:mm:ss tt}");
                Console.WriteLine($"{flight.Status,-13} {GetBoardingGate(terminal, flight.FlightNumber),-13}\n");
            }
        }

        //prints main menu
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
                        DisplayFlightInfo(terminalFive);
                        break;
                    case "2":
                        DisplayBoardingGates(terminalFive);
                        break;
                    case "3":
                        AssignGateToFlight(terminalFive);
                        break;
                    case "4":
                        CreateNewFlight(terminalFive);
                        break;
                    case "5":
                        DisplayAirlineFlights(terminalFive);
                        break;
                    case "6":
                        ModifyFlightDetails(terminalFive);
                        break;
                    case "7":
                        DisplayScheduledFlights(terminalFive);
                        break;
                }
            }
            
        }
    }
}
