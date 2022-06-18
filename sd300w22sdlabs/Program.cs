Dictionary<int, int> moneyFloat = new Dictionary<int, int>
{
    {1 ,4}
};

Dictionary<Product, int> inventory = new Dictionary<Product, int>
{
    {new Product("Chocolate-covered Beans", 2, "A12"),  3}
};

VendingMachine vendingMachine = new VendingMachine(moneyFloat, inventory);

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

Console.WriteLine(vendingMachine.VendItem("A12", new List<int> { 5 }));

class VendingMachine
{
    public int SerialNumber { get; set; }
    public Dictionary<int, int> MoneyFloat { get; set; }
    public Dictionary<Product, int> Inventory { get; set; }

    public static int SerialNumberCounter = 1;

    public VendingMachine(Dictionary<int, int> moneyFloat, Dictionary<Product, int> inventory)
    {
        SerialNumber = SerialNumberCounter;
        SerialNumberCounter++;
        MoneyFloat = moneyFloat;
        Inventory = inventory;
    }

    public string StockItem(Product product, int quantity)
    {
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
            return "Error: Does not exist";
        }

        int quantity = Inventory[product];

        if (quantity == 0)
        {
            return "Error: Out of stock";
        }

        if (product.Price > money.Sum())
        {
            return "Error: Insufficient money provided";
        }

        int change = money.Sum() - product.Price;

        if (MoneyFloat.Sum(e => e.Key * e.Value) < change)
        {
            return "Error: Insufficient change";
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
        Name = name;
        Price = price;
        Code = code;
    }
}
