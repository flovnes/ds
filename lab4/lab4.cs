namespace lab4
{
    delegate T MathOp<T>(T a, T b);
    delegate void FormatNumber(double number);

    class Program
    {
        static readonly object locker = new();

        static double Add(double a, double b)
        {
            double result = a + b;
            lock (locker) {
                Console.WriteLine("Add result = {0:N}", result);
            }
            return result;
        }

        static double Divide(double a, double b)
        {
            double result = a / b;
            lock (locker) {
                Console.WriteLine("Divide result = {0:N}", result);
            }
            return result;
        }

        static double Multiply(double a, double b)
        {
            double result = a * b;
            lock (locker) {
                Console.WriteLine("Multiply result = {0:N}", result);
            }
            return result;
        }

        static void FormatNumberAsCurrency(double number)
        {
            lock (locker) {
                Console.WriteLine("A Currency: {0:C}", number);
            }
        }

        static void FormatNumberWithCommas(double number)
        {
            lock (locker) {
                Console.WriteLine("With Commas: {0:N}", number);
            }
        }

        static void FormatNumberWithTwoPlaces(double number)
        {
            lock (locker) {
                Console.WriteLine("With 3 places: {0:F3}", number);
            }
        }

        static void Main()
        {
            double a = 1.0;
            double b = 3.0;
            
            FormatNumber format = FormatNumberAsCurrency;
            format += FormatNumberWithCommas;
            format += FormatNumberWithTwoPlaces;
            
            format(12345.6789);

            List<MathOp<double>> opsList = [Add, Divide, Multiply];

            List<Thread> threads = [];

            foreach (MathOp<double> op in opsList) {
                MathOp<double> currentOp = op; 
                Thread t = new(() => { currentOp(a, b); });
                threads.Add(t);
                t.Start();
            }

            foreach (Thread t in threads) 
                t.Join();
        }
    }
}