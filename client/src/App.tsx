import { ShortenerForm } from "./components/ShortenerForm";


function App() {
    return (
        <main className="min-h-screen bg-gray-500 flex flex-col items-center justify-center px-4">
            <div className="flex flex-col items-center gap-8 w-full">
                <div className="text-center">
                    <h1 className="text-4xl font-bold text-gray-900">URL Shortener</h1>
                    <p className="text-gray-500 mt-2">Paste a long URL and get a short one instantly.</p>
                </div>
                <ShortenerForm />
            </div>
        </main>
    )
}

export default App;
