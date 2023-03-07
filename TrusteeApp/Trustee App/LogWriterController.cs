using System.Text.Json;
using System.Text.Json.Nodes;

namespace TrusteeApp
{
    public static class LogWriterController
    {
        public static void Write(string error)
        {
            try
            {
                using(var sr = new StreamWriter("LogReport.txt", true))
                {
                    sr.WriteLineAsync(DateTime.Now.ToString() + " - " + error);
                }
            }
            catch { throw; }
        }
    }
}