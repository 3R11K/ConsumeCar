//fazer requisicoes para o backend
import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5063/api'
});

export default api;

