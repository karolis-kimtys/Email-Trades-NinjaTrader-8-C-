#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class EmailTradesNT8 : Indicator
	{
		
		private Account account;
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Send emails when a trade happens on your account. ";
				Name										= "EmailTradesNT8";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= true;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				ToEmail					= "example@email.com";
				EmailPartFilled					= false;
			}
			else if (State == State.Configure)
			{
			}
			
			else if (State == State.DataLoaded)
			{
				// Find our Sim101 account
				lock (Account.All)
					account = Account.All.FirstOrDefault(a => a.Name == AccountName);

				// Subscribe to account item updates
				if (account != null)
					
					account.ExecutionUpdate += OnExecutionUpdate;
			}
			
			else if(State == State.Terminated)
			{
				// Make sure to unsubscribe to the account item subscription
        		if (account != null)
            		
					account.ExecutionUpdate -= OnExecutionUpdate;
			}
		}
		
		private void OnExecutionUpdate(object sender, ExecutionEventArgs e)

	    {
			if(e.Execution.Order != null)
			{
				if (e.Execution.Order.OrderState == OrderState.Filled)
				{
					SendMail(ToEmail, "Execution on your account",
					string.Format("Instrument: {0} Quantity: {1} Price: {2}",e.Execution.Instrument.FullName, e.Quantity, e.Price)
					);
				}
				
				if (EmailPartFilled && e.Execution.Order.OrderState == OrderState.PartFilled)
				{
					SendMail(ToEmail, "Partial execution on your account",
					string.Format("Instrument: {0} Quantity: {1} Price: {2}",e.Execution.Instrument.FullName, e.Quantity, e.Price)
					);
				}
			}

	         NinjaTrader.Code.Output.Process(string.Format("Instrument: {0} Quantity: {1} Price: {2}",

	              e.Execution.Instrument.FullName, e.Quantity, e.Price), PrintTo.OutputTab1);

	    }

		protected override void OnBarUpdate()
		{
			//Add your custom indicator logic here.
		}

		#region Properties
		
		[TypeConverter(typeof(NinjaTrader.NinjaScript.AccountNameConverter))]
		public string AccountName { get; set; }
		
		[NinjaScriptProperty]
		[Display(Name="To Email", Order=1, GroupName="Parameters")]
		public string ToEmail
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="EmailPartFilled", Order=3, GroupName="Parameters")]
		public bool EmailPartFilled
		{ get; set; }
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private EmailTradesNT8[] cacheEmailTradesNT8;
		public EmailTradesNT8 EmailTradesNT8(string toEmail, bool emailPartFilled)
		{
			return EmailTradesNT8(Input, toEmail, emailPartFilled);
		}

		public EmailTradesNT8 EmailTradesNT8(ISeries<double> input, string toEmail, bool emailPartFilled)
		{
			if (cacheEmailTradesNT8 != null)
				for (int idx = 0; idx < cacheEmailTradesNT8.Length; idx++)
					if (cacheEmailTradesNT8[idx] != null && cacheEmailTradesNT8[idx].ToEmail == toEmail && cacheEmailTradesNT8[idx].EmailPartFilled == emailPartFilled && cacheEmailTradesNT8[idx].EqualsInput(input))
						return cacheEmailTradesNT8[idx];
			return CacheIndicator<EmailTradesNT8>(new EmailTradesNT8(){ ToEmail = toEmail, EmailPartFilled = emailPartFilled }, input, ref cacheEmailTradesNT8);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.EmailTradesNT8 EmailTradesNT8(string toEmail, bool emailPartFilled)
		{
			return indicator.EmailTradesNT8(Input, toEmail, emailPartFilled);
		}

		public Indicators.EmailTradesNT8 EmailTradesNT8(ISeries<double> input , string toEmail, bool emailPartFilled)
		{
			return indicator.EmailTradesNT8(input, toEmail, emailPartFilled);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.EmailTradesNT8 EmailTradesNT8(string toEmail, bool emailPartFilled)
		{
			return indicator.EmailTradesNT8(Input, toEmail, emailPartFilled);
		}

		public Indicators.EmailTradesNT8 EmailTradesNT8(ISeries<double> input , string toEmail, bool emailPartFilled)
		{
			return indicator.EmailTradesNT8(input, toEmail, emailPartFilled);
		}
	}
}

#endregion
