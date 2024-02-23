# Umbraco Commerce Demo Store

Welcome to the Umbraco Commerce Demo Store, an example webstore setup using Umbraco Commerce in [Umbraco](https://umbraco.com).

![Screenshot](assets/screenshot.png)

## About this Demo

In this demo you will find a fully configured basic webstore using Umbraco Commerce on Umbraco. The store is based around a fictitious tea retailer called Blendid which lists a variety of teas from multiple companies and also displays them in categories. Where products have multiple pricing options, variant nodes are used to provide buying options for those particular product choices.

The site also showcases a basic shopping cart setup with cart management features via the Umbraco Commerce API as well as a checkout flow following all the main steps required for a Umbraco Commerce order entity. On checkout, there will also be examples of order confirmation emails that will be sent (**TIP** Use something like [Papercut](https://github.com/ChangemakerStudios/Papercut) to capture these without them needing to be actually sent).

In the back office you'll find a suggested content structure for a working Umbraco Commerce webstore and you'll also be able to browse the store setup and example orders in the settings and commerce sections respectively.

## System Requirements

To get started with the Umbraco Commerce demo store you will need:

* Visual Studio 2019.
* .NET Core SDK 8.0.100 or newer.

This demo store supports two database editions. By default it uses a `sqlite` database, but it also comes with a `.\dbs\DemoStore_v13_starter.bacpac` file which is exported from a SQL Server 2022.

`sqlite` edition is fine if you just want to look around the store, but if you want to mess around, SQL Server edition is less likely be locked up when thing goes wrong.

## Getting Started

Clone or download this repository locally (it includes all the files you will need including a fully configured SQLlite database)

````
git clone https://github.com/umbraco/Umbraco.Commerce.DemoStore.git
````

Once you have all the files downloaded you can open the `Umbraco.Commerce.DemoStore.sln` solution file in the root of the repository in Visual Studio. Make sure the `Umbraco.Commerce.DemoStore.Web` project is the startup project by right clicking the project in the Solution Explorer and choosing `Set as StartUp Project`, and then press `Ctrl + F5` to launch the site.

*Optional* - Import `.\db\UmbracoCommerceDemoStore_v13.1.0.bacpac` using Data-tier application and update `ConnectionStrings` in `appsettings.json` to be similar like this:
```json
  "ConnectionStrings": {
    "umbracoDbDSN": "Server=.;Database=UmbracoCommerceDemoStore_v13.1.0;User Id={your_db_username};Password={your_db_password};TrustServerCertificate=true;",
    "umbracoDbDSN_ProviderName": "Microsoft.Data.SqlClient"
  }
```

To login to the back office you can do so using the credentails:

* **Email** `admin@example.com`
* **Password** `password1234`


## License

Copyright © 2023 Umbraco

This demo store is [licensed under MIT](LICENSE.md). The core Umbraco Commerce product is licensed under Umbracos commercial license.

