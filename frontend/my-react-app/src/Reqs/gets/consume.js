import api from "../../utils/api";

export default async function consume(state, km, eficiency) {
    try{
        console.log("calculating");
        const response = await api.get(`/Consume/${eficiency}/${km}/${state}`);
        return response.data;
    }catch(error){
        console.error("Error calculating:", error);
        throw error;
    }
}