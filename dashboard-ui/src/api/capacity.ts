const API_BASE_URL = "http://10.132.150.138:52024/api/CapacityBridge";
//const API_BASE_URL = "https://localhost:7074/api/CapacityBridge";

export async function getWeekCapacity() { 
    const response = await fetch(`${API_BASE_URL}/week-capacity`);
    const data = await response.json();
    return data;
}

export async function getProductName() {
    const response = await fetch(`${API_BASE_URL}/product-names`);
    const data = await response.json();
    return data;
}

export async function getDateCapacity(type: number) {
    const response = await fetch(`${API_BASE_URL}/date-capacity/${type}`);
    const data = await response.json();
    return data;
}

export async function getDateOffLine(type: number) {
    const response = await fetch(`${API_BASE_URL}/date-offline/${type}`);
    const data = await response.json();
    return data;
}

export async function getProcessDate() {
    const response = await fetch(`${API_BASE_URL}/process-data`);
    const data = await response.json();
    return data;
}
