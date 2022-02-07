using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerVisionOCRDemo
{
	class Program
	{
		// Add your Computer Vision subscription key and endpoint
		static string subscriptionKey = Settings.AzureApiKey;
		static string endpoint = Settings.AzureApiEndpoint;

        // Download these images (link in prerequisites), or you can use any appropriate image on your local machine.
        // Save your local image in your bin/Debug/netcoreX.X folder.
        //private const string READ_TEXT_LOCAL_IMAGE = "printed_text.jpg";
        private const string READ_TEXT_LOCAL_IMAGE = "coupon1.jpg";
        static void Main(string[] args)
		{
			ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

			// Extract text (OCR) from a URL image using the Read API
			ReadFileLocal(client, READ_TEXT_LOCAL_IMAGE).Wait();
		}

		// Authenticates the client
		public static ComputerVisionClient Authenticate(string endpoint, string key)
		{
			ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
			{
				Endpoint = endpoint
			};
			return client;
		}
        public static async Task ReadFileLocal(ComputerVisionClient client, string localFile)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("READ FILE FROM LOCAL");
            Console.WriteLine();

            // Read text from file
            var textHeaders = await client.ReadInStreamAsync(File.OpenRead(localFile));

            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // <snippet_extract_response>
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Reading text from local file {Path.GetFileName(localFile)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_extract_response>

            // <snippet_extract_display>
            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            Console.WriteLine();
        }
    }
}
