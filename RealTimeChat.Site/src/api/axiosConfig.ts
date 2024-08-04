import axios from 'axios';

export const baseUrl = import.meta.env.VITE_BACKEND_URL; //coloque um arquivo .env com a url do backend: VITE_BACKEND_URL=url do backend

const api = axios.create({
  baseURL: `${baseUrl}/api`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;
