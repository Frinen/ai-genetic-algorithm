using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Genetic g = new Genetic();
            g.start();
        }
    }

    class Genetic
    {
        List<double> population;
        List<double> fitness;
        List<double> chanses;
        List<double> fathers;
        List<string> pearentsBinary;
        double maxfitness;
        double sum;
        public Genetic()
        {
            fitness = new List<double>();
            chanses = new List<double>();
            fathers = new List<double>();
            population = new List<double>();
            
            for(int i = 0; i<10;i++)
            {
                Random rnd = new Random();
                population.Add(rnd.Next(2, 31));
            }
        }

        public void start()
        {
            sum = 0;
            for(int i = 0; i < population.Count;i++)
            {
                var f = func(population[i]);
                fitness.Add(f);
                sum += f;
                if(i==0)
                {
                    maxfitness = f;
                }
                if(maxfitness < f)
                {
                    maxfitness = f;
                }
            }
            Console.WriteLine($"Max fitnrss {maxfitness}");
            double fitnessDiff=0.5;
            
            while(fitnessDiff>0.1)
            {
                var oldfit = maxfitness;
                roulete();
                NewPopulation();
                maxfitness = func(population[0]);
                fitness = new List<double>();
                for (int i = 0; i < population.Count; i++)
                {
                    var f = func(population[i]);
                    if(maxfitness<f)
                    {
                        maxfitness = f;
                    }
                    fitness.Add(f);
                }
                fitnessDiff = Math.Abs( oldfit - maxfitness);
                Console.WriteLine($"Max fitnrss {maxfitness}");
                Console.WriteLine($"Fitnrss dif {fitnessDiff} (old-) {oldfit} (new-) {maxfitness}");
                Console.WriteLine(" ");
            }
            Console.ReadLine();
            
        }

        double func(double x)
        {
            return (Math.Pow(x, 2) * -1+50);
        }
        void roulete()
        {
            foreach(var one in fitness)
            {
                chanses.Add((one/sum)*100);

            }
            Random rnd = new Random(100);
            List<int> persents = new List<int>();
            for(int i = 0; i<10;i++)
            {
                persents.Add(rnd.Next(100));
            }

            foreach (var persent in persents)
            {
                double count = 0;
                var addtmp = chanses[0];
                for (int i = 0; i < chanses.Count;i++ )
                {
                    count += chanses[i];
                    if (count > persent)
                        break;
                    addtmp = chanses[i];
                }
                fathers.Add(addtmp);

                /*foreach (var chan in chanse)
                {
                    count += chan;
                    if (count > persent)
                    {
                        if(population[chanse.FindLastIndex(chan)>0)
                            fathers.Add(population[chanse.IndexOf(chan)-1]);
                        else
                            fathers.Add(population[chanse.IndexOf(chan)]);
                        break;
                    }
                }*/
            }
        }

        void NewPopulation()
        {
            pearentsBinary = new List<string>();
            var maxlen = 0;
            foreach (var pearent in fathers)
            {
                pearentsBinary.Add(Convert.ToString((int)pearent, 2));
                if(maxlen < Convert.ToString((int)pearent, 2).Length)
                    maxlen = Convert.ToString((int)pearent, 2).Length;
            }
            

            for (int j = 0; j < pearentsBinary.Count; j++)
            {
                var len = maxlen - pearentsBinary[j].Length;
                string addbin = "";

                if (pearentsBinary[j].Length + 1 == maxlen)
                {
                    pearentsBinary[j] = "0" + pearentsBinary[j];
                }
                else
                {
                    for (int i = 0; i < len; i++)
                    {
                        addbin += "0";
                    }
                    pearentsBinary[j] = addbin + pearentsBinary[j];
                }
            }
            var he = pearentsBinary;
            Mutation();
            var hehe = pearentsBinary;
            

            Crossover();
        }

        void Mutation()
        {
            Random rnd = new Random();
            
            for(int i = 0; i<pearentsBinary.Count; i++)
            {
                if (/*rnd.Next(10)*/ 8 == 8)
                {
                    int position = rnd.Next(pearentsBinary[i].Length);
                    var chego = pearentsBinary[i][position];
                    var blyat = pearentsBinary[i][position].Equals('1');
                    if (pearentsBinary[i][position].Equals('1'))
                    {
                        Console.WriteLine(pearentsBinary[i]);
                        var tmp = pearentsBinary[i].Remove(position, 1);
                        tmp = tmp.Insert(position, "0");
                        pearentsBinary[i] = tmp;
                        Console.WriteLine(pearentsBinary[i]);
                    }
                    else
                    {
                        Console.WriteLine(pearentsBinary[i]);
                        var tmp = pearentsBinary[i].Remove(position, 1);
                        tmp = tmp.Insert(position, "1");
                        pearentsBinary[i] = tmp;
                        Console.WriteLine(pearentsBinary[i]);
                    }
                    Console.WriteLine("");
                }
            }
        }

        void Crossover()
        {
            Random rnd = new Random();
            List<Pair> pairs = new List<Pair>();
            while(pearentsBinary.Count>1)
            {
                int position = rnd.Next(pearentsBinary.Count - 1);
                string parent1 = pearentsBinary[position];
                pearentsBinary.RemoveAt(position);
                position = rnd.Next(pearentsBinary.Count - 1);
                string parent2 = pearentsBinary[position];
                pearentsBinary.RemoveAt(position);
                pairs.Add(new Pair(parent1, parent2));
            }
            List<double> nextPopulation = new List<double>();
            foreach(var pair in pairs)
            {
                int position = rnd.Next(1, pair.parent1.Length);
                string binStr = pair.parent1.Substring(0, position) + pair.parent2.Substring(position);
                nextPopulation.Add(Convert.ToInt32(binStr,2));
                binStr = pair.parent2.Substring(0, position) + pair.parent1.Substring(position);
                nextPopulation.Add(Convert.ToInt32(binStr, 2));
            }
            var test = nextPopulation;
            population = nextPopulation;
        }
        class Pair
        {
            public Pair(string _parent1, string _parent2)
            {
                parent1 = _parent1;
                parent2 = _parent2;
            }
            public string parent1 { get; set; }
            public string parent2 { get; set; }
        }
    }

}
