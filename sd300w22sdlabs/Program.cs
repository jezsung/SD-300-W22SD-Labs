Dictionary<int, int> moneyFloat = new Dictionary<int, int>
{
    {1 ,4}
};

Dictionary<Product, int> inventory = new Dictionary<Product, int>
{
    {new Product("Chocolate-covered Beans", 2, "A12"),  3}
};

VendingMachine vendingMachine = new VendingMachine(moneyFloat, inventory, "BARCODE1");

try
{
    vendingMachine.StockItem(new Product("Fried Spider", 4, "A13"), 4);
    foreach (KeyValuePair<Product, int> entity in inventory)
    {
        Console.WriteLine($"{entity.Key.Name}: {entity.Value}");
    }

    vendingMachine.StockFloat(5, 2);
    foreach (KeyValuePair<int, int> entity in moneyFloat)
    {
        Console.WriteLine($"${entity.Key}: {entity.Value}");
    }

    Console.WriteLine(vendingMachine.VendItem("A12", new List<int> { 1 }));
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}


class VendingMachine
{
    public int SerialNumber { get; set; }
    private Dictionary<int, int> MoneyFloat { get; set; }
    private Dictionary<Product, int> Inventory { get; set; }
    public string Barcode { get; }

    private static int SerialNumberCounter = 1;

    public VendingMachine(Dictionary<int, int> moneyFloat, Dictionary<Product, int> inventory, string barcode)
    {
        if (barcode.Length == 0)
        {
            throw new Exception("VendingMachine barcode cannot be empty");
        }

        SerialNumber = SerialNumberCounter;
        SerialNumberCounter++;
        MoneyFloat = moneyFloat;
        Inventory = inventory;
        Barcode = barcode;
    }

    public string StockItem(Product product, int quantity)
    {
        if (quantity < 0)
        {
            throw new Exception("Quantity cannot be negative");
        }

        Inventory.Add(product, quantity);

        return string.Join(", ", Inventory.Select(e => $"{e.Key}: {e.Value}"));
    }

    public string StockFloat(int moneyDenomination, int quantity)
    {
        MoneyFloat.Add(moneyDenomination, quantity);

        return string.Join(", ", MoneyFloat.Select(e => $"${e.Key}: {e.Value}"));
    }

    public string VendItem(string code, List<int> money)
    {
        Product product = Inventory.FirstOrDefault(e => e.Key.Code == code).Key;

        if (product == null)
        {
            throw new Exception($"Product with the code {code} does not exist");
        }

        int quantity = Inventory[product];

        if (quantity == 0)
        {
            throw new Exception($"Product with the code {code} is out of stock");
        }

        if (product.Price > money.Sum())
        {
            throw new Exception("Insufficient money provided");
        }

        int change = money.Sum() - product.Price;

        if (MoneyFloat.Sum(e => e.Key * e.Value) < change)
        {
            throw new Exception("The machine does not have enough money to return the change");
        }

        int temp = change;

        foreach (int denomination in MoneyFloat.Select(e => e.Key).OrderByDescending((k) => k).ToList())
        {
            int requiredCoinCount = temp / denomination;

            if (requiredCoinCount > 0)
            {
                if (MoneyFloat[denomination] - requiredCoinCount >= 0)
                {
                    temp -= requiredCoinCount * denomination;
                    MoneyFloat[denomination] = MoneyFloat[denomination] - requiredCoinCount;
                }
                else
                {
                    temp -= MoneyFloat[denomination] * denomination;
                    MoneyFloat[denomination] = 0;
                }
            }
        }

        Inventory[product] -= 1;

        return $"Please enjoy your {product.Name} and take your change of ${change}";
    }
}

class Product
{
    public string Name { get; set; }
    public int Price { get; set; }
    public string Code { get; set; }

    public Product(string name, int price, string code)
    {
        if (name == null || name.Length == 0)
        {
            throw new Exception("Product name cannot be null or empty");
        }
        if (code == null || code.Length == 0)
        {
            throw new Exception("Product code cannot be null or empty");
        }
        if (price <= 0)
        {
            throw new Exception("Product price cannot be negative or zero");
        }

        Name = name;
        Price = price;
        Code = code;
    }
}
