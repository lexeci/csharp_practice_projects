using System;

class EnergyCostCalculator
{
    static void Main()
    {
        Console.WriteLine("=== Розрахунок вартості електроенергії за рік ===");

        Console.Write("Введіть споживання пристрою (Вт): ");
        double powerWatts = Convert.ToDouble(Console.ReadLine());

        Console.Write("Скільки годин на добу працює пристрій?: ");
        double hoursPerDay = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введіть тариф за 1 кВт·год (наприклад, 0.22 для £ або 4.32 для грн): ");
        double ratePerKWh = Convert.ToDouble(Console.ReadLine());

        double energyKWhPerYear = (powerWatts * hoursPerDay * 365) / 1000;
        double annualCost = energyKWhPerYear * ratePerKWh;

        Console.WriteLine($"\n📊 Споживання на рік: {energyKWhPerYear:F2} кВт·год");
        Console.WriteLine($"💸 Вартість на рік: {annualCost:F2} {GetCurrencySymbol(ratePerKWh)}");
    }

    static string GetCurrencySymbol(double rate)
    {
        if (rate < 1)
            return "£";
        else if (rate > 1 && rate < 10)
            return "грн";
        else
            return "₴/інша";
    }
}
