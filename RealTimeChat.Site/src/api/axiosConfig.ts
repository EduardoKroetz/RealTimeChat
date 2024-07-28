import axios from 'axios';

export const baseUrl = "https://localhost:5088";

const api = axios.create({
  baseURL: `${baseUrl}/api`, // URL base da API
  headers: {
    'Content-Type': 'application/json'
  },
});

export default api;
