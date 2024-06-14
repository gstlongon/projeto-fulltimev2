<script setup>
  ////IMPORTS
  import Sidebar from '../components/Sidebar.vue'
  import Swal from 'sweetalert2';
  import Modal from '../components/Modal.vue';
  import Modal2 from '../components/Modal2.vue';
  import { ref, onMounted } from "vue";
  import axios from "axios";

  const showModal = ref(false);
  const editModal = ref(false);
  const deleteModal = ref(false);
  const lojaValido = ref(false); 
  const clienteValido = ref(false); 
  const kgValido = ref(false); 
  const statusValido = ref(false); 
  const encomendaBusca = ref('');
  const novaEncomenda = ref({clienteId: '',lojaId: '',kg: '0',  status: ''});
  const summy = ref({ distancia: '', duracao: '' });
  let encomendaSelecionado = ref({});
  let encomendas = ref('')
  let lojas = ref('')
  let clientes = ref('')

  //  
  onMounted(() => {
    buscarEncomendas();
    buscarClientes();
    buscarLojas();
  });


  const pegarEncomendasNome = async (id) => {
    if (id == '') {   
      try {
      encomendas.value = '';
      const response = await axios.get(`https://localhost:7204/api/Encomenda`);
      if (response.data && Object.keys(response.data).length !== 0) {
        encomendas.value = response.data;
      } 
    } catch (error) {
      console.error('Erro ao pegar encomendas: ', error);
    }
    }else{
      try {
        encomendas.value = '';
        const response = await axios.get(`https://localhost:7204/api/Encomenda/b=${id}`);
        if (response.data && Object.keys(response.data).length !== 0) {
          encomendas.value = response.data;
        } 
      } catch (error) {
        console.error('Erro ao pegar encomendas: ', error);
      }
    } 
  };

  const buscarNome = async () => {
    try {
      await pegarEncomendasNome(encomendaBusca.value);
    } catch (error) {
      console.error('Erro ao buscar encomendas: ', error);
    }
  };

  const verificarStatus = () => {
    const status = novaEncomenda.value.status;
    if (status == '') {
      statusValido.value = false;
    } else {
      statusValido.value = true;
    }
  };

  const verificarLoja = () => {
    const loja = novaEncomenda.value.lojaId;
    if (loja == '') {
      lojaValido.value = false;
    } else {
      lojaValido.value = true;
    }
  };
  
  const verificarCliente = () => {
    const cliente = novaEncomenda.value.clienteId;
    if (cliente == '') {
      clienteValido.value = false;
    } else {
      clienteValido.value = true;
    }
  };

  const verificarKg = () => {
    if (novaEncomenda.value.kg <= 0) {
        kgValido.value = false;
    } else {
        kgValido.value = true;
    }
  };

  const statusClass = (status) => {
    switch (status) {
      case 'Disponivel':
        return 'text-green-500';
      case 'Em Rota':
        return 'text-yellow-600';
      case 'Entregue':
        return 'text-red-600';
      default:
        return 'text-gray-600';
    }
  };
  
  const buscarEncomendas = async () => {
    try {
      encomendas.value = '';
      const response = await axios.get(`https://localhost:7204/api/Encomenda`);
        
      if (response.data && Object.keys(response.data).length !== 0) {
          encomendas.value = response.data
      } 
    } catch (error) {
      console.error('Erro ao pegar encomendas: ', error);
    }
  };

  const dataForm = async (index) => {
    encomendaSelecionado = {
      id: encomendas.value[index].id,
      clienteId: encomendas.value[index].clienteId,
      lojaId: encomendas.value[index].lojaId,
      kg: encomendas.value[index].kg,
      distancia: encomendas.value[index].distancia,
      duracao: encomendas.value[index].duracao,
      status: encomendas.value[index].status,
      latitudeOrigem: encomendas.value[index].latitudeOrigem,
      longitudeOrigem: encomendas.value[index].longitudeOrigem,
      latitudeDestino: encomendas.value[index].latitudeDestino,
      longitudeDestino: encomendas.value[index].longitudeDestino,
    }
  }

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

  const adicionarSummaryBD = () => {
  const positionOrigem = { lat: infosLoja(novaEncomenda.value.lojaId).latitude, lng: infosLoja(novaEncomenda.value.lojaId).longitude};
  const positionDestino = { lat: infosCliente(novaEncomenda.value.clienteId).latitude, lng: infosCliente(novaEncomenda.value.clienteId).longitude };
    const platform = new H.service.Platform({
      apikey: 'eGEbMqmjPEdw473hAUUXR5t_22Ys36iC6n4NfKGCu8Q' 
    });
    const router = platform.getRoutingService(null, 8);
    const params = {
      representation: 'display',
      transportMode: 'car',
      origin: `${positionOrigem.lat},${positionOrigem.lng}`,
      destination: `${positionDestino.lat},${positionDestino.lng}`,
      return: 'summary',
    };
    router.calculateRoute(params, (result) => {
      const route = result.routes[0];
      const travelSummary = route.sections[0].summary;
      const distance = (travelSummary.length / 1000).toFixed(2);
      const totalSeconds = travelSummary.baseDuration;
      const hours = Math.floor(totalSeconds / 3600);
      const minutes = Math.floor((totalSeconds % 3600) / 60);
      const travelTime = `${hours}h ${minutes}min`;
      summy.value = {
        distancia: distance,
        duracao: travelTime
      };
      adicionarEncomenda();
    }, erroSummaryBD);
  };

  const erroSummaryBD = (error) => {
      console.error('Erro ao calcular a rota:', error);
  };

  const infosCliente = (clienteId) =>{
    if(clientes.value){
      return clientes.value.find(cliente => cliente.id === clienteId) || {};
    }else{
      return {};
    }
  };

  const infosLoja = (lojaId) =>{
    if(lojas.value){
      return lojas.value.find(loja => loja.id === lojaId) || {};
    }else{
      return {};
    }
  };


  const adicionarEncomenda = async () => {  
    try {
      const response = await axios.post('https://localhost:7204/api/Encomenda', {
        clienteId: novaEncomenda.value.clienteId,
        lojaId: novaEncomenda.value.lojaId,
        kg: novaEncomenda.value.kg,
        latitudeOrigem: infosLoja(novaEncomenda.value.lojaId).latitude,
        longitudeOrigem: infosLoja(novaEncomenda.value.lojaId).longitude,
        latitudeDestino: infosCliente(novaEncomenda.value.clienteId).latitude,
        longitudeDestino: infosCliente(novaEncomenda.value.clienteId).longitude,
        distancia: summy.value.distancia,
        duracao: summy.value.duracao,
        status: novaEncomenda.value.status 
      });
      
      if (response.status === 201) { 
        Swal.fire({
          position: "top",
          icon: "success",
          title: "Encomenda cadastrada com sucesso!",
          showConfirmButton: false,
          timer: 1500,
          backdrop: false
        });
        novaEncomenda.value = {
          clienteId: '',
          lojaId: '',
          kg: '0',
          status: '',
        };
        summy.value = {
          distancia: '',
          duracao: '',
        };
        showModal.value = false; 
        kgValido.value = false; 
        clienteValido.value = false; 
        lojaValido.value = false; 
        statusValido.value = false; 
        buscarEncomendas()
      }
    } catch (error) {
      console.error('Erro ao adicionar encomenda: ', error);
    }
  };

  const editarEncomenda = async () => {
    try {
      const response = await axios.put(`https://localhost:7204/api/Encomenda/${encomendaSelecionado.id}`, {
        clienteId: encomendaSelecionado.clienteId,
        lojaId: encomendaSelecionado.lojaId,
        kg: encomendaSelecionado.kg,
        distancia: encomendaSelecionado.distancia,
        duracao: encomendaSelecionado.duracao,
        status: encomendaSelecionado.status,
        latitudeOrigem: encomendaSelecionado.latitudeOrigem,
        longitudeOrigem: encomendaSelecionado.longitudeOrigem,
        latitudeDestino: encomendaSelecionado.latitudeDestino,
        longitudeDestino: encomendaSelecionado.longitudeDestino,
      });
      
      if (response.status === 201 || response.status === 200 || response.status === 204) { 
        Swal.fire({
          position: "top",
          icon: "success",
          title: "Encomenda editado com sucesso!",
          showConfirmButton: false,
          timer: 1500,
          backdrop: false
        });
        editModal.value = false; 
        buscarEncomendas()
      }
    } catch (error) {
      console.error('Erro ao editar encomenda: ', error);
    }
  };

  const apagarEncomenda = async () => {
    try {
      const response = await axios.delete(`https://localhost:7204/api/Encomenda/${encomendaSelecionado.id}`)
      if (response.status === 201 || response.status === 200 || response.status === 204) { 
        Swal.fire({
          position: "top",
          icon: "success",
          title: "Encomenda apagado com sucesso!",
          showConfirmButton: false,
          timer: 1500,
          backdrop: false
        });
        deleteModal.value = false;
        buscarEncomendas()
      }
    } catch (error) {
      console.error('Erro ao deletar encomenda: ', error);
    }
  };
