import axios from 'axios';

export const baseUrl = "http://localhost:5000"; 

const api = axios.create({
  baseURL: `${baseUrl}/api`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;
