import { useState } from 'react'
import { shortenUrl } from '../api/shortener'

export const ShortenerForm = () => {
  const [url, setUrl] = useState('')
  const [shortUrl, setShortUrl] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const [copied, setCopied] = useState(false)

  const handleShorten = async () => {
    setError('')
    setShortUrl('')
    setLoading(true)

    try {
      const result = await shortenUrl(url)
      setShortUrl(result.shortUrl)
    } catch (e: any) {
      setError(e.response?.data?.error ?? 'Something went wrong.')
    } finally {
      setLoading(false)
    }
  }

  const handleCopy = () => {
    navigator.clipboard.writeText(shortUrl)
    setCopied(true)
    setTimeout(() => setCopied(false), 2000)
  }

  return (
    <div className="flex flex-col gap-4 w-full max-w-xl">
      <input
        type="text"
        placeholder="Paste your long URL here..."
        value={url}
        onChange={e => setUrl(e.target.value)}
        className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 text-gray-800"
      />

      <button
        onClick={handleShorten}
        disabled={loading || !url}
        className="w-full py-3 bg-blue-600 hover:bg-blue-700 disabled:bg-blue-300 text-white font-semibold rounded-lg transition-colors"
      >
        {loading ? 'Shortening...' : 'Shorten URL'}
      </button>

      {error && (
        <p className="text-red-500 text-sm">{error}</p>
      )}

      {shortUrl && (
        <div className="flex items-center gap-2 p-4 bg-gray-100 rounded-lg">
          <a
            href={shortUrl}
            target="_blank"
            rel="noopener noreferrer"
            className="text-blue-600 hover:underline truncate flex-1"
          >
            {shortUrl}
          </a>
          <button
            onClick={handleCopy}
            className="px-3 py-1 bg-blue-600 hover:bg-blue-700 text-white text-sm rounded-lg transition-colors whitespace-nowrap"
          >
            {copied ? 'Copied!' : 'Copy'}
          </button>
        </div>
      )}
    </div>
  )
}
