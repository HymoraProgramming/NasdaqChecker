# NasdaqChecker

A simple C# console application that fetches and displays the market capitalization of companies listed in the **NASDAQ-100** index.

## Features

- Fetches the latest NASDAQ-100 stock list.
- Retrieves and displays each company's **market capitalization**.
- Shows the **date and time** of the data snapshot.
- Displays the **source** of the data.
- Supports caching for better performance and reduced API usage.

## Technologies Used

- .NET 9
- `HttpClient` for API communication
- JSON parsing with `System.Text.Json`
- Environment variable configuration (via `.env` file)

## How It Works

1. The app loads your API key from environment variables.
2. It fetches a list of NASDAQ-100 tickers from an external source.
3. For each ticker, it fetches the market cap using an API.
4. Caches are used to avoid redundant API calls.

## Setup Instructions

1. **Clone the repository:**

   ```bash
   git clone https://github.com/HymoraProgramming/NasdaqChecker.git
   cd NasdaqChecker

## API Keys

Create a file named `appsettings.Development.json` in the root of the project with the following structure:

```json
{
  "ApiKeys": {
    "FMP_API_KEY": "your_fmp_api_key_here",
    "EARNINGS_API_KEY": "your_earnings_api_key_here"
  }
}

Then set its properties in Visual Studio:

Right-click the file → Properties → Copy to Output Directory → Copy if newer

