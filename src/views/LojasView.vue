<script setup>
  import Sidebar from '../components/Sidebar.vue'
  import Swal from 'sweetalert2';
  import Modal from '../components/Modal.vue';
  import Modal2 from '../components/Modal2.vue';
  import { ref, onMounted } from "vue";
  import axios from "axios";

  const showModal = ref(false);
  const editModal = ref(false);
  const deleteModal = ref(false);
  const cepValido = ref(false); 
  const telefoneValido = ref(false); 
  const cnpjValido = ref(false);
  const numeroValido = ref(false); 
  const nomeValido = ref(false); 
  const emailValido = ref(false);
  const lojaBusca = ref('');
  const cep = ref('');
  const coords = ref({ lat: '', lng: '' });
  const novaLoja = ref({nome: '',cnpj: '',telefone: '',email: '',cep: '',logradouro: '',bairro: '',estado: '',numero: ''});
  const endereco = ref({logradouro: '',bairro: '',cidade: '',estado: ''});
  let lojaSelecionado = ref({});
  let lojas = ref('')

  const verificarCnpj = () => {
    const documentoLimpo = novaLoja.value.cnpj.replace(/\D/g, '');
    if (documentoLimpo.length === 14) {
        novaLoja.value.cnpj = documentoLimpo.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
        
        let multiplicadoresPrimeiroDigito = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        let multiplicadoresSegundoDigito = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        
        let soma = 0;
        
        for (let i = 0; i < 12; i++) {
            soma += parseInt(documentoLimpo.charAt(i)) * multiplicadoresPrimeiroDigito[i];
        }
        
        let resto = soma % 11;
        let digitoVerificador1 = resto < 2 ? 0 : 11 - resto;
        
        soma = 0;
        
        for (let i = 0; i < 13; i++) {
            soma += parseInt(documentoLimpo.charAt(i)) * multiplicadoresSegundoDigito[i];
        }
        
        resto = soma % 11;
        let digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

        if (parseInt(documentoLimpo.charAt(12)) !== digitoVerificador1 || parseInt(documentoLimpo.charAt(13)) !== digitoVerificador2) {
            cnpjValido.value = false;
        }
        
        cnpjValido.value = true;
    } else {
        cnpjValido.value = false;
    }
  };

  const verificarEmail = () => {
      const email = novaLoja.value.email.trim();

      if (email.length === 0) {
          emailValido.value = false;
          return;
      }

      const regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

      if (regexEmail.test(email)) {
          emailValido.value = true;
      } else {
          emailValido.value = false;
      }
  };

  const verificarNome = () => {
      const nomeCompleto = novaLoja.value.nome.trim();
      if (nomeCompleto.length === 0) {
          nomeValido.value = false;
          return;
      }
      const partesNome = nomeCompleto.split(" ");
      if (partesNome.length < 2) {
          nomeValido.value = false;
          return;
      }
      const nomeFormatado = partesNome.map((parte) => parte.charAt(0).toUpperCase() + parte.slice(1).toLowerCase()).join(" ");
      novaLoja.value.nome = nomeFormatado;
      nomeValido.value = true;
  };

  const verificarTelefone = () => {
    const telefoneLimpo = novaLoja.value.telefone.replace(/\D/g, '');
    novaLoja.value.telefone = telefoneLimpo.replace(/^(\d{2})(\d{4,5})(\d{4})$/, '($1) $2-$3');
    if  (telefoneLimpo.length > 9){
      telefoneValido.value = true;
    }else{
      telefoneValido.value = false;
    }
  };

  const verificarNumero = () => {
      const numero = novaLoja.value.numero.trim();
      if (numero.length > 0) {
          numeroValido.value = true;
      } else {
          numeroValido.value = false;
      }
  };


  const buscarEndereco = async () => {
  const cepLimpo = cep.value.replace(/\D/g, '');
  if (cepLimpo.length !== 8) {
    limparEndereco();
    cepValido.value = false;
    return;
  }
  cep.value = cep.value.replace(/\D/g, '').replace(/^(\d{5})(\d)/, '$1-$2');
  const inputs = document.querySelectorAll('.input__adress')
  try {
    const response = await axios.get(`https://api.postmon.com.br/v1/cep/${cep.value}`);
    
    if (response.data && Object.keys(response.data).length !== 0) {
      endereco.value = response.data;
      cepValido.value = true;
    } else {
      inputs.forEach(element => { 
        element.value = "";
        cepValido.value = true;
      });
      limparEndereco()
      cepValido.value = false;
    }
    
  } catch (error) {
    limparEndereco()
    cepValido.value = false;
  }
  };
  
  const limparEndereco = () => {
    endereco.value = {
      logradouro: '',
      bairro: '',
      cidade: '',
      estado: ''
    };
  };

  const buscarLojas = async () => {
    try {
      lojas.value = '';
      const response = await axios.get(`https://localhost:7204/api/Loja`);
        
      if (response.data && Object.keys(response.data).length !== 0) {
          lojas.value = response.data
      } 
    } catch (error) {
      console.error('Erro ao pegar lojas: ', error);
    }
  };

  const dataForm = async (index) => {
    lojaSelecionado = {
      id: lojas.value[index].id,
      nome: lojas.value[index].nome,
      cnpj: lojas.value[index].cnpj,
      telefone: lojas.value[index].telefone,
      email: lojas.value[index].email,
      cep: lojas.value[index].cep,
      logradouro: lojas.value[index].logradouro,
      bairro: lojas.value[index].bairro,
      estado: lojas.value[index].estado,
      numero: lojas.value[index].numero,
      cidade: lojas.value[index].cidade,
      latitude: lojas.value[index].latitude,
      longitude: lojas.value[index].longitude,
    }
  }

  const pegarLojasNome = async (nome) => {
    if (nome == '') {   
      try {
      lojas.value = '';
      const response = await axios.get(`https://localhost:7204/api/Loja`);
      if (response.data && Object.keys(response.data).length !== 0) {
        lojas.value = response.data;
      } 
    } catch (error) {
      console.error('Erro ao pegar as lojas: ', error);
    }
    }else{
      try {
        lojas.value = '';
        const response = await axios.get(`https://localhost:7204/api/Loja/b=${nome}`);
        if (response.data && Object.keys(response.data).length !== 0) {
          lojas.value = response.data;
        } 
      } catch (error) {
        console.error('Erro ao pegar as lojas: ', error);
      }
    } 
  };

  const buscarNome = async () => {
    try {
      await pegarLojasNome(lojaBusca.value);
    } catch (error) {
      console.error('Erro ao buscar as lojas: ', error);
    }
  };

  const puxarCords = async () => {
    const address = `${endereco.value.logradouro}, ${novaLoja.value.numero}, ${endereco.value.cidade} - ${endereco.value.estado}, ${cep}`;
    const encodedAddress = encodeURIComponent(address);
    const url = `https://geocode.search.hereapi.com/v1/geocode?q=${encodedAddress}&apiKey=eGEbMqmjPEdw473hAUUXR5t_22Ys36iC6n4NfKGCu8Q`;
    try {
      const response = await axios.get(url);
      if (response.data.items.length > 0) {
        const location = response.data.items[0].position;
        coords.value = {
          lat: location.lat,
          lng: location.lng
        };
        adicionarLoja();
      } else {
        console.error('Nenhum resultado encontrado para o endereço fornecido.');
      }
    } catch (error) {
      console.error('Erro ao buscar coordenadas:', error);
    }
  };

  const adicionarLoja = async () => {  
    try {
      const response = await axios.post('https://localhost:7204/api/Loja', {
        nome: novaLoja.value.nome,
        email: novaLoja.value.email,
        telefone: novaLoja.value.telefone,
        cnpj: novaLoja.value.cnpj,
        logradouro: endereco.value.logradouro,
        bairro: endereco.value.bairro,
        cep: cep.value,
        numero: novaLoja.value.numero,
        cidade: endereco.value.cidade,
        estado: endereco.value.estado,
        latitude: String(coords.value.lat), 
        longitude: String(coords.value.lng),
      });
      
      if (response.status === 201) { 
        Swal.fire({
          position: "top",
          icon: "success",
          title: "Loja cadastrado com sucesso!",
          showConfirmButton: false,
          timer: 1500,
          backdrop: false
        });
        novaLoja.value = {
          nome: '',
          cnpj: '',
          telefone: '',
          email: '',
          cep: '',
          logradouro: '',
          bairro: '',
          estado: '',
          numero: '',
          complemento: ''
        };
        endereco.value = {
          logradouro: '',
          bairro: '',
          cidade: '',
          estado: ''
        };
        coords.value = {
          lat: '',
          lng: '',
        };
        cep.value = ""
        showModal.value = false;
        cepValido.value = false;  
        telefoneValido.value = false; 
        cnpjValido.value = false; 
        numeroValido.value = false; 
        nomeValido.value = false; 
        emailValido.value = false;  
        buscarLojas()
      }
    } catch (error) {
      console.error('Erro ao adicionar loja: ', error);
    }
  };

  const editarLoja = async () => {
    try {
      const response = await axios.put(`https://localhost:7204/api/Loja/${lojaSelecionado.id}`, {
        nome: lojaSelecionado.nome,
        email: lojaSelecionado.email,
        cnpj: lojaSelecionado.cnpj,
        telefone: lojaSelecionado.telefone,
        logradouro: lojaSelecionado.logradouro,
        bairro: lojaSelecionado.bairro,
        cep: lojaSelecionado.cep,
        numero: lojaSelecionado.numero,
        cidade: lojaSelecionado.cidade,
        estado: lojaSelecionado.estado,
        latitude: lojaSelecionado.latitude,
        longitude: lojaSelecionado.longitude,
      });
      
      if (response.status === 201 || response.status === 200 || response.status === 204) { 
        Swal.fire({
          position: "top",
          icon: "success",
          title: "Loja editado com sucesso!",
          showConfirmButton: false,
          timer: 1500,
          backdrop: false
        });
        editModal.value = false; 
        buscarLojas()
      }
    } catch (error) {
      console.error('Erro ao editar loja: ', error);
    }
  };

  const apagarLoja = async () => {
    try {
      const response = await axios.delete(`https://localhost:7204/api/Loja/${lojaSelecionado.id}`)
      if (response.status === 201 || response.status === 200 || response.status === 204) { 
        Swal.fire({
          position: "top",
          icon: "success",
          title: "Loja apagado com sucesso!",
          showConfirmButton: false,
          timer: 1500,
          backdrop: false
        });
        buscarLojas()  
        editModal.value = false; 
      }
    } catch (error) {
      console.error('Erro ao deletar loja: ', error);
    }
  };

  onMounted(() => {
    buscarLojas();
  });
