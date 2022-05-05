using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace flaskeautoamt_nu_med_classer
{
    class Bottle
    {
        static int count = 1;
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int number;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public Bottle()
        {

        }

        public Bottle(string name)
        {
            this.name = name;
            this.number = count;
            count++;
            //Console.WriteLine(this.name + this.number);
        }

        public void ProduceDrink()
        {           
            while (true)
            {
                Random rng = new Random();
                int temp = rng.Next(1, 3);
                string type;
                if (temp == 1)
                {
                    type = "beer ";
                }
                else
                {
                    type = "soda ";
                }
                lock (Program.products)
                {
                    if (Program.products.Count < 20)
                    {
                        
                        Bottle bottle = new Bottle(type);
                        Program.products.Enqueue(bottle);
                    }
                    else
                    {
                        Monitor.PulseAll(Program.products);
                        Monitor.Wait(Program.products);
                    }
                }
                Thread.Sleep(100 / 15);
            }
        }

        public void SplitBottles()
        {
            while (true)
            {
                lock (Program.products)
                {
                    while (Program.products.Count == 0)
                    {
                        Monitor.PulseAll(Program.products);
                        Monitor.Wait(Program.products);
                    }
                    Bottle bottle;
                    Program.products.TryDequeue(out bottle);
                    if (bottle.Name == "beer ")
                    {
                        lock (Program.beers)
                        {
                            if (Program.beers.Count < 20)
                            {
                                Program.beers.Enqueue(bottle);
                            }
                            else
                            {
                                Monitor.PulseAll(Program.beers);
                                Monitor.PulseAll(Program.products);
                                Monitor.Wait(Program.beers);
                                Monitor.Wait(Program.products);
                            }
                        }
                    }
                    else
                    {
                        lock (Program.sodas)
                        {
                            if (Program.sodas.Count < 20)
                            {
                                Program.sodas.Enqueue(bottle);
                            }
                            else
                            {
                                Monitor.PulseAll(Program.sodas);
                                Monitor.PulseAll(Program.products);
                                Monitor.Wait(Program.sodas);
                                Monitor.Wait(Program.products);
                            }
                        }
                    }
                }
                Thread.Sleep(100 / 15);
            }
        }
        public void TakeBeer()
        {
            while (true)
            {
                lock (Program.beers)
                {
                    while (Program.beers.Count == 0)
                    {
                        Monitor.PulseAll(Program.beers);
                        Monitor.Wait(Program.beers);
                    }
                    Bottle bottle;
                    Program.beers.TryDequeue(out bottle);
                    Console.WriteLine(Thread.CurrentThread.Name + " added " + bottle.name + bottle.number + " to the inventory");
                }
                Thread.Sleep(100 / 15);
            }
        }

        public void TakeSoda()
        {
            while (true)
            {
                lock (Program.sodas)
                {
                    while (Program.sodas.Count == 0)
                    {
                        Monitor.PulseAll(Program.sodas);
                        Monitor.Wait(Program.sodas);
                    }
                    Bottle bottle;
                    Program.sodas.TryDequeue(out bottle);
                    Console.WriteLine(Thread.CurrentThread.Name + " added " + bottle.name + bottle.number + " to the inventory");
                }
                Thread.Sleep(100 / 15);
            }
        }
    }
}

