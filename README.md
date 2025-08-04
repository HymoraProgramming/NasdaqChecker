# NasdaqChecker

🧠 A simple C# console application that fetches and displays the market capitalization of companies listed in the **NASDAQ-100** index.

## 📌 Features

- Fetches the latest NASDAQ-100 stock list.
- Retrieves and displays each company's **market capitalization**.
- Shows the **date and time** of the data snapshot.
- Displays the **source** of the data.
- Supports caching for better performance and reduced API usage.

## 💡 Technologies Used

- [.NET 8](https://dotnet.microsoft.com/)
- `HttpClient` for API communication
- JSON parsing with `System.Text.Json`
- Environment variable configuration (via `.env` file)

## 📷 Preview

![screenshot](docs/screenshot.png) <!-- you can update this path if you add a screenshot -->

## ⚙️ How It Works

1. The app loads your API key from environment variables.
2. It fetches a list of NASDAQ-100 tickers from an external source.
3. For each ticker, it fetches the market cap using an API.
4. Caches are used to avoid redundant API calls.

## 🛠️ Setup Instructions

1. **Clone the repository:**

   ```bash
   git clone https://github.com/HymoraProgramming/NasdaqChecker.git
   cd NasdaqChecker
