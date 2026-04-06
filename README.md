# URL Shortener

Paste a long URL, get a short one. That's it.

Built with .NET 10 and React as a learning project to explore
system design concepts like caching, rate limiting and database indexing.

## Stack

- **Backend** — .NET 10, PostgreSQL, Redis
- **Frontend** — React, TypeScript, Tailwind CSS
- **Infrastructure** — Docker

## Running locally

Start the database and cache:

```bash
docker compose up -d
```

Run the API:

```bash
cd src/UrlShortener.Api
dotnet ef database update
dotnet run
```

Run the frontend:

```bash
cd client
npm install
npm run dev
```

Open `http://localhost:5173` and start shortening.
