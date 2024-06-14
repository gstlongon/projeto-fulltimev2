<script setup>
import axios from 'axios';
import { ref, onMounted } from 'vue';
import Sidebar from '../components/Sidebar.vue';
import { lojasList, clientesList, motoristasList, encomendasList, rotasList } from '../components/ListSave.vue';
import Swal from 'sweetalert2';

const clientes = ref(0);
const motoristas = ref(0);
const rotas = ref(0);
const lojas = ref(0);
const encomendas = ref(0);


const fetchData = async (url, targetRef) => {
  try {
    const response = await axios.get(url);
    if (response.data) {
      targetRef.value = response.data.length;
    } 
  } catch (error) {
    console.error('Erro ao buscar dados: ', error);
  }
}  


onMounted(() => {
  fetchData('https://localhost:7204/api/Cliente', clientes);
  fetchData('https://localhost:7204/api/Motorista', motoristas);
  fetchData('https://localhost:7204/api/Trajeto', rotas);
  fetchData('https://localhost:7204/api/Loja', lojas);
  fetchData('https://localhost:7204/api/Encomenda', encomendas);
});


const adicionarTodasLojas = async () => {
  try {
    for (const loja of lojasList) {
      try {
        await axios.post('https://localhost:7204/api/Loja', {
          nome: loja.nome,
          email: loja.email,
          telefone: loja.telefone,
          cnpj: loja.cnpj,
          logradouro: loja.logradouro,
          bairro: loja.bairro,
          cep: loja.cep,
          numero: loja.numero,
          cidade: loja.cidade,
          estado: loja.estado,
          latitude: loja.latitude,
          longitude: loja.longitude,
        });
        
      } catch (error) {
        console.error(`Erro ao adicionar a loja ${loja.nome}: ${error.message}`);
      }
    }
  } catch (error) {
    console.error(`Erro ao carregar o arquivo de lojas: ${error.message}`);
  }
  fetchData('https://localhost:7204/api/Loja', lojas);
};
const adicionarTodosClientes = async () => {
  try {
    for (const cliente of clientesList) {
      try {
        await axios.post('https://localhost:7204/api/Cliente', {
          nome: cliente.nome,
          email: cliente.email,
          telefone: cliente.telefone,
          cpf: cliente.cpf,
          logradouro: cliente.logradouro,
          bairro: cliente.bairro,
          cep: cliente.cep,
          numero: cliente.numero,
          cidade: cliente.cidade,
          estado: cliente.estado,
          latitude: cliente.latitude,
          longitude: cliente.longitude,
        });
        
      } catch (error) {
        console.error(`Erro ao adicionar o cliente ${cliente.nome}: ${error.message}`);
      }
    }
  } catch (error) {
    console.error(`Erro ao carregar o arquivo de clientes: ${error.message}`);
  }
  fetchData('https://localhost:7204/api/Cliente', clientes);
};
const adicionarTodosMotoristas = async () => {
  try {
    for (const motorista of motoristasList) {
      try {
        await axios.post('https://localhost:7204/api/Motorista', {
          nome: motorista.nome,
          email: motorista.email,
          cpf: motorista.cpf,
          telefone: motorista.telefone,
          tipoVeiculo: motorista.tipoVeiculo,
          placaVeiculo: motorista.placaVeiculo,       
          logradouro: motorista.logradouro,
          bairro: motorista.bairro,
          cep: motorista.cep,
          numero: motorista.numero,
          cidade: motorista.cidade,
          estado: motorista.estado 
        });
        
      } catch (error) {
        console.error(`Erro ao adicionar o motorista ${motorista.nome}: ${error.message}`);
      }
    }
  } catch (error) {
    console.error(`Erro ao carregar o arquivo de motoristas: ${error.message}`);
  }
  fetchData('https://localhost:7204/api/Motorista', motoristas);
};
const adicionarTodasRotas = async () => {
  try {
    for (const rota of rotasList) {
      try {
        await axios.post('https://localhost:7204/api/Trajeto', {
          motoristaId: rota.motoristaId,
          dataInicial: rota.dataInicial,
          encomendas: rota.encomendas,
          pesoTotal: rota.pesoTotal,
          dataFinal: rota.dataFinal,
          placaVeiculo: rota.placaVeiculo,       
          distancia: rota.distancia,
          linhaMapa: rota.linhaMapa,
        });
        
      } catch (error) {
        console.error(`Erro ao adicionar a rota: ${error.message}`);
      }
    }
  } catch (error) {
    console.error(`Erro ao carregar o arquivo da Rota: ${error.message}`);
  }
  fetchData('https://localhost:7204/api/Trajeto', rotas);
};
const adicionarTodasEncomendas = async () => {
  try {
    for (const encomenda of encomendasList) {
      try {
        await axios.post('https://localhost:7204/api/Encomenda', {
          clienteId: encomenda.clienteId,
          email: encomenda.email,
          lojaId: encomenda.lojaId,
          kg: encomenda.kg,
          latitudeOrigem: encomenda.latitudeOrigem,
          longitudeOrigem: encomenda.longitudeOrigem,       
          latitudeDestino: encomenda.latitudeDestino,
          longitudeDestino: encomenda.longitudeDestino,
          distancia: encomenda.distancia,
          duracao: encomenda.duracao,
          status: encomenda.status
        });
        
      } catch (error) {
        console.error(`Erro ao adicionar a encomenda: ${error.message}`);
      }
    }
  } catch (error) {
    console.error(`Erro ao carregar o arquivo de encomendas: ${error.message}`);
  }
  fetchData('https://localhost:7204/api/Encomenda', encomendas);
};
const envioArquivoTodos = async () => {
   Swal.fire({
    title: "Selecione o Arquivo",
    input: "file",
    confirmButtonColor: 'red',
    preConfirm: () => {
      adicionarTodasLojas();
      adicionarTodosClientes();
      adicionarTodosMotoristas();
      adicionarTodasRotas();
      adicionarTodasEncomendas();
      Swal.fire({
        position: "top",
        icon: "success",
        title: "Cadastrado com sucesso!",
        showConfirmButton: false,
        timer: 1500,
        backdrop: false
      });
    },
  });
};


