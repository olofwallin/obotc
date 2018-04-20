namespace obotc.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;
    using System.Web.Script.Serialization;
    using System.Dynamic;


    [Serializable]
    public class TravelPlanDialog : IDialog<string>
    {
        private int attempts = 3;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Where do you want to go?");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            /* If the message returned is a valid name, return it to the calling dialog. */
            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {
                // Make travel plan request
                dynamic travelPlan = this.getTravelPlan();

                string plan;
                plan = travelPlan.TripList.Trip[0].LegList.Leg.name
                                        + " from "
                                        + travelPlan.TripList.Trip[0].LegList.Leg.Origin.name
                                        + " to "
                                        + travelPlan.TripList.Trip[0].LegList.Leg.Destination.name
                                        + " leaves at "
                                        + travelPlan.TripList.Trip[0].LegList.Leg.Origin.time
                                        + " and takes "
                                        + travelPlan.TripList.Trip[0].dur
                                        + " minutes";


                await context.PostAsync(plan);

                /* Completes the dialog, removes it from the dialog stack, and returns the result to the parent/calling
                    dialog. */
                context.Done(message.Text);
            }
            /* Else, try again by re-prompting the user. */
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry, I don't understand your reply. Where do you want to go?");

                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    /* Fails the current dialog, removes it from the dialog stack, and returns the exception to the 
                        parent/calling dialog. */
                    context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
                }
            }
        }

        private dynamic getTravelPlan()
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic data;

            try
            {
                data = serializer.Deserialize(json, typeof(object));
            }
            catch
            {
                throw new Exception("Something bad happened when trying to parse travel plan json");
            }

            return data;
        }

        const string json = "{\"TripList\":{\"noNamespaceSchemaLocation\":\"hafasRestTrip.xsd\",\"Trip\":[{\"dur\":\"8\",\"chg\":\"0\",\"co2\":\"0.00\",\"LegList\":{\"Leg\":{\"idx\":\"0\",\"name\":\"tunnelbanansgrönalinje19\",\"type\":\"METRO\",\"dir\":\"Hässelbystrand\",\"line\":\"19\",\"Origin\":{\"name\":\"T-Centralen\",\"type\":\"ST\",\"id\":\"400102051\",\"lon\":\"18.061639\",\"lat\":\"59.331295\",\"routeIdx\":\"14\",\"time\":\"20:46\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"Destination\":{\"name\":\"Thorildsplan\",\"type\":\"ST\",\"id\":\"400101161\",\"lon\":\"18.015839\",\"lat\":\"59.331493\",\"routeIdx\":\"20\",\"time\":\"20:54\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"JourneyDetailRef\":{\"ref\":\"ref%3D481035%2F165313%2F428836%2F54073%2F74%3Fdate%3D2018-04-11%26station_evaId%3D400102051%26station_type%3Ddep%26lang%3Dsv%26format%3Djson%26\"},\"GeometryRef\":{\"ref\":\"ref%3D481035%2F165313%2F428836%2F54073%2F74%26startIdx%3D14%26endIdx%3D20%26lang%3Dsv%26format%3Djson%26\"}}},\"PriceInfo\":{\"TariffZones\":{\"$\":\"SL\"},\"TariffRemark\":{\"$\":\"SL\"}}},{\"dur\":\"8\",\"chg\":\"0\",\"co2\":\"0.00\",\"LegList\":{\"Leg\":{\"idx\":\"0\",\"name\":\"tunnelbanansgrönalinje18\",\"type\":\"METRO\",\"dir\":\"Alvik\",\"line\":\"18\",\"Origin\":{\"name\":\"T-Centralen\",\"type\":\"ST\",\"id\":\"400102051\",\"lon\":\"18.061639\",\"lat\":\"59.331295\",\"routeIdx\":\"14\",\"time\":\"20:53\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"Destination\":{\"name\":\"Thorildsplan\",\"type\":\"ST\",\"id\":\"400101161\",\"lon\":\"18.015839\",\"lat\":\"59.331493\",\"routeIdx\":\"20\",\"time\":\"21:01\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"JourneyDetailRef\":{\"ref\":\"ref%3D849897%2F287605%2F766902%2F100153%2F74%3Fdate%3D2018-04-11%26station_evaId%3D400102051%26station_type%3Ddep%26lang%3Dsv%26format%3Djson%26\"},\"GeometryRef\":{\"ref\":\"ref%3D849897%2F287605%2F766902%2F100153%2F74%26startIdx%3D14%26endIdx%3D20%26lang%3Dsv%26format%3Djson%26\"}}},\"PriceInfo\":{\"TariffZones\":{\"$\":\"SL\"},\"TariffRemark\":{\"$\":\"SL\"}}},{\"dur\":\"8\",\"chg\":\"0\",\"co2\":\"0.00\",\"LegList\":{\"Leg\":{\"idx\":\"0\",\"name\":\"tunnelbanansgrönalinje19\",\"type\":\"METRO\",\"dir\":\"Hässelbystrand\",\"line\":\"19\",\"Origin\":{\"name\":\"T-Centralen\",\"type\":\"ST\",\"id\":\"400102051\",\"lon\":\"18.061639\",\"lat\":\"59.331295\",\"routeIdx\":\"14\",\"time\":\"20:56\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"Destination\":{\"name\":\"Thorildsplan\",\"type\":\"ST\",\"id\":\"400101161\",\"lon\":\"18.015839\",\"lat\":\"59.331493\",\"routeIdx\":\"20\",\"time\":\"21:04\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"JourneyDetailRef\":{\"ref\":\"ref%3D147567%2F54157%2F170242%2F35933%2F74%3Fdate%3D2018-04-11%26station_evaId%3D400102051%26station_type%3Ddep%26lang%3Dsv%26format%3Djson%26\"},\"GeometryRef\":{\"ref\":\"ref%3D147567%2F54157%2F170242%2F35933%2F74%26startIdx%3D14%26endIdx%3D20%26lang%3Dsv%26format%3Djson%26\"}}},\"PriceInfo\":{\"TariffZones\":{\"$\":\"SL\"},\"TariffRemark\":{\"$\":\"SL\"}}},{\"dur\":\"8\",\"chg\":\"0\",\"co2\":\"0.00\",\"LegList\":{\"Leg\":{\"idx\":\"0\",\"name\":\"tunnelbanansgrönalinje18\",\"type\":\"METRO\",\"dir\":\"Alvik\",\"line\":\"18\",\"Origin\":{\"name\":\"T-Centralen\",\"type\":\"ST\",\"id\":\"400102051\",\"lon\":\"18.061639\",\"lat\":\"59.331295\",\"routeIdx\":\"14\",\"time\":\"21:03\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"Destination\":{\"name\":\"Thorildsplan\",\"type\":\"ST\",\"id\":\"400101161\",\"lon\":\"18.015839\",\"lat\":\"59.331493\",\"routeIdx\":\"20\",\"time\":\"21:11\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"JourneyDetailRef\":{\"ref\":\"ref%3D976371%2F329766%2F15532%2F317691%2F74%3Fdate%3D2018-04-11%26station_evaId%3D400102051%26station_type%3Ddep%26lang%3Dsv%26format%3Djson%26\"},\"GeometryRef\":{\"ref\":\"ref%3D976371%2F329766%2F15532%2F317691%2F74%26startIdx%3D14%26endIdx%3D20%26lang%3Dsv%26format%3Djson%26\"}}},\"PriceInfo\":{\"TariffZones\":{\"$\":\"SL\"},\"TariffRemark\":{\"$\":\"SL\"}}},{\"dur\":\"8\",\"chg\":\"0\",\"co2\":\"0.00\",\"LegList\":{\"Leg\":{\"idx\":\"0\",\"name\":\"tunnelbanansgrönalinje19\",\"type\":\"METRO\",\"dir\":\"Hässelbystrand\",\"line\":\"19\",\"Origin\":{\"name\":\"T-Centralen\",\"type\":\"ST\",\"id\":\"400102051\",\"lon\":\"18.061639\",\"lat\":\"59.331295\",\"routeIdx\":\"14\",\"time\":\"21:06\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"Destination\":{\"name\":\"Thorildsplan\",\"type\":\"ST\",\"id\":\"400101161\",\"lon\":\"18.015839\",\"lat\":\"59.331493\",\"routeIdx\":\"20\",\"time\":\"21:14\",\"date\":\"2018-04-11\",\"track\":\"1\"},\"JourneyDetailRef\":{\"ref\":\"ref%3D202746%2F72554%2F944332%2F404584%2F74%3Fdate%3D2018-04-11%26station_evaId%3D400102051%26station_type%3Ddep%26lang%3Dsv%26format%3Djson%26\"},\"GeometryRef\":{\"ref\":\"ref%3D202746%2F72554%2F944332%2F404584%2F74%26startIdx%3D14%26endIdx%3D20%26lang%3Dsv%26format%3Djson%26\"}}},\"PriceInfo\":{\"TariffZones\":{\"$\":\"SL\"},\"TariffRemark\":{\"$\":\"SL\"}}}]}}";
    }

    
}