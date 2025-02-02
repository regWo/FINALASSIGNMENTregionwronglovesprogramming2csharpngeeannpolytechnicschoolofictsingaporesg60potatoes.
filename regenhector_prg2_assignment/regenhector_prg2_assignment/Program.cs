namespace regenhector_prg2_assignment
{
    class Program
    {
        static readonly string[] yesNoA = {"Y", "N"};
        static readonly string[] specialRequestCodesA = {"CFFT", "DDJB", "LWTT", "NONE"};
        private static List<string> airportListA = new List<string>();

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

        static string GetBoardingGateCode(Terminal terminal, string flightNumber)
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

        static bool ValidateDate(string expectedDateTime /*dd/MM/yyyy hh:mm*/)
        {
            char[] delimiters = { '/', ' ', ':' };
            string[] dateInfo = expectedDateTime.Split(delimiters);

            //check if there are 5 items
            if (dateInfo.Length != 5)
            {
                Console.WriteLine("Invalid date format! Please try again.");
                return false;
            }

            bool allNumbers = true;
            foreach (string item in dateInfo)
            {
                if (!int.TryParse(item, out _))
                {
                    allNumbers = false;
                    break;
                }
            }

            if (!allNumbers)
            {
                Console.WriteLine("Date and time must be numbers! Please try again.");
                return false;
            }

            if (int.Parse(dateInfo[2]) < DateTime.Today.Year)
            {
                Console.WriteLine($"Year must be at least {DateTime.Today.Year}! Please try again.");
                return false;
            }

            if (int.Parse(dateInfo[1]) > 12 || int.Parse(dateInfo[1]) < 1)
            {
                Console.WriteLine("Month must be between 1 and 12! Please try again.");
                return false;
            }

            int maxMonthDate = int.Parse(dateInfo[1]) switch
            {
                1 => 31,
                2 => 28 /*see below*/,
                3 => 31,
                4 => 30,
                5 => 31,
                6 => 30,
                7 => 31,
                8 => 31,
                9 => 30,
                10 => 31,
                11 => 30,
                12 => 31
            };

            int year = int.Parse(dateInfo[2]);
            if (((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)) /*if leap year*/ &&
                int.Parse(dateInfo[1]) == 2 /*if february*/) maxMonthDate = 29;

            if (int.Parse(dateInfo[0]) > maxMonthDate)
            {
                Console.WriteLine($"Date must not exceed {maxMonthDate}! Please try again.");
                return false;
            }

            if (int.Parse(dateInfo[3]) < 0 || int.Parse(dateInfo[3]) > 23)
            {
                Console.WriteLine("Hour must be between 0 and 23! Please try again.");
                return false;
            }

            if (int.Parse(dateInfo[4]) < 0 || int.Parse(dateInfo[4]) > 59)
            {
                Console.WriteLine("Minute must be between 0 and 59! Please try again.");
                return false;
            }

            if (DateTime.Parse(expectedDateTime) < DateTime.Now)
            {
                Console.WriteLine("Date and time must be in the future! Please try again.");
                return false;
            }

            return true;
        }

        static string CapitaliseString(string idkman)
        {
            string capitalised;
            if (idkman == null) throw new ArgumentException("Input cannot be null!");
            else if (idkman == "") throw new ArgumentException("Input cannot be an empty string!");
            else
            {
                capitalised = char.ToUpper(idkman[0]) + idkman.Substring(1);
            }

            return capitalised;
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
                string? s = sr.ReadLine();
                while ((s = sr.ReadLine()) != null)
                {
                    string[] boardingGateInfo = s.Split(",");
                    terminal.AddBoardingGate(new BoardingGate(boardingGateInfo[0],
                        Convert.ToBoolean(boardingGateInfo[2]), Convert.ToBoolean(boardingGateInfo[1]),
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

                        if (!airportListA.Contains(flightInfo[1])) airportListA.Add(flightInfo[1]);
                        if (!airportListA.Contains(flightInfo[2])) airportListA.Add(flightInfo[2]);
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

                Console.WriteLine($"{flight.FlightNumber,-13} {airlineCompany,-18} {flight.Origin,-18} {flight.Destination,-17} {flight.ExpectedTime: dd/MM/yyyy h:mm:ss tt}");
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

            string flightNumber;
            while (true)
            {
                Console.Write("Enter Flight Number: ");
                flightNumber = Console.ReadLine().Trim();

                flightNumber = flightNumber?.ToUpper();

                if (!terminal.Flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine("Invalid flight number! Please try again.");
                    continue;
                }

                bool alreadyAssigned = false;
                foreach (BoardingGate existingFlightGate in terminal.BoardingGates.Values)
                {
                    if (existingFlightGate.Flight == terminal.Flights[flightNumber])
                    {
                        Console.WriteLine("Flight is already assigned to a gate! Please try again.");
                        alreadyAssigned = true;
                    }
                }
                if (alreadyAssigned) continue;

                break;
            }

            Flight flight = terminal.Flights[flightNumber];

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
                boardingGateName = Console.ReadLine().Trim();
                boardingGateName = boardingGateName?.ToUpper();

                //VALIDATION
                if (!terminal.BoardingGates.ContainsKey(boardingGateName))
                {
                    Console.WriteLine("Invalid boarding gate! Please try again.");
                    continue;
                }

                BoardingGate boardingGate = terminal.BoardingGates[boardingGateName];

                //VALIDATION
                //NOTE NO NEED TO CHECK FOR SPECIAL REQUEST CODE MATCH FOR BASIC FEATURES, MIGHT WANT TO IMPLEMENT LATER FOR ADVANCED
                if (boardingGate.Flight != null)
                {
                    Console.WriteLine("A flight already exists for this boarding gate. Please try again.");
                    continue;
                }

                Console.WriteLine($"Supports DDJB: {boardingGate.SupportsDDJB}\nSupports CFFT: {boardingGate.SupportsCFFT}\nSuuports LWTT: {boardingGate.SupportsLWTT}");

                terminal.BoardingGates[boardingGateName].Flight = flight;
                break;
            }

            

            Console.WriteLine($"Flight Number: {flightNumber}\nOrigin: {flight.Origin}\nDestination: {flight.Destination}\n" +
                              $"Expected Time: {flight.ExpectedTime: dd/MM/yyyy h:mm:ss tt}\nSpecial Request Code: {specialRequestCode}\n" +
                              $"Boarding Gate: {boardingGateName}");

            bool firstIterationUS = true;
            string willUpdateStatus;
            do
            {
                if (!firstIterationUS) Console.WriteLine("Invalid answer! Please try again.");
                Console.Write("Would you like to update the status of the flight? (Y/N): ");
                willUpdateStatus = Console.ReadLine().Trim();
                willUpdateStatus = willUpdateStatus?.ToUpper();

                firstIterationUS = false;
            } while (!yesNoA.Contains(willUpdateStatus));
            

            //default to on time, if the user enters y this will be overwritten so it doesn't matter
            //flights might have their own statuses but it probably won't matter
            terminal.Flights[flightNumber].Status = "On Time";

            if (willUpdateStatus == "Y")
            {
                Console.WriteLine("1. Delayed\n2. Boarding\n3. On Time");
                string[] newStatusOptionsA = {"1", "2", "3"};

                //VALIDATION
                bool firstIterationFS = true;
                string newFlightStatusOption;
                do
                {
                    if (!firstIterationFS) Console.WriteLine("Invalid flight status! Please try again.");
                    Console.Write("Please select the new status of the flight: ");
                    newFlightStatusOption = Console.ReadLine().Trim();

                    firstIterationFS = false;
                } while (!newStatusOptionsA.Contains(newFlightStatusOption));

                if (newFlightStatusOption == "1") terminal.Flights[flightNumber].Status = "Delayed";
                else if (newFlightStatusOption == "2") terminal.Flights[flightNumber].Status = "Boarding";

                //no need for "3" (refer to the comment above)
            }

            //Entering "N" will default to On Time

            Console.WriteLine($"Flight {flightNumber} has been assigned to Boarding Gate {boardingGateName}");
        }

        //Feature 6
        static void CreateNewFlight(Terminal terminal)
        {
            while (true)
            {
                string flightNumber;
                string flightOrigin;
                string flightDestination;
                string flightSpecialRequestCode;
                string flightExpectedTime = ""; //avoid error

                //this should always run
                while (true)
                {
                    Console.Write("Enter Flight Number: ");
                    flightNumber = Console.ReadLine().Trim();
                    flightNumber = flightNumber?.ToUpper();

                    string[] flightNumberInfo = flightNumber.Split(" ");
                    if (flightNumberInfo.Length != 2)
                    {
                        Console.WriteLine("Invalid flight number format! Please try again.");
                        continue;
                    }

                    if (flightNumberInfo[1].Length != 3)
                    {
                        Console.WriteLine("Invalid flight number format! Please try again.");
                        continue;
                    }

                    bool isNumeric = int.TryParse(flightNumberInfo[1], out _);
                    if (!isNumeric)
                    {
                        Console.WriteLine("Invalid flight number format! Please try again.");
                        continue;
                    }

                    if (!terminal.Airlines.ContainsKey(flightNumberInfo[0]))
                    {
                        Console.WriteLine("Airline does not exist! Please try again.");
                        continue;
                    }

                    if (terminal.Flights.ContainsKey(flightNumber))
                    {
                        Console.WriteLine("Flight number already exists! Please try again.");
                        continue;
                    }

                    break;
                }

                while (true)
                {
                    //EITHER MUST BE SINGAPORE PLEASE FIX
                    Console.Write("Enter Origin: ");
                    flightOrigin = Console.ReadLine().Trim();
                    flightOrigin = CapitaliseString(flightOrigin);

                    Console.Write("Enter Destination: ");
                    flightDestination = Console.ReadLine().Trim();
                    flightDestination = CapitaliseString(flightDestination);

                    if ((flightOrigin != "Singapore (SIN)" && flightDestination != "Singapore (SIN)") ||
                        (flightOrigin == "Singapore (SIN)" && flightDestination == "Singapore (SIN)"))
                    {
                        Console.WriteLine("Either the destination or origin has to be Singapore! Please try again.");
                        continue;
                    }

                    break;
                }

                bool isExpectedTimeValid = false;
                while (!isExpectedTimeValid)
                {
                    Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                    flightExpectedTime = Console.ReadLine().Trim();

                    try
                    {
                        isExpectedTimeValid = ValidateDate(flightExpectedTime);
                    }
                    catch
                    {
                        //hopefully catch wont run
                        Console.WriteLine("Invalid date format! Please try again");
                    }
                }

                while (true)
                {
                    Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                    flightSpecialRequestCode = Console.ReadLine().Trim();
                    flightSpecialRequestCode = flightSpecialRequestCode?.ToUpper();

                    if (!specialRequestCodesA.Contains(flightSpecialRequestCode))
                    {
                        Console.WriteLine("Invalid Special Request Code! Please try again");
                        continue;
                    }

                    flightSpecialRequestCode = (flightSpecialRequestCode == "NONE") ? "" : flightSpecialRequestCode;
                    break;
                }

                try
                {
                    Flight newFlight = CreateFlightType(new string[]
                    {
                    flightNumber, flightOrigin, flightDestination, flightExpectedTime, flightSpecialRequestCode
                    });

                    foreach (Airline airline in terminal.Airlines.Values)
                    {
                        bool canAddFlight = airline.AddFlight(newFlight);
                        if (canAddFlight) break;
                    }

                    terminal.Flights[newFlight.FlightNumber] = newFlight;



                    using (StreamWriter sw = new StreamWriter("flights.csv", true))
                    {
                        sw.WriteLine(
                            $"{flightNumber},{flightOrigin},{flightDestination},{flightExpectedTime: h:mm tt},{flightSpecialRequestCode}");
                    }
                }
                catch (FormatException fe)
                {
                    Console.WriteLine("Invalid date time! Please try again.");
                    Console.WriteLine(fe.Message);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("An error has occured.");
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine($"Flight {flightNumber} has been added!");

                bool firstIteration =  true;
                string addAnotherFlight;
                do
                {
                    if (!firstIteration) Console.WriteLine("Invalid answer! Please try again.");
                    Console.Write("Would you like to add another flight? (Y/N): ");
                    addAnotherFlight = Console.ReadLine().Trim();
                    addAnotherFlight = addAnotherFlight?.ToUpper();

                    firstIteration = false;
                } while (!yesNoA.Contains(addAnotherFlight));

                if (addAnotherFlight == "Y") continue;

                //this will not execute if addAnotherFlight == "Y"
                break;
            }
        }

        //Feature 7
        static void DisplayAirlineFlights(Terminal terminal)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");

            Console.WriteLine($"{"Airline Code",-12} {"Airline Name",-18}");

            foreach (Airline airline in terminal.Airlines.Values)
            {
                Console.WriteLine($"{airline.Code,-12} {airline.Name,-18}");
            }

            bool firstIteration = true;
            string userAirlineCode;
            do
            {
                if (!firstIteration) Console.WriteLine("Invalid airline code! Please try again.");
                Console.Write("Enter Airline Code: ");
                userAirlineCode = Console.ReadLine().Trim();
                userAirlineCode = userAirlineCode?.ToUpper();

                firstIteration = false;
            } while (!terminal.Airlines.ContainsKey(userAirlineCode));



            Console.WriteLine("=============================================");
            Console.WriteLine($"List of Flights for {terminal.Airlines[userAirlineCode].Name}");
            Console.WriteLine("=============================================");

            Console.WriteLine($"{"Flight Number",-13} {"Airline Name",-18} {"Origin",-18} {"Destination",-18} {"Expected Departure/Arrival Time",-22}");
            foreach (Flight flight in terminal.Airlines[userAirlineCode].Flights.Values)
            {
                string airlineCompany = terminal.GetAirlineFromFlight(flight).Name;
                Console.WriteLine($"{flight.FlightNumber,-13} {airlineCompany,-18} {flight.Origin,-18} {flight.Destination,-17} {flight.ExpectedTime: d/MM/yyyy h:mm:ss tt}");
            }
        }
        //Feature 8
        static void DisplayFlightInfo(Terminal terminal, string flightNumber, string whatUpdated /*bad name lol*/)
        {
            string airlineCompany = terminal.GetAirlineFromFlight(terminal.Flights[flightNumber] /*Flight object*/).Name;

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

            string flightBoardingGate = GetBoardingGateCode(terminal, flightNumber);
            Console.WriteLine($"Boarding Gate: {flightBoardingGate}");
        }

        static void ModifyBasicInfo(Terminal terminal, string flightNumber)
        {
            string newOrigin;
            string newDestination;
            while (true)
            {
                //EITHER MUST BE SINGAPORE PLEASE FIX
                Console.Write("Enter Origin: ");
                newOrigin = Console.ReadLine().Trim();
                newOrigin = CapitaliseString(newOrigin);

                Console.Write("Enter Destination: ");
                newDestination = Console.ReadLine().Trim();
                newDestination = CapitaliseString(newDestination);

                if ((newOrigin != "Singapore (SIN)" && newDestination != "Singapore (SIN)") ||
                    (newOrigin == "Singapore (SIN)" && newDestination == "Singapore (SIN)"))
                {
                    Console.WriteLine("Either the destination or origin has to be Singapore! Please try again.");
                    continue;
                }

                break;
            }
            terminal.Flights[flightNumber].Origin = newOrigin;
            terminal.Flights[flightNumber].Destination = newDestination;

            string newExpectedTime = ""; //thank you c#
            bool isExpectedTimeValid = false;
            while (!isExpectedTimeValid)
            {
                Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                newExpectedTime = Console.ReadLine().Trim();

                try
                {
                    isExpectedTimeValid = ValidateDate(newExpectedTime);
                }
                catch
                {
                    //hopefully catch wont run
                    Console.WriteLine("Invalid date format! Please try again");
                }
            }

            terminal.Flights[flightNumber].ExpectedTime = DateTime.Parse(newExpectedTime);

            DisplayFlightInfo(terminal, flightNumber, "Flight updated!");
        }

        static void ModifyStatus(Terminal terminal, string flightNumber)
        {
            string[] statusesA = { "delayed", "boarding", "on time", "scheduled" };

            bool firstIteration = true;
            string newFlightStatus;
            do
            {
                if (!firstIteration) Console.WriteLine("Invalid flight status! Please try again.");
                Console.Write("Enter new flight status: ");
                newFlightStatus = Console.ReadLine().Trim();
                newFlightStatus = newFlightStatus?.ToLower();

                firstIteration = false;
            } while (!statusesA.Contains(newFlightStatus));

            terminal.Flights[flightNumber].Status = CapitaliseString(newFlightStatus);

            DisplayFlightInfo(terminal, flightNumber, "Status updated!");
        }

        static void ModifySpecialRequestCode(Terminal terminal, string flightNumber)
        {
            bool firstIteration = true;
            string newSpecialRequestCode;
            do
            {
                if (!firstIteration) Console.WriteLine("Invalid special request code! Please try again.");
                Console.Write("Enter new special request code (CFFT/DDJB/LWTT/None): ");
                newSpecialRequestCode = Console.ReadLine().Trim();
                newSpecialRequestCode = newSpecialRequestCode?.ToUpper();

                firstIteration = false;
            } while (!specialRequestCodesA.Contains(newSpecialRequestCode));

            newSpecialRequestCode = (newSpecialRequestCode == "NONE") ? "" : newSpecialRequestCode;

            terminal.Flights[flightNumber] = CreateFlightType(new string[]
            {
                flightNumber, terminal.Flights[flightNumber].Origin, terminal.Flights[flightNumber].Destination,
                terminal.Flights[flightNumber].ExpectedTime.ToString() /*VERY BAD*/, newSpecialRequestCode
            });

            DisplayFlightInfo(terminal, flightNumber, "Special Request Code updated!");
        }

        static void ModifyBoardingGate(Terminal terminal, string flightNumber)
        {
            string specialRequestCode = terminal.Flights[flightNumber].GetType().Name.Substring(0, 4);

            while (true)
            {
                Console.Write("Enter new boarding gate: ");
                string newBoardingGate = Console.ReadLine().Trim();
                newBoardingGate = newBoardingGate?.ToUpper();

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


                //this should only execute if boarding gate exists and supports SRC
                terminal.BoardingGates[newBoardingGate].Flight = terminal.Flights[flightNumber];
                break;
            }
        }

        static void DeleteFlight(Terminal terminal, string flightNumber)
        {
            bool firstIteration = true;
            string deleteConfirmation;
            do
            {
                if (!firstIteration) Console.WriteLine("Invalid answer! Please try again.");
                Console.Write("Are you sure you want to delete this flight? (Y/N): ");
                deleteConfirmation = Console.ReadLine().Trim();
                deleteConfirmation = deleteConfirmation?.ToUpper();

                firstIteration = false;
            } while (!yesNoA.Contains(deleteConfirmation));

            if (deleteConfirmation == "N") return;

            //no need for if == "Y" (early return with N)
            //this is just for safety's sake
            //i think all other times flight is used in other classes it's by reference
            //but i could be wrong and it's probably better to just ensure it's totally deleted
            terminal.Airlines[terminal.GetAirlineFromFlight(terminal.Flights[flightNumber]).Code].Flights
                .Remove(flightNumber);
            terminal.Flights.Remove(flightNumber);

            string flightBoardingGate = GetBoardingGateCode(terminal, flightNumber);
            if (flightBoardingGate != "Unassigned") terminal.BoardingGates[flightBoardingGate].Flight = null!;
        }

        static void ModifyFlightDetails(Terminal terminal)
        {
            DisplayAirlineFlights(terminal);

            bool firstIterationFN = true;
            string userFlightNumber;
            do
            {
                if (!firstIterationFN) Console.WriteLine("Flight does not exist! Please try again.");
                Console.Write("Choose an existing Flight to modify or delete: ");
                userFlightNumber = Console.ReadLine().Trim();
                userFlightNumber = userFlightNumber?.ToUpper();

                firstIterationFN = false;
            } while (!terminal.Flights.ContainsKey(userFlightNumber));

            Console.WriteLine("1. Modify Flight");
            Console.WriteLine("2. Delete Flight");

            bool firstIterationUO = true;
            string userOption;
            do
            {
                if (!firstIterationUO) Console.WriteLine("Invalid answer! Please try again.");
                Console.Write("Choose an option: ");
                userOption = Console.ReadLine().Trim();

                firstIterationUO = false;
            } while (!(userOption == "1" || userOption == "2"));

            if (userOption == "1")
            {
                string[] optionsA = { "1", "2", "3", "4" };
                bool firstIteration = true;
                string optionOneChoice;
                do
                {
                    if (!firstIteration) Console.WriteLine("Invalid answer! Please try again.");
                    Console.WriteLine("1. Modify Basic Information");
                    Console.WriteLine("2. Modify Status");
                    Console.WriteLine("3. Modify Special Request Code");
                    Console.WriteLine("4. Modify Boarding Gate");
                    Console.Write("Choose an option: ");
                    optionOneChoice = Console.ReadLine().Trim();

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

                    firstIteration = false;
                } while (!optionsA.Contains(optionOneChoice));
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
                Console.WriteLine($"{flight.Status,-13} {GetBoardingGateCode(terminal, flight.FlightNumber),-13}\n");
            }
        }

        static void ProcessUnassignedFlights(Terminal terminal)
        {
            Queue<Flight> flightGateQueue = new Queue<Flight>();
            int unassignedGatesCount = 0;
            int processedManually = 0;
            int processedAutomatically = 0;

            foreach (Flight flight in terminal.Flights.Values)
            {
                string flightNumber = flight.FlightNumber;
                string boardingGate = GetBoardingGateCode(terminal, flightNumber);

                if (boardingGate == "Unassigned") flightGateQueue.Enqueue(flight);
            }

            foreach (BoardingGate boardingGate in terminal.BoardingGates.Values)
            {
                if (boardingGate.Flight == null) unassignedGatesCount++;
                else processedManually++;
            }

            Console.WriteLine($"Number of flights with no boarding gates: {flightGateQueue.Count}");
            Console.WriteLine($"Number of gates with no flights: {unassignedGatesCount}");

            //this doesnt actually do anything its just for a loop
            while (flightGateQueue.Count > 0)
            {
                Flight dequeuedFlight = flightGateQueue.Dequeue();
                string specialRequestCode = dequeuedFlight.GetType().Name.Substring(0, 4);

                if (specialRequestCode != "NORM")
                {
                    foreach (BoardingGate boardingGate in terminal.BoardingGates.Values)
                    {
                        bool supportsSpecialRequest = specialRequestCode switch
                        {
                            "NORM" => true,
                            "CFFT" => boardingGate.SupportsCFFT,
                            "DDJB" => boardingGate.SupportsDDJB,
                            "LWTT" => boardingGate.SupportsLWTT,
                            _ => false
                        };

                        if (supportsSpecialRequest && boardingGate.Flight == null)
                        {
                            boardingGate.Flight = dequeuedFlight;
                            DisplayFlightInfo(terminal, dequeuedFlight.FlightNumber, "");
                            processedAutomatically++;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (BoardingGate boardingGate in terminal.BoardingGates.Values)
                    {
                        if (boardingGate.Flight == null && (!boardingGate.SupportsCFFT && !boardingGate.SupportsDDJB && !boardingGate.SupportsLWTT))
                        {
                            boardingGate.Flight = dequeuedFlight;
                            DisplayFlightInfo(terminal, dequeuedFlight.FlightNumber, "");
                            processedAutomatically++;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine($"Number of flights and gates processed and assigned: {processedAutomatically + processedManually}.");
            if (processedManually == 0) Console.WriteLine("Percentage change in processed and assigned flights and gates: Undefined.");
            else Console.WriteLine($"Percentage change in processed and assigned flights and gates: {((processedAutomatically / processedManually) * 100):F2}.");
        }

        static void DisplayDailyAirlineFee(Terminal terminal)
        {
            int unassignedCount = 0;
            double feesEarned = 0;

            foreach (Flight flight in terminal.Flights.Values)
            {
                string boardingGate = GetBoardingGateCode(terminal, flight.FlightNumber);
                if (boardingGate == "Unassigned")
                {
                    Console.WriteLine($"Flight {flight.FlightNumber} has not been assigned a boarding gate! Please assign and try again.");
                    unassignedCount++;
                }
            }

            if (unassignedCount != 0) return; //ensure all flights are assigned a gate

            foreach (Airline airline in terminal.Airlines.Values)
            {
                Console.WriteLine($"{airline.Name} fees charged:");
                feesEarned += airline.CalculateFees();
            }

            Console.WriteLine($"Total earned in fees: {feesEarned:C}");
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
            Console.WriteLine("8. Process Unassigned Flights");
            Console.WriteLine("9. Display Daily Airline Fee");
            Console.WriteLine("0. Exit");
            Console.Write("Please select your option: ");
            string userOption = Console.ReadLine().Trim();

            return userOption;
        }

        static void Main()
        {
            //DO NOT TOUCH THESE ARE THE INITIALISERS
            Terminal terminalFive = new Terminal("Terminal Name");
            InitAirlines(terminalFive);
            InitBoardingGates(terminalFive);
            InitFlights(terminalFive);

            Console.WriteLine("Loading Airlines...");
            Console.WriteLine($"{terminalFive.Airlines.Count} Airlines Loaded!");
            Console.WriteLine("Loading Boarding Gates...");
            Console.WriteLine($"{terminalFive.BoardingGates.Count} Boarding GatesLoaded!");
            Console.WriteLine("Loading Flights...");
            Console.WriteLine($"{terminalFive.Flights.Count} Flights Loaded!\n\n\n\n");
            //DO NOT TOUCH THESE ARE THE INITIALISERS

            string[] acceptableOptions = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            while (true)
            {
                string userOption = DisplayMainMenu();
                if (userOption == "0")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }

                if (acceptableOptions.Contains(userOption))
                {
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
                        case "8":
                            ProcessUnassignedFlights(terminalFive);
                            break;
                        case "9":
                            DisplayDailyAirlineFee(terminalFive);
                            break;
                    }
                }

                else Console.WriteLine("\nInvalid option! Please try again.\n");
            }
        }
    }
}
