using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace ConsoleApp1
{
    class Program
    {
        public static string HexKey(byte[] data)
        {
            using (SHA256 alg = SHA256.Create())
            {
                alg.ComputeHash(data);
                return BitConverter.ToString(alg.Hash).Replace("-", "");
            }
        }
        static string HMAC(string str, string key)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            using (HMACSHA256 hmac = new HMACSHA256(bkey))
            {
                byte[] bstr = Encoding.Default.GetBytes(str);
                byte[] bhash = hmac.ComputeHash(bstr);
                return BitConverter.ToString(bhash).Replace("-", "");
            }
        }
        static byte[] RandomNG()
        {
            byte[] bytes = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }
        static void Main(string[] args)
        {
            while (true)
            {
                string s;
                Console.WriteLine("Enter an odd number of lines separated by a space: ");
                s = Console.ReadLine();
                string[] s1 = s.Split(' ');
                int n = s1.Length;
                bool allUnique = s1.Distinct().Count() == n;
                if (n % 2 != 0 && allUnique && n != 1)
                {
                    while (true)
                    {
                        Console.Clear();
                        byte[] data = RandomNG();
                        string key = HexKey(data);
                        Random rnd = new Random();
                        int sc = rnd.Next(n), check;
                        string hmac = HMAC(s1[sc], key);
                        Console.WriteLine("HMAC: " + hmac);
                        Console.WriteLine("Available moves:");
                        for (int i = 0; i < n; i++)
                        {
                            Console.WriteLine((i + 1) + " - " + s1[i]);
                        }
                        Console.WriteLine((n + 1) + " - Выход");
                        Console.Write("Enter your move: ");
                        while (!int.TryParse(Console.ReadLine(), out check))
                        {
                            Console.WriteLine("Error! Write number 1 - " + (n + 1));
                        }
                        if (check >= n + 1) break;
                        Console.WriteLine("Your move: " + s1[check - 1]);
                        Console.WriteLine("Computer move: " + s1[sc]);
                        if (check - 1 == sc) Console.WriteLine("Draw!");
                        else
                        {
                            int t = ((n - 1) / 2) + 1;
                            int loseL = (n - (t - check)) + 1;
                            if (loseL > n) loseL -= n;
                            int check1 = check;
                            if (check1 == 1) check1 = n;
                            if (loseL <= check1)
                            {
                                if (sc >= (loseL - 1) && sc <= (check1 - 1))
                                {
                                    Console.WriteLine("You won!!!");
                                }
                                else
                                {
                                    Console.WriteLine("You lose!!!");
                                }
                            }
                            else
                            {
                                if (sc >= (loseL - 1) || sc <= (check1 - 1))
                                {
                                    Console.WriteLine("You won!!!");
                                }
                                else
                                {
                                    Console.WriteLine("You lose!!!");
                                }
                            }
                        }
                        Console.WriteLine("HMAC key: " + key);
                        Console.WriteLine("Press any Button");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect input");
                    Console.WriteLine("For example: '1 2 3' or 'Rock Paper Scissors'");
                }
            }
        }
    }
}
