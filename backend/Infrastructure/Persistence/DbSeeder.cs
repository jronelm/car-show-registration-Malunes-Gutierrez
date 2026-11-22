namespace Infrastructure.Persistence;

using Domain.Entities;
using Domain.Enums;
using static Domain.Enums.RegistrationType;
using static Domain.Enums.RegistrationStatus;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        
        if (db.Registrations.Any())
            return;

        var start = new DateTime(2026, 10, 1, 8, 0, 0, DateTimeKind.Utc);

        var rows = new (string Name, string Contact, string Email, string Address,
                        string Car, string Plate, RegistrationType Type,
                        RegistrationStatus Status, decimal Paid)[]
        {
            ("Maria Santos",     "09171110001", "maria.santos@example.com",     "Magallanes St., Sorsogon City", "Honda Civic EK9",          "SRS 1001", Exhibitor,  Confirmed, 1500m),
            ("Pedro Reyes",      "09171110002", "pedro.reyes@example.com",      "Rizal St., Legazpi City",       "Toyota AE86 Trueno",       "SRS 1002", Competitor, Confirmed, 2500m),
            ("Ana Villanueva",   "09171110003", "ana.villanueva@example.com",   "Burgos St., Naga City",         "Nissan Skyline R34",       "SRS 1003", VIP,        CheckedIn, 5000m),
            ("Carlo Mendoza",    "09171110004", "carlo.mendoza@example.com",    "Pepita Park, Sorsogon City",    "Mazda RX-7 FD",            "SRS 1004", Competitor, Pending,   0m),
            ("Liza Ramos",       "09171110005", "liza.ramos@example.com",       "Bulan, Sorsogon",               "Subaru Impreza WRX STI",   "SRS 1005", Exhibitor,  Pending,   500m),
            ("Jose Garcia",      "09171110006", "jose.garcia@example.com",      "Gubat, Sorsogon",               "Mitsubishi Lancer Evo IX", "SRS 1006", Competitor, CheckedIn, 2500m),
            ("Rosa Aquino",      "09171110007", "rosa.aquino@example.com",      "Irosin, Sorsogon",              "Porsche 911 Carrera",      "SRS 1007", VIP,        Confirmed, 2500m),
            ("Miguel Torres",    "09171110008", "miguel.torres@example.com",    "Barcelona, Sorsogon",           "Ford Mustang GT",          "SRS 1008", VIP,        Pending,   0m),
            ("Grace Bautista",   "09171110009", "grace.bautista@example.com",   "Castilla, Sorsogon",            "Toyota Supra MK4",         "SRS 1009", Competitor, Confirmed, 1000m),
            ("Ramon Cruz",       "09171110010", "ramon.cruz@example.com",       "Donsol, Sorsogon",              "Honda S2000",              "SRS 1010", Exhibitor,  CheckedIn, 1500m),
            ("Juan Dela Cruz",   "09171110011", "juan.delacruz@example.com",    "Rizal St., Naga City",          "Nissan GT-R R35",          "SRS 1011", Competitor, Confirmed, 1000m),
            ("Juanita Lopez",    "09171110012", "juanita.lopez@example.com",    "Pilar, Sorsogon",               "Volkswagen Golf GTI",      "SRS 1012", Exhibitor,  Confirmed, 1500m),
            ("Andrea Navarro",   "09171110013", "andrea.navarro@example.com",   "Matnog, Sorsogon",              "BMW M3 E46",               "SRS 1013", VIP,        CheckedIn, 5000m),
            ("Rico Fernandez",   "09171110014", "rico.fernandez@example.com",   "Juban, Sorsogon",               "Chevrolet Camaro SS",      "SRS 1014", Competitor, Pending,   0m),
            ("Ana Marie Flores", "09171110015", "anamarie.flores@example.com",  "Magallanes, Sorsogon",          "Mini Cooper S",            "SRS 1015", Exhibitor,  Pending,   1000m),
            ("Dennis Castillo",  "09171110016", "dennis.castillo@example.com",  "Prieto Diaz, Sorsogon",         "Mazda MX-5 Miata",         "SRS 1016", Competitor, Confirmed, 2500m),
            ("Bea Domingo",      "09171110017", "bea.domingo@example.com",      "Albay District, Legazpi City",  "Mercedes-Benz C63 AMG",    "SRS 1017", VIP,        Confirmed, 5000m),
            ("Allan Pascual",    "09171110018", "allan.pascual@example.com",    "Daraga, Albay",                 "Suzuki Swift Sport",       "SRS 1018", Exhibitor,  Confirmed, 750m),
            ("Kevin Soriano",    "09171110019", "kevin.soriano@example.com",    "Camalig, Albay",                "Toyota GR86",              "SRS 1019", Competitor, CheckedIn, 2500m),
            ("Nina Salazar",     "09171110020", "nina.salazar@example.com",     "Tabaco City, Albay",            "Audi R8",                  "SRS 1020", VIP,        Pending,   1000m),
            ("Tomas Hernandez",  "09171110021", "tomas.hernandez@example.com",  "Polangui, Albay",               "Honda Integra Type R",     "SRS 1021", Exhibitor,  CheckedIn, 1500m),
            ("Lea Morales",      "09171110022", "lea.morales@example.com",      "Ligao City, Albay",             "Datsun 240Z",              "SRS 1022", Competitor, Cancelled, 1000m),
            ("Victor Rivera",    "09171110023", "victor.rivera@example.com",    "Oas, Albay",                    "Ferrari 458 Italia",       "SRS 1023", VIP,        Cancelled, 0m),
            ("Ella Gomez",       "09171110024", "ella.gomez@example.com",       "Guinobatan, Albay",             "Hyundai Veloster N",       "SRS 1024", Exhibitor,  Cancelled, 1500m),
            ("Paolo Diaz",       "09171110025", "paolo.diaz@example.com",       "Pioduran, Albay",               "Toyota Corolla AE92",      "SRS 1025", Competitor, Confirmed, 1500m),
        };

        var registrations = rows.Select((r, i) => new Registration
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = $"CS27-{i + 1:D4}",
            RegistrationDate = start.AddDays(i).AddMinutes(i * 7),
            ParticipantName = r.Name,
            ContactNumber = r.Contact,
            Email = r.Email,
            Address = r.Address,
            CarMakeModel = r.Car,
            PlateNumber = r.Plate,
            RegistrationType = r.Type,
            Status = r.Status,
            RegistrationFee = RegistrationFees.For(r.Type),
            AmountPaid = r.Paid
        });

        db.Registrations.AddRange(registrations);
        db.SaveChanges();
    }
}