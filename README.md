# 🌤️ WeatherWise

> A clean, responsive weather dashboard built with **ASP.NET Core MVC**, **C#**, and the **OpenWeatherMap REST API**.

WeatherWise allows users to search for any city worldwide and instantly view current weather conditions and a 5-day forecast through a simple, responsive interface.

---

## ✨ Overview

**WeatherWise** was developed as a personal portfolio project to demonstrate practical full-stack development skills using the .NET ecosystem.

The application focuses on doing one thing well: providing weather information quickly without unnecessary complexity. Users can search for a city and view its current temperature, conditions, humidity, wind speed, atmospheric pressure, sunrise/sunset times, and a 5-day forecast.

The project demonstrates:

* Third-party REST API integration
* Asynchronous programming with `async/await`
* Dependency Injection and `HttpClient`
* Separation of concerns using a layered MVC architecture
* Secure API key management
* Responsive front-end development
* User-friendly loading, empty, and error states

---

## 🚀 Features

### 🔎 City Search

Search for cities anywhere in the world, including:

`Durban` · `Johannesburg` · `Cape Town` · `London` · `New York` · `Tokyo`

### 🌡️ Current Weather

Displays:

* City and country
* Current temperature
* Weather condition
* Weather icon
* Feels-like temperature
* Humidity
* Wind speed
* Atmospheric pressure
* Sunrise and sunset times

### 📅 5-Day Forecast

Provides a simplified 5-day outlook containing:

* Day
* Weather icon
* Temperature
* Weather condition

### ⚡ User Experience

* Loading state while weather data is retrieved
* Friendly error messages
* Empty state before the first search
* Input validation
* Responsive desktop, tablet, and mobile layouts

---

## 🛠️ Tech Stack

| Technology                          | Purpose                            |
| ----------------------------------- | ---------------------------------- |
| **C#**                              | Application development            |
| **ASP.NET Core MVC (.NET 8)**       | Web application framework          |
| **OpenWeatherMap API**              | Weather data                       |
| **HttpClient / IHttpClientFactory** | API communication                  |
| **Dependency Injection**            | Service management                 |
| **System.Text.Json**                | JSON serialization/deserialization |
| **Razor Views**                     | Server-side UI                     |
| **HTML5 & CSS3**                    | Front-end structure and styling    |
| **JavaScript**                      | Client-side interactions           |
| **Fetch API**                       | AJAX requests                      |
| **Bootstrap 5**                     | Responsive UI                      |

---

## 🏗️ Architecture

WeatherWise uses a simple layered MVC architecture designed around separation of responsibilities:

```text
Controller
     ↓
Service Layer
     ↓
OpenWeatherMap API
     ↓
ViewModel
     ↓
Razor View
```

### Controller

`WeatherController` handles incoming requests and coordinates application flow. Data-fetching logic is delegated to the service layer rather than being placed directly inside the controller.

### Service Layer

`IWeatherService` and `WeatherService` handle:

* API communication
* Asynchronous requests
* JSON processing
* Mapping API responses to view models
* Error handling

### Models

The application separates external API models from application-facing view models.

This prevents the UI from becoming tightly coupled to the structure of the external API.

### Views

Razor views provide the user interface, while JavaScript manages the client-side request lifecycle, including:

`Loading → Success → Error`

---

## 📁 Project Structure

```text
WeatherWise/
│
├── Controllers/
│   └── WeatherController.cs
│
├── Services/
│   ├── IWeatherService.cs
│   ├── WeatherService.cs
│   └── CityNotFoundException.cs
│
├── Models/
│   ├── OpenWeatherModels.cs
│   └── WeatherViewModel.cs
│
├── Views/
│   ├── Weather/
│   │   └── Index.cshtml
│   └── Shared/
│       └── _Layout.cshtml
│
└── wwwroot/
    ├── css/
    │   └── site.css
    └── js/
        └── site.js
```

The project intentionally avoids unnecessary architectural complexity such as repositories or CQRS. The goal is to keep the codebase **simple, maintainable, and appropriately scoped** for the application.

---

## 🖥️ Screenshots

<img width="1912" height="961" alt="image" src="https://github.com/user-attachments/assets/d724fae1-bf43-4083-bd77-8698f8102d20" />


### Weather Search Result

<img width="1892" height="960" alt="image" src="https://github.com/user-attachments/assets/fc7ae5c7-163a-4d52-ad37-b6b592d04f29" />


### Error State

<img width="1916" height="957" alt="image" src="https://github.com/user-attachments/assets/b88d7bb8-d0c5-4601-bdf1-12b9d46e2ffb" />

