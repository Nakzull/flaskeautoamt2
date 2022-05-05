using System;
using System.Collections.Generic;
using System.Threading;

namespace flaskeautoamt_nu_med_classer
{
    class Program
    {
        public static Queue<Bottle> products = new Queue<Bottle>(20);
        public static Queue<Bottle> beers = new Queue<Bottle>(20);
        public static Queue<Bottle> sodas = new Queue<Bottle>(20);
        static void Main(string[] args)
        {
            Bottle bottleProducer = new Bottle();

            Thread t1 = new Thread(bottleProducer.ProduceDrink);
            Thread t2 = new Thread(bottleProducer.SplitBottles);
            Thread t3 = new Thread(bottleProducer.TakeBeer);
            Thread t4 = new Thread(bottleProducer.TakeSoda);

            t1.Name = "Bottle Producer";
            t2.Name = "Bottle Sorter";
            t3.Name = "Beer Sorter";
            t4.Name = "Soda Sorter";

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
        }
    }
}
