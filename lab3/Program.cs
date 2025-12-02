namespace lab3
{
    delegate void FormatNumber(double number);

    class Program
    {
        static readonly object locker = new();

        static void FormatNumberAsCurrency(double number)
        {
            lock (locker)
            {
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

        static void Main(string[] args)
        {
            FormatNumber format = FormatNumberAsCurrency;
            format += FormatNumberWithCommas;
            format += FormatNumberWithTwoPlaces;
            double numberToProcess = 12345.6789;

            Thread t = new(() => format(numberToProcess));
            t.Start();
            
            t.Join(); 
        }
    }
}