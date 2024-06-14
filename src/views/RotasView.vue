<script setup>
import { ref, onMounted } from 'vue';
import Modal from '../components/Modal.vue';
import Modal2 from '../components/Modal2.vue';
import Swal from 'sweetalert2';
import Sidebar from '../components/Sidebar.vue';
import axios from 'axios';

let map;
let ui;
const showModal = ref(false);
const editModal = ref(false);
const deleteModal = ref(false);
const dadosModal = ref(false);
const motoristaValido = ref(false); 
const lojaValido = ref(false); 
const dataValido = ref(false); 

const encomendasSelecionadas = ref([]);
const pesoTotal2 = ref(0);
const horarioFinal2 = ref('');
const dataInicial = ref('');
const distancia = ref('');
const linhaMapa = ref('');
const rotaBusca = ref('');
const rotas = ref([]);
const clientes = ref([]);
const encomendas = ref([]);
const encomendasBusca = ref([]);
const motoristas = ref([]);
const lojas = ref([]);
const lojasRotas = ref([]);
const routeColors = {};
const modalSelecionadoDados = ref({});
const novaRota = ref({
  motoristaId: '',  lojaId: ''
});
let rotasSelecionada = ref({});
const veiculoIconMap = {
  Carro: '/src/assets/svg/carro.svg',
  Caminhão: '/src/assets/svg/caminhao.svg',
  Van: '/src/assets/svg/van.svg',
  Picape: '/src/assets/svg/picape.svg',
  Moto: '/src/assets/svg/moto.svg'
};
const vehicleWeightLimit = {
  'Moto': 20,
  'Carro': 100,
  'Picape': 200,
  'Van': 500,
  'Caminhão': 2000
};


const formatarData = (dateString) =>{
  const options = {year: 'numeric',month: '2-digit',day: '2-digit',hour: '2-digit',minute: '2-digit',hour12: false};
  return new Date(dateString).toLocaleString('pt-BR', options).replace(',', '');
};

const formatarDataHora = (dateString) =>{
  const options = {hour: '2-digit',minute: '2-digit',hour12: false};
  return new Date(dateString).toLocaleString('pt-BR', options).replace(',', '');
};

const calcularDataHora = (dataInicial, dataFinal) => {
  const diff = new Date(dataFinal) - new Date(dataInicial);
  const horas = Math.floor(diff / (1000 * 60 * 60));
  const minutos = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
  return `${horas.toString().padStart(2, '0')}h ${minutos.toString().padStart(2, '0')}min`;
};
  
const buscarRotas = async () => {
  try {
    const response = await axios.get('https://localhost:7204/api/Trajeto');
    if (response.data && Object.keys(response.data).length !== 0) {
      rotas.value = response.data;
    }
    coresStart();
    setTimeout(() => {
      puxarRotasMap();
    },1000); 
  } catch (error) {
    console.error('Erro ao pegar as rotas: ', error);
  }
};

const buscarMotoristas = async () => {
  try {
    const response = await axios.get('https://localhost:7204/api/Motorista');
    if (response.data && Object.keys(response.data).length !== 0) {
      motoristas.value = response.data;
    }
  } catch (error) {
    console.error('Erro ao pegar os clientes: ', error);
  }
};

const buscarClientes = async () => {
  try {
    const response = await axios.get('https://localhost:7204/api/Cliente');
    if (response.data && Object.keys(response.data).length !== 0) {
      clientes.value = response.data;
    }
  } catch (error) {
    console.error('Erro ao pegar os clientes: ', error);
  }
};

const buscarEncomendas = async () => {
  try {
    const response = await axios.get('https://localhost:7204/api/Encomenda');
    if (response.data && Object.keys(response.data).length !== 0) {
      encomendas.value = response.data;
    }
  } catch (error) {
    console.error('Erro ao pegar as encomendas: ', error);
  }
};

const buscarEncomendasCadastro = async () => {
  try {
    const response = await axios.get('https://localhost:7204/api/Encomenda');
    if (response.data && Object.keys(response.data).length !== 0) {
      encomendasBusca.value = response.data;
    }
  } catch (error) {
    console.error('Erro ao pegar as encomendas: ', error);
  }
};

const buscarLojas = async () => {
  try {
    const response = await axios.get('https://localhost:7204/api/Loja');
    if (response.data && Object.keys(response.data).length !== 0) {
      lojas.value = response.data;
    }
  } catch (error) {
    console.error('Erro ao pegar os clientes: ', error);
  }
};

const verificarAbrirCadastro = async () => {
  if (!encomendasBusca.value || encomendasBusca.value.length === 0) {
    Swal.fire({
      position: "top",
      icon: "error",
      title: "Nenhuma encomenda Disponivel para a rota!",
      showConfirmButton: false,
      timer: 2000,
      backdrop: false
    });
    return;
  }

  const lojaContador = {};
  let temDisponivel = false;

  encomendasBusca.value.forEach(encomenda => {
    if (encomenda.status === "Disponivel") {
      temDisponivel = true;
      const lojaId = encomenda.lojaId;
      const lojaInfo = infosLoja(lojaId);
      const endereco = `${lojaInfo.bairro}, ${lojaInfo.cidade}/${lojaInfo.estado}`;
      if (lojaContador[lojaId]) {
        lojaContador[lojaId].qtd++;
      } else {
        lojaContador[lojaId] = {
          nome: lojaInfo.nome,
          id: lojaId,
          qtd: 1,
          endereco: endereco
        };
      }
    }
  });

  if (!temDisponivel) {
    Swal.fire({
      position: "top",
      icon: "error",
      title: "Nenhuma encomenda Disponivel para a rota!",
      showConfirmButton: false,
      timer: 2000,
      backdrop: false
    });
    return;
  }

  lojasRotas.value = Object.values(lojaContador);
  showModal.value = true;
};

const buscarEncomendasSelect = async () => {
  try {
    const response = await axios.get('https://localhost:7204/api/Encomenda');
    if (response.data && Object.keys(response.data).length !== 0) {
      if (novaRota.value.lojaId === '') {
        encomendasBusca.value = response.data;
        lojaValido.value = false;
      } else {
        encomendasBusca.value = response.data.filter(encomenda => encomenda.lojaId === novaRota.value.lojaId);
        lojaValido.value = true;
      }
    }
  } catch (error) {
    console.error('Erro ao pegar as encomendas: ', error);
  }
};

const infosCliente = (clienteId) =>{
  return clientes.value.find(cliente => cliente.id === clienteId) || {};
};

const infosEncomenda = (encomendaId) =>{
  return encomendas.value.find(encomenda => encomenda.id === encomendaId) || {};
};
const infosMotorista = (motoristaId) =>{
  return motoristas.value.find(motorista => motorista.id === motoristaId) || {};
};
const infosLoja = (lojaId) =>{
  return lojas.value.find(loja => loja.id === lojaId) || {};
};

const infosRota = (rotaId) =>{
  return rotas.value.find(rota => rota.id === rotaId) || {};
};

const infosLojaEncomenda = (encomendas) => {
  const primeiroId = parseInt(encomendas.split(',')[0].trim());
  const encomenda = infosEncomenda(primeiroId);
  if (encomenda) {
    const primeiraLojaId = encomenda.lojaId;
    return infosLoja(primeiraLojaId) || {};
  }
  return {};
};

