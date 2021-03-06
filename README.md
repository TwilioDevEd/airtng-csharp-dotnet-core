<a  href="https://www.twilio.com">
<img  src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg"  alt="Twilio"  width="250"  />
</a>

# AirTNG App: Part 1 - Workflow Automation with Twilio - ASP.NET Core MVC

![](https://github.com/TwilioDevEd/airtng-csharp-dotnet-core/workflows/dotNETCore/badge.svg)

> We are currently in the process of updating this sample template. If you are encountering any issues with the sample, please open an issue at [github.com/twilio-labs/code-exchange/issues](https://github.com/twilio-labs/code-exchange/issues) and we'll try to help you.

## About

Learn how to automate your workflow using Twilio's REST API and Twilio SMS. This example app is a vacation rental site, where the host can confirm a reservation via SMS.

[Read the full tutorial here](https://www.twilio.com/docs/tutorials/walkthrough/workflow-automation/csharp/mvc)!

Implementations in other languages:

| Python | Java | Node | PHP | Ruby |
| :--- | :--- | :----- | :-- | :--- |
| [Done](https://github.com/TwilioDevEd/airtng-flask) | [Done](https://github.com/TwilioDevEd/airtng-servlets)  | [Done](https://github.com/TwilioDevEd/airtng-node)  | [Done](https://github.com/TwilioDevEd/airtng-laravel) | TBD  |

<!--
### How it works

**TODO: Describe how it works**
-->

## Set up

### Requirements

- [dotnet](https://dotnet.microsoft.com/)
- A Twilio account - [sign up](https://www.twilio.com/try-twilio)
- [ngrok](https://ngrok.com/)

### Twilio Account Settings

This application should give you a ready-made starting point for writing your
own application. Before we begin, we need to collect
all the config values we need to run the application:

| Config&nbsp;Value | Description                                                                                                                                                  |
| :---------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Account&nbsp;Sid  | Your primary Twilio account identifier - find this [in the Console](https://www.twilio.com/console).                                                         |
| Auth&nbsp;Token   | Used to authenticate - [just like the above, you'll find this here](https://www.twilio.com/console).                                                         |
| Phone&nbsp;number | A Twilio phone number in [E.164 format](https://en.wikipedia.org/wiki/E.164) - you can [get one here](https://www.twilio.com/console/phone-numbers/incoming) |

### Local development

After the above requirements have been met:

1. Clone this repository and `cd` into it

```bash
git clone git@github.com:TwilioDevEd/airtng-csharp-dotnet-core.git
cd airtng-csharp-dotnet-core
```

2. Build to install the dependencies

```bash
dotnet build
```

3. Set your environment variables

```bash
cp AirTNG.Web/twilio.json.example AirTNG.Web/twilio.json
```

See [Twilio Account Settings](#twilio-account-settings) to locate the necessary environment variables.

4. Install [EF Core CLI](https://docs.microsoft.com/en-gb/ef/core/what-is-new/ef-core-3.0/breaking-changes#the-ef-core-command-line-tool-dotnet-ef-is-no-longer-part-of-the-net-core-sdk) if it's not already installed.

```
dotnet tool install --global dotnet-ef --version 3.0.0
```


5. Create the local DB. This also should be executed in `AirTNG.Web` directory.

```
dotnet ef database update
```

6. Run the application

```bash
dotnet run --project AirTNG.Web
```

7. Navigate to [http://localhost:5000](http://localhost:5000)

8. To let our Twilio phone number use the callback endpoint we exposed, our development server will need to be publicly accessible. You could expose the application to the wider Internet using [ngrok](https://ngrok.com/). [Here](https://www.twilio.com/blog/2015/09/6-awesome-reasons-to-use-ngrok-when-testing-webhooks.html), there is an interesting article about why we recommend you to use ngrok.

```
ngrok http 5000
```

Keep in mind that our endpoint is:

```
https://<your-ngrok-subdomain>.ngrok.io/Sms/Handle
```

9. Register your webhook with your Twilio Number at `https://www.twilio.com/console/phone-numbers/`. Your webhook url should include the ngrok host from the previous step and should look similar to `https://<your-ngrok-subdomain>.ngrok.io/Sms/Handle`.

   ![Configure Messaging](webhook.png)

That's it!

### Docker

If you have [Docker](https://www.docker.com/) already installed on your machine, you can use our `docker-compose.yml` to setup your project.

1. Make sure you have the project cloned.
2. Setup the `twilio.json` file as outlined in the [Local Development](#local-development) steps.
3. Run `docker-compose up`.
4. Follow the steps in [Local Development](#local-development) on how to expose your port to Twilio using a tool like [ngrok](https://ngrok.com/) and configure the remaining parts of your application.

### Tests

You can run the tests locally by typing:

```bash
dotnet test
```

## Resources

- The CodeExchange repository can be found [here](https://github.com/twilio-labs/code-exchange/).

## Contributing

This template is open source and welcomes contributions. All contributions are subject to our [Code of Conduct](https://github.com/twilio-labs/.github/blob/master/CODE_OF_CONDUCT.md).

[Visit the project on GitHub](https://github.com/twilio-labs/sample-template-dotnet)

## License

[MIT](http://www.opensource.org/licenses/mit-license.html)

## Disclaimer

No warranty expressed or implied. Software is as is.

[twilio]: https://www.twilio.com
