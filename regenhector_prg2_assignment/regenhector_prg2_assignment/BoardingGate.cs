namespace regenhector_prg2_assignment;

public class BoardingGate
{
    //attributes
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT { get; set; }
    public Flight Flight { get; set; }

    //constructors
    public BoardingGate() {}

    public BoardingGate(string gn, bool cfft, bool ddjb, bool lwtt, Flight f)
    {
        GateName = gn;
        SupportsCFFT = cfft;
        SupportsDDJB = ddjb;
        SupportsLWTT = lwtt;
        Flight = f;
    }

    //methods
    public double CalculateFees()
    {
        return 300;
    }

    public override string ToString()
    {
        return $"{GateName} {SupportsCFFT} {SupportsDDJB} {SupportsLWTT} {Flight}";
    }
}