</script>

<template>
  <div class="flex w-full gap-8"  >
    <Sidebar />
    <div class="p-7  w-full">
      <div class="client__header bg-gray-100 py-4 px-8 rounded-lg shadow-md transition-all duration-300 hover:shadow-lg">
        <div class="flex justify-between items-center">
          <h1 class="text-2xl font-bold text-gray-700">Lojas</h1>
          <div class="flex items-center">
            <input type="text" v-model="lojaBusca" @input="buscarNome" placeholder="Pesquisar por nome..." class="bg-white border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:border-red-600 transition-colors duration-300">
            <button class="modal__btn bg-red-600 text-white font-semibold px-4 py-2 ml-2 rounded-lg flex items-center hover:bg-red-700 transition-all duration-300 transform hover:scale-105" @click="showModal = true">
              <img class="mr-2" src="/src/assets/svg/addb.svg" alt="Adicionar Loja">
              <span>Adicionar Loja</span>
            </button>
          </div>
        </div>
      </div>
      <div class="client__body">
        <div class="container mx-auto py-8">
          <div v-if="lojas" class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
            <div v-for="(loja, index) in lojas" :key="index" class="bg-white shadow-md rounded-lg overflow-hidden transform transition duration-300 hover:shadow-xl hover:scale-105">
              <div class="p-6">
                <img src="/src/assets/img/user4.png" alt="Foto da Loja" class="w-24 h-24 rounded-full mx-auto transition duration-300 hover:scale-110">
                <h3 class="text-center text-lg font-bold mt-4">{{ loja.nome }}</h3>
                <p class="text-center text-gray-600">{{ loja.cidade }}/{{ loja.estado }}</p>
                <div class="mt-4 text-center">
                  <p>{{ loja.email }}</p>
                  <p>{{ loja.telefone }}</p>
                </div>
                <div class="border-t mt-4 pt-4 flex justify-around">
                  <button @click="editModal = true, dataForm(index)" class="text-gray-700 hover:text-blue-500 transform transition duration-300 hover:scale-125">
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
              <h2 class="text-xl  font-bold">Nada para mostrar aqui :(</h2>
              <img class="mx-auto mt-8" src="/src/assets/svg/nothing.svg" alt="">
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>


  <Modal :show="showModal">
    <h2 class="text-center text-lg mb-8">Cadastrar Loja</h2>
    <div class="grid grid-cols-3 gap-4">
      <div class="input__box mb-2">
        <label for="nome" class="block w-full">Nome:</label>
        <input v-model="novaLoja.nome":class="{ 'border-green-500': nomeValido, 'border-red-500': !nomeValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="nome" type="text" placeholder="Nome Sobrenome"@input="verificarNome"maxlength="30">
      </div>
      <div class="input__box mb-2">
        <label for="doc" class="block w-full">CNPJ:</label>
      <input v-model="novaLoja.cnpj":class="{ 'border-green-500': cnpjValido, 'border-red-500': !cnpjValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="cnpj" type="text" placeholder="00.000.000/0001-00" @input="verificarCnpj"maxlength="18">
      </div>
      <div class="input__box">
        <label for="telefone" class="block w-full">Telefone:</label>
        <input v-model="novaLoja.telefone":class="{ 'border-green-500': telefoneValido, 'border-red-500': !telefoneValido }"  class="w-full h-[40px] p-2 border rounded mt-2" name="telefone" type="text" placeholder="(00) 00000-0000"@input="verificarTelefone" maxlength="15">
      </div>
      <div class="input__box">
        <label for="email" class="block w-full">E-mail:</label>
        <input v-model="novaLoja.email":class="{ 'border-green-500': emailValido, 'border-red-500': !emailValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="email" type="text" placeholder="email@dominio"@input="verificarEmail" maxlength="30"> 
      </div>
      <div class="input__box">
        <label for="cep" class="block w-full">CEP:</label>
        <input v-model="cep":class="{ 'border-green-500': cepValido, 'border-red-500': !cepValido }" @input="buscarEndereco"  class="w-full h-[40px] p-2 border rounded mt-2" name="cep" type="text" placeholder="00000-000" maxlength="9">
      </div>
      <div class="input__box">
        <label for="numero" class="block w-full">Número:</label>
        <input v-model="novaLoja.numero":class="{ 'border-green-500': numeroValido, 'border-red-500': !numeroValido }"  class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="numero" type="text" placeholder="0" @input="verificarNumero" maxlength="10">
      </div>
    </div>
    <div class="text-center text-base2 mt-4" style="max-height: 40px; min-height: 40px;">
        <label v-if="(cepValido)" class="block w-full">{{endereco.logradouro}}, {{endereco.bairro}} - {{endereco.cidade}}/{{endereco.estado}}</label>
      </div>
    <button class="close__btn" @click="showModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button @click="puxarCords" class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white" :disabled="!(nomeValido && cnpjValido && telefoneValido && emailValido && cepValido && numeroValido)">
      Cadastrar
      <span v-if="!(nomeValido && cnpjValido && telefoneValido && emailValido && cepValido && numeroValido)" class="text-sm ml-2 text-gray-400">(Preencha todos os campos corretamente)</span>
    </button>

  </Modal>
  
  <Modal :show="editModal">
    <h2 class="text-center text-lg mb-8">Editar {{ lojaSelecionado.nome }}</h2>
    <div class="grid grid-cols-3 gap-4">
      <div class="input__box mb-2">
        <label for="nome" class="block w-full">Nome:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="nome" type="text" v-model="lojaSelecionado.nome" placeholder="Digite o nome">
      </div>
      <div class="input__box mb-2">
        <label for="doc" class="block w-full">CNPJ:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" v-model="lojaSelecionado.cnpj" placeholder="Digite CNPJ">
      </div>
      <div class="input__box">
        <label for="doc" class="block w-full">Telefone:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" v-model="lojaSelecionado.telefone" placeholder="Digite o telefone">
      </div>
      <div class="input__box">
        <label for="email" class="block w-full">E-mail:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="email" type="text" v-model="lojaSelecionado.email" placeholder="Digite o E-mail">
      </div>
      <div class="input__box">
        <label for="cep" class="block w-full">CEP:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="cep" type="text" v-model="lojaSelecionado.cep" placeholder="Digite o CEP">
      </div>
      <div class="input__box">
        <label for="logradouro" class="block w-full">Logradouro:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="logradouro" v-model="lojaSelecionado.logradouro" type="text" placeholder="Digite o logradouro">
      </div>
      <div class="input__box">
        <label for="bairro" class="block w-full">Bairro:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="bairro" type="text" v-model="lojaSelecionado.bairro" placeholder="Digite o bairro">
      </div>
      <div class="input__box">
        <label for="complemento" class="block w-full">Cidade:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="complemento" type="text" v-model="lojaSelecionado.cidade" placeholder="Digite o complemento">
      </div>
      <div class="input__box">
        <label for="estado" class="block w-full">Estado:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="estado" type="text" v-model="lojaSelecionado.estado" placeholder="Digite o estado">
      </div>
      <div class="input__box">
        <label for="numero" class="block w-full">Número:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="numero" type="text" v-model="lojaSelecionado.numero" placeholder="Digite o número">
      </div>
      <div class="input__box">
        <label for="latitude" class="block w-full">Latitude:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="latitude" type="text" v-model="lojaSelecionado.latitude" placeholder="Digite a Latitude">
      </div>
      <div class="input__box">
        <label for="longitude" class="block w-full">Longitude:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="longitude" type="text" v-model="lojaSelecionado.longitude" placeholder="Digite a Longitude">
      </div>
      
    </div>
    <button class="close__btn" @click="editModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#006400] rounded mt-4 text-white" @click="editarLoja()">
      Editar
    </button>
  </Modal>
  <Modal2 :show="deleteModal">
    <h2 class="text-center text-lg mb-8">Deletar {{ lojaSelecionado.nome }}</h2>
    <p class="text-center">Tem certeza que deseja deletar a loja?</p>
    <button class="close__btn" @click="deleteModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white" @click="apagarLoja(), deleteModal = false">
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
