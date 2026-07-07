import http from 'k6/http';
import { check, sleep } from 'k6';

// Configuración de la prueba: 50 usuarios concurrentes por 30 segundos
export const options = {
    vus: 50,
    duration: '30s',
};

export default function () {
    const url = 'http://localhost:5053/api/transactions/process'; // Ajusta tu puerto
    const payload = JSON.stringify({
        userId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
        merchantId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
        amount: 150.0,
        ipAddress: '192.168.1.1',
        originCountry: 'PE'
    });

    const params = {
        headers: { 'Content-Type': 'application/json' },
    };

    // Disparamos la petición
    const res = http.post(url, payload, params);

    // Validamos lo que responde la API
    check(res, {
        'Status es 200 (Aprobado)': (r) => r.status === 200,
        'Status es 429 (Rate Limit Funciona)': (r) => r.status === 429,
    });

    // Pequeña pausa para simular el comportamiento humano
    sleep(0.1); 
}