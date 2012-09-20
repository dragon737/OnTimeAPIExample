﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OnTimeApi;
using System.Configuration;

namespace WinApp
{
	static class Program
	{
		public static Settings Settings { get; private set; }

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// Initialize Settings from App.config
			Settings = new Settings(
				onTimeUrl: ConfigurationManager.AppSettings.Get("OnTimeUrl"),
				clientId: ConfigurationManager.AppSettings.Get("ClientId"),
				clientSecret: ConfigurationManager.AppSettings.Get("ClientSecret")
			);

			// Set up exception handling
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
//			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			// set up and start form
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ApiForm()); // not passing the form to Run so closing it doesn't exit the app
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if(e.ExceptionObject is OnTimeException)
				HandleUnhandledException((OnTimeException)e.ExceptionObject);
		}

		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			if(e.Exception is OnTimeException)
				HandleUnhandledException((OnTimeException)e.Exception);
		}

		static void HandleUnhandledException(OnTimeException e)
		{
			MessageBox.Show(
				"An error occurred when accessing OnTime: " + e.Message,
				"Error accessing OnTime API",
				MessageBoxButtons.OK, 
				MessageBoxIcon.Error);
		}
	}
}