const apagarTodosBD = async (endpoint, lista) => {
  try {
    await axios.delete(`https://localhost:7204/api/${endpoint}`);
    fetchData(`https://localhost:7204/api/${endpoint}`, lista);
  } catch (error) {
    console.error(`Erro ao apagar todos os ${endpoint.toLowerCase()}:`, error);
  }
};
const apagarTodas = async () => {
  Swal.fire({
    title: "Deseja deletar tudo?",
    text: "Ira apagar todos os dados do banco!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonText: "Cancelar",
    cancelButtonColor: "#d33",
    confirmButtonText: "Apagar"
  }).then((result) => {
    if (result.isConfirmed) {
      const endpoints = [
        { endpoint: 'Loja', lista: lojas },
        { endpoint: 'Cliente', lista: clientes },
        { endpoint: 'Motorista', lista: motoristas },
        { endpoint: 'Trajeto', lista: rotas },
        { endpoint: 'Encomenda', lista: encomendas }
      ];

      endpoints.forEach(async ({ endpoint, lista }) => {
        await apagarTodosBD(endpoint, lista);
      });

      Swal.fire({
        position: "top",
        icon: "success",
        title: "Tudo foi apagado!",
        showConfirmButton: false,
        timer: 1500,
        backdrop: false
      });
    }
  });
};
</script>