</script>

<!-- template -->
<template>
  <div class="flex w-full gap-8"  >
    <Sidebar />
    <div class="p-7  w-full">
      <div class="client__header bg-gray-100 py-4 px-8 rounded-lg shadow-md transition-all duration-300 hover:shadow-lg">
        <div class="flex justify-between items-center">
          <h1 class="text-2xl font-bold text-gray-700">Encomendas</h1>
          <div class="flex items-center">
            <input type="text" v-model="encomendaBusca" @input="buscarNome" placeholder="Pesquisar por nome..." class="bg-white border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:border-red-600 transition-colors duration-300">
            <button class="modal__btn bg-red-600 text-white font-semibold px-4 py-2 ml-2 rounded-lg flex items-center hover:bg-red-700 transition-all duration-300 transform hover:scale-105" @click="showModal = true">
              <img class="mr-2" src="/src/assets/svg/addb.svg" alt="Adicionar Encomenda">
              <span>Adicionar Encomenda</span>
            </button>
          </div>
        </div>
      </div>
      <div class="client__body">
        <div class="container mx-auto py-8">
          <div v-if="encomendas" class="grid grid-cols-3 gap-6">
            <div v-for="(encomenda, index) in encomendas" :key="index" class="bg-white shadow-md rounded-lg overflow-hidden transform transition duration-300 hover:shadow-xl hover:scale-105">
              <div class="p-6">
                <h3 class="text-center text-xl font-bold mb-1">Encomenda {{ encomenda.id }}</h3>
                <div class="flex flex-col justify-around items-center mb-2">
                
                  <div class="text-center">
                    <svg width="50" height="50" viewBox="0 0 0.938 0.938" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="mt-2 mx-auto transition duration-300 hover:scale-110 iconify iconify--gis">
                      <path d="M.094.156V.125A.03.03 0 0 0 .063.15zm.75 0L.875.15A.03.03 0 0 0 .844.125zm.063.313V.5A.03.03 0 0 0 .938.463zm-.875 0L.001.463A.03.03 0 0 0 .031.5zM.22.657H.188v.031h.031zm.5 0v.031h.031V.657zM0 .938h.938V.875H0zM.063.469v.438h.063V.469zm.75 0v.438h.063V.469zM.094.188h.75V.125h-.75zM.813.163l.063.313.061-.012L.874.151zm.093.275H.031v.063h.875zM.062.475.125.162.064.15.001.463zM.063.063h.813V0H.063zm.125.406v.188h.063V.469zm.031.219h.5V.625h-.5zM.75.657V.469H.687v.188z" fill=#ff0000 />
                    </svg>
                    <p class="text-gray-600 mt-2"><strong>Origem: </strong>{{ infosLoja(encomenda.lojaId).nome }}</p>
                    <p class="text-gray-600 mb-4">{{ infosLoja(encomenda.lojaId).bairro }}, {{ infosLoja(encomenda.lojaId).cidade }}, {{ infosLoja(encomenda.lojaId).estado }}</p>
                  </div>
                  <div class="text-center">
  
                    <svg width="50" height="50" viewBox="0 0 1.2 1.2" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class=" mx-auto transition duration-300 hover:scale-110 iconify iconify--gis">
                      <path fill=#ff0000 d="M.786.636a.3.3 0 1 0-.371 0 .5.5 0 0 0-.311.409.05.05 0 0 0 .1.011.4.4 0 0 1 .795 0 .05.05 0 0 0 .05.045h.006a.05.05 0 0 0 .044-.055.5.5 0 0 0-.313-.409M.6.6A.2.2 0 1 1 .8.4a.2.2 0 0 1-.2.2"/>
                    </svg>
                    <p class="text-gray-600 mt-2"><strong>Destino: </strong>{{ infosCliente(encomenda.clienteId).nome }}</p>
                    <p class="text-gray-600 mb-4">{{ infosCliente(encomenda.clienteId).bairro }}, {{ infosCliente(encomenda.clienteId).cidade }}, {{ infosCliente(encomenda.clienteId).estado }}</p>
                    <p class="text-gray-600 font-bold mt-2">{{ encomenda.distancia }}km - {{ encomenda.duracao }} - {{ encomenda.kg }}kg</p>
                    <p :class="statusClass(encomenda.status)" class="font-bold mt-2">{{ encomenda.status }}</p>
                  </div>
                </div>
                <div class="border-t mt-6 pt-4 flex justify-around">
                  <button @click="editModal = true, dataForm(index)"  class="text-gray-700 hover:text-blue-500 transform transition duration-300 hover:scale-125">
                    <img src="/src/assets/svg/editar.svg" alt="Editar" title="Editar" class="w-7 h-7">
                  </button>
                  <button @click="dataForm(index), deleteModal = true" class="text-gray-700 hover:text-red-500 transform transition duration-300 hover:scale-125">
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
    </div>
  </div>

  <Modal :show="showModal">
    <h2 class="text-center text-lg mb-8">Cadastrar Encomenda</h2>
    <div class="grid grid-cols-2 gap-4">
      <div class="input__box">
        <label for="loja" class="block w-full">Loja (Origem):</label>
        <select v-model="novaEncomenda.lojaId":class="{ 'border-green-500': lojaValido, 'border-red-500': !lojaValido }"class="w-full h-[40px] p-2 border rounded mt-2" name="loja" id="loja" @change="verificarLoja">
          <option  value="" disabled selected>Selecione a loja</option>
          <option v-for="loja in lojas" :key="loja.id" :value="loja.id">{{ loja.nome }} - {{ loja.bairro }}, {{ loja.cidade }}/{{ loja.estado }}</option>
        </select>
      </div>
      <div class="input__box">
        <label for="cliente" class="block w-full">Cliente (Destino):</label>
        <select v-model="novaEncomenda.clienteId":class="{ 'border-green-500': clienteValido, 'border-red-500': !clienteValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="cliente" id="cliente" @change="verificarCliente">
          <option value="" disabled selected>Selecione o cliente</option>
          <option v-for="cliente in clientes" :key="cliente.id" :value="cliente.id">{{ cliente.nome }} - {{ cliente.bairro }}, {{ cliente.cidade }}/{{ cliente.estado }}</option>
        </select>
      </div>
      <div class="input__box">
        <label for="kg" class="block w-full">KG da encomenda:</label>
        <input v-model="novaEncomenda.kg":class="{ 'border-green-500': kgValido, 'border-red-500': !kgValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="kg" type="number" placeholder="valor do KG da encomenda" @input="verificarKg">
      </div>
      <div class="input__box">
          <label for="Status" class="block w-full">Status:</label>
          <select v-model="novaEncomenda.status":class="{ 'border-green-500': statusValido, 'border-red-500': !statusValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="status" id="status" @change="verificarStatus">
              <option value="" disabled selected>Selecione o status</option>
              <option value="Disponivel">Disponivel</option>
              <option value="Em Rota">Em Rota</option>
              <option value="Entregue">Entregue</option>
          </select>
      </div>
    </div>
    <button class="close__btn" @click="showModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button @click="adicionarSummaryBD" class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white" :disabled="!(lojaValido && clienteValido && kgValido && statusValido)">
      Cadastrar
      <span v-if="!(lojaValido && clienteValido && kgValido && statusValido)" class="text-sm ml-2 text-gray-400">(Preencha todos os campos corretamente)</span>
    </button>
  </Modal>
  
  <Modal :show="editModal">
    <h2 class="text-center text-lg mb-8">Editar Encomenda {{ encomendaSelecionado.id }}</h2>
    <div class="grid grid-cols-3 gap-4">

      <div class="input__box">
        <label for="clienteId" class="block w-full">Cliente:</label>
        <select class="w-full h-[40px] p-2 border rounded mt-2" name="clienteId" id="clienteId" v-model="encomendaSelecionado.clienteId">
          <option value="" disabled>Selecione o cliente</option>
          <option v-for="cliente in clientes" :key="cliente.id" :value="cliente.id">{{ cliente.nome }} - {{ cliente.bairro }}, {{ cliente.cidade }}/{{ cliente.estado }}</option>
        </select>
      </div>
      <div class="input__box">
        <label for="lojaId" class="block w-full">Loja:</label>
        <select class="w-full h-[40px] p-2 border rounded mt-2" name="lojaId" id="lojaId" v-model="encomendaSelecionado.lojaId">
          <option value="" disabled>Selecione a loja</option>
          <option v-for="loja in lojas" :key="loja.id" :value="loja.id">{{ loja.nome }} - {{ loja.bairro }}, {{ loja.cidade }}/{{ loja.estado }}</option>
        </select>
      </div>
      
      <div class="input__box">
        <label for="kg" class="block w-full">KG:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="kg" type="number" v-model="encomendaSelecionado.kg" placeholder="Digite o kg">
      </div>
      <div class="input__box">
        <label for="distancia" class="block w-full">Distancia:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="distancia" type="text" v-model="encomendaSelecionado.distancia" placeholder="Digite a Distancia">
      </div>
      <div class="input__box">
        <label for="duracao" class="block w-full">Duracão:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="duracao" type="text" v-model="encomendaSelecionado.duracao" placeholder="Digite a Duração">
      </div>
      <div class="input__box">
          <label for="status" class="block w-full">Status:</label>
          <select v-model="encomendaSelecionado.status" class="w-full h-[40px] p-2 border rounded mt-2" name="status" id="status">
              <option value="" disabled selected>Selecione o status</option>
              <option value="Disponivel">Disponivel</option>
              <option value="Em Rota">Em Rota</option>
              <option value="Entregue">Entregue</option>
          </select>
      </div>
      <div class="input__box">
        <label for="latitudeOrigem" class="block w-full">Latitude Origem:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="latitudeOrigem" type="text" v-model="encomendaSelecionado.latitudeOrigem" placeholder="Digite a Latitude Origem">
      </div>
      <div class="input__box">
        <label for="longitudeOrigem" class="block w-full">Longitude Origem:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="longitudeOrigem" type="text" v-model="encomendaSelecionado.longitudeOrigem" placeholder="Digite a Longitude  Origem">
      </div>
      <div class="input__box">
        <label for="latitudeDestino" class="block w-full">Latitude Destino:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="latitudeDestino" type="text" v-model="encomendaSelecionado.latitudeDestino" placeholder="Digite a Latitude Destino">
      </div>
      <div class="input__box">
        <label for="longitudeDestino" class="block w-full">Longitude Destino:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="longitudeDestino" type="text" v-model="encomendaSelecionado.longitudeDestino" placeholder="Digite a Longitude  Destino">
      </div>
      
    </div>
    <button class="close__btn" @click="editModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#006400] rounded mt-4 text-white" @click="editarEncomenda()">
      Editar
    </button>
  </Modal>
  <Modal2 :show="deleteModal">
    <h2 class="text-center text-lg mb-8">Deletar Encomenda {{ encomendaSelecionado.id }}</h2>
    <p class="text-center">Tem certeza que deseja deletar a encomenda?</p>
    <button class="close__btn" @click="deleteModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white" @click="apagarEncomenda()">
      Deletar
    </button>
  </Modal2>
</template>

<style scoped>
  .close__btn {
    position: absolute;
    top: 20px;
    right: 20px;
  }
</style>
