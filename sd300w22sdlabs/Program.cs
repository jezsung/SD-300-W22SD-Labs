class Hotel
{
    public string Name { get; set; }
    public string Address { get; set; }
    public List<Room> Rooms { get; set; }
    public List<Client> Clients { get; set; }

    public Hotel(string name, string address, List<Room> rooms, List<Client> clients)
    {
        Name = name;
        Address = address;
        Rooms = rooms;
        Clients = clients;
    }
}

class Room
{
    public string Number { get; set; }
    public int Capacity { get; set; }
    public bool Occupied { get; set; }
    public List<Reservation> Reservations { get; set; }

    public Room(string number, int capacity, bool occupied, List<Reservation> reservations)
    {
        Number = number;
        Capacity = capacity;
        Occupied = occupied;
        Reservations = reservations;
    }
}

class Client
{
    public string Name { get; set; }
    public long CreditCard { get; set; }
    public List<Reservation> Reservations { get; set; }

    public Client(string name, long creditCard, List<Reservation> reservations)
    {
        Name = name;
        CreditCard = creditCard;
        Reservations = reservations;
    }
}

class Reservation
{
    public DateTime Date { get; set; }
    public int Occupants { get; set; }
    public bool IsCurrent { get; set; }
    public Client Client { get; set; }
    public Room Room { get; set; }

    public Reservation(DateTime date, int occupants, bool isCurrent, Client client, Room room)
    {
        Date = date;
        Occupants = occupants;
        IsCurrent = isCurrent;
        Client = client;
        Room = room;
    }
}


class VIPClient : Client
{
    public int VIPNumber { get; set; }
    public int VIPPoints { get; set; }

    public VIPClient(string name, long creditCard, List<Reservation> reservations, int vipNumber, int vipPoints) : base(name, creditCard, reservations)
    {
        VIPNumber = vipNumber;
        VIPPoints = vipPoints;
    }
}


class PremiumRoom : Room
{
    public string AdditionalAmentities { get; set; }
    public int VIPValue { get; set; }

    public PremiumRoom(string number, int capacity, bool occupied, List<Reservation> reservations, string additionalAmentities, int vipValue) : base(number, capacity, occupied, reservations)
    {
        AdditionalAmentities = additionalAmentities;
        VIPValue = vipValue;
    }
}