<template>
  <div class="flex w-full h-full gap-8">
    <Sidebar />
    <div class="p-7 w-full">
      <div class="client__header bg-gray-100 py-4 px-8 rounded-lg shadow-md transition-all duration-300 hover:shadow-lg">
        <div class="flex justify-between items-center">
          <h1 class="text-2xl font-bold text-gray-700">Dashboard</h1>
          <div class="flex items-center">
            <button class="modal__btn bg-red-600 text-white font-semibold px-4 py-2 ml-2 rounded-lg flex items-center hover:bg-red-700 transition-all duration-300 transform hover:scale-105" @click="envioArquivoTodos">
              <img class="mr-2" src="/src/assets/svg/envio2.svg" alt="Adicionar Todos">
              <span>Adicionar Todos</span>
            </button>
            <button class="modal__btn bg-red-600 text-white font-semibold px-4 py-2 ml-2 rounded-lg flex items-center hover:bg-red-700 transition-all duration-300 transform hover:scale-105" @click="apagarTodas">
              <img class="mr-2" src="/src/assets/svg/lixo2.svg" alt="Apagar Todos">
              <span>Apagar Todos</span>
            </button>
          </div>
        </div>
      </div>
      <div class="grid grid-cols-3 gap-6 p-7">
        <div class="w-full p-8 bg-gradient-to-r from-pink-100 to-pink-200 rounded-lg shadow-lg transform transition duration-500 hover:scale-105 flex flex-col justify-center items-center text-center">
          <div class="mb-8">
            <svg class="mb-4" width="80px" height="80px" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg"><path d="M512 505.6c-108.8 0-204.8-89.6-204.8-204.8S396.8 102.4 512 102.4c108.8 0 204.8 89.6 204.8 204.8S620.8 505.6 512 505.6z m0-358.4c-83.2 0-153.6 70.4-153.6 153.6s64 153.6 153.6 153.6 153.6-70.4 153.6-153.6S595.2 147.2 512 147.2z" fill="#D72822" /><path d="M832 864c0-211.2-147.2-377.6-326.4-377.6s-326.4 166.4-326.4 377.6H832z" fill="#F4B1B2" /><path d="M832 889.6H147.2v-25.6c0-224 160-403.2 352-403.2s352 179.2 352 396.8v25.6l-19.2 6.4z m-633.6-51.2h608C800 659.2 665.6 512 505.6 512c-166.4 0-294.4 147.2-307.2 326.4zM710.4 499.2c-12.8 0-25.6-12.8-25.6-25.6s12.8-25.6 25.6-25.6c64 0 121.6-51.2 121.6-121.6 0-51.2-32-96-83.2-115.2-12.8-6.4-19.2-19.2-12.8-32 6.4-12.8 19.2-19.2 32-12.8 70.4 19.2 115.2 83.2 115.2 160-6.4 96-83.2 172.8-172.8 172.8z" fill="#D72822" /><path d="M966.4 806.4h-57.6c-12.8 0-25.6-12.8-25.6-25.6s12.8-25.6 25.6-25.6h32c-12.8-140.8-115.2-249.6-236.8-249.6-12.8 0-25.6-12.8-25.6-25.6s12.8-25.6 25.6-25.6c160 0 288 147.2 288 326.4v25.6h-25.6z" fill="#D72822" /><path d="M300.8 499.2c-6.4 0-6.4 0 0 0-44.8 0-89.6-12.8-121.6-44.8-32-32-44.8-76.8-44.8-121.6 0-70.4 44.8-134.4 115.2-160 12.8-6.4 25.6 0 32 12.8 6.4 12.8 0 25.6-12.8 32-57.6 19.2-89.6 64-89.6 115.2 0 32 12.8 64 32 83.2 19.2 25.6 51.2 32 83.2 38.4 19.2 0 25.6 12.8 25.6 25.6s-6.4 19.2-19.2 19.2z" fill="#D72822" /><path d="M89.6 806.4H12.8v-25.6c0-179.2 128-320 288-320 12.8 0 25.6 12.8 25.6 25.6s-12.8 25.6-25.6 25.6C179.2 512 76.8 620.8 64 761.6h32c12.8 0 25.6 12.8 25.6 25.6-6.4 6.4-12.8 19.2-32 19.2z" fill="#D72822" /></svg>
            <span class="block text-gray-600">Clientes</span>
          </div>
          <span class="block text-xl font-bold text-gray-700">{{ clientes }}</span>
        </div>
        <div class="w-full p-8 bg-gradient-to-r from-green-100 to-green-200 rounded-lg shadow-lg transform transition duration-500 hover:scale-105 flex flex-col justify-center items-center text-center">
          <div class="mb-8">
            <svg class="mb-4" width="80px" height="80px" viewBox="0 0 15 15" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M1.5 2.5V2C1.26166 2 1.05645 2.16823 1.00971 2.40194L1.5 2.5ZM13.5 2.5L13.9903 2.40194C13.9435 2.16823 13.7383 2 13.5 2V2.5ZM14.5 7.5V8C14.6498 8 14.7917 7.93284 14.8867 7.81699C14.9816 7.70115 15.0197 7.54883 14.9903 7.40194L14.5 7.5ZM0.5 7.5L0.00970966 7.40194C-0.0196679 7.54883 0.0183596 7.70115 0.113329 7.81699C0.208298 7.93284 0.350203 8 0.5 8V7.5ZM3.5 10.5H3V11H3.5V10.5ZM11.5 10.5V11H12V10.5H11.5ZM0 15H15V14H0V15ZM1 7.5V14.5H2V7.5H1ZM13 7.5V14.5H14V7.5H13ZM1.5 3H13.5V2H1.5V3ZM13.0097 2.59806L14.0097 7.59806L14.9903 7.40194L13.9903 2.40194L13.0097 2.59806ZM14.5 7H0.5V8H14.5V7ZM0.99029 7.59806L1.99029 2.59806L1.00971 2.40194L0.00970966 7.40194L0.99029 7.59806ZM1 1H14V0H1V1ZM3 7.5V10.5H4V7.5H3ZM3.5 11H11.5V10H3.5V11ZM12 10.5V7.5H11V10.5H12Z" fill="#00FF00"/>
            </svg>
            <span class="block text-gray-600">Lojas</span>
          </div>
          <span class="block text-xl font-bold text-gray-700">{{ lojas }}</span>
        </div>
        <div class="w-full p-8 bg-gradient-to-r from-blue-100 to-blue-200 rounded-lg shadow-lg transform transition duration-500 hover:scale-105 flex flex-col justify-center items-center text-center">
          <div class="mb-8">
            <svg class="mb-4" fill="blue" width="80px" height="80px" viewBox="0 0 2.4 2.4" xmlns="http://www.w3.org/2000/svg"><path d="M0.1 1.25v0.5a0.1 0.1 0 0 0 0.1 0.1h0.1a0.3 0.3 0 0 0 0.6 0h0.6a0.3 0.3 0 0 0 0.6 0h0.1a0.1 0.1 0 0 0 0.1 -0.1v-1.2a0.3 0.3 0 0 0 -0.3 -0.3h-0.9a0.3 0.3 0 0 0 -0.3 0.3v0.2H0.6a0.3 0.3 0 0 0 -0.24 0.12l-0.24 0.32a0.06 0.06 0 0 0 -0.007 0.014l-0.006 0.011a0.1 0.1 0 0 0 -0.007 0.035m1.6 0.6a0.1 0.1 0 1 1 0.1 0.1 0.1 0.1 0 0 1 -0.1 -0.1m-0.7 -1.3a0.1 0.1 0 0 1 0.1 -0.1h0.9a0.1 0.1 0 0 1 0.1 0.1v1.1h-0.078a0.3 0.3 0 0 0 -0.444 0H1Zm-0.2 0.6H0.4l0.12 -0.16a0.1 0.1 0 0 1 0.08 -0.04h0.2Zm-0.3 0.7a0.1 0.1 0 1 1 0.1 0.1 0.1 0.1 0 0 1 -0.1 -0.1m-0.2 -0.5h0.5v0.278a0.3 0.3 0 0 0 -0.422 0.022H0.3Z"/></svg>
            <span class="block text-gray-600">Motoristas</span>
          </div>
          <span class="block text-xl font-bold text-gray-700">{{ motoristas }}</span>
        </div>
        
      </div>
      <div class="grid grid-cols-2 gap-6 p-7">
        <div class="w-full p-8 bg-gradient-to-r from-purple-100 to-purple-200 rounded-lg shadow-lg transform transition duration-500 hover:scale-105 flex flex-col justify-center items-center text-center">
          <div class="mb-8">
            <svg class="mb-4" width="80" height="80" viewBox="0 0 1.5 1.5" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="m.05.35.7-.3.7.3m-1.4 0 .7.3m.7-.3-.7.3m.7-.3v.8l-.7.3m.7-1.1-.7.3v.8m0-.8v.8m0-.8-.7-.3m.7 1.1-.7-.3v-.8" stroke="#ea46ec" stroke-linejoin="round" stroke-width=".1"/>
            </svg>
            <span class="block text-gray-600 ">Encomendas</span>
          </div>
          <span class="block text-xl font-bold text-gray-700">{{ encomendas }}</span>
        </div>
        <div class="w-full p-8 bg-gradient-to-r from-yellow-100 to-yellow-200 rounded-lg shadow-lg transform transition duration-500 hover:scale-105 flex flex-col justify-center items-center text-center">
          <div class="mb-8">
            <img src="/src/assets/svg/routes.svg" alt="" class="mb-4 w-20 h-20">
            <span class="block text-gray-600 ">Rotas</span>
          </div>
          <span class="block text-xl font-bold text-gray-700">{{ rotas }}</span>
        </div>  
      </div>
    </div>
  </div>
</template>

<style scoped>
.client__header {
  transition: all 0.3s ease-in-out;
}

.modal__btn {
  transition: transform 0.3s ease-in-out;
}

.modal__btn:hover {
  transform: scale(1.05);
}

.grid div {
  transition: transform 0.5s ease-in-out;
}

.grid div:hover {
  transform: scale(1.05);
}
</style>