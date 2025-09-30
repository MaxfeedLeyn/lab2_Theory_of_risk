using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
namespace lab2
{
    class Program
    {
        public class Agreement
        {
            public float ChanceOfSituation;
            public float ChanceOfSucceed;
            public string? NameOfSituation;
            public int Losses;
            public int Profit;

            private void SetChances()
            {
                Console.Write("Enter chance of happening: " + NameOfSituation + ": ");
                while (true)
                {
                    if (!float.TryParse(Console.ReadLine(), out ChanceOfSituation))
                    {
                        Console.WriteLine("Nah");
                    }
                    else if(ChanceOfSituation > 1) Console.WriteLine("No");
                    else
                    {
                        break;
                    }
                }
                ChanceOfSucceed = 1 - ChanceOfSituation;
            }

            private void SetProfits()
            {
                Console.Write("Input profit if situation will not happen: ");
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out Profit))
                    {
                        Console.WriteLine("Nah");
                    }
                    else
                    {
                        break;
                    }
                }
                Console.Write("Input profit if situation will happen(most likely negative one): ");
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out Losses))
                    {
                        Console.WriteLine("Nah");
                    }
                    else if(Losses > Profit) Console.WriteLine("Are you sure that they are losses?");
                    else
                    {
                        break;
                    }
                }
            }

            public void Set()
            {
                Console.Write("Enter name of situation that you would like to insure: ");
                NameOfSituation = Console.ReadLine();
                SetChances();
                SetProfits();
            }

        }

        static void Main()
        {
            try
            {
                int n = 0;
                Console.Write("Enter count of agreement: ");
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out n))
                    {
                        Console.WriteLine("Nah");
                    }
                    else
                    {
                        break;
                    }
                }
                Agreement[] agreements = new Agreement[n];
                for(int i = 0; i < n; i++)
                    agreements[i] = new Agreement();
                
                foreach (Agreement agreement in agreements)
                {
                    agreement.Set();
                }

                float[] M = new float[n], V = new float[n], O = new float[n],
                    CV = new float[n], SSV = new float[n], CSV = new float[n];
                
                for (int i = 0; i < n; i++)
                {
                    M[i] = agreements[i].ChanceOfSucceed * agreements[i].Profit +  agreements[i].ChanceOfSituation * agreements[i].Losses;
                }

                for (int i = 0; i < n; i++)
                    V[i] = (agreements[i].Losses - M[i]) * (agreements[i].Losses - M[i]) * agreements[i].ChanceOfSituation
                           + (agreements[i].Profit - M[i]) * (agreements[i].Profit - M[i]) * agreements[i].ChanceOfSucceed;

                for (int i = 0; i < n; i++)
                    O[i] = (float)Math.Sqrt(V[i]);

                for (int i = 0; i < n; i++)
                    CV[i] = O[i] / M[i];
                
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine($"Coefficient of variation: CV{i + 1} = {CV[i]}");    
                }
                
                var minInfo = CV
                    .Select((value, index) => new { Value = value, Index = index })
                    .Aggregate((minSoFar, next) => next.Value < minSoFar.Value ? next : minSoFar);
                
                Console.WriteLine($"The least risky in terms of the coefficient of variation is: {minInfo.Index} with" +
                                  $" value {minInfo.Value}");

                for (int i = 0; i < n; i++)
                {
                    float tmp = (agreements[i].Losses - M[i]) * (agreements[i].Losses - M[i]) * agreements[i].ChanceOfSituation;
                    SSV[i] = (float)Math.Sqrt(tmp);
                }

                for (int i = 0; i < n; i++)
                    CSV[i] = SSV[i] / M[i];

                for (int i = 0; i < n; i++)
                    Console.WriteLine($"Coefficient of variation: CSV{i + 1} = {CSV[i]}");
                
                var minInfo2 = CSV
                    .Select((value, index) => new { Value = value, Index = index })
                    .Aggregate((minSoFar, next) => next.Value < minSoFar.Value ? next : minSoFar);
                
                Console.WriteLine($"The least risky in terms of the coefficient of semi-variation is: {minInfo2.Index} with" +
                                  $" value {minInfo2.Value}");
            }
            catch (Exception)
            {
                Console.WriteLine("Something goes wrong");
            }
        }
    }
}