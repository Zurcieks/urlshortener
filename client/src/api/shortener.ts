import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5017/api",
});

export interface ShortenResponse {
  shortUrl: string;
}

export const shortenUrl = async (
  originalUrl: string,
): Promise<ShortenResponse> => {
  const response = await api.post<ShortenResponse>("/urlshortener/shorten", {
    originalUrl,
  });
  return response.data;
};