const infosClienteEncomenda = (encomendas) => {
  const listaIds = encomendas.split(',').map(id => parseInt(id.trim()));
  const ultimoId = listaIds[listaIds.length - 1];

  const encomenda = infosEncomenda(ultimoId);
  if (encomenda) {
    const clienteId = encomenda.clienteId;
    return infosCliente(clienteId) || {};
  }
  return {};
};

const infosClienteEncomenda2 = (encomendas) => {
  const listaIds = encomendas.split(',').map(id => parseInt(id.trim()));
  const clientesInfo = [];

  listaIds.forEach(id => {
    const encomenda = infosEncomenda(id);
    if (encomenda) {
      const clienteId = encomenda.clienteId;
      const clienteInfo = infosCliente(clienteId);
      if (clienteInfo) {
        clientesInfo.push(clienteInfo);
      }
    }
  });

  return clientesInfo;
};

const adicionarRota = async () => {
  try {
    const response = await axios.post('https://localhost:7204/api/Trajeto', {
      motoristaId: novaRota.value.motoristaId,
      dataInicial: dataInicial.value,
      encomendas: String(encomendasSelecionadas.value.map(e => e.id)),
      pesoTotal: pesoTotal2.value,
      dataFinal: horarioFinal2.value,
      distancia: distancia.value,
      linhaMapa: linhaMapa.value,
    });
    if (response.status === 201) {
      Swal.fire({
        position: "top",
        icon: "success",
        title: "Rota cadastrada com sucesso!",
        showConfirmButton: false,
        timer: 1500,
        backdrop: false
      });
      showModal.value = false;
      await atualizarStatusEncomendas(encomendasSelecionadas.value);
      linhaMapa.value = '';
      distancia.value = '';
      horarioFinal2.value = '';
      pesoTotal2.value = 0;
      dataInicial.value = '';
      novaRota.value = {
        motoristaId: '',
        lojaId: '',
      };
      dataValido.value = false; 
      motoristaValido.value = false; 
      lojaValido.value = false; 
      buscarRotas();
      buscarEncomendasCadastro();
    }
  } catch (error) {
    console.error('Erro ao adicionar Rota: ', error);
  }
};

const atualizarStatusEncomendas = async (encomendas) => {
  try {
    for (const encomenda of encomendas) {
      await axios.put(`https://localhost:7204/api/Encomenda/${encomenda.id}`, {
        clienteId: encomenda.clienteId,
        lojaId: encomenda.lojaId,
        kg: encomenda.kg,
        distancia: encomenda.distancia,
        duracao: encomenda.duracao,
        status: 'Em Rota',
        latitudeOrigem: encomenda.latitudeOrigem,
        longitudeOrigem: encomenda.longitudeOrigem,
        latitudeDestino: encomenda.latitudeDestino,
        longitudeDestino: encomenda.longitudeDestino,
      });
    }
    encomendasSelecionadas.value = [''];
  } catch (error) {
    console.error('Erro ao atualizar status das encomendas: ', error);
  }
};

const editarRota = async () => {
  editModal.value = false; 
  Swal.fire({
    position: "top",
    icon: "error",
    title: "Edit desativado no momento!",
    showConfirmButton: false,
    timer: 1500,
    backdrop: false
  });
//   try {
//     const response = await axios.put(`https://localhost:7204/api/Rota/${rotasSelecionada.value.id}`, {
//       motoristaId: rotasSelecionada.value.motoristaId,
//       dataInicial: rotasSelecionada.value.dataInicial,
//       encomendas: rotasSelecionada.value.encomendas,
//       pesoTotal: rotasSelecionada.value.pesoTotal,
//       dataFinal: rotasSelecionada.value.dataFinal,
//       distancia: rotasSelecionada.value.distancia,
//       linhaMapa: rotasSelecionada.value.linhaMapa,
//     });
//     if (response.status === 201 || response.status === 200 || response.status === 204) { 
//       Swal.fire({
//         position: "top",
//         icon: "success",
//         title: "Rota editada com sucesso!",
//         showConfirmButton: false,
//         timer: 1500,
//         backdrop: false
//       });
//       editModal.value = false; 
//       buscarRotas()
//     }
//   } catch (error) {
//     console.error('Erro ao editar Rota: ', error);
//   }
};

const apagarRota = async () => {
  try {
    const response = await axios.delete(`https://localhost:7204/api/Trajeto/${rotasSelecionada.value.id}`)
      
      if (response.status === 201 || response.status === 200 || response.status === 204) { 
        Swal.fire({
          position: "top",
          icon: "success",
          title: "Rota apagada com sucesso!",
          showConfirmButton: false,
          timer: 1500,
          backdrop: false
        });
        buscarRotas();
        editModal.value = false;
      }
    } catch (error) {
      console.error('Erro ao deletar Rota: ', error);
    }
};

const dataFormRotas = (index) => {
  rotasSelecionada.value = {
    id: rotas.value[index].id,
    motoristaId: rotas.value[index].motoristaId,
    pesoTotal: rotas.value[index].pesoTotal,
    dataInicial: rotas.value[index].dataInicial,
    dataFinal: rotas.value[index].dataFinal,
    distancia: rotas.value[index].distancia,
    encomendas: rotas.value[index].encomendas,
    linhaMapa: rotas.value[index].linhaMapa,
  };
};

let modalDados = async (id) => {
  try {
    const response = await axios.get(`https://localhost:7204/api/Trajeto/${id}`);
    modalSelecionadoDados.value = {
      id: response.data.id,
      dataInicial: response.data.dataInicial,
      dataFinal: response.data.dataFinal,
      pesoTotal: response.data.pesoTotal,
      distancia: response.data.distancia,
      motoristaId: response.data.motoristaId,
      encomendas: response.data.encomendas,
      nomeMotorista: infosMotorista(response.data.motoristaId).nome,
      veiculoMotorista: infosMotorista(response.data.motoristaId).tipoVeiculo,
      telefoneMotorista: infosMotorista(response.data.motoristaId).telefone,
      cpfMotorista: infosMotorista(response.data.motoristaId).cpf,
      placaMotorista: infosMotorista(response.data.motoristaId).placaVeiculo,
      nomeLoja: infosLojaEncomenda(response.data.encomendas).nome,
      cnpjLoja: infosLojaEncomenda(response.data.encomendas).cnpj,
      telefoneLoja: infosLojaEncomenda(response.data.encomendas).telefone,
      cepLoja: infosLojaEncomenda(response.data.encomendas).cep,
      logradouroLoja: infosLojaEncomenda(response.data.encomendas).logradouro,
      bairroLoja: infosLojaEncomenda(response.data.encomendas).bairro,
      cidadeLoja: infosLojaEncomenda(response.data.encomendas).cidade,
      estadoLoja: infosLojaEncomenda(response.data.encomendas).estado,
      numeroLoja: infosLojaEncomenda(response.data.encomendas).numero,
      ///Cliente
      Clientes: infosClienteEncomenda2(response.data.encomendas),
    };
  } catch (error) {
    console.error('Erro ao pegar a rota: ', error);
  }
  dadosModal.value = true;
  infosModa(id); ; 
};

