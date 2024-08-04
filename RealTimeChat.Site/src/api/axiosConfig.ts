import axios from 'axios';

export const baseUrl = process.env.BACKEND_URL; 
console.log(baseUrl)

const api = axios.create({
  baseURL: `${baseUrl}/api`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;
