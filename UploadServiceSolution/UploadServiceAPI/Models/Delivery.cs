public class Delivery
{

        public int pakkeID { get; set; }
        public string medlemsNavn { get; set; }
        public string pickupAdresse { get; set; }
        public string afleveringsAdresse { get; set; }


        // Konstruktør med parametre (Constructor with Parameters)
        public Delivery(string medlemsnavn, string pickupadresse, int pakkeid, string afleveringsadresse)
        {
                medlemsNavn = medlemsnavn;
                pickupAdresse = pickupadresse;
                pakkeID = pakkeid;
                afleveringsAdresse = afleveringsadresse;
        }
        public Delivery() { }
}