---

## ⚙️ Getting Started

### Prerequisites

Before running WeatherWise, make sure you have:

* **Visual Studio 2022** or later
* **.NET 8 SDK**
* ASP.NET and web development workload
* An **OpenWeatherMap API key**

### 1. Clone the Repository

```bash
git clone https://github.com/Diyalakha24/WeatherWise.git
cd WeatherWise
```

### 2. Open the Project

Open:

```text
WeatherWise.sln
```

in Visual Studio 2022 or later.

### 3. Configure the API Key

The API key should **never be committed to source control**.

In Visual Studio:

**Right-click the WeatherWise project → Manage User Secrets**

Add:

```json
{
  "OpenWeatherMap": {
    "ApiKey": "5a8769e1ee57c51c1801dae09db0233b"
  }
}
```

You can obtain an API key from:

[OpenWeatherMap API](https://openweathermap.org/api)

### 4. Restore Dependencies

Visual Studio will normally restore the required packages automatically.

Alternatively:

```bash
dotnet restore
```

### 5. Run the Application

Using Visual Studio:

```text
F5
```

Or from the terminal:

```bash
dotnet run
```

Open the local URL displayed in the terminal, for example:

```text
https://localhost:xxxx
```

---

## 🌍 API Integration

WeatherWise uses the **OpenWeatherMap REST API** to retrieve live weather information.

Two endpoints are used:

### Current Weather

```text
/data/2.5/weather
```

Provides current conditions such as temperature, humidity, wind, pressure, sunrise, and sunset.

### 5-Day Forecast

```text
/data/2.5/forecast
```

Returns forecast information in 3-hour intervals. WeatherWise processes these results to create a simplified 5-day forecast for the user interface.

All API requests are performed asynchronously using a typed `HttpClient` registered through Dependency Injection.

The API key is retrieved from application configuration and is **never hard-coded or exposed to the browser**.

For local development, .NET User Secrets are used. In a production environment, an environment variable or dedicated secret-management service can be used.

---

## 🧪 Testing

The project was manually tested against common user and API scenarios.

| Scenario            | Test                       | Expected Result                                  |
| ------------------- | -------------------------- | ------------------------------------------------ |
| Valid city          | Search `Durban`            | Current weather and 5-day forecast are displayed |
| Invalid city        | Search `xyzabc123`         | Friendly city-not-found message                  |
| Empty search        | Submit without a city      | Validation message is displayed                  |
| API/network failure | Disable network and search | Friendly API failure message is displayed        |

### Example Error Messages

```text
Please enter a city name.
```

```text
City not found. Please check the city name and try again.
```

```text
Unable to retrieve weather data. Please try again later.
```

The service layer is built around the `IWeatherService` interface, making the application easier to unit test in the future. API interactions could be tested using a mocked `HttpMessageHandler` without making real network requests.

---

## 💡 What I Learned

Building WeatherWise strengthened my understanding of:

* Integrating third-party REST APIs with ASP.NET Core
* Working with JSON serialization and deserialization
* Using `HttpClient` and `IHttpClientFactory`
* Dependency Injection in .NET
* Writing asynchronous C# using `async/await`
* Applying separation of concerns
* Designing ViewModels for clean UI consumption
* Handling API failures and invalid user input
* Securely managing application secrets
* Building responsive interfaces with Bootstrap
* Using JavaScript and the Fetch API for asynchronous UI updates

---

## 🔮 Future Improvements

Potential improvements include:

* 🌦️ Weather-based recommendations such as *"Bring an umbrella"*
* ⭐ Favourite and saved cities
* 📍 Automatic location detection
* 🌙 Dark mode
* 🕐 Detailed hourly forecasts
* 📊 Historical weather information
* 🌡️ Temperature unit selection
* 📱 Progressive Web App support

---

## 🤖 AI-Assisted Development

AI tools such as **ChatGPT** and **GitHub Copilot** were used to support research, debugging, understanding of programming concepts, and development throughout the project. Final implementation, testing, and development decisions were completed and reviewed by the developer.

---

## 📄 License

This project is available for **personal and educational reference**.

---

## 👩‍💻 Author

**Diya Lakha**

Built as a personal portfolio project to demonstrate practical software development skills using **C#, ASP.NET Core, REST APIs, and responsive web development**.

[![GitHub](https://img.shields.io/badge/GitHub-Diyalakha24-181717?style=flat\&logo=github)](https://github.com/Diyalakha24)

