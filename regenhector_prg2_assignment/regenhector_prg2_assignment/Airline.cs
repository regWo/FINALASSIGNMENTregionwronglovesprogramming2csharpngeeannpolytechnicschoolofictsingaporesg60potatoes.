namespace regenhector_prg2_assignment
{
    public class Airline
    {
        //attributes
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();

        //constructors
        public Airline() { }

        public Airline(string n, string c)
        {
            Name = n;
            Code = c;
        }

        public Airline(string n, string c, Dictionary<string, Flight> f)
        {
            Name = n;
            Code = c;
            Flights = f;
        }

        //methods
        public bool AddFlight(Flight addF)
        {
            string airlineCode = addF.FlightNumber.Substring(0, 2);

            if (airlineCode == Code)
            {
                Flights[addF.FlightNumber] = addF;
                return true;
            }
            return false;
        }

        public double CalculateFees()
        {
            double airlineFee = 0;
            double subtractionDiscount = 0;
            string[] originDiscountList = { "Dubai (DXB)", "Bangkok (BKK)", "Tokyo (NRT)" };
            //List<string> originDiscountList = new List<string> { "Dubai (DXB)", "Bangkok (BKK)", "Tokyo (NRT)" };

            foreach (Flight flight in Flights.Values)
            {
                airlineFee += flight.CalculateFees();
                int flightHour = flight.ExpectedTime.Hour;

                //before 11 am or after 9 pm
                if (flightHour < 11 || flightHour > 21)
                {
                    subtractionDiscount += 110;
                }

                //.Contains is from the LINQ module for arrays
                //if there's an issue remove the string array and replace it with the commented list
                if (originDiscountList.Contains(flight.Origin))
                {
                    subtractionDiscount += 25;
                }

                if (flight is NORMFlight)
                {
                    subtractionDiscount += 50;
                }
            }

            int numberOf3Discounts = Flights.Count() / 3;
            subtractionDiscount += (numberOf3Discounts * 350);

            //i also don't know what to call this
            double additionalBillDiscount = (Flights.Count() > 5) ? 0.97 : 1;
            airlineFee *= additionalBillDiscount;

            return airlineFee - subtractionDiscount;
        }

        public bool RemoveFlight(Flight removeF)
        {
            //checks if flight is in airline's flight list
            if (Flights.ContainsKey(removeF.FlightNumber))
            {
                Flights.Remove(removeF.FlightNumber);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Name} {Code}";
        }
    }
}