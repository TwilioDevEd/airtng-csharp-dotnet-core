<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>

# AirTNG App: Part 1 - Workflow Automation with Twilio - ASP.NET Core MVC

Learn how to automate your workflow using Twilio's REST API and Twilio SMS. This example app is a vacation rental site, where the host can confirm a reservation via SMS.

[Read the full tutorial here](https://www.twilio.com/docs/tutorials/walkthrough/workflow-automation/csharp/mvc)!

## Local Development

1. You will need to configure Twilio to send requests to your application when SMS are received.

   You will need to provision at least one Twilio number with sms capabilities so the application's users can make property reservations. You can buy a number [right here](https://www.twilio.com/user/account/phone-numbers/search). Once you have a number you need to configure it to work with your application. Open [the number management page](https://www.twilio.com/user/account/phone-numbers/incoming) and open a number's configuration by clicking on it.

   Remember that the number where you change the _SMS webhook_ must be the same one you set on the `TwilioPhoneNumber` settings.

   <!--![Configure Voice](http://howtodocs.s3.amazonaws.com/twilio-number-config-all-med.gif)-->

   To start using `ngrok` in our project you'll have execute to the following line in the _command prompt_.

   ```
   ngrok http 5000
   ```

   Keep in mind that our endpoint is:

   ```
   http://<your-ngrok-subdomain>.ngrok.io/Sms/Handle
   ```

2. Clone this repository and `cd` into it.

    ```
    git clone git@github.com:TwilioDevEd/airtng-csharp-dotnet-core.git

    cd airtng-csharp-dotnet-core/AirTNG.Web
    ```

3. Create a new file `twilio.json` and update the content.
   ```json
   {
     "Twilio": {
       "AccountSid": "Your Twilio Account SID",
       "AuthToken": "Your Twilio Auth Token",
       "PhoneNumber": "Your Twilio Phone Number"
     }
   }
   ```

4. Build the solution.

5. Run `dotnet ef database update` to create the local DB

6. Run the application `dotnet run`.

7. Check it out at [http://localhost:5000](http://localhost:5000)

That's it!

To let our Twilio Phone number use the callback endpoint we exposed, our development server will need to be publicly accessible. [We recommend using ngrok to solve this problem](https://www.twilio.com/blog/2015/09/6-awesome-reasons-to-use-ngrok-when-testing-webhooks.html).

### Run unit tests

1. cd into AirTNG.Web.Tests project

    `cd airtng-csharp-dotnet-core/AirTNG.Web.Tests`
    
2. Run tests

    `dotnet test`

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.

