using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Program
{
    public static List<int> MinCoins(List<int> coinCategories, int amount)
    {
        coinCategories = coinCategories.OrderByDescending(c => c).ToList();

        List<int> result = new List<int>();

        foreach (int coinCategory in coinCategories)
        {
            while (amount >= coinCategory)
            {
                amount -= coinCategory;
                result.Add(coinCategory);
            }

        }
        return result;
    }


    static void Main(string[] args)
    {
        List<int> CoinCategories = new List<int> { 1, 5, 10, 20, 50, 100 };

        int amount = 69;

        List<int> result = MinCoins(CoinCategories, amount);

        Console.WriteLine($"Coins used to form the total Amount: {amount}");
        for (int i = 0; i < result.Count; i++)
            Console.Write(result[i] + " ");
        Console.WriteLine("\nTotal Coins: " + result.Count);



    }
}