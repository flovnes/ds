class Test
{
    static double[] x = [1, 2, 3, 4, 5];
    static double[] y = [7.1, 27.8, 62.1, 110, 161];
    static int n = 0;

    static double a1, b1, a2, b2;
    static double d1, d2;

    public static void Main (string[] args)
    {
        if (x.Length == y.Length) {
            n = x.Length;
        }

        for (int i=0; i<n; i++) {
            x [i] = Math.Log (x [i]);
        }

        Thread thread1 = new(ThreadFunction1);
        Thread thread2 = new(ThreadFunction2);

        thread1.Start();
        thread2.Start();

        thread1.Join(); 
        thread2.Join();

        Console.WriteLine ($"d1 (y = a1*lnx + b1) = {d1}");
        Console.WriteLine ($"d2 (ln(y) = a2*lnx + b2) = {d2}");
        
        if (d1 < d2) {
            Console.WriteLine ("Result Point Vector: ");
            Console.WriteLine ($"y = {a1:F4} * lnx + {b1:F4}");
        } else {
            Console.WriteLine ("Result Point Vector: ");
            Console.WriteLine ($"y = {Math.Pow (Math.E, a2):F4} * x^ {b2:F4}");
        }
        Console.Read();

    }

    static void ThreadFunction1()
    {
        double Xi = 0, Xi2 = 0, XiYi = 0, Yi = 0;
        
        for (int i=0; i<n; i++) {
            Xi += x [i];
            Xi2 += x [i] * x [i];
            XiYi += x [i] * y [i]; 
            Yi += y [i];
        }
        
        double det = n * Xi2 - Xi * Xi;

        if (det == 0) return;

        a1 = (n * XiYi - Xi * Yi) / det;
        b1 = (Yi * Xi2 - Xi * XiYi) / det;
        
        double sum_diff_sq = 0;
        for (int i=0; i<n; i++) {
            double predicted_y = a1 * x[i] + b1;
            sum_diff_sq += (y[i] - predicted_y) * (y[i] - predicted_y);
        }
        
        d1 = Math.Sqrt (sum_diff_sq); 

        Console.WriteLine ("Thread1");
    }

    static void ThreadFunction2() {
        double Xi = 0, Xi2 = 0, XiYi = 0, Yi = 0; 

        for (int i=0; i<n; i++) {
            double current_ln_y = Math.Log (y [i]); 
            
            Xi += x [i]; 
            Xi2 += x [i] * x [i];
            XiYi += x [i] * current_ln_y; 
            Yi += current_ln_y;
        }
        
        double det = n * Xi2 - Xi * Xi;
        
        if (det == 0) return;

        a2 = (n * XiYi - Xi * Yi) / det;
        b2 = (Yi * Xi2 - Xi * XiYi) / det;
        
        double sum_diff_sq = 0;
        for (int i=0; i<n; i++) {
            double current_ln_y = Math.Log (y [i]);
            double predicted_ln_y = a2 * x[i] + b2;
            sum_diff_sq += (current_ln_y - predicted_ln_y) * (current_ln_y - predicted_ln_y);
        }
        
        d2 = Math.Sqrt (sum_diff_sq);

        Console.WriteLine ("Thread2");
    }
}