const infosModa = (id) => {
  setTimeout(() => {
    const clientest = infosClienteEncomenda2(infosRota(id).encomendas);
    const container = document.getElementById('clientes-container');
    clientest.forEach((cliente, index) => {
      const clienteDiv = document.createElement('div');
      clienteDiv.classList.add('info');
      const numero = index + 1;
      clienteDiv.innerHTML = `
        <p>
          <p class="text-lg mb-2 mt-4">Endereço <strong>${numero}</strong></p>
          <strong>Cliente:</strong> ${cliente.nome} <br>
          <strong>CPF:</strong> ${cliente.cpf} <br>
          <strong>Telefone:</strong> ${cliente.telefone} <br>
          <strong>Endereço:</strong> ${cliente.logradouro} ${cliente.numero},
          ${cliente.bairro}, ${cliente.cidade}/${cliente.estado} - ${cliente.cep}
        </p>
      `;
      container.appendChild(clienteDiv);
    });
  },200); 
};

const iniciarMapa = () => {
  const loader = document.getElementById('loader');
  const platform = new H.service.Platform({
    apikey: 'eGEbMqmjPEdw473hAUUXR5t_22Ys36iC6n4NfKGCu8Q'
  });
  const defaultLayers = platform.createDefaultLayers();
  const mapContainer = document.getElementById('mapContainer');
  map = new H.Map(
    mapContainer,
    defaultLayers.vector.normal.map,
    {
      zoom: 8.5,
      center: { lat: -22.2175, lng: -49.9512 },
    }
  );
  const behavior = new H.mapevents.Behavior(new H.mapevents.MapEvents(map));
  ui = H.ui.UI.createDefault(map, defaultLayers);
  map.getViewModel().addEventListener('sync', () => {
    'FAZER FUNCAO'
  });
};

const puxarRotasMap = () => {
  map.removeObjects(map.getObjects());
  rotas.value.forEach(rota => {
    let foi = true
    const { id, encomendas } = rota;
    const encomendasArray = encomendas.split(',').map(id => parseInt(id.trim()));
    encomendasArray.forEach((encomendaId,index) => {
      const encomenda = infosEncomenda(encomendaId);
      const cliente = infosCliente(encomenda.clienteId);
      const numero = index + 1;
      const info = 'Cliente: ' + cliente.nome + '<br>' + cliente.logradouro + " " + cliente.numero + ', ' + cliente.cidade + '/' + cliente.estado + ' - ' + cliente.cep;
      const titulo = 'Rota ' + rota.id + '<br>' + 'Endereço ' + '(Destino ' + numero + ')';
      const text = numero;
      const position = { lat: parseFloat(cliente.latitude), lng: parseFloat(cliente.longitude) };
      adicionarMarcador(map, position, id, info, titulo, text);
      adicionarRotaMapa(map, id);
      if (foi) {
        const loja = infosLoja(encomenda.lojaId);
        const titulo2 = 'Rota ' + rota.id + '<br>' + 'Loja (Origem)';
        const text = '0';
        const info2 = 'Loja: ' + loja.nome + '<br>' + loja.logradouro + " " + loja.numero + ', ' + loja.cidade + '/' + loja.estado + ' - ' + loja.cep;
        const positionLoja = { lat: parseFloat(loja.latitude), lng: parseFloat(loja.longitude) };
        adicionarMarcador(map, positionLoja, id, info2, titulo2, text);
        foi = false;
      }
    });
  });
};

const coresStart = () => {
  const vibrantColors = [
    '#00008B', // Azul escuro
    '#006400', // Verde escuro
    '#4B0082', // Roxo escuro
    '#556B2F', // Verde oliva escuro
    '#FF8C00', // Laranja escuro
    '#FFD700', // Amarelo dourado
    '#1E90FF', // Azul Dodger escuro
    '#8B008B', // Magenta escuro
    '#2E8B57', // Verde mar escuro
    '#008B8B'  // Ciano escuro
  ];

  let colorIndex = 0;

  rotas.value.forEach(rota => {
    const { id } = rota;
    if (colorIndex >= vibrantColors.length) {
      console.warn('Not enough unique colors to assign to all routes');
      return;
    }
    routeColors[id] = vibrantColors[colorIndex];
    colorIndex = (colorIndex + 1) % vibrantColors.length;
  });
};

const adicionarRotaMapa = (map, id) => {
  const color = routeColors[id];
  const routeShape = infosRota(id).linhaMapa;
  const rotas = routeShape.split(',');

  rotas.forEach(rota => {
    const partes = rota.split(':');
    const numero = partes[0];
    const coordenadas = partes[1];
    const linestring = H.geo.LineString.fromFlexiblePolyline(coordenadas);
    const routeLine = new H.map.Polyline(linestring, {style: { strokeColor: color, lineWidth: 5 }});
    map.addObject(routeLine);
  });
};

const darZoomMapa = async (latitude1, longitude1, latitude2, longitude2) => {
  const bounds = new H.geo.Rect(
    Math.min(latitude1, latitude2), Math.min(longitude1, longitude2),
    Math.max(latitude1, latitude2), Math.max(longitude1, longitude2)
  );
  map.getViewModel().setLookAtData({
    bounds: bounds
  });
  Swal.fire({
    position: "top",
    icon: "success",
    title: "Rota localizada com sucesso!",
    showConfirmButton: false,
    timer: 1500,
    backdrop: false
  });
};

const resetarMapa = () => {
  map.setCenter({ lat: -22.2175, lng: -49.9512  });
  map.setZoom(8.5);
  Swal.fire({
    position: "top",
    icon: "success",
    title: "Mapa resetado com sucesso!",
    showConfirmButton: false,
    timer: 1500,
    backdrop: false
  });
};

