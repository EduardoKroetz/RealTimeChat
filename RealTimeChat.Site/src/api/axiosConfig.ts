import axios from 'axios';

export const baseUrl = "http://localhost:8080"; 

const api = axios.create({
  baseURL: `${baseUrl}/api`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;
