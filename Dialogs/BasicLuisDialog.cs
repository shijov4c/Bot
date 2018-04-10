using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Sample.QnABot;

namespace QnABot.Dialogs
{
	[Serializable]
	public class BasicLuisDialog: LuisDialog<object>
	{

		public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
			ConfigurationManager.AppSettings["LuisAppId"],
			ConfigurationManager.AppSettings["LuisAPIKey"],
			domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
		{
		}

		[LuisIntent("None")]
		public async Task NoneIntent(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
		{
			var faqDialog = new BasicQnAMakerDialog();
			var messageToForward = await message;
			await context.Forward(faqDialog, AfterFAQDialog, messageToForward, CancellationToken.None);
		}

		// Go to https://luis.ai and create a new intent, then train/publish your luis app.
		// Finally replace "Gretting" with the name of your newly created intent in the following handler
		[LuisIntent("Greeting")]
		public async Task GreetingIntent(IDialogContext context, LuisResult result)
		{
			await this.ShowLuisResult(context, result);
		}

		[LuisIntent("Cancel")]
		public async Task CancelIntent(IDialogContext context, LuisResult result)
		{
			await this.ShowLuisResult(context, result);
		}

		[LuisIntent("Help")]
		public async Task HelpIntent(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
		{
			if (result.TopScoringIntent.Score < 0.5)
			{
				var faqDialog = new BasicQnAMakerDialog();
				var messageToForward = await message;
				await context.Forward(faqDialog, AfterFAQDialog, messageToForward, CancellationToken.None);
			}
			else
			{
				await this.ShowLuisResult(context, result);
			}
		}

		[LuisIntent("StockPrice")]
		public async Task StockPriceIntent(IDialogContext context, LuisResult result)
		{
			await this.ShowLuisResult(context, result);
		}

		private async Task ShowLuisResult(IDialogContext context, LuisResult result)
		{
			await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
			context.Wait(MessageReceived);
		}

		private async Task AfterFAQDialog(IDialogContext context, IAwaitable<IMessageActivity> result)
		{
			var messageHandled = await result;
			//if (!messageHandled)
			//{
			await context.PostAsync("Did that answer your question?");
			//}

			context.Wait(MessageReceived);
		}
	}
}