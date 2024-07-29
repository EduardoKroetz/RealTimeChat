import axios from 'axios';

export const baseUrl = "https://localhost:7136";

const api = axios.create({
  baseURL: `${baseUrl}/api`, // URL base da API
  headers: {
    'Content-Type': 'application/json'
  },
});

export default api;
