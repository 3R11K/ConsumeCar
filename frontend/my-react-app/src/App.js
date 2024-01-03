import React, { useState } from 'react';
import carInfo from './Reqs/gets/carInfo';
import consume from './Reqs/gets/consume';

function App() {
  const [onSelect, setOnSelect] = useState(null);
  const [modelo, setModelo] = useState('');
  const [ano, setAno] = useState(0);
  const [marca, setMarca] = useState('');
  const [info, setInfo] = useState({});
  const [showKM, setShowKM] = useState(false);
  const [km, setKM] = useState(0);
  const [cost, setCost] = useState(0);

  const states = [
    'AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA',
    'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN',
    'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'
  ];

  const [selectedState, setSelectedState] = useState('');

  const handleStateChange = (e) => {
    const newState = e.target.value;
    setSelectedState(newState);
    if(onSelect){
      onSelect(newState);
    }
  };

  const handleModeloChange = (event) => {
    setModelo(event.target.value);
  };

  const handleAnoChange = (event) => {
    setAno(event.target.value);
  };

  const handleMarcaChange = (event) => {
    setMarca(event.target.value);
  }

  const handleKM = (event) => {
    setKM(event.target.value);
  };

  const handleSubmit = () => {
    carInfo(modelo, ano, marca).then((response) => {
      setInfo(response);
      setShowKM(true);
    });
  };

  const handleCalculating = () => {
    consume(selectedState, km, info.fuelEfficiency).then((response) => {
      console.log(response);
      setCost(response);
    });
  };
  return (
    <div className="App">
      <input
        type="text"
        placeholder="Modelo do carro"
        value={modelo}
        onChange={handleModeloChange}
      />
      <input
        type="number"
        placeholder="Ano do carro"
        value={ano}
        onChange={handleAnoChange}
      />
      <input
      type="text"
      placeholder="Marca do carro"
      value={marca}
      onChange={handleMarcaChange}/>
      <button onClick={handleSubmit}>Enviar</button>

      {info.fuelEfficiency && (
        <div>
          <p>Marca: {info.make}</p>
          <p>Modelo: {info.model}</p>
          <p>Ano: {info.year}</p>
          <p>Eficiencia: {info.fuelEfficiency.toFixed(2)}</p>
        </div>
      )}
      {showKM && (
        <div>
          <p>Calcule quanto irá gastar de combustivel e uma aproximação de custo:</p>
          <label htmlFor="state">Select a state:</label>
      <select id="state" onChange={handleStateChange} value={selectedState}>
        <option value="">-- Select State --</option>
        {states.map((state) => (
          <option key={state} value={state}>
            {state}
          </option>
        ))}
      </select>
          <input
          type="number"
          placeholder="Distancia em KM"
          onChange={handleKM}/>
          <button onClick={handleCalculating}>Calcular</button>
          <p>Distancia: {km} KM</p>
          {
            cost !== 0 && (
              <div>
                <p>Custo: R$ {cost.toFixed(2)}</p>
              </div>
            )
          }
        </div>
      )}
    </div>
  );
}

export default App;
