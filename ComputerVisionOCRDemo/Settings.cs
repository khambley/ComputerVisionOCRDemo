using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerVisionOCRDemo
{
	//Go to Microsoft OCR quickstart to find out how to set these up https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/quickstarts-sdk/client-library?tabs=visual-studio&pivots=programming-language-csharp
	public static class Settings
	{
		public static string AzureApiKey
		{
			get
			{
				return "<Your API Key Here>";
			}
		}
		public static string AzureApiEndpoint
		{
			get
			{
				return "<Your endpoint here.>";
			}
		}
	}
}
