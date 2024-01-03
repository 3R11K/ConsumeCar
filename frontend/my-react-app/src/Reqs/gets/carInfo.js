import api from "../../utils/api";

export default async function carInfo(model, year, marca) {
  try {
    console.log("Fetching car information...");
    const response = await api.get(`/Car/${model}/${year}/${marca}`);
    return response.data;
  } catch (error) {
    // Handle errors appropriately
    console.error("Error fetching car information:", error);
    throw error; // You may want to handle the error differently based on your use case
  }
}
