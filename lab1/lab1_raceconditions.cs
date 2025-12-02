namespace lab1;

class Test
{
    static readonly double[] original_y = [7.1, 27.8, 62.1, 110, 161];

    static double[] x = [1, 2, 3, 4, 5];
    static double[] y;
    static int n = 0;

    static double a1, b1, a2, b2;
    static double d1, d2;

    static void ResetData()
    {
        y = original_y.ToArray();
        x = [1, 2, 3, 4, 5];
        n = x.Length;

        for (int i = 0; i < n; i++)
        {
            x[i] = Math.Log(x[i]);
        }
    }

    static void RunRace()
    {
        ResetData();

        Thread thread1 = new(ThreadFunction1);
        Thread thread2 = new(ThreadFunction2);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine($" d1 = {d1:F10}");
        Console.WriteLine($" d2 = {d2:F10}");
    }

    public static void Main(string[] args)
    {
        for (int run = 1; run <= 2; run++)
        {
            Console.Write("\n");
            RunRace();
        }
    }

    static void ThreadFunction1()
    {

        double Xi = 0, Xi2 = 0, XiYi = 0, Yi = 0;

        for (int i = 0; i < n; i++)
        {
            Xi += x[i];
            Xi2 += x[i] * x[i];
            XiYi += x[i] * y[i];
            Yi += y[i];
        }

        double det = n * Xi2 - Xi * Xi;
        if (det == 0) return;

        a1 = (n * XiYi - Xi * Yi) / det;
        b1 = (Yi * Xi2 - Xi * XiYi) / det;

        double sum_diff_sq = 0;
        for (int i = 0; i < n; i++)
        {
            double predicted_y = a1 * x[i] + b1;
            sum_diff_sq += (y[i] - predicted_y) * (y[i] - predicted_y);
        }

        d1 = Math.Sqrt(sum_diff_sq);
    }

    static void ThreadFunction2()
    {

        for (int i = 0; i < n; i++)
        {
            y[i] = Math.Log(y[i]);
            Thread.Sleep(7); 
        }

        double Xi = 0, Xi2 = 0, XiYi = 0, Yi = 0;

        for (int i = 0; i < n; i++)
        {
            Xi += x[i];
            Xi2 += x[i] * x[i];
            XiYi += x[i] * y[i];
            Yi += y[i];
        }

        double det = n * Xi2 - Xi * Xi;
        if (det == 0) return;

        a2 = (n * XiYi - Xi * Yi) / det;
        b2 = (Yi * Xi2 - Xi * XiYi) / det;

        double sum_diff_sq = 0;
        for (int i = 0; i < n; i++)
        {
            double predicted_ln_y = a2 * x[i] + b2;
            sum_diff_sq += (y[i] - predicted_ln_y) * (y[i] - predicted_ln_y);
        }

        d2 = Math.Sqrt(sum_diff_sq);
    }
}