const adicionarMarcador = (map, position, id, info, titulo, text) => {
  const color = routeColors[id];
  const pinColor = "white";
  const svgMarkup = `<svg width="54" height="62" xmlns="http://www.w3.org/2000/svg">
    <path d="M27 0C15.117 0 5 10.117 5 22.5S27 62 27 62s22-27.5 22-39.5S38.883 0 27 0zm0 33c-5.799 0-10.5-4.701-10.5-10.5S21.201 12 27 12s10.5 4.701 10.5 10.5S32.799 33 27 33z" fill="${color}" stroke="${pinColor}" stroke-width="2"/>
    <text x="27" y="25" font-size="12pt" font-family="Arial" font-weight="bold" text-anchor="middle" fill="${color}">${text}</text>
  </svg>`;
  const icon = new H.map.Icon(svgMarkup);

  const marker = new H.map.Marker(position, { icon });
  map.addObject(marker);

  const showInfo = () => {
    Swal.fire({
      title: `${titulo}`,
      html: `
        ${info}<br>
      `,
      iconHtml: '<svg width="50" height="50" viewBox="0 0 6.25 6.25" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="iconify iconify--gis"><path d="M1.313 2C.591 2 0 2.589 0 3.309c0 .279.089.538.24.751l.913 1.578c.128.167.213.135.319-.009l1.007-1.713a1 1 0 0 0 .05-.116 1.3 1.3 0 0 0 .096-.491C2.625 2.589 2.034 2 1.313 2m0 .613c.389 0 .698.308.698.696s-.31.695-.698.695-.698-.308-.698-.695.309-.696.698-.696M5.484 0a.767.767 0 0 0-.765.763c0 .163.052.314.14.438l.532.92c.075.097.124.079.186-.005l.587-.999a.4.4 0 0 0 .029-.068.8.8 0 0 0 .056-.286A.766.766 0 0 0 5.484 0m0 .358c.227 0 .407.18.407.406s-.18.406-.407.406-.407-.18-.407-.406.18-.406.407-.406"/><path d="M5.513 2.338q-.21.004-.422.019l.022.346a8 8 0 0 1 .407-.019zm-.748.051a4 4 0 0 0-.652.127l.081.335c.197-.056.4-.092.607-.118zm-.976.243a2 2 0 0 0-.188.099.9.9 0 0 0-.25.223.55.55 0 0 0-.108.272l.318.035a.2.2 0 0 1 .039-.09.6.6 0 0 1 .161-.14 1 1 0 0 1 .149-.079zm-.164.784-.206.265a1 1 0 0 0 .15.108h.002c.165.094.334.146.491.197l.09-.332a2.3 2.3 0 0 1-.432-.171.5.5 0 0 1-.094-.067m.833.331-.088.333.042.013.051.016c.17.055.332.114.477.196l.147-.307a3 3 0 0 0-.534-.22h-.001L4.5 3.761zm.929.481-.226.245a.5.5 0 0 1 .113.2v.001a.65.65 0 0 1 .005.283l.314.067c.026-.141.03-.296-.012-.449a.84.84 0 0 0-.194-.347m-.218.937a1 1 0 0 1-.114.098 1.7 1.7 0 0 1-.382.19l.103.328a2 2 0 0 0 .453-.227 1 1 0 0 0 .165-.143zm-.79.381a7 7 0 0 1-.614.13l.049.342q.321-.052.642-.136zm-.926.177a15 15 0 0 1-.63.072l.028.345q.321-.03.643-.073zm-.947.099q-.317.025-.635.042l.016.346q.321-.017.642-.042zm-.954.058q-.19.009-.382.015l.011.346.385-.015z" fill-rule="evenodd"/></svg>',
      iconColor: color,
      confirmButtonText: 'Fechar',
      confirmButtonColor: color
    });
  };

  marker.addEventListener('tap', showInfo);
};

const verificarPesoHorarios = () => {
  if  (novaRota.value.motoristaId != ''){
    if  (dataInicial.value != ''){
      if  (novaRota.value.lojaId != ''){
        let pesoTotal = encomendasSelecionadas.value.reduce((total, encomenda) => total + encomenda.kg, 0);
        const limitePesoVeiculo = vehicleWeightLimit[infosMotorista(novaRota.value.motoristaId).tipoVeiculo];
        if (pesoTotal > limitePesoVeiculo) {
          Swal.fire({
            position: "top",
            icon: "error",
            title: "Peso da encomenda maior que o suportado!",
            showConfirmButton: false,
            timer: 1500,
            backdrop: false
          });
          encomendasSelecionadas.value.pop();
        }
        const horarioFinal = new Date(dataInicial.value);
      
        encomendasSelecionadas.value.forEach(encomenda => {
        const duracao = encomenda.duracao;
        const regex = /(\d+)h\s*(\d+)min/;
        const match = duracao.match(regex);
          
        if (match) {
          const horas = parseInt(match[1]);
          const minutos = parseInt(match[2]);
          horarioFinal.setHours(horarioFinal.getHours() + horas);
          horarioFinal.setMinutes(horarioFinal.getMinutes() + minutos);
          }
        });
        const horarioLimite = new Date(dataInicial.value);
        horarioLimite.setHours(18, 0, 0, 0);
        if (horarioFinal < horarioLimite) {
          pesoTotal2.value = pesoTotal
          horarioFinal2.value = formatarData(horarioFinal)
        }else{  
          Swal.fire({
            position: "top",
            icon: "error",
            title: "O horário de chegada ultrapassa as 18:00h.",
            showConfirmButton: false,
            timer: 1500,
            backdrop: false
          });
          encomendasSelecionadas.value.pop();
        }
      }else{
        Swal.fire({
            position: "top",
            icon: "error",
            title: "Selecione a Loja",
            showConfirmButton: false,
            timer: 1500,
            backdrop: false
          });
        encomendasSelecionadas.value.pop();
      }
    }else{
      Swal.fire({
          position: "top",
          icon: "error",
          title: "Selecione a Data",
          showConfirmButton: false,
          timer: 1500,
          backdrop: false
        });
      encomendasSelecionadas.value.pop();
    }
  }else{
    Swal.fire({
        position: "top",
        icon: "error",
        title: "Selecione o Motorista",
        showConfirmButton: false,
        timer: 1500,
        backdrop: false
      });
    encomendasSelecionadas.value.pop();
  }
};

const calcularRota = async () => {
  if (encomendasSelecionadas.value.length === 0) {
    alert('Nenhuma encomenda selecionada.');
    return;
  }

  const loja = infosLoja(encomendasSelecionadas.value[0].lojaId);
  const positionOrigem = { 
    lat: parseFloat(loja.latitude), 
    lng: parseFloat(loja.longitude) 
  };

  const platform = new H.service.Platform({
    apikey: 'eGEbMqmjPEdw473hAUUXR5t_22Ys36iC6n4NfKGCu8Q'
  });

  const waypoints = encomendasSelecionadas.value.map((encomenda) => {
    const cliente = infosCliente(encomenda.clienteId);
    return `${cliente.latitude},${cliente.longitude}`;
  });

  const origin = `${positionOrigem.lat},${positionOrigem.lng}`;
  const destination = waypoints.pop(); 
  const vias = waypoints.map((waypoint) => `&via=${waypoint}`).join('');

  const queryString = `origin=${origin}&transportMode=car&destination=${destination}${vias}&return=polyline,summary&apikey=eGEbMqmjPEdw473hAUUXR5t_22Ys36iC6n4NfKGCu8Q`;

  fetch(`https://router.hereapi.com/v8/routes?${queryString}`)
    .then(response => response.json())
    .then(result => {
      if (result.routes && result.routes.length > 0) {
        const route = result.routes[0];
        if (route.sections && route.sections.length > 0) {
          let totalDistance = 0;
          let totalTime = 0;
          route.sections.forEach((section, index) => {
            totalDistance += section.summary.length;
            totalTime += section.summary.duration;
          });
          const distance = (totalDistance / 1000).toFixed(2);
          const timeHours = Math.floor(totalTime / 3600);
          const timeMinutes = Math.floor((totalTime % 3600) / 60);
          const horarioFinal = new Date(dataInicial.value);
          horarioFinal.setHours(horarioFinal.getHours() + timeHours);
          horarioFinal.setMinutes(horarioFinal.getMinutes() + timeMinutes);
          const year = horarioFinal.getFullYear();
          const month = String(horarioFinal.getMonth() + 1).padStart(2, '0'); 
          const day = String(horarioFinal.getDate()).padStart(2, '0');
          const hours = String(horarioFinal.getHours()).padStart(2, '0');
          const minutes = String(horarioFinal.getMinutes()).padStart(2, '0');

          horarioFinal2.value = `${year}-${month}-${day}T${hours}:${minutes}`;

          const routeSections = route.sections.map((section, index) => `${index + 1}:${section.polyline}`).join(',');

          linhaMapa.value = routeSections;
          distancia.value = distance;

          adicionarRota();
        } else {
          console.error('Erro: Sem seções na rota calculada.');
        }
      } else {
        console.error('Erro: Sem rotas encontradas.');
      }
    })
    .catch(error => {
      console.error('Erro ao calcular a rota:', error);
    });
  };

  const verificarMotorista = () => {
    const motorista = novaRota.value.motoristaId;
    if (motorista == '') {
        motoristaValido.value = false;
    } else {
      motoristaValido.value = true;
    }
  };

  const validarData = (event) => {
  const hoje = new Date();
  const dataSelecionada = new Date(event.target.value); 
  if (dataSelecionada < hoje) {
    event.target.value = '';
    dataInicial.value = '';
    dataValido.value = false;
    Swal.fire({
      position: "top",
      icon: "error",
      title: "Não pode data passada!",
      showConfirmButton: false,
      timer: 1500,
      backdrop: false
    });
    return;
  }else{
    dataValido.value = true;
  }
  const horaSelecionada = dataSelecionada.getHours();
  if (horaSelecionada < 8 || horaSelecionada >= 18) {
    event.target.value = ''; 
    dataValido.value = false;
    dataInicial.value = '';
    Swal.fire({
      position: "top",
      icon: "error",
      title: "Entregas somente das 08:00 às 18:00!",
      showConfirmButton: false,
      timer: 1500,
      backdrop: false
    });
  }else{
    dataValido.value = true;
  }
  };

