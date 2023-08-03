using System.Net;
using System.Net.NetworkInformation;

namespace GetHostInfo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> results = new List<string>();
            string hostName = Dns.GetHostName();
            results.Add(Environment.NewLine + "Host Name: " + hostName);
            results.Add(Environment.NewLine + "User Name: " + Environment.UserName);
            results.Add(Environment.NewLine + "User Domain Name: " + Environment.UserDomainName);
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine(results[0]);
            Console.WriteLine(results[1]);
            Console.WriteLine(results[2]);

            NetworkInterface? networkInterface = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.Name.Contains("Ethernet") && !n.Name.Contains("(")).ToList().Single();
            string physicalAddress = networkInterface.GetPhysicalAddress().ToString();
            int l = (physicalAddress.Length / 2) - 1;
            int s = 2;
            for (int i = 0; i < l; i++)
            {
                physicalAddress = physicalAddress.Insert(s, "-");
                s += 3;
            }
            results.Add(Environment.NewLine + "Physical Address: " + physicalAddress + $"  -  [{networkInterface.GetPhysicalAddress()}]");
            Console.WriteLine(results[3]);
            IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
            foreach (UnicastIPAddressInformation ipInfo in ipProperties.UnicastAddresses)
            {
                if (ipInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    results.Add(Environment.NewLine + "IPv4 Address: " + ipInfo.Address);
                    results.Add(Environment.NewLine + "Interface Description: " + networkInterface.Description);
                    Console.WriteLine(results[4]);
                    Console.WriteLine(results[5]);
                    Console.WriteLine(Environment.NewLine + "----------------------------------------------------------");
                    break;
                }
            }
            string fileName = $"{Environment.UserName}_{hostName}_HostInfo.txt";
            string downloadDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string filePath = Path.Combine(downloadDirectory, fileName);
            if (File.Exists(filePath))
            {
                Console.WriteLine(Environment.NewLine + "The existed file has been deleted!");
                File.Delete(filePath);
            }
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string line in results)
                {
                    writer.WriteLine(line);
                }
            }
            Console.WriteLine(Environment.NewLine + "The new text file created at: " + filePath);
            Console.WriteLine(Environment.NewLine + "   ------------- End ------------        ");
            Console.WriteLine("\n\nPress 'Enter' key to close this window . . .");
            Console.ReadLine();
        }
    }
}