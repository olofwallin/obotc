using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace obotc.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as IMessageActivity;

            if (message.Text.ToLower().Contains("resa"))
            {
                // User said 'resa', so invoke the Travel Plan Dialog and wait for it to finish.
                // Then, call ResumeAfterTravelPlanDialog.
                context.Call(new TravelPlanDialog(), this.TravelDialogResumeAfter);
            }
            else if (message.Text.ToLower().Contains("namn"))
            {
                // User said 'namn', so invoke the Travel Plan Dialog and wait for it to finish.
                // Then, call ResumeAfterTravelPlanDialog.
                context.Call(new NameDialog(), this.NameDialogResumeAfter);
            }
            else
            {
                await this.SendUnknownMessageAsync(context);
            }

            //context.Wait(this.MessageReceivedAsync);
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            await context.PostAsync("Hi, I'm the Basic Multi Dialog bot. Currently I only know how to plan your trip.");
        }

        private async Task SendUnknownMessageAsync(IDialogContext context)
        {
            await context.PostAsync("I dont know what you mean. Try again.");
        }


        private async Task TravelDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var resultsFromNameDialog = await result;

                await context.PostAsync($"Travel plan to {resultsFromNameDialog} provided.");

            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task NameDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var resultsFromNameDialog = await result;

                await context.PostAsync($"Hello {resultsFromNameDialog}!");



            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                await this.SendWelcomeMessageAsync(context);
            }
        }
    }
}