const pegarRotaId = async (id) => {
  if (id == '') {   
    try {
    rotas.value = '';
    const response = await axios.get(`https://localhost:7204/api/Trajeto`);
    if (response.data && Object.keys(response.data).length !== 0) {
      rotas.value = response.data;
    } 
  } catch (error) {
    console.error('Erro ao pegar as rotas: ', error);
  }
  }else{
    try {
      rotas.value = '';
      const response = await axios.get(`https://localhost:7204/api/Trajeto/b=${id}`);
      if (response.data && Object.keys(response.data).length !== 0) {
        rotas.value = response.data;
      } 
    } catch (error) {
      console.error('Erro ao pegar as rotas: ', error);
    }
  } 
};
  
const buscarId = async () => {
  try {
    await pegarRotaId(rotaBusca.value);
  } catch (error) {
    console.error('Erro ao buscar as rotas: ', error);
  }
};

onMounted(() => {
  buscarRotas();
  buscarClientes();
  buscarMotoristas();
  buscarLojas(); 
  buscarEncomendas();
  iniciarMapa();
  buscarEncomendasCadastro();
});

</script>

<template>
  <div class="flex w-full gap-8">
    <Sidebar/>
    <div class="p-7 w-full">
      <div class="client__header bg-gray-100 py-4 px-8 rounded-lg shadow-md transition-all duration-300 hover:shadow-lg">
        <div class="flex justify-between items-center">
          <h1 class="text-2xl font-bold text-gray-700">Mapa</h1>
          <div class="flex items-center">
            <button class="modal__btn bg-red-600 text-white font-semibold px-4 py-2 ml-2 rounded-lg flex items-center hover:bg-red-700 transition-all duration-300 transform hover:scale-105" @click="resetarMapa()">
              <img class="mr-2" src="/src/assets/svg/mapa.svg" alt="Resetar Mapa">
              <span>Resetar Mapa</span>
            </button>
          </div>
        </div>
      </div>
      <div class="client__body">
        <div class="container w-full mx-auto py-8">
          <div id="mapContainer" class="map-container"></div>
        </div>
      </div>
      <div class="client__header bg-gray-100 py-4 px-8 rounded-lg shadow-md transition-all duration-300 hover:shadow-lg">
        <div class="flex justify-between items-center">
          <h1 class="text-2xl font-bold text-gray-700">Rotas</h1>
          <div class="flex items-center">
            <input type="text" v-model="rotaBusca" @input="buscarId()" placeholder="Pesquisar por id..." class="bg-white border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:border-red-600 transition-colors duration-300">
            <button class="modal__btn bg-red-600 text-white font-semibold px-4 py-2 ml-2 rounded-lg flex items-center hover:bg-red-700 transition-all duration-300 transform hover:scale-105" @click="verificarAbrirCadastro()">
              <img class="mr-2" src="/src/assets/svg/mapaadd.svg" alt="Cadastrar Rota">
              <span>Cadastrar Rota</span>
            </button>
          </div>
        </div>
      </div>
      <div class="client__body">
        <div class="container mx-auto py-8">
          <div v-if="rotas" class="grid grid-cols-3 gap-6">
            <div v-for="(rota, index) in rotas" :key="index" class="bg-white shadow-md rounded-lg overflow-hidden transform transition duration-300 hover:shadow-xl hover:scale-105 ">
              <div class="p-6">
                <h3 class="text-center text-xl font-bold mb-1" :style="{ color: routeColors[rota.id] }">Rota: {{ rota.id }}</h3>
                <h3 class="text-center text-base font-bold mb-4" :style="{ color: routeColors[rota.id] }">{{ formatarData(rota.dataInicial) }} - {{ formatarDataHora(rota.dataFinal) }}</h3>
                <div class="flex flex-col justify-around items-center mb-2">
                  <div class="text-center">
                    <svg width="50" height="50" viewBox="0 0 0.938 0.938" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class=" mx-auto transition duration-300 hover:scale-110 iconify iconify--gis">
                      <path d="M.094.156V.125A.03.03 0 0 0 .063.15zm.75 0L.875.15A.03.03 0 0 0 .844.125zm.063.313V.5A.03.03 0 0 0 .938.463zm-.875 0L.001.463A.03.03 0 0 0 .031.5zM.22.657H.188v.031h.031zm.5 0v.031h.031V.657zM0 .938h.938V.875H0zM.063.469v.438h.063V.469zm.75 0v.438h.063V.469zM.094.188h.75V.125h-.75zM.813.163l.063.313.061-.012L.874.151zm.093.275H.031v.063h.875zM.062.475.125.162.064.15.001.463zM.063.063h.813V0H.063zm.125.406v.188h.063V.469zm.031.219h.5V.625h-.5zM.75.657V.469H.687v.188z" fill=#ff0000 />
                    </svg>
                    <p class="text-gray-600 mt-2">{{ infosLojaEncomenda(rota.encomendas).nome }}</p>
                    <p class="text-gray-600 mb-4">{{ infosLojaEncomenda(rota.encomendas).bairro }}, {{ infosLojaEncomenda(rota.encomendas).cidade }}, {{ infosLojaEncomenda(rota.encomendas).estado }}</p>
                  </div>
                    <div class="text-center">
                      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 50 50" xml:space="preserve" width="50" height="50" aria-hidden="true" class=" mx-auto transition duration-300 hover:scale-110 iconify iconify--gis">
                        <path fill=#ff0000 d="M15.457 17.094a7.748 7.748 0 0 0 7.92 0c2.905 -1.726 4.481 -5.141 4.323 -9.369C27.51 2.61 23.293 0.135 19.457 0.001a1.14 1.14 0 0 0 -0.08 0C15.541 0.135 11.325 2.61 11.134 7.725c-0.158 4.228 1.418 7.642 4.324 9.369M19.417 2.255c2.862 0.115 5.796 1.863 6 5.395h-0.841c-1.297 -1.481 -3.145 -2.279 -5.154 -2.279h0c-2.014 0 -3.864 0.798 -5.162 2.279h-0.841c0.204 -3.533 3.138 -5.28 6 -5.395m-4.612 7.675c0.363 0 0.705 -0.161 0.919 -0.454 0.868 -1.185 2.215 -1.859 3.696 -1.858 1.476 0 2.821 0.671 3.689 1.855 0.215 0.293 0.556 0.457 0.919 0.457h1.288c-0.34 2.393 -1.421 4.193 -3.104 5.192 -1.725 1.025 -3.867 1.019 -5.592 -0.006 -1.682 -0.999 -2.764 -2.793 -3.104 -5.186zm28.573 27.319c0 7.031 -5.72 12.751 -12.751 12.751 -0.629 0 -1.14 -0.51 -1.14 -1.14s0.51 -1.14 1.14 -1.14c5.774 0 10.472 -4.698 10.472 -10.472 0 -0.629 0.51 -1.14 1.14 -1.14s1.14 0.51 1.14 1.14m-4.722 -0.228c0 -0.522 -0.049 -1.034 -0.142 -1.53a1.14 1.14 0 0 0 -0.011 -0.059c-0.742 -3.796 -4.094 -6.669 -8.104 -6.669 -4.553 0 -8.258 3.704 -8.258 8.257 0 4.553 3.705 8.257 8.258 8.257 4.553 0 8.257 -3.704 8.257 -8.257m-2.328 0.249c-0.115 2.796 -2.098 5.102 -4.833 5.621v-4.76c1.709 -0.09 3.332 -0.382 4.833 -0.861m-5.929 -6.227c2.582 0 4.786 1.645 5.621 3.942 -1.695 0.588 -3.617 0.896 -5.621 0.896 -2.003 0 -3.926 -0.308 -5.622 -0.897 0.835 -2.297 3.04 -3.942 5.622 -3.942m-1.184 11.847c-2.621 -0.518 -4.72 -2.825 -4.834 -5.621 1.501 0.479 3.125 0.771 4.834 0.861zM11.567 28.335 9.131 41.495h9.621c0.629 0 1.14 0.51 1.14 1.14s-0.51 1.14 -1.14 1.14H7.761c-0.339 0 -0.66 -0.159 -0.876 -0.419s-0.306 -0.611 -0.244 -0.944L9.326 27.912c0.639 -3.448 3.17 -5.988 6.945 -6.97 2.001 -0.521 4.208 -0.527 6.215 -0.02 2.17 0.549 3.985 1.646 5.25 3.172 0.402 0.485 0.334 1.203 -0.15 1.604 -0.485 0.402 -1.203 0.334 -1.604 -0.15 -1.918 -2.314 -5.675 -3.301 -9.137 -2.401 -1.093 0.284 -2.047 0.732 -2.837 1.316l5.878 5.878c0.445 0.445 0.445 1.166 0 1.612 -0.445 0.445 -1.167 0.445 -1.612 0l-5.834 -5.835c-0.427 0.66 -0.723 1.409 -0.873 2.216"/>
                      </svg>
                    <p class="text-gray-600 mt-2">{{ infosMotorista(rota.motoristaId).nome }}</p>
                    <img :src="veiculoIconMap[infosMotorista(rota.motoristaId).tipoVeiculo]" alt="Tipo de Veículo" class="w-5 h-5 mx-auto mt-2" />
                    <p class="text-gray-600 mb-4"> {{ infosMotorista(rota.motoristaId).tipoVeiculo }}</p>
                  </div>
                  <div class="text-center">
                    <svg width="60" height="60" viewBox="0 0 6.25 6.25" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class=" mx-auto transition duration-300 hover:scale-110 iconify iconify--gis">
                      <path fill=#ff0000 d="M1.313 2C0.591 2 0 2.589 0 3.309c0 0.279 0.089 0.538 0.24 0.751l0.913 1.578c0.128 0.167 0.213 0.135 0.319 -0.009l1.007 -1.713c0.02 -0.037 0.036 -0.076 0.05 -0.116A1.294 1.294 0 0 0 2.625 3.309C2.625 2.589 2.034 2 1.313 2m0 0.613c0.389 0 0.698 0.308 0.698 0.696S1.701 4.004 1.313 4.004c-0.388 0 -0.698 -0.308 -0.698 -0.695S0.924 2.613 1.313 2.613M5.484 0C5.064 0 4.719 0.344 4.719 0.763c0 0.163 0.052 0.314 0.14 0.438l0.532 0.92c0.075 0.097 0.124 0.079 0.186 -0.005l0.587 -0.999c0.012 -0.021 0.021 -0.044 0.029 -0.068a0.75 0.75 0 0 0 0.056 -0.286C6.25 0.344 5.905 0 5.484 0m0 0.358c0.227 0 0.407 0.18 0.407 0.406s-0.18 0.406 -0.407 0.406 -0.407 -0.18 -0.407 -0.406 0.18 -0.406 0.407 -0.406"/><path fill=#ff0000  d="M5.513 2.338c-0.14 0.003 -0.281 0.009 -0.422 0.019l0.022 0.346a7.875 7.875 0 0 1 0.407 -0.019zm-0.748 0.051c-0.217 0.027 -0.436 0.065 -0.652 0.127l0.081 0.335c0.197 -0.056 0.4 -0.092 0.607 -0.118zM3.789 2.632a1.531 1.531 0 0 0 -0.188 0.099l0 0 0 0c-0.086 0.054 -0.176 0.123 -0.25 0.223 -0.054 0.072 -0.097 0.164 -0.108 0.272l0.318 0.035c0.002 -0.025 0.015 -0.059 0.039 -0.09h0v0c0.038 -0.051 0.094 -0.097 0.161 -0.14l0 0a1.25 1.25 0 0 1 0.149 -0.079zM3.625 3.416l-0.206 0.265c0.049 0.045 0.1 0.079 0.15 0.108l0.001 0 0.001 0c0.165 0.094 0.334 0.146 0.491 0.197l0.09 -0.332c-0.157 -0.05 -0.305 -0.099 -0.432 -0.171 -0.036 -0.021 -0.068 -0.043 -0.094 -0.067m0.833 0.331 -0.088 0.333 0.042 0.013 0.051 0.016c0.17 0.055 0.332 0.114 0.477 0.196l0.147 -0.307c-0.176 -0.099 -0.358 -0.163 -0.534 -0.22l-0.001 0 -0.053 -0.017zm0.929 0.481 -0.226 0.245c0.053 0.058 0.094 0.128 0.113 0.2l0 0 0 0.001c0.023 0.084 0.023 0.183 0.005 0.283l0.314 0.067c0.026 -0.141 0.03 -0.296 -0.012 -0.449 -0.037 -0.135 -0.107 -0.253 -0.194 -0.347M5.17 5.165a0.75 0.75 0 0 1 -0.114 0.098h0c-0.113 0.079 -0.243 0.139 -0.382 0.19l0.103 0.328c0.152 -0.056 0.307 -0.125 0.453 -0.227l0 0 0 0a1.063 1.063 0 0 0 0.165 -0.143zm-0.79 0.381c-0.201 0.054 -0.406 0.095 -0.614 0.13l0.049 0.342c0.214 -0.036 0.428 -0.079 0.642 -0.136zm-0.926 0.177c-0.209 0.029 -0.419 0.052 -0.63 0.072l0.028 0.345c0.214 -0.02 0.429 -0.044 0.643 -0.073zm-0.947 0.099c-0.211 0.017 -0.423 0.03 -0.635 0.042l0.016 0.346c0.214 -0.012 0.428 -0.025 0.642 -0.042zm-0.954 0.058c-0.127 0.006 -0.254 0.011 -0.382 0.015l0.011 0.346a35 35 0 0 0 0.385 -0.015z" fill-rule="evenodd"/>
                    </svg>
                    <p class="text-gray-600 mt-2">{{ rota.encomendas }}</p>
                    <p class="text-gray-600 mb-4">{{ rota.distancia }}KM - {{ rota.pesoTotal }}KG - {{ calcularDataHora(rota.dataInicial,rota.dataFinal) }}</p>
                  </div>
                </div>
                <div class="border-t mt-6 pt-4 flex justify-around">
                  <button @click="darZoomMapa(infosLojaEncomenda(rota.encomendas).latitude,infosLojaEncomenda(rota.encomendas).longitude,infosClienteEncomenda(rota.encomendas).latitude,infosClienteEncomenda(rota.encomendas).longitude)" class="text-gray-700 hover:text-blue-500 transform transition duration-300 hover:scale-125"> 
                    <img src="/src/assets/svg/localizar.svg" alt="Localizar" title="Localizar" class="w-7 h-7">
                  </button>
                  <button @click="modalDados(rota.id)" class="text-gray-700 hover:text-blue-500 transform transition duration-300 hover:scale-125">
                    <img src="/src/assets/svg/detalhes.svg" alt="Detalhes" title="Detalhes" class="w-7 h-7">
                  </button>
                  <button @click="editModal = true, dataFormRotas(index)" class="text-gray-700 hover:text-blue-500 transform transition duration-300 hover:scale-125">
                    <img src="/src/assets/svg/editar.svg" alt="Editar" title="Editar" class="w-7 h-7">
                  </button>
                  <button @click="dataFormRotas(index), deleteModal = true" class="text-gray-700 hover:text-red-500 transform transition duration-300 hover:scale-125">
                    <img src="/src/assets/svg/lixo.svg" alt="Apagar" title="Apagar" class="w-7 h-7">
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div v-else>
            <div class="p-8 text-center">
              <h2 class="text-xl font-bold">Nada para mostrar aqui :(</h2>
              <img class="mx-auto mt-8" src="/src/assets/svg/nothing.svg" alt="">
            </div>
          </div>
        </div>
      </div>
    <Modal :show="showModal">
      <h2 class="text-center text-lg">Cadastrar Rota </h2>
      <p class="text-left text-base">Peso Total: <strong>{{ pesoTotal2 }} KG</strong></p>
      <p class="text-left text-base mb-8" v-if="horarioFinal2 !== '' && horarioFinal2 !== formatarData(dataInicial)">
      Horário Final: <strong>{{ horarioFinal2 }}</strong>
      </p>
      <p class="text-left text-base mb-8" v-else>
      Horário Final:  <strong>N/A</strong>
      </p>
      <div class="grid grid-cols-3 gap-4">
        <div class="input__box">
          <label for="motorista" class="block w-full">Motorista: </label>
          <select v-model="novaRota.motoristaId":class="{ 'border-green-500': motoristaValido, 'border-red-500': !motoristaValido }" class="w-full h-[40px] p-2 border rounded mt-2":disabled="!(encomendasSelecionadas == '')" name="motorista" id="motorista"@change="verificarMotorista">
            <option value="" disabled selected>Selecione o motorista</option>
            <option v-for="motorista in motoristas" :key="motorista.id" :value="motorista.id">{{ motorista.nome }} - Veiculo: {{ motorista.tipoVeiculo }} - {{  vehicleWeightLimit[motorista.tipoVeiculo] }}KG</option>
          </select>
        </div>
        <div class="input__box">
          <label for="data" class="block w-full">Data:</label>
          <input v-model="dataInicial":class="{ 'border-green-500': dataValido, 'border-red-500': !dataValido }"
                class="w-full h-[40px] p-2 border rounded mt-2":disabled="!(encomendasSelecionadas == '')"
                name="data"
                type="datetime-local"
                @input="validarData($event)">
        </div>
        <div class="input__box">
          <label for="loja" class="block font-semibold text-gray-700">Lojas: </label>
          <select v-model="novaRota.lojaId":class="{ 'border-green-500': lojaValido, 'border-red-500': !lojaValido }" class="w-full h-10 px-4 mt-1 border rounded-md focus:outline-none focus:ring focus:ring-blue-500":disabled="!(encomendasSelecionadas == '')" name="lojasRotas" id="lojasRotas" @change="buscarEncomendasSelect">
            <option value="" disabled selected>Selecione a loja</option>
            <option v-for="(lojarota, index) in lojasRotas" :key="lojarota.id" :value="lojarota.id">({{ lojarota.qtd }}) {{ lojarota.nome }} - {{ lojarota.endereco }}</option>
          </select>
        </div>
      </div>
      <div class="mt-4" style="max-height: 138px; min-height: 138px; overflow-y: auto;">
          <label for="encomendas" class="block w-full">Encomendas:</label>
          <div class="encomendas-list">
            <div v-for="encomenda in encomendasBusca.filter(e => e.status === 'Disponivel')" :key="encomenda.id" class="encomenda-info">
              <div class="flex items-center gap-2">
                <label class="custom-checkbox">
                  <input type="checkbox" :id="encomenda.id" :value="encomenda" v-model="encomendasSelecionadas" @change="verificarPesoHorarios">
                  <span class="checkmark"></span>
                </label>
                <label :for="encomenda.id">
                  Origem: {{ infosLoja(encomenda.lojaId).bairro }}, {{ infosLoja(encomenda.lojaId).cidade }}/{{ infosLoja(encomenda.lojaId).estado }}  <br>Destino: {{ infosCliente(encomenda.clienteId).bairro }}, {{ infosCliente(encomenda.clienteId).cidade }}/{{ infosCliente(encomenda.clienteId).estado }} <br> Tempo: {{ encomenda.duracao }} - Peso: {{ encomenda.kg }}KG
                </label>
              </div>
            </div>
          </div>
        </div>
      <button class="close__btn" @click="showModal = false">
        <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
      </button>
      <button @click="calcularRota" class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white":disabled="!(motoristaValido && lojaValido && dataValido && encomendasSelecionadas != '')">
        Cadastrar
        <span v-if="!(motoristaValido && lojaValido && dataValido && encomendasSelecionadas != '')" class="text-sm ml-2 text-gray-400">(Preencha tudo corretamente!)</span>
      </button>
    </Modal> 
    <Modal :show="editModal">
      <h2 class="text-center text-xl mb-8":style="{ color: routeColors[rotasSelecionada.id] }">Editar Rota {{ rotasSelecionada.id }}</h2>
      <div class="grid grid-cols-2 gap-4">

        <div class="input__box">
          <label for="motoristaId" class="block w-full">Motorista:</label>
          <select class="w-full h-[40px] p-2 border rounded mt-2" name="motoristaId" id="motoristaId" v-model="rotasSelecionada.motoristaId">
            <option value="" disabled>Selecione o motorista</option>
            <option v-for="motorista in motoristas" :key="motorista.id" :value="motorista.id">{{ motorista.nome }}</option>
          </select>
        </div>
        <div class="input__box">
          <label for="data" class="block w-full">Data Inicial:</label>
          <input v-model="rotasSelecionada.dataInicial" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="datainicial" type="datetime-local">
        </div>
        <div class="input__box">
          <label for="data" class="block w-full">Data Final:</label>
          <input v-model="rotasSelecionada.dataFinal" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="datafinal" type="datetime-local">
        </div>
        <div class="input__box">
          <label for="distancia" class="block w-full">Distancia:</label>
          <input v-model="rotasSelecionada.distancia" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="distancia" type="text">
        </div>
        <div class="input__box">
          <label for="kg" class="block w-full">KG da encomenda:</label>
          <input v-model="rotasSelecionada.pesoTotal" class="w-full h-[40px] p-2 border rounded mt-2" name="kg" type="number" placeholder="valor do KG da encomenda">
        </div>      
        <div class="input__box">
          <label for="encomendas" class="block w-full">Encomendas:</label>
          <input v-model="rotasSelecionada.encomendas" class="w-full h-[40px] p-2 border rounded mt-2" name="encomendas" type="text" placeholder="Encomends">
        </div> 
        </div>
      <button class="close__btn" @click="editModal = false">
        <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
      </button>
      <button class="w-full h-[40px] bg-[#006400] rounded mt-4 text-white":style="{ backgroundColor: routeColors[rotasSelecionada.id] }" @click="editarRota()">
        Editar
      </button>
    </Modal>
    <Modal :show="dadosModal">
    <div class="modal">
      <div class="modal-content">
        <div class="modal-header":style="{ color: routeColors[modalSelecionadoDados.id] }">
          <h2 class="text-center text-xl mb-12"><strong>Detalhes da Rota {{ modalSelecionadoDados.id }}</strong></h2>
          <button class="close__btn" @click="dadosModal = false">
            <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none">
              <path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/>
            </svg>
          </button>
        </div>
        <div class="modal-body">
          <div>
            <h3 class="text-center text-xl mb-4 mt-10">Entrega</h3>
            <p>
              <strong>Rota:</strong> {{ modalSelecionadoDados.id }} <br>
              <strong>Data Estimada:</strong> {{ formatarData(modalSelecionadoDados.dataInicial) }} - {{ formatarDataHora(modalSelecionadoDados.dataFinal) }}<br>
              <strong>Peso:</strong> {{ modalSelecionadoDados.pesoTotal }} KG<br>
              <strong>Distância:</strong> {{ modalSelecionadoDados.distancia }} KM<br>
              <strong>Duração Estimada:</strong> {{ calcularDataHora(modalSelecionadoDados.dataInicial,modalSelecionadoDados.dataFinal) }} <br>
            </p>
          </div>

          <div>
            <h3 class="text-center text-xl mb-4 mt-10">Motorista</h3>
            <p>
              <strong>Nome:</strong> {{ modalSelecionadoDados.nomeMotorista }} <br>
              <strong>CPF:</strong> {{ modalSelecionadoDados.cpfMotorista }} <br>
              <strong>Telefone:</strong> {{ modalSelecionadoDados.telefoneMotorista }} <br>
              <strong>Veículo:</strong> {{ modalSelecionadoDados.veiculoMotorista }}
              <strong>Placa:</strong> {{ modalSelecionadoDados.placaMotorista }}
            </p>
          </div>
          <div>
            <h3 class="text-center text-xl mb-4 mt-10">Loja</h3>
            <p>
              <strong>Nome:</strong> {{ modalSelecionadoDados.nomeLoja }} <br>
              <strong>CNPJ:</strong> {{ modalSelecionadoDados.cnpjLoja }} <br>
              <strong>Telefone:</strong> {{ modalSelecionadoDados.telefoneLoja }} <br>
              <strong>Endereço:</strong> {{ modalSelecionadoDados.logradouroLoja }} {{ modalSelecionadoDados.numeroLoja }},
              {{ modalSelecionadoDados.bairroLoja }}, {{ modalSelecionadoDados.cidadeLoja }}/{{ modalSelecionadoDados.estadoLoja }} -
              {{ modalSelecionadoDados.cepLoja }}
            </p>
          </div>
          <div id="clientes-container">
            <div>
              <h3 class="text-center text-xl mb-4 mt-10">Encomendas</h3>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white":style="{ backgroundColor: routeColors[modalSelecionadoDados.id] }" @click="dadosModal = false">
            Fechar
          </button>
        </div>
      </div>
    </div>
  </Modal>  
  <Modal2 :show="deleteModal">
    <h2 class="text-center text-xl mb-8":style="{ color: routeColors[rotasSelecionada.id] }">Deletar Rota {{ rotasSelecionada.id }}</h2>
    <p class="text-center">Tem certeza que deseja deletar a rota ?</p>
    <button class="close__btn" @click="deleteModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white":style="{ backgroundColor: routeColors[rotasSelecionada.id] }" @click="apagarRota(), deleteModal = false">
      Deletar
    </button>
  </Modal2>
    </div>
  </div>
</template>

<style scoped>
  #mapContainer {
    width: 100%;
    height: 500px;
    position: relative;
  }

