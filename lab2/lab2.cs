namespace lab2
{
    class Program
    {
        public static double e;
        public delegate double MyFunc(double x);
        static bool done;
        static readonly object locker = new();

        public static void Main(string[] args)
        {
            e = 1e-6;

            Thread thread1 = new(ThreadFunction1);
            Thread thread2 = new(ThreadFunction2);
            
            thread1.Start();
            thread2.Start();

            Thread t = new(Go);
            t.Start();
            Go();

            thread1.Join();
            thread2.Join();
            t.Join();
        }

        public static double g1(double x) => 0.5 * Math.Cos(x);

        public static double g2(double x) => (2 * x - Math.Log(x)) / 3;

        public static double calc(double x, MyFunc g)
        {
            double previousX;
            double difference;
            do {
                previousX = x;
                x = g(x);
                difference = Math.Abs(previousX - x);
            } while (difference >= e);
            
            return x;
        }

        static void ThreadFunction1()
        {
            double initialX = 0.5;
            MyFunc g = g1;
            double result = calc(initialX, g);

            lock (locker) {
                Console.WriteLine("\nResult from Thread 1:");
                Console.WriteLine(" 2x - cos(x) = 0");
                Console.WriteLine($" X = {result:F5}");
            }
        }

        static void ThreadFunction2()
        {
            double initialX = 0.75;
            MyFunc g = g2;
            double result = calc(initialX, g);

            lock (locker) {
                Console.WriteLine("\nResult from Thread 2:");
                Console.WriteLine(" x + ln(x) = 0");
                Console.WriteLine($" X = {result:F5}");
            }
        }
        
        static void Go()
        {
            lock (locker) {
                if (!done) {
                    Console.WriteLine("Done");
                    done = true;
                }
            }
        }
    }
}