.map-container {
  height: 500px;
}
.close__btn {
  position: absolute;
  top: 20px;
   right: 20px;
}
.btn {
    display: inline-block;
    padding: 10px 20px;
    font-size: 16px;
    border: 2px solid transparent;
    border-radius: 5px;
    background-color: transparent;
    color: #333;
    position: relative;
    overflow: hidden;
    cursor: pointer;
    transition: color 0.3s ease;
  }

  .btn::before {
    content: "";
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: transparent;
    z-index: -1;
    transition: transform 0.4s ease;
    transform: scaleX(0);
    transform-origin: left;
  }

  .btn:hover::before {
    transform: scaleX(1);
  }

  .btn:hover {
    color: #ff0000; 
  }

  .error-message {
    color: red;
    margin-top: 10px;
  }

  .border-color-dynamic {
    border: 5px solid;
  }

  .border-color-dynamic {
    border-color: var(--border-color);
  }

  .custom-checkbox {
    display: inline-block;
    vertical-align: middle;
    position: relative;
    width: 16px;
    height: 16px;
    border-radius: 3px;
    border: 1px solid #ccc;
    cursor: pointer;
    margin-right: 10px;
  }

  .custom-checkbox input[type="checkbox"] {
    position: absolute;
    opacity: 0;
    cursor: pointer;
  }

  .custom-checkbox input[type="checkbox"]:checked + .checkmark {
    background-color: #2196F3;
  }

  .checkmark {
    position: absolute;
    top: 0;
    left: 0;
    height: 16px;
    width: 16px;
    background-color: #fff;
  }

  .custom-input {
    width: 100%;
    height: 40px;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 5px;
    font-size: 16px;
    box-sizing: border-box;
    margin-bottom: 10px;
  }

  .encomenda-info {
    padding: 10px;
    border-bottom: 1px solid #ccc;
